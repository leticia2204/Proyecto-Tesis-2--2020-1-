using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}
public class Player : MonoBehaviour
{
    [Header("Datos generales")]
    public PlayerState currentState;
    public float speed;
    private Rigidbody2D rb2d;
    private Vector3 change;
    private Animator anim;

    [Header("Datos del jugador")]
    public FloatValue currentHealth;
    public Signal playerHealthSignal;
    public VectorValue startingPosition;

    [Header("Inventario del jugador")]
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;

    [Header("Golpes")]
    public Signal playerHit;

    [Header("Magia")]
    public Signal reduceMagic;

    [Header("Armas")]
    public GameObject projectile;
    public Item bow;

    [Header("Datos iniciales del jugador y mapa")]
    public GameObject initialMap;
    bool movePrevent;

    [Header("Adicionales")]
    Vector2 mov;
    CircleCollider2D attackCollider;

    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    void Awake()
    {
        Assert.IsNotNull(initialMap);
    }
    void Start()
    {
        currentState = PlayerState.walk;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        anim.SetFloat("movX", 0);
        anim.SetFloat("movY", -1);
        transform.position = startingPosition.initialValue;

        //Recuperamos el collider de ataque
        attackCollider = transform.GetChild(0).GetComponent<CircleCollider2D>();
        //Desactivar colisiones al inicio
        attackCollider.enabled = false;
        Camera.main.GetComponent<MainCamara>().SetBound(initialMap);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == PlayerState.interact)
        {
            return;
        }

        Movimientos();
        Animaciones();
        AtaqueHacha();

        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());

        }
        else if (Input.GetButtonDown("Second Weapon") && currentState != PlayerState.attack
           && currentState != PlayerState.stagger)
        {
            if (playerInventory.CheckForItem(bow)) {
                StartCoroutine(SecondAttackCo());
            }
        }else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }

    //Ataque de hacha
    private IEnumerator AttackCo()
    {
        anim.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        anim.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    //Ataque de lanza:
    private IEnumerator SecondAttackCo()
    {
        currentState = PlayerState.attack;
        yield return null;
        Lanza();
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private void Lanza()
    {
        if (playerInventory.currentMagic > 0)
        {
            Vector2 temp = new Vector2(anim.GetFloat("movX"), anim.GetFloat("movY"));
            Lanza arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Lanza>();
            arrow.Setup(temp, ChooseArrowDirection());
            playerInventory.ReduceMagic(arrow.magicCost);
            reduceMagic.Raise();
        }
    }

    //Direccion de la lanza:
    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(anim.GetFloat("movY"), anim.GetFloat("movX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }
    void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime);   
    }

    void Movimientos()
    {
        mov = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
    }

    void Animaciones()
    {
        if (mov != Vector2.zero)
        {
            anim.SetFloat("movX", mov.x);
            anim.SetFloat("movY", mov.y);
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }
    }

    void PreventMovement()
    {
        if (movePrevent)
        {
            mov = Vector2.zero;
        }
    }

    IEnumerator EnableMovementAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        movePrevent = false;
    }

    public void RaiseItem()
    {
        if(playerInventory.currentItem != null)
        {
            if(currentState != PlayerState.interact)
            {
                //anim.SetBool("currentItem", true); //activar animacion
                currentState = PlayerState.interact;
                receivedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                //anim.SetBool("currentItem", false); //desactivar animacion
                currentState = PlayerState.idle;
                receivedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
        
    }

    void AtaqueHacha()
    {
        // Buscamos el estado actual mirando la información del animador
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        bool attacking = stateInfo.IsName("Player_Attack_Hacha");

        if (Input.GetButtonDown("attack") && !attacking)
        {
            anim.SetTrigger("attacking");
        }

        if (mov != Vector2.zero)
        {
            attackCollider.offset = new Vector2(mov.x / 2, mov.y / 2);
        }

        // Activamos el collider a la mitad de la animación de ataque
        if (attacking)
        { // El normalized siempre resulta ser un ciclo entre 0 y 1 
            float playbackTime = stateInfo.normalizedTime;

            if (playbackTime > 0.33 && playbackTime < 0.66) attackCollider.enabled = true;
            else attackCollider.enabled = false;

            //Actualizar estado a atacando
            currentState = PlayerState.attack;
        }

        if (currentState != PlayerState.interact)
            currentState = PlayerState.walk;
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            playerHit.Raise();
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (rb2d != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            rb2d.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            rb2d.velocity = Vector2.zero;
        }
    }
    
    void UpdateAnimationAndMove()
    {
        if(change != Vector3.zero)
        {
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
            anim.SetFloat("movX", change.x);
            anim.SetFloat("movY", change.y);
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        rb2d.MovePosition(
            transform.position + change * speed * Time.deltaTime
        );
    }

    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }
}

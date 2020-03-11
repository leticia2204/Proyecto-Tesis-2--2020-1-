using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cofres : Interactable
{
    [Header("Contents")]
    public Item contents;                       //contenido del cofre
    public Inventory playerInventory;           //inventario del jugador
    public bool isOpen;                         //verifica si el cofre ya se abrio
    public BoolValue storedOpen;

    [Header("Signals nd dialogues")]
    public Signal raiseItem;
    public GameObject dialogBox;                //ventaba de dialogo
    public Text dialogText;                     //Texto de la ventana de dialogo 

    [Header("Animation")]
    Animator anim;                             //animacion que se muestra
    void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = storedOpen.RuntimeValue;
        if (isOpen)
        {
            anim.SetBool("opened", true);
        }
    }

    
    void Update()
    {
        if (Input.GetButtonDown("interact") && playerInRange) //Presionar C y esta en rango
        {
            if (!isOpen)
            {
                //Acciones de abrir el cofre
                abrirCofre();
            }
            else
            {
                //El cofre ya esta abierto
                cofreYaAbierto();
            }
        }
    }

    public void abrirCofre()
    {
        //Mostrar ventana de dialogo
        dialogBox.SetActive(true);
        //Asignar texto
        dialogText.text = contents.itemDescription;
        //añadir objeto al inventario
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;
        //agregar animacion al encontrar objeto --por el momento no se añade
        raiseItem.Raise();
        //Abrir el cofre
        isOpen = true;
        //Levantar pista
        context.Raise();
        anim.SetBool("opened", true);
        storedOpen.RuntimeValue = isOpen;
    }

    public void cofreYaAbierto()
    {
        //Cerra ventana de dialogo
        dialogBox.SetActive(false);
        //borrar item
        //playerInventory.currentItem = null;
        //Levantar signal y detener animacion --animacion no se añade aun
        raiseItem.Raise();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            //Debug.Log("Player in range");
            context.Raise();
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && !isOpen)
        {
            //Debug.Log("Player left range");
            context.Raise();
            playerInRange = false;
        }
    }
}

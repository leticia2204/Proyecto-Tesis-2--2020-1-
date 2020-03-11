using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    key,
    enemy,
    button
}

public class Puertas : Interactable
{
    [Header("Door variables")]
    public DoorType thisDoorType;
    public bool open = false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playerInRange && thisDoorType == DoorType.key)
            {
                //Se verifica si se tiene la llave
                if (playerInventory.numberOfKeys > 0)
                {
                    //Remover la llave del inventario
                    playerInventory.numberOfKeys--;
                    //Si la tiene.. abrir la puerta
                    Open();
                }
            }
        }
    }

    //Se abre la puerta
    public void Open()
    {
        //TQuitar sprite de la puerta
        doorSprite.enabled = false;
        //Setear a true
        open = true;
        //Quitar box Collider
        physicsCollider.enabled = false;
    }

    //Se cierra la puerta
    public void Close()
    {

    }
}
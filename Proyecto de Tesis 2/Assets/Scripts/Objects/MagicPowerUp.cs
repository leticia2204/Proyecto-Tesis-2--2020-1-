﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerUp : PowerUp
{
    public Inventory playerInventory;
    public float magicValue;
    void Start()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInventory.currentMagic += magicValue;
            powerupSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
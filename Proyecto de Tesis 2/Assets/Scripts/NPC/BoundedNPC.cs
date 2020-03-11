﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundedNPC : Carteles
{
    private Vector3 directionVector;
    private Transform myTrasform;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Animator anim;
    public Collider2D bounds;    //espacio en donde el NPC se puede mover

    private bool isMoving;
    public float minMoveTime;
    public float maxMoveTime;
    private float moveTimeSeconds;
    public float minWaitTime;
    public float maxWaitTime;
    private float waitTimeSeconds;
    void Start()
    {
        moveTimeSeconds = Random.Range(minMoveTime,maxMoveTime);
        waitTimeSeconds = Random.Range(minWaitTime,maxWaitTime);
        anim = GetComponent<Animator>();
        myTrasform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        ChangeDirection();
    }

    public override void Update()
    {
        base.Update();
        if (isMoving)
        {
            moveTimeSeconds -= Time.deltaTime;
            if(moveTimeSeconds <= 0)
            {
                moveTimeSeconds = Random.Range(minMoveTime, maxMoveTime);
                isMoving = false;
                
            }
            if (!playerInRange)
            {
                Move();
            }
        }
        else
        {
            waitTimeSeconds -= Time.deltaTime;
            if(waitTimeSeconds <= 0)
            {
                ChooseDifferentDirection();
                isMoving = true;
                waitTimeSeconds = Random.Range(minWaitTime, maxWaitTime);
            }
        }
         
    }

    private void ChooseDifferentDirection()
    {
        Vector3 temp = directionVector;
        ChangeDirection();
        int loops = 0;
        while (temp == directionVector && loops < 100)
        {
            loops++;
            ChangeDirection();
        }
    }

    private void Move()
    {
        Vector3 temp = myTrasform.position + directionVector * speed * Time.deltaTime;
        if (bounds.bounds.Contains(temp))
        {
            myRigidbody.MovePosition(temp);
        }
        else
        {
            ChangeDirection();
        }
    }
    void ChangeDirection()
    {
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                //derecha
                directionVector = Vector3.right;
                break;
            case 1:
                //arriba
                directionVector = Vector3.up;
                break;
            case 2:
                //izquierda
                directionVector = Vector3.left;
                break;
            case 3:
                //abajo
                directionVector = Vector3.down;
                break;
            default:
                break;
        }
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        anim.SetFloat("movX", directionVector.x);
        anim.SetFloat("movY", directionVector.y);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ChooseDifferentDirection();
    }
}

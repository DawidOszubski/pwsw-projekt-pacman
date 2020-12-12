﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody2D rb;
    public Sprite pausedSprite;

    SoundManager soundManager;

    public AudioClip eatingVirus;
    public AudioClip pacmanDies;
    public AudioClip syringeUse;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = new Vector2(-1, 0) * speed;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void FixedUpdate()
    {

        float horzMove = Input.GetAxisRaw("Horizontal");
        float vertMove = Input.GetAxisRaw("Vertical");

        Vector2 moveVect;

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);

        if(Input.GetKey("a"))
        {
            if (localVelocity.x > 0)
            {
                moveVect = new Vector2(horzMove, 0);
                transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);

                rb.velocity = moveVect * speed;

                transform.localScale = new Vector2(1, 1);

                transform.localRotation = Quaternion.Euler(0, 0, 0);
            } else {
                
                moveVect = new Vector2(horzMove, 0);

                if (canIMoveInDirection(moveVect))
                {
                    transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);

                    rb.velocity = moveVect * speed;

                    transform.localScale = new Vector2(1, 1);

                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
           
        } else if (Input.GetKey("d"))
        {
           if (localVelocity.x < -0.1) 
           {
                moveVect = new Vector2(horzMove, 0);
                transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);

                rb.velocity = moveVect * speed;

                transform.localScale = new Vector2(-1, 1);

                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {

                moveVect = new Vector2(horzMove, 0);

                if (canIMoveInDirection(moveVect))
                {
                    transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);

                    rb.velocity = moveVect * speed;

                    transform.localScale = new Vector2(-1, 1);

                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            
        } 
        else if (Input.GetKey("w"))
        {
            if (localVelocity.y > 0)
            {
                moveVect = new Vector2(0, vertMove);
                transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);

                rb.velocity = moveVect * speed;

                transform.localScale = new Vector2(1, 1);

                transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
            else
            {

                moveVect = new Vector2(0, vertMove);

                if (canIMoveInDirection(moveVect))
                {
                    transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);

                    rb.velocity = moveVect * speed;

                    transform.localScale = new Vector2(1, 1);

                    transform.localRotation = Quaternion.Euler(0, 0, 270);
                }
            }

        } 
        else if (Input.GetKey("s"))
        {
            if (localVelocity.y < 0)
            {
                moveVect = new Vector2(0, vertMove);
                transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);

                rb.velocity = moveVect * speed;

                transform.localScale = new Vector2(1, 1);

                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                moveVect = new Vector2(0, vertMove);

                if (canIMoveInDirection(moveVect))
                {
                    transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f);

                    rb.velocity = moveVect * speed;

                    transform.localScale = new Vector2(1, 1);

                    transform.localRotation = Quaternion.Euler(0, 0, 90);
                }
            }

        }











        stopPacman();

    }

    private bool canIMoveInDirection(Vector2 dir)
    {
        Vector2 pos = transform.position;

        Transform point = GameObject.Find("GridGB").GetComponent<GameScene>().gBPoints[(int)pos.x, (int)pos.y];

        if(point != null)
        {
            GameObject pointGO = point.gameObject;

            Vector2[] vectToNextPoint = pointGO.GetComponent<TurningPoint>().vectToNextPoint;

            foreach(Vector2 vect in vectToNextPoint)
            {
                if(vect == dir)
                {
                    return true;
                }
            }

        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        bool hitAWall = false;

        if(col.gameObject.tag == "Point")
        {
            Vector2[] vectToNextPoint = col.GetComponent<TurningPoint>().vectToNextPoint;

            if(Array.Exists(vectToNextPoint, element => element == rb.velocity.normalized))
            {
                hitAWall = false;
            } else
            {
                hitAWall = true;
            }

            transform.position = new Vector2((int)col.transform.position.x + 0.5f, (int)col.transform.position.y + 0.5f);

            if (hitAWall)
                rb.velocity = Vector2.zero;
        }

        if(col.gameObject.tag == "Pill")
        {
            pickUpPill(col);
        }

    }

    private void stopPacman()
    {
        if(rb.velocity == Vector2.zero)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = pausedSprite;

            soundManager.pausePacman();
        } else
        {
            GetComponent<Animator>().enabled = true;

            soundManager.unPausePacman();
        }
    }

    public void pickUpPill(Collider2D col)
    {
        addPoint();

        Destroy(col.gameObject);
    }

    public void addPoint()
    {
        Text textUIComp = GameObject.Find("Score").GetComponent<Text>();

        int score = int.Parse(textUIComp.text);

        score += 10;

        textUIComp.text = score.ToString();
    }

}

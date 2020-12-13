using System.Collections;
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

    GameScene gameScene;

    Virus redVirusScript;
    Virus pinkVirusScript;
    Virus blueVirusScript;
    Virus orangeVirusScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        gameScene = FindObjectOfType(typeof(GameScene)) as GameScene;

        GameObject redVirusGO = GameObject.Find("RedVirus");
        GameObject pinkVirusGO = GameObject.Find("PinkVirus");
        GameObject blueVirusGO = GameObject.Find("BlueVirus");
        GameObject orangeVirusGO = GameObject.Find("OrangeVirus");

        redVirusScript = (Virus)redVirusGO.GetComponent(typeof(Virus));
        pinkVirusScript = (Virus)pinkVirusGO.GetComponent(typeof(Virus));
        blueVirusScript = (Virus)blueVirusGO.GetComponent(typeof(Virus));
        orangeVirusScript = (Virus)orangeVirusGO.GetComponent(typeof(Virus));

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
            if (localVelocity.x > 0 && gameScene.IsValidSPace(transform.position.x - 1, transform.position.y))
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
           if (localVelocity.x < -0.1 && gameScene.IsValidSPace(transform.position.x + 1, transform.position.y)) 
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
            if (localVelocity.y > 0 && gameScene.IsValidSPace(transform.position.x, transform.position.y + 1))
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
            if (localVelocity.y < 0 && gameScene.IsValidSPace(transform.position.x, transform.position.y - 1))
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

        Transform point = GameObject.Find("GBGrid").GetComponent<GameScene>().gBPoints[(int)pos.x, (int)pos.y];

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

        if (col.gameObject.tag == "Point")
        {
            Vector2[] vectToNextPoint = col.GetComponent<TurningPoint>().vectToNextPoint;

            if (Array.Exists(vectToNextPoint, element => element == rb.velocity.normalized))
            {
                hitAWall = false;
            }
            else
            {
                hitAWall = true;
            }

            transform.position = new Vector2((int)col.transform.position.x + 0.5f, (int)col.transform.position.y + 0.5f);

            if (hitAWall)
                rb.velocity = Vector2.zero;

        }

        Vector2 pmMoveVect = new Vector2(0, 0);

        if (transform.position.x < 2 && transform.position.y == 15.5)
        {
            transform.position = new Vector2(24.5f, 15.5f);
            pmMoveVect = new Vector2(-1, 0);
            rb.velocity = pmMoveVect * speed;
        }
        else if (transform.position.x > 25 && transform.position.y == 15.5)
        {
            transform.position = new Vector2(2f, 15.5f);
            pmMoveVect = new Vector2(1, 0);
            rb.velocity = pmMoveVect * speed;
        }

        if (col.gameObject.tag == "Pill")
        {
            pickUpPill(col);
        }

        if (col.gameObject.tag == "Syringe")
        {

            SoundManager.Instance.playOnce(SoundManager.Instance.syringeUse);

            redVirusScript.turnVirusBlue();
            pinkVirusScript.turnVirusBlue();
            blueVirusScript.turnVirusBlue();
            orangeVirusScript.turnVirusBlue();

            addPoint(50);

            Destroy(col.gameObject);

        }

        if (col.gameObject.tag == "Virus")
        {
            String virusName = col.GetComponent<Collider2D>().gameObject.name;

            AudioSource audioSource = soundManager.GetComponent<AudioSource>();

            if (virusName == "RedVirus")
            {
                if (redVirusScript.isVirusBlue)
                {
                    redVirusScript.ResetVirusAfterEaten(gameObject);
                    SoundManager.Instance.playOnce(SoundManager.Instance.eatingVirus);
                    addPoint(400);
                } else
                {
                    SoundManager.Instance.playOnce(SoundManager.Instance.pacmanDies);

                    audioSource.Stop();

                    Destroy(gameObject);
                }
            } else if (virusName == "PinkVirus")
            {
                if (pinkVirusScript.isVirusBlue)
                {
                    pinkVirusScript.ResetVirusAfterEaten(gameObject);
                    SoundManager.Instance.playOnce(SoundManager.Instance.eatingVirus);
                    addPoint(400);
                }
                else
                {
                    SoundManager.Instance.playOnce(SoundManager.Instance.pacmanDies);

                    audioSource.Stop();

                    Destroy(gameObject);
                }
            } else if (virusName == "BlueVirus")
            {
                if (blueVirusScript.isVirusBlue)
                {
                    blueVirusScript.ResetVirusAfterEaten(gameObject);
                    SoundManager.Instance.playOnce(SoundManager.Instance.eatingVirus);
                    addPoint(400);
                }
                else
                {
                    SoundManager.Instance.playOnce(SoundManager.Instance.pacmanDies);

                    audioSource.Stop();

                    Destroy(gameObject);
                }
            } else if (virusName == "OrangeVirus")
            {
                if (orangeVirusScript.isVirusBlue)
                {
                    orangeVirusScript.ResetVirusAfterEaten(gameObject);
                    SoundManager.Instance.playOnce(SoundManager.Instance.eatingVirus);
                    addPoint(400);
                }
                else
                {
                    SoundManager.Instance.playOnce(SoundManager.Instance.pacmanDies);

                    audioSource.Stop();

                    Destroy(gameObject);
                }
            }

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
        addPoint(10);

        Destroy(col.gameObject);
    }

    public void addPoint(int points)
    {
        Text textUIComp = GameObject.Find("Score").GetComponent<Text>();

        int score = int.Parse(textUIComp.text);

        score += points;

        textUIComp.text = score.ToString();
    }

}

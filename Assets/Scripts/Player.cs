using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 4f; // prędkość Pacmana
    private Rigidbody2D rb; // do poruszania Pacmanem
    public Sprite pausedSprite; // grafika używana gdy Pacman się zatrzyma

    SoundManager soundManager; // referencja do obiektu odtwarzającego dźwięki
    GameScene gameScene; // referencja do planszy gry

    // referencje do wirusów
    Virus redVirusScript;
    Virus pinkVirusScript;
    Virus blueVirusScript;
    Virus orangeVirusScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // zapisanie referencji do rigidbody pacmana

        gameScene = FindObjectOfType(typeof(GameScene)) as GameScene; // pobranie referencji do planszy gry

        // wczytanie referencji do obiektów wirusów
        GameObject redVirusGO = GameObject.Find("RedVirus");
        GameObject pinkVirusGO = GameObject.Find("PinkVirus");
        GameObject blueVirusGO = GameObject.Find("BlueVirus");
        GameObject orangeVirusGO = GameObject.Find("OrangeVirus");

        // wczytanie referencji do skryptów wirusów
        redVirusScript = (Virus)redVirusGO.GetComponent(typeof(Virus));
        pinkVirusScript = (Virus)pinkVirusGO.GetComponent(typeof(Virus));
        blueVirusScript = (Virus)blueVirusGO.GetComponent(typeof(Virus));
        orangeVirusScript = (Virus)orangeVirusGO.GetComponent(typeof(Virus));
    }

    void Start()
    {
        rb.velocity = new Vector2(-1, 0) * speed; // nadanie początkowej prędkości
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>(); // pobranie referencji do obiektu odtwarzającego dźwięk
    }

    // metoda wywoływana co 0.02 sekundy
    private void FixedUpdate()
    {
        if(Input.GetKey("a")) moveLeft(); // poruszanie się w lewo
        else if (Input.GetKey("d")) moveRight(); // poruszanie się w prawo
        else if (Input.GetKey("w")) moveUp(); // poruszanie się w górę
        else if (Input.GetKey("s")) moveDown(); // poruszanie się w dół

        stopPacman(); // zatrzymanie znimacji i dźwięków Pacmana jeśli jest taka potrzeba
    }

    public void moveLeft()
    {
        float horzMove = Input.GetAxisRaw("Horizontal"); // pobranie wartości od użytkowanika
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity); // pobranie kierunku w jakim porusza się aktualnie Pacman
        Vector2 moveVect = new Vector2(horzMove, 0); // kierunek w którym chce się ruszyć

        // jeśli znajduje się pomiędzy TurningPoint lub znajduje się w TurningPoint i mogę się ruszyć w lewo
        if (localVelocity.x > 0 && gameScene.IsValidSPace(transform.position.x - 1, transform.position.y)
            || canIMoveInDirection(moveVect))
        {
            transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f); // wyśrodkowanie pozycji do centrum kratki
            rb.velocity = moveVect * speed; // nadanie pędu Pacmanowi w lewo
            transform.localScale = new Vector2(1, 1); // odwrócenie Pacmana w lewo
            transform.localRotation = Quaternion.Euler(0, 0, 0); // ustawienie orientacji
        }
    }

    public void moveRight()
    {
        float horzMove = Input.GetAxisRaw("Horizontal"); // pobranie wartości od użytkowanika
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity); // pobranie kierunku w jakim porusza się aktualnie Pacman
        Vector2 moveVect = new Vector2(horzMove, 0); // kierunek w któym chce się ruszyć

        // jeśli znajduje się pomiędzy TurningPoint lub znajduje się w TurningPoint i mogę się ruszyć w lewo
        if (localVelocity.x < -0.1 && gameScene.IsValidSPace(transform.position.x + 1, transform.position.y)
            || canIMoveInDirection(moveVect))
        {
            transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f); // wyśrodkowanie pozycji do centrum kratki
            rb.velocity = moveVect * speed; // nadanie pędu Pacmanowi w prawo
            transform.localScale = new Vector2(-1, 1); // odwrócenie Pacmana w lewo
            transform.localRotation = Quaternion.Euler(0, 0, 0); // ustawienie orientacji
        }
    }

    public void moveUp()
    {
        float vertMove = Input.GetAxisRaw("Vertical"); // pobranie wartości od użytkowanika
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity); // pobranie kierunku w jakim porusza się aktualnie Pacman
        Vector2 moveVect = new Vector2(0, vertMove); // kierunek w którym chce sięruszyć

        // jeśli znajduje się pomiędzy TurningPoint lub znajduje się w TurningPoint i mogę się ruszyć w górę
        if (localVelocity.y > 0 && gameScene.IsValidSPace(transform.position.x, transform.position.y + 1)
            || canIMoveInDirection(moveVect))
        {
            transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f); // wyśrodkowanie pozycji do centrum kratki
            rb.velocity = moveVect * speed; // nadanie pędu Pacmanowi w górę
            transform.localScale = new Vector2(1, 1); // zwracamy Pacmana w lewo
            transform.localRotation = Quaternion.Euler(0, 0, 270); // obracamy go, aby patrzył w górę
        }
    }

    public void moveDown()
    {
        float vertMove = Input.GetAxisRaw("Vertical"); // pobranie wartości od użytkowanika
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity); // pobranie kierunku w jakim porusza się aktualnie Pacman
        Vector2 moveVect = new Vector2(0, vertMove); // kierunek w którym chce się poruszyć

        // jeśli znajduje się pomiędzy TurningPoint lub znajduje się w TurningPoint i mogę się ruszyć w dół
        if (localVelocity.y < 0 && gameScene.IsValidSPace(transform.position.x, transform.position.y - 1)
            || canIMoveInDirection(moveVect))
        {
            transform.position = new Vector2((int)transform.position.x + 0.5f, (int)transform.position.y + 0.5f); // wyśrodkowanie pozycji do centrum kratki
            rb.velocity = moveVect * speed; // nadanie pędu Pacmanowi w dół
            transform.localScale = new Vector2(1, 1); // zwracamy Pacmana w lewo
            transform.localRotation = Quaternion.Euler(0, 0, 90); // obracamy go, aby patrzył w dół
        }
    }

    private bool canIMoveInDirection(Vector2 dir)
    {
        // pozycja Pacmana
        Vector2 pos = transform.position; 
        // pobranie współrzędnych Obiektu TurningPoint o takich samych współrzędnych co Pacman
        Transform point = GameObject.Find("GBGrid").GetComponent<GameScene>().gBPoints[(int)pos.x, (int)pos.y];

        // jeśli pobrano współrzędne
        if (point != null) 
        {
            // pobranie Obiektu TurningPoint
            GameObject pointGO = point.gameObject;
            // pobranie pobliskich obiektów TurningPoint
            Vector2[] vectToNextPoint = pointGO.GetComponent<TurningPoint>().vectToNextPoint; 

            // sprawdzamy, czy możemy poruszyć się w żądanym kierunku
            foreach (Vector2 vect in vectToNextPoint) if (vect == dir) return true;
        }

        return false; // jeśli nie mżemy poruszyć się w żądanym kierunku
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // jeśli Pacman spotka TurningPoint
        if (col.gameObject.tag == "Point")
        {
            // pobranie tablicy pobliskich punktów
            Vector2[] vectToNextPoint = col.GetComponent<TurningPoint>().vectToNextPoint;

            // sprawdzamy czy w pobranej tablicy nie ma punkt w kierunku w którym porusza się Pacman
            if (!Array.Exists(vectToNextPoint, element => element == rb.velocity.normalized))
            {
                // zatrzymanie Pacmana
                rb.velocity = Vector2.zero;
                // wyśrodkowanie Pacmana do centrum kratki
                transform.position = new Vector2((int)col.transform.position.x + 0.5f, (int)col.transform.position.y + 0.5f);
            }
        }

        // przechodzenie pacmana przez portale
        if (transform.position.x < 2 && transform.position.y == 15.5)
        {
            transform.position = new Vector2(24.5f, 15.5f); // nowa pozycja
            Vector2 pmMoveVect = new Vector2(-1, 0); // nowy wektor ruchu
            rb.velocity = pmMoveVect * speed; // nadane pacmanowi pędu zgodnie z wektorem i prędkością Pacmana
        }
        else if (transform.position.x > 25 && transform.position.y == 15.5)
        {
            transform.position = new Vector2(2f, 15.5f);
            Vector2 pmMoveVect = new Vector2(1, 0);
            rb.velocity = pmMoveVect * speed;
        }

        // jeśli Pacman spotka pigułkę
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
        // jeśli Pacman się nie porusza
        if(rb.velocity == Vector2.zero)
        {
            GetComponent<Animator>().enabled = false; // zatrzymanie animacji
            GetComponent<SpriteRenderer>().sprite = pausedSprite; // ustawienie grafiki Pacmana
            soundManager.pausePacman(); // zatrzynanie dźwięków jedzenie tabletek
        } 
        // jeśli Pacman się porusza
        else
        {
            GetComponent<Animator>().enabled = true; // uruchomienie animacji
            soundManager.unPausePacman(); // włączenie dźwięków Pacmana
        }
    }

    // metoda uruchamiana gdy Pacman spotka pigułkę
    public void pickUpPill(Collider2D col)
    {
        addPoint(10); // dodanie 10. punktów
        Destroy(col.gameObject); // zniszczenie pigłki
    }

    // metoda zwiększająca liczbę punktów o podaną wartość
    public void addPoint(int points)
    {
        Text textUIComp = GameObject.Find("Score").GetComponent<Text>(); // pobranie tekstu z punktami
        int score = int.Parse(textUIComp.text); // zamiana teksu na liczbę
        score += points; // zwiększenie liczby punków
        textUIComp.text = score.ToString(); // wyświetlenie nowej liczby
    }

}

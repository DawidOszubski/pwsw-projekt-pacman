using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{
    public int timeToDisapear; // liczba określająca czas do zniknięcia strzykawki
    private PointsCounter pc; // referencja do licznika punktów

    // referencje do wirusów
    Virus redVirusScript;
    Virus pinkVirusScript;
    Virus blueVirusScript;
    Virus orangeVirusScript;

    // Start is called before the first frame update
    void Start()
    {
        // referencja do licznika punktów
        pc = GameObject.Find("PointsCounter").GetComponent<PointsCounter>();

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

        Invoke("destroySyringe", timeToDisapear); // po timeToDisapear sekundach strzykawka zniknie
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pickUpSyringe()
    {
        // dźwięk użycia strzykawki
        SoundManager.Instance.playOnce(SoundManager.Instance.syringeUse);

        // zamiana wszystkich wirusów w niebieskie uciekające wirusy
        redVirusScript.turnVirusBlue();
        pinkVirusScript.turnVirusBlue();
        blueVirusScript.turnVirusBlue();
        orangeVirusScript.turnVirusBlue();

        // dodanie 50 punktów graczowi
        pc.addPoint(50);

        // usunięcie strzykawki z planszy
        Destroy(gameObject); // zniszcz strzykawkę
    }

    private void destroySyringe()
    {
        Destroy(gameObject); // zniczsz strzykawkę
    }
}

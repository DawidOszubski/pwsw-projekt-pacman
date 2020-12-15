using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour
{
    private PointsCounter pc;

    // metoda do inicjalizacji zmiennych
    void Start()
    {
        // referencja do licznika punktów
        pc = GameObject.Find("PointsCounter").GetComponent<PointsCounter>();
    }

    public void pickUpPill()
    {
        pc.addPoint(10); // dodajemy 10 punktów
        Destroy(gameObject); // zniszcz pigułkę
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningPoint : MonoBehaviour
{
    public TurningPoint[] nextPoints;
    public Vector2[] vectToNextPoint;

    // funkcja używana do inicjalizacji
    void Start()
    {
        // tablica przechowująca vektory do pobliskich TurningPoint
        vectToNextPoint = new Vector2[nextPoints.Length];

        for(int i = 0; i < nextPoints.Length; i++)
        {
            // pobranie TurningPoint z tablicy
            TurningPoint nextPoint = nextPoints[i];
            // wyliczenie wektora z (TurningPoint) this do (TurningPoint) nextPoints[i]
            Vector2 pointVect = nextPoint.transform.localPosition - transform.localPosition; 
            // normalizacja i zapisanie wyliczonego wektora
            vectToNextPoint[i] = pointVect.normalized;
        }

    }
}

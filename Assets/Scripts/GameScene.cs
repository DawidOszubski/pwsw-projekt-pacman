using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameScene : MonoBehaviour
{
	// tablica przechowująca wszystkie miejsca w których można zmieniać kierunki (TurningPoints)
    public Transform[,] gBPoints = new Transform[27, 30];
	// tablica przechowująca punkty po których Pacman może się poruszać
    public bool[,] validBlock = new bool[28, 31];

    // funkcja używana do inicjalizacji
    void Start()
    {
		// zapisanie referencji do obiektu zawierającego wszystkie obiekty TurningPoint
        GameObject turningPoints = GameObject.Find("TurningPoints");

		// pętla przetważające wszystkie obiekty TurningPoint znajdujące się w obiekci TurningPoints
        foreach(Transform point in turningPoints.transform)
        {
            Vector2 pos = point.position; // pbranie pozycji obiektu TurningPoint
            gBPoints[(int)pos.x, (int)pos.y] = point; // zapisanie pozycji w tablicy
        }

		// ręczne dodanie wszystkich punktów po których Pacman może się poruszać do tablicy validBlock
		AddYRowXRange(1, 1, 26);
		AddXColYRange(1, 1, 4);
		AddXColYRange(12, 1, 4);
		AddXColYRange(15, 1, 4);
		AddXColYRange(26, 1, 4);
		AddYRowXRange(4, 1, 6);
		AddYRowXRange(4, 9, 12);
		AddYRowXRange(4, 15, 18);
		AddYRowXRange(4, 21, 26);

		AddXColYRange(3, 4, 7);
		AddXColYRange(6, 4, 29);
		AddXColYRange(9, 4, 7);
		AddXColYRange(18, 4, 7);
		AddXColYRange(21, 4, 29);
		AddXColYRange(24, 4, 7);

		AddYRowXRange(7, 1, 3);
		AddYRowXRange(7, 6, 21);
		AddYRowXRange(7, 24, 26);

		AddXColYRange(1, 7, 10);
		AddXColYRange(12, 7, 10);
		AddXColYRange(15, 7, 10);
		AddXColYRange(26, 7, 10);

		AddXColYRange(9, 10, 19);
		AddXColYRange(18, 10, 19);

		AddYRowXRange(13, 9, 18);

		AddYRowXRange(16, 0, 9);
		AddYRowXRange(16, 18, 27);

		AddYRowXRange(19, 9, 18);

		AddXColYRange(12, 19, 22);
		AddXColYRange(15, 19, 22);

		AddYRowXRange(22, 1, 6);
		AddYRowXRange(22, 9, 12);
		AddYRowXRange(22, 15, 18);
		AddYRowXRange(22, 21, 26);

		AddXColYRange(1, 22, 29);
		AddXColYRange(9, 22, 25);
		AddXColYRange(18, 22, 25);
		AddXColYRange(26, 22, 29);

		AddYRowXRange(25, 1, 26);

		AddXColYRange(12, 25, 29);
		AddXColYRange(15, 25, 29);

		AddYRowXRange(29, 1, 12);
		AddYRowXRange(29, 15, 26);
	}

	// metoda dodająca rząd współrzędnych po których może się poruszać Pacman
    void AddYRowXRange(int yRow, int xStart, int xEnd)
    {
        for(int i = xStart; i <= xEnd; i++)
        {
            validBlock[i, yRow] = true;
        }
    }

	// metoda dodająca kolumne współrzędnych po których może się poruszać Pacman
    void AddXColYRange(int xCol, int yStart, int yEnd)
    {
        for (int i = yStart; i <= yEnd; i++)
        {
            validBlock[xCol, i] = true;
        }
    }

	// publiczna metoda sprawdzająca czy Pacman może stanąć na dane pole
    public bool IsValidSPace(float x, float y)
    {
        x = (float)Math.Floor(Convert.ToDouble(x));
        y = (float)Math.Floor(Convert.ToDouble(y));

        if(validBlock[(int)x + 1, (int)y + 1])
        {
            return true;
        } else
        {
            return false;
        }
    }
}

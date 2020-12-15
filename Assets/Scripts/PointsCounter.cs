using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsCounter : MonoBehaviour
{
    private int levelCounter = 0;
    private int pointsInThisLevel = 0;
    private int allPointsCounter = 0;
    private Text textUIComp;

    // metoda do inicjalizacji
    void Start()
    {
        textUIComp = GameObject.Find("Score").GetComponent<Text>(); // pobranie referencji do tekstu z wynikiem
    }

    public void addPoint(int num)
    {
        pointsInThisLevel += num;
        textUIComp.text = pointsInThisLevel.ToString();
    }

    public void endLevel()
    {
        allPointsCounter += pointsInThisLevel;
        pointsInThisLevel = 0;
        textUIComp.text = "0";
        levelCounter++;
    }

    public int getLevelCounter()
    {
        return levelCounter;
    }

    public int getPointsInThisLevel()
    {
        return pointsInThisLevel;
    }

    public int getAllPointsCounter()
    {
        return allPointsCounter;
    }
}

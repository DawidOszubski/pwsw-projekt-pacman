using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public Transform[,] gBPoints = new Transform[27, 30];
    private GameObject turningPoints;

    // Use this for initializatino
    void Update()
    {

        turningPoints = GameObject.Find("TurningPoints");

        foreach(Transform point in turningPoints.transform)
        {
            Vector2 pos = point.position;
            gBPoints[(int)pos.x, (int)pos.y] = point;
        }

    }
}

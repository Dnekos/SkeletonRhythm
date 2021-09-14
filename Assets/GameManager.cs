using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int score = 0; // static so its easier for beat dots to see it


    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<Text>().text = "Score = " + score;
    }
}

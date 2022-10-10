using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class HighScore : MonoBehaviour
{
    public Text CurrentScore;

    public Text bScore;

    void Start()
    {
        //List of scores
        string path = Application.dataPath + "/HSs.txt";

        List<string> fileLine = File.ReadAllLines(path).ToList();

        fileLine.Sort();

        //Current score
        string path1 = Application.dataPath + "/HS.txt";

        string fileLine1 = File.ReadAllText(path1);

        //Showing scores
        CurrentScore.text = fileLine1;

        bScore.text = fileLine.First();



    }
}

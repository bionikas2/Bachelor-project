using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float startTime;
    private bool finished = false;
    public string finishedTime = "";

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(finished)
        {
        Debug.Log(finishedTime);
        return;
        }

        else{
        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString();

        string seconds = (t % 60).ToString("00.00");

        timerText.text = minutes + ":" + seconds;

        finishedTime = timerText.text;
        }
    }

    public void Finish()
    {
        finished = true;

        string paths = Application.dataPath + "/HSs.txt";

        string path = Application.dataPath + "/HS.txt";

        File.AppendAllText(paths, finishedTime + "\n");

        File.WriteAllText(path, finishedTime);

    }

}

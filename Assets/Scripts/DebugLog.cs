//This script was taken from Stackoverflow answer #1: https://stackoverflow.com/questions/67704820/how-do-i-print-unitys-debug-log-to-the-screen-gui
//and modiefied slightly


using UnityEngine;
using System.Collections;

public class ZzzLog : MonoBehaviour
{
    uint qsize = 12;  // number of messages to keep
    Queue myLogQueue = new Queue();

    void Start() {
        DontDestroyOnLoad(this.gameObject); //Keep text persistent
        

    }

    void OnEnable() {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable() {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type) {
        if (type==LogType.Log){
            myLogQueue.Enqueue(logString);
            }
        if (type == LogType.Exception)
            myLogQueue.Enqueue(stackTrace);
        while (myLogQueue.Count > qsize)
            myLogQueue.Dequeue();
    }

    void OnGUI() {
        GUILayout.BeginArea(new Rect(17, Screen.height-225 , 400, 400));
        GUILayout.Label("\n" + string.Join("\n", myLogQueue.ToArray()));
        GUILayout.EndArea();
    }
}
using UnityEngine;
using System.Collections;

public class log : MonoBehaviour {
    public UnityEngine.UI.InputField text;

    void Start()
    {
        Application.logMessageReceived += Application_logMessageReceived;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        text.text += condition;
    }
}

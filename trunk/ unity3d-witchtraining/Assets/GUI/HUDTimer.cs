using UnityEngine;
using System.Collections;

public class HUDTimer : MonoBehaviour
{
    
    private float timerwidth = 115.0f;
    private float timerheight = 100.0f;

    public GUISkin timerSkin;

    private string timerValue = "0:00:00";
    
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("TimerUpdate", 1.0f, 0.13f);

        timerSkin = (GUISkin)Resources.Load("TimerGUISkin");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TimerUpdate()
    {
        float minutes = Mathf.FloorToInt(Time.time / 60);
        float seconds = Time.time % 60;
        timerValue = string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }

    void OnGUI()
    {
        GUI.skin = timerSkin;


        float screenX = (Screen.width * 0.5f) - (timerwidth *0.5f);
        float screenY = (Screen.height) - (timerheight *0.5f) - 15;

        
        //GUI area for time
        GUILayout.BeginArea(new Rect(screenX, screenY, timerwidth, timerheight));

        GUILayout.TextField(timerValue);

        GUILayout.EndArea();
        
    }
}

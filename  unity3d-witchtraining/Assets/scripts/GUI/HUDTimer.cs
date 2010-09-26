using UnityEngine;
using System.Collections;

public class HUDTimer : MonoBehaviour
{
    
    public float timerwidth = 100.0f;
    public float timerheight = 100.0f;


    private string timerValue = "0:00:00";
    
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("TimerUpdate", 1.0f, 0.13f);
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
       
        float screenX = (Screen.width * 0.5f) - (timerwidth *0.5f);
        float screenY = (Screen.height) - (timerheight *0.5f);

        
        //GUI area for time
        GUILayout.BeginArea(new Rect(screenX, screenY, timerwidth, timerheight));

        GUILayout.TextField(timerValue);

        GUILayout.EndArea();
        
    }
}

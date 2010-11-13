using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour
{

    public GUISkin pauseSkin;
    private Texture2D pauseBackground;


    // Use this for initialization
    void Start()
    {
         pauseSkin = (GUISkin)Resources.Load("TimerGUISkin");
         pauseBackground = (Texture2D)Resources.Load("grey-background");
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnGUI()
    {
        GUI.skin = pauseSkin;

        float textWidth = 100;
        float textHeight = 40;
        float textPosX = Screen.width * 0.5f -  (textWidth * 0.5f);
        float textPosY = Screen.height * 0.5f-  (textHeight * 0.5f);

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), pauseBackground);

        GUI.TextField(new Rect(textPosX, textPosY, textWidth, textHeight), "Paused"); 
       
    }
}

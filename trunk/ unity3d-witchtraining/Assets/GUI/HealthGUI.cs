using UnityEngine;
using System.Collections;

public class HealthGUI : MonoBehaviour
{
    
    Texture2D healthBG;
    PlayerManager playerManager;
    GUISkin healthSkin;

    // Use this for initialization
    void Start()
    {
        healthBG = (Texture2D)Resources.Load("hud-playerHealth");

        healthSkin = (GUISkin)Resources.Load("TimerGUISkin");

        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {

        GUI.skin = healthSkin;

        float screenYBG = Screen.height - 200;
        float playerHealth = playerManager.Health();


        //GUI area for time
        GUILayout.BeginArea(new Rect(10, screenYBG, healthBG.width, 240));

        GUI.Label(new Rect(0, 50, healthBG.width, 200), healthBG);
        GUI.Label(new Rect(150, 82, 200, 200), playerHealth.ToString());

        GUILayout.EndArea();


    }

}

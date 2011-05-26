using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour
{

    public GUISkin titleSkin;
    private Texture2D titleGraphic;
	
	
	private GameManager gameManager;
	
    // Use this for initialization
    void Start()
    {
         titleSkin = (GUISkin)Resources.Load("TimerGUISkin");
         titleGraphic = (Texture2D)Resources.Load("Title-Logo");

         gameManager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnGUI()
    {
        GUI.skin = titleSkin;

		float titleMenuW = 350.0f;
		float titleMenuH = Screen.height;
		
		
		
        //GUI area for main menu buttons
		GUILayout.BeginArea(new Rect(50, 20, titleMenuW, titleMenuH));
		
		GUI.Label(new Rect (0,50,220,134), titleGraphic);
		
		if (GUI.Button (new Rect (10,210,180,50), "Level Select")) {
				Application.LoadLevel ("level-select");
				gameManager.SetState( GameManager.GameState.LevelSelect);
				Destroy(this);
			}

        

        GUILayout.EndArea(); 
       
    }
}

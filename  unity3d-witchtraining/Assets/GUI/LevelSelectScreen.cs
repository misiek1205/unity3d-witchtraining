using UnityEngine;
using System.Collections;

public class LevelSelectScreen : MonoBehaviour
{

    //public GUISkin titleSkin;
    private GameManager gameManager;
	private GUIManager guiManager;

    // Use this for initialization
    void Awake() 
	{
		gameManager = GetComponent<GameManager>();
		guiManager = GetComponent<GUIManager>();
	}
	
	void Start()
    {
		Debug.Log(guiManager);
    }

    void Update() {
    }

    void OnGUI()
    {
        GUI.skin = guiManager.guiSkin;


        float titleMenuW = 350.0f;
        float titleMenuH = Screen.height;

        GUILayout.BeginArea(new Rect(50, 20, titleMenuW, titleMenuH));

        if (GUI.Button(new Rect(10, 210, 180, 50), "Challenge 1"))
        {
            Application.LoadLevel("level");
            gameManager.SetState(GameManager.GameState.InGame);
            Destroy(this);
        }



        GUILayout.EndArea(); 

    }
}

using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour
{

    public enum GUIState {
        TitleScreen,
        Menu,
        LevelWin,
        LevelSelect,
        GameOver,
        Credits,
        ChallengeInstructions,
        InGame
    };

    private GameManager gameManager;

    //local variable that represents states
    private GUIState _state;

    public GUISkin guiSkin;


    public Texture titleTexture;

    // Use this for initialization
    void Start()
    {
        //for now start as in game until other GUI screens can be done
        _state = GUIState.InGame;
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        
    }


    void Update()
    {

        //Pause button logic
        //adds pause button component if paused, otherwise will remove it
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (gameManager.IsPaused() == true)
                gameManager.IsPaused(false);
            else
                gameManager.IsPaused(true);
        }

        if (gameManager.IsPaused())
        {
            if (!GetComponent<PauseScreen>())
            {
                this.gameObject.AddComponent<PauseScreen>();
                PauseFade("pause");
            }
        }
        else
        {
            Destroy(this.gameObject.GetComponent<PauseScreen>());

            //fading in/out pause
            PauseFade("unpause");
        }

    }

    void OnGUI()
    {
        //Title GUI Screen

        switch(_state)
        {
            //if Title Screen
            case GUIState.TitleScreen:

            float _titleHeight = 300.0f;
            float _titleWidth = 300.0f;

            float screenX = (Screen.width * 0.5f) - (_titleWidth * 0.5f);
            float screenY = (Screen.height * 0.5f) - (_titleHeight * 0.5f);

            //GUILayout.Box(titleTexture, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height), GUILayout.); 

            GUILayout.BeginArea(new Rect(screenX, screenY, _titleWidth, _titleHeight));
                GUILayout.TextField(_titleHeight.ToString());

            GUILayout.EndArea();




                break;
            
            //if InGame GUI Screen
            case GUIState.InGame:

                
            //create HUDTimer Object if it doesn't exist             
            if (!GetComponent<HUDTimer>())
                {
                    this.gameObject.AddComponent<HUDTimer>();
                   // _hudTimer = GetComponent<HUDTimer>();
                }
                


                //do level select GUI things here
               
                break;

         }        
      

    } //end ONGUI()

   

    private void PauseFade(string paused)
        {
            if (paused == "pause")
            {
                //Time.timeScale -= Time.deltaTime;
                Time.timeScale = 0;
            }
            else
            {
               // Time.timeScale += Time.deltaTime;
                Time.timeScale = 1;
            }
        }

}

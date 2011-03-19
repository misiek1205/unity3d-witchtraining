using UnityEngine;
using System.Collections;


/* GUIManager class -
 * watches out for the different states that the game might be in. The enum structure stores all of the different states
 * 
 
 * 
 * */


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
    public GUIState _state;

    public GUISkin guiSkin;


    public Texture titleTexture;

    // Use this for initialization
    void Start()
    {
        //for now start as in game until other GUI screens can be done
        _state = GUIState.TitleScreen;
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        DontDestroyOnLoad(this);

    }


    void Update()
    {


        if (gameManager.IsPaused())
        {
            if (!GetComponent<PauseScreen>())
            {
                this.gameObject.AddComponent<PauseScreen>();
                Time.timeScale = 0;
            }
        }
        else
        {
            Destroy(this.gameObject.GetComponent<PauseScreen>());

            //fading in/out pause
            Time.timeScale = 1;
        }

    }

    void OnGUI()
    {
        //Title GUI Screen

        switch(_state)
        {
            //if Title Screen
            case GUIState.TitleScreen:

		if (!GetComponent<TitleScreen>()) {
			this.gameObject.AddComponent<TitleScreen>();
			}
			
                break;
            
            //if InGame GUI Screen
            case GUIState.InGame:
                
            //create HUDTimer Object if it doesn't exist             
            if (!GetComponent<HUDTimer>()) {
                    this.gameObject.AddComponent<HUDTimer>();
            }
			
					//Pause button logic
        //adds pause button component if paused, otherwise will remove it
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (gameManager.IsPaused() == true)
                gameManager.IsPaused(false);
            else
                gameManager.IsPaused(true);
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
                
            }
            else
            {
               // Time.timeScale += Time.deltaTime;
                
            }
        }

}

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        TitleScreen,
        Menu,
        LevelWin,
        LevelSelect,
        GameOver,
        Credits,
        ChallengeInstructions,
        InGame
    };



    //local variable that represents states
    private GameState _state;
    private bool _isPaused = false;    


    void Start()
    {
        _state = GameState.TitleScreen;
    }

    public void SetState(GameState state) { _state = state; }
    public GameState State() { return _state; }
   
    public bool IsPaused()    {  return _isPaused;     }
    public void IsPaused(bool paused)  {  _isPaused = paused;   }

}

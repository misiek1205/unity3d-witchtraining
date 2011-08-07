using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private int _currentLevel;
    private ArrayList[] _levelsCompleted;
    


    // Use this for initialization
    void Start()
    {
        _currentLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //getter/setter for current level
    public int CurrentLevel() { return _currentLevel; }
    public void SetCurrentLevel(int level) { _currentLevel = level; }

}

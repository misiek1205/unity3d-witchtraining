using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    private bool _isPaused = false;

   
    public bool IsPaused()
    {
        return _isPaused;
    }

    public void IsPaused(bool paused)
    {
        _isPaused = paused;
    }

}

using UnityEngine;
using System.Collections;

public class PlayerBombThrowing : MonoBehaviour
{
    public GameObject bombPrefab;
    private PlayerManager _player;
 
    //stores start time for bomb throwing
    private float bombPowerTimer = 0.0f;
    //stores length of time in seconds that bomb button was held
    private float bombTimerLength;
    private bool canThrowBomb = true;
    private float bombOffsetTime = 1.0f;

    // Use this for initialization
    void Start()
    {
        _player = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Bomb") && canThrowBomb)
        {
            //start adding up power of throw
            bombPowerTimer = Time.time;
        }


        if (Input.GetButtonUp("Bomb") && canThrowBomb)
        {

     

            Debug.Log("asdf");

            //calculate power of bomb
            bombTimerLength = Time.time - bombPowerTimer;

            //instantiate and throw the bomb
            GameObject bomb = (GameObject)Instantiate(bombPrefab, transform.position, Quaternion.LookRotation(_player.MoveDirection()));
            bomb.rigidbody.velocity = _player.MoveDirection() * 50.0f * bombTimerLength;

            canThrowBomb = false;
        }


        if (Time.time - bombPowerTimer > bombOffsetTime)
        {
            canThrowBomb = true;
        }


        
    }
}

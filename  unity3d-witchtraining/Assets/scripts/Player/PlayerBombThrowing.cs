using UnityEngine;
using System.Collections;

public class PlayerBombThrowing : MonoBehaviour
{
    public GameObject bombPrefab;
    private PlayerManager _player;
    public Transform animationGameObject;
    public Transform rightHand;
    
    //used for mixing transform for throwing animations
    public Transform torso;
    

    //stores start time for bomb throwing
    private float bombPowerTimer = 0.0f;
    //stores length of time in seconds that bomb button was held
    private float bombTimerLength;
    private bool canThrowBomb = true;
    private float bombOffsetTime = 1.0f;

    // Use this for initialization
    void Awake()
    {
        _player = GetComponent<PlayerManager>();

        animationGameObject.animation["throwWind"].layer = 9;
        animationGameObject.animation["throwWind"].wrapMode = WrapMode.ClampForever;

        animationGameObject.animation["throwRelease"].layer = 9;
        animationGameObject.animation["throwRelease"].wrapMode = WrapMode.Once;

        animationGameObject.animation["throwRelease"].AddMixingTransform(torso);
        animationGameObject.animation["throwWind"].AddMixingTransform(torso);

        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Bomb") && canThrowBomb)
        {
            //start adding up power of throw
            bombPowerTimer = Time.time;

            animationGameObject.animation.Blend("throwWind", 1.0f, 0.2f);
            GameObject bomb = (GameObject)Instantiate(bombPrefab, rightHand.transform.position, Quaternion.LookRotation(_player.MoveDirection()));
            bomb.rigidbody.useGravity = false;
            bomb.rigidbody.velocity = Vector3.zero;
            bomb.rigidbody.position = rightHand.transform.position;
            bomb.gameObject.transform.parent = rightHand.transform;

        }
        
        if (Input.GetButtonUp("Bomb") && canThrowBomb)
        {

            animationGameObject.animation.Play("throwRelease");

            //Debug.Log("asdf");

            //calculate power of bomb
            bombTimerLength = Time.time - bombPowerTimer;

            //instantiate and throw the bomb
            GameObject bomb = (GameObject)Instantiate(bombPrefab, rightHand.transform.position, Quaternion.LookRotation(_player.MoveDirection()));
            bomb.rigidbody.velocity = _player.MoveDirection() * 50.0f * bombTimerLength;

            canThrowBomb = false;
        }


        if (Time.time - bombPowerTimer > bombOffsetTime)
        {
            canThrowBomb = true;
        }


        
    }
}

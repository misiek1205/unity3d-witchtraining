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

    //temporary bomb object while holding it
    private GameObject bombTemp;
    private bool isHoldingBomb = false;

    // Use this for initialization
    void Awake()
    {
        _player = GetComponent<PlayerManager>();

        animationGameObject.animation["throwWind"].layer = 11;
        animationGameObject.animation["throwWind"].wrapMode = WrapMode.ClampForever;

        animationGameObject.animation["throwRelease"].layer = 11;
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
            bombTemp = (GameObject)Instantiate(bombPrefab, transform.position, Quaternion.LookRotation(_player.MoveDirection()));

            bombTemp.gameObject.transform.parent = rightHand.transform;
            bombTemp.gameObject.transform.position = rightHand.transform.position;


            //take off all gravity
            bombTemp.gameObject.rigidbody.useGravity = false;
            bombTemp.gameObject.rigidbody.velocity = Vector3.zero;
            bombTemp.gameObject.rigidbody.rotation = Quaternion.identity;

            //bool is for making sure the bomb's position doesn't veer when moving around
            // will allow KeepBombPosition to function
            isHoldingBomb = true;

        }
        
        if (Input.GetButtonUp("Bomb") && canThrowBomb)
        {
            //delete bomb object that is being held
            Destroy(bombTemp);
            isHoldingBomb = false;

            animationGameObject.animation.Play("throwRelease");

            //Debug.Log("asdf");

            //calculate power of bomb
            bombTimerLength = Time.time - bombPowerTimer;

            //instantiate and throw the bomb
            GameObject bomb = (GameObject)Instantiate(bombPrefab, rightHand.transform.position, Quaternion.LookRotation(_player.MoveDirection()));

            //make sure the strongest it can get is only after 3 seconds and no more
            if (bombTimerLength > 3)
            {
                bomb.rigidbody.velocity = _player.MoveDirection() * 50.0f * 3;
            }
            else
            {
                bomb.rigidbody.velocity = _player.MoveDirection() * 50.0f * bombTimerLength;
            }

            canThrowBomb = false;
        }


        if (Time.time - bombPowerTimer > bombOffsetTime)
        {
            canThrowBomb = true;
        }

        if (isHoldingBomb)
        {
            KeepBombPosition();
        }
        
    }


    private void KeepBombPosition()
    {
        bombTemp.gameObject.transform.position = rightHand.transform.position;
    }
}

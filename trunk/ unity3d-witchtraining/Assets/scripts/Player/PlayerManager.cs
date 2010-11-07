using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{

    private ThirdPersonController thirdPersonController;
    private ThirdPersonFlyingController thirdPersonFlyingController;

    private Vector3 _moveDirection;
    private bool isFlying;

    // Use this for initialization
    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        thirdPersonFlyingController = GetComponent<ThirdPersonFlyingController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        isFlying = thirdPersonFlyingController.IsFlying();


        if (isFlying)
            _moveDirection = GetComponent<ThirdPersonFlyingController>().MoveDirection();        
        else 
           _moveDirection = GetComponent<ThirdPersonController>().MoveDirection();

      
    }


    public Vector3 MoveDirection()
    {
        return _moveDirection;
    }
}

using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{

    private ThirdPersonController thirdPersonController;
    private ThirdPersonFlyingController thirdPersonFlyingController;
	private MouseOrbit mouseOrbit;
	private PlayerLookTarget playerLookTarget;
	private HeadLookController headLookController;
	
    private Vector3 _moveDirection;
	private float _moveSpeed;
    private bool _isFlying;
	private Vector3 _lookDirection;
	private float _inAirVelocity;
	
    // Use this for initialization
    void Start()
    {
		thirdPersonController = GetComponent<ThirdPersonController>();
        thirdPersonFlyingController = GetComponent<ThirdPersonFlyingController>();
		
		mouseOrbit = GameObject.Find("LookTarget").GetComponent<MouseOrbit>();
		playerLookTarget = GetComponent<PlayerLookTarget>();
		headLookController = GetComponent<HeadLookController>();
				
		
		_lookDirection = thirdPersonController.MoveDirection();
		_inAirVelocity = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        _isFlying = thirdPersonFlyingController.IsFlying();

			
				
        if (_isFlying)
		{
            _moveDirection = thirdPersonFlyingController.MoveDirection(); 
						
			mouseOrbit.enabled = false;	
			//playerLookTarget.enabled = false;
			headLookController.enabled = false;
		}
        else
		{
            _moveDirection = thirdPersonController.MoveDirection();
			mouseOrbit.enabled = true;
			//playerLookTarget.enabled = true;
			headLookController.enabled = true;
		}


      
    }


    public Vector3 MoveDirection()
    {
        return _moveDirection;
    }
	public Vector3 LookDirection()
	{
	return _lookDirection;	
	}
	public void LookDirection(Vector3 look)
	{
		_lookDirection = look;			
	}
	
	public void InAirVelocity(float vel){_inAirVelocity = vel;	}	
	public float InAirVelocity(){	return _inAirVelocity;	}
	
	public void MoveSpeed(float speed){_moveSpeed = speed;	}	
	public float MoveSpeed(){	return _moveSpeed;	}
	
	public bool IsFlying()	{	return _isFlying;	}
	
}

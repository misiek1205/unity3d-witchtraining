using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{

    private ThirdPersonController thirdPersonController;
    private ThirdPersonFlyingController thirdPersonFlyingController;
	private MouseOrbit mouseOrbit;
	//private PlayerLookTarget playerLookTarget;
	private HeadLookController headLookController;
	
	//stores numbers of cat items. Some challeneges need multiple items collected to win.
	private int _catsCollected;
	
    private Vector3 _moveDirection;
	private float _moveSpeed;
    private bool _isFlying;
	private Vector3 _lookDirection;
	private float _inAirVelocity;

    private short _health;
    private MouseFlying mouseAimFlying;
	
    // Use this for initialization
    void Start()
    {
		thirdPersonController = GetComponent<ThirdPersonController>();
        thirdPersonFlyingController = GetComponent<ThirdPersonFlyingController>();
		
		mouseOrbit = GameObject.Find("LookTarget").GetComponent<MouseOrbit>();
        mouseAimFlying = GameObject.Find("LookTarget").GetComponent<MouseFlying>();

		//playerLookTarget = GetComponent<PlayerLookTarget>();
		headLookController = GetComponent<HeadLookController>();
				
		
		_lookDirection = thirdPersonController.MoveDirection();
		_inAirVelocity = 0.0f;

        _health = 100;
		
		_catsCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        _isFlying = thirdPersonFlyingController.IsFlying();

			
				
        if (_isFlying)
		{
            mouseOrbit.enabled = false;
            mouseAimFlying.enabled = true;
			//playerLookTarget.enabled = false;
			headLookController.enabled = false;
		}
        else
		{
          	mouseOrbit.enabled = true;
            mouseAimFlying.enabled = false;
			//playerLookTarget.enabled = true;
			headLookController.enabled = true;
		}

        _moveDirection = thirdPersonFlyingController.MoveDirection(); 
      
    }


    public Vector3 MoveDirection()    {
        return _moveDirection;
    }
	
	public Vector3 LookDirection()	{
	return _lookDirection;	
	}
	
	public void LookDirection(Vector3 look)	{
		_lookDirection = look;			
	}
	
	public void InAirVelocity(float vel){
		_inAirVelocity = vel;	
	}	
	public float InAirVelocity(){	
		return _inAirVelocity;	
	}
	
	public void MoveSpeed(float speed){
		_moveSpeed = speed;	
	}	
	
	public float MoveSpeed(){	
		return _moveSpeed;	
	}
	
	public bool IsFlying()	{	
		return _isFlying;	
	}

    public void Health(short health) {
        if (health > 100)
           _health = 100;
        else
           _health = health; 
    
    }
    
	public float Health() { 
		return _health; 
	}
	
	public int CatsCollected() { 
		return _catsCollected; 
	}
	
	public void CatsCollected(int collected) { 
		_catsCollected = collected;; 
	}
}

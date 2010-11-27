using UnityEngine;
using System.Collections;



public class ThirdPersonFlyingController : MonoBehaviour
{


    public float flyingSpeed = 15.0f;
    public float flyingBackSpeed = -5.0f;
    
    public float runFlyMultiplier = 3.1f;
    public float elevateMultipler = 4.0f;
    
    private bool isFlying = false;
    private bool isFlyingBackward = false;
    private ThirdPersonController characterController;
    // Move the controller
    CharacterController controller;
    private float jumpOffset = 0.5f;
    
    //helps Lerp work right when doing moveSpeeds
   // private float a = 0.0f;



    // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.

    // The current move direction in x-z
    private Vector3 moveDirection = Vector3.zero;
    // The current x-z move speed
    private float moveSpeed;
    public float rotateSpeed = 100.0f;
    public float speedSmoothing = 10.0f;
    private float elevateSpeed = 0.0f;
    public float elevateAmount = 7.0f;
    //Class that smoothes input when pressed
    //private SmoothInputAxes axes;
    
    //alternate camera when in fly mode
    //private PlayerCamera normalCameraSmoothFollow;
    private CameraManager camManager;
 	private PlayerManager playerManager;
	public GameObject lookObject;

    // The last collision flags returned from controller.Move
    private CollisionFlags collisionFlags;

    void Awake() {

        characterController = GetComponent<ThirdPersonController>();
        controller = GetComponent<CharacterController>();

        moveDirection = transform.TransformDirection(Vector3.forward);
		
		
		
        //assign to Component in main camera
       // normalCameraSmoothFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCamera>();
        camManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
		playerManager = GetComponent<PlayerManager>();
		
		Screen.lockCursor = true;
		
    }
	
	
    void Update() {

        if (characterController.IsJumping() && !isFlying || characterController.IsFalling() )
        {

            //next jump button shouldn't be too close to first jump time
            if ((characterController.LastJumpTime() + jumpOffset) > Time.time )
                return;


            if (Input.GetButtonDown("Jump"))
              ActivateFlying();               
            
        }


        if (isFlying)
            FlyingUpdate();

    }


    private void ActivateFlying()
    {
        isFlying = true;
        characterController.enabled = false;
        
        moveDirection = transform.TransformDirection(Vector3.forward);
		
		
        //tells camera manager to update camera to flying version
        camManager.CameraChanged("PlayerFly");
		
		playerManager.LookDirection( new Vector3(0 ,0, 1));
        
		
		moveSpeed = playerManager.MoveSpeed();
		
		
    }


    void FlyingUpdate()
    {

        // ----------------Movement Controls      ---------------------//
      




        //sets target direction by making a vector with the keys that you have pressed.
        // the 1.0f is the vertical value. If you try to turn with only h being pressed, it will act funny
        Vector3 targetDirection = /*Input.GetAxis("Horizontal") * transform.right + */ 0.3f * transform.forward;
       targetDirection = targetDirection.normalized; 

       //creates a vector that will smoothly rotate toward destination. Normalized for good measure    
        moveDirection = Vector3.Lerp(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime);
		//moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
        moveDirection = moveDirection.normalized;

        //Does the final rotation along as there is being input pressed (targetDirection)
        /*
        if (targetDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);


        }
         * */

        //--Find out the movement
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //Vector3 targetDirection2 = v * transform.forward;
        //Vector3 targetDirection2 = h * transform.right + v * transform.forward;

        // Choose target speed
        //* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
        //float targetDirection3 = Mathf.Min(targetDirection2.magnitude, 1.0f);;
		float targetSpeed;

        if (Mathf.Abs(v) < 0.1f && Mathf.Abs(h) < 0.1f )
        {
            targetSpeed = 0;
            isFlyingBackward = false;
        }
        else if (v < -0.1f)
        {
            targetSpeed = flyingBackSpeed;
            isFlyingBackward = true;
        }
        else
        {
            targetSpeed = flyingSpeed;
            isFlyingBackward = false;
        }
                   
            



        // Pick speed modifier if Run is pressed
        if (Input.GetButton("Run"))
        {
            //only add multiplier if going forward
            if (!isFlyingBackward)
            {
                targetSpeed *= runFlyMultiplier;
            }
        }



        //sets the current smooth speed
        // Smooth the speed based on the current target direction
        float curSmooth = speedSmoothing * Time.deltaTime;


        //sets the movement after all the scenarios have been calculated
        moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		playerManager.MoveSpeed(moveSpeed);
        //Debug.Log("Move Speed:" + moveSpeed + " Target Speed:" + targetSpeed + " CurSmoth:" + curSmooth );

        // Calculate actual motion
        Vector3 movement = moveDirection * moveSpeed;
        movement *= Time.deltaTime;

		

       collisionFlags = controller.Move(movement);

           



        
            // ----------------FLying rotation Controls      ---------------------//
      
        //for mouse movement
        float yawMouse = Input.GetAxis("Mouse X");
        float pitchMouse = Input.GetAxis("Mouse Y");        
        Vector3 targetFlyRotation = Vector3.zero;

        Screen.lockCursor = true;
       
        if (Mathf.Abs(yawMouse) > 0.1f || Mathf.Abs(pitchMouse) > 0.1f) 
        {
            targetFlyRotation = yawMouse * transform.right + pitchMouse * transform.up;
            targetFlyRotation.Normalize();
            targetFlyRotation *= Time.deltaTime * 3.0f;

            
            //limit x rotation if looking too much up or down
            //Log out the limitX value for this to make sense
            float limitX = Quaternion.LookRotation(moveDirection + targetFlyRotation).eulerAngles.x;


                //70 sets the rotation limit in the down direction
                //290 sets limit for up direction
            if (limitX < 90 && limitX > 70 || limitX > 270 && limitX < 290) 
            {
                //Debug.Log("restrict motion");
            }
            else
            {
                moveDirection += targetFlyRotation;
               //does the actual rotation on the object if no limits are breached
                transform.rotation = Quaternion.LookRotation(moveDirection);
				
				
            }

            
			Vector3 newLookDirection = lookObject.transform.position -  transform.position;
			newLookDirection.Normalize();
		
			playerManager.LookDirection(newLookDirection);
         

        }




    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.moveDirection.y > 0.01)
            return;


        //special collision things while in flight mode
        if (hit.gameObject.tag == "World" && isFlying || hit.gameObject.tag == "Buildings" && isFlying)
        {
            isFlying = false;
            characterController.enabled = true;
            moveSpeed = 0.0f;

            //slowly change camera 
            camManager.CameraChanged("PlayerGrounded");
                        
        }

            



   }
	
    public Vector3 MoveDirection()
    {
        return moveDirection;
    }
    public bool IsFlying()
    {
        return isFlying;
    }
    public float MoveSpeed()
    {
        return moveSpeed;
    }
   

}//end class
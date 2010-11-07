using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class ThirdPersonController : MonoBehaviour
{



    #region Properties

    // The speed when walking
    private float walkSpeed = 4.0f;
    // after trotAfterSeconds of walking we trot with trotSpeed
    private float trotSpeed = 9.0f;
    // when pressing "Fire3" button (cmd) we start running
    private float runSpeed = 18.0f;

    public float inAirControlAcceleration = 3.0f;

    // How high do we jump when pressing jump and letting go immediately
    public float jumpHeight = 3.0f;
    // We add extraJumpHeight meters on top when holding the button down longer while jumping
    public float extraJumpHeight = 2.5f;

    // The gravity for the character
    public float gravity = 10.0f;

    // The gravity in controlled descent mode
    private float controlledDescentGravity = 2.0f;
    public float speedSmoothing = 10.0f;
    public float rotateSpeed = 500.0f;
    private float trotAfterSeconds = 3.0f;

   // private bool canJump = true;
    private bool canControlDescent = true;
    //private bool canWallJump = false;

    private float jumpRepeatTime = 0.05f;
   // private float wallJumpTimeout = 0.15f;
    private float jumpTimeout = 0.15f;
    private float groundedTimeout = 0.25f;

    // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
    private float lockCameraTimer = 0.0f;

    // The current move direction in x-z
    private Vector3 moveDirection = Vector3.zero;
    // The current vertical speed
    private float verticalSpeed = 0.0f;
    // The current x-z move speed
    private float moveSpeed = 0.0f;

    // The last collision flags returned from controller.Move
    private CollisionFlags collisionFlags;

    // Are we jumping? (Initiated with jump button and not grounded yet)
    private bool jumping = false;
    private bool jumpingReachedApex = false;

    // Are we moving backwards (This locks the camera to not do a 180 degree spin)
   // private bool movingBack = false;
    // Is the user pressing any keys?
    private bool isMoving = false;
    // When did the user start walking (Used for going into trot after a while)
    private float walkTimeStart = 0.0f;
    // Last time the jump button was clicked down
    private float lastJumpButtonTime = -10.0f;
    // Last time we performed a jump
    private float lastJumpTime = -1.0f;


    // the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
   // private float lastJumpStartHeight = 0.0f;
    // When did we touch the wall the first time during this jump (Used for wall jumping)

    private Vector3 inAirVelocity = Vector3.zero;
    private float lastGroundedTime = 0.0f;
    private bool isControllable = true;
    private bool areKeysPressed = false;

    private bool isFalling = false;

    #endregion

    #region Functions
    //sets the move direction to current Forward vector in world space
    void Awake()  {       
       moveDirection = transform.TransformDirection(new Vector3(1, 1, 1));
    }

    
    void OnEnable()
    {
        //tells character controller what forward direction was last (because flying controller took over and had own forward direction)
        PlayerManager pmanager = GetComponent<PlayerManager>();
        moveDirection = pmanager.MoveDirection();
    }

    public void HidePlayer()  {
        GameObject.Find("rootJoint").GetComponent<SkinnedMeshRenderer>().enabled = false; // stop rendering the player.
        isControllable = false;	// disable player controls.
    }

    public void ShowPlayer()  {
        GameObject.Find("rootJoint").GetComponent<SkinnedMeshRenderer>().enabled = true;
        isControllable = true;
    }

    private void UpdateSmoothedMovementDirection()
    {
        Transform cameraTransform = Camera.main.transform;
        bool grounded = IsGrounded();

        // Forward vector relative to the camera along the x-z plane	
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        forward.y = 0;
        forward = forward.normalized;

        // Right vector relative to the camera
        // Always orthogonal to the forward vector
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //check to see if character is moving character at all
        if (v == 0 && h == 0)
            areKeysPressed = false;
        else
            areKeysPressed = true;
            

        // Are we moving backwards or looking backwards
       /* if (v < -0.2)
            movingBack = true;
        else
            movingBack = false;
        * */

        bool wasMoving = isMoving;
        isMoving = Mathf.Abs(h) > 0.1 || Mathf.Abs(v) > 0.1;

        // Target direction relative to the camera
        Vector3 targetDirection = h * right + v * forward;

        // Grounded controls
        if (grounded)
        {
            // Lock camera for short period when transitioning moving & standing still
            lockCameraTimer += Time.deltaTime;
            if (isMoving != wasMoving)
                lockCameraTimer = 0.0f;

            // We store speed and direction seperately,
            // so that when the character stands still we still have a valid forward direction
            // moveDirection is always normalized, and we only update it if there is user input.
            if (targetDirection != Vector3.zero)
            {
                // If we are really slow, just snap to the target direction
                if (moveSpeed < walkSpeed * 0.9 && grounded)
                {
                    moveDirection = targetDirection.normalized;
                }
                // Otherwise smoothly turn towards it
                else
                {
                    moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
                    moveDirection = moveDirection.normalized;
                }
            }

            // Smooth the speed based on the current target direction
            float curSmooth = speedSmoothing * Time.deltaTime;

            // Choose target speed
            //* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
            float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);

            // Pick speed modifier
            if (Input.GetButton("Run"))
            {
                targetSpeed *= runSpeed;
            }
            else if (Time.time - trotAfterSeconds > walkTimeStart)
            {
                targetSpeed *= trotSpeed;
            }
            else
            {
                targetSpeed *= walkSpeed;
            }

            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);

            // Reset walk time start when we slow down
            if (moveSpeed < walkSpeed * 0.3)
                walkTimeStart = Time.time;
        }
        // In air controls
        else
        {
            // Lock camera while in air
           if (jumping)
                lockCameraTimer = 0.0f;

            if (isMoving)               
                    inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
        }



    }

    public bool IsGrounded()
    {
        return (collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

    private float CalculateJumpVerticalSpeed (float targetJumpHeight)
{
	// From the jump height and gravity we deduce the upwards speed 
	// for the character to reach at the apex.
	return Mathf.Sqrt(2.0f * targetJumpHeight * gravity);
}

    private void ApplyJumping()
    {
        // Prevent jumping too fast after each other
        if (lastJumpTime + jumpRepeatTime > Time.time)
            return;

       

        if (IsGrounded())
        {
            // Jump
            // - Only when pressing the button down
            // - With a timeout so you can press the button slightly before landing		
            if ( Time.time < lastJumpButtonTime + jumpTimeout)
            {
                verticalSpeed = CalculateJumpVerticalSpeed(jumpHeight);
                SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
                jumping = true;
                lastJumpTime = Time.time;
                
            }
        }
    }

    void Update()
    {

       
 
        if (!isControllable)
	    {
		    // kill all inputs if not controllable.
		    Input.ResetInputAxes();
	    }

	if (Input.GetButtonDown ("Jump"))
	{
		lastJumpButtonTime = Time.time;
	}

	UpdateSmoothedMovementDirection();	
	ApplyGravity ();
    ApplyJumping ();

    IsFalling();

	// Calculate actual motion
	Vector3 movement = moveDirection * moveSpeed + new Vector3(0, verticalSpeed, 0) + inAirVelocity;
	movement *= Time.deltaTime;
	
	// Move the controller
	CharacterController controller = GetComponent<CharacterController>();
	
	collisionFlags = controller.Move(movement);
	
	// Set rotation to the move direction
	if (IsGrounded() && Input.GetAxisRaw("Horizontal") != 0 )
		transform.rotation = Quaternion.LookRotation(moveDirection);
	else
	{

	    	Vector3 xzMove = movement;
			xzMove.y = 0;
			if (xzMove.sqrMagnitude > 0.001)
			{
				transform.rotation = Quaternion.LookRotation(xzMove);
			}

	}	
	
	// We are in jump mode but just became grounded
	if (IsGrounded())
	{
		lastGroundedTime = Time.time;
		inAirVelocity = Vector3.zero;
		if (jumping)
		{
			jumping = false;
			SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
		}
	}


  }


 private void OnControllerColliderHit (ControllerColliderHit hit)
{
	if (hit.moveDirection.y > 0.01) 
		return;
	

    if (hit.gameObject.tag == "World" || hit.gameObject.tag == "Buildings")
    {
        //reset jumping apex;
        jumpingReachedApex = false;
        isFalling = false;

        
    }

    //hurt player if he is falling fast enough and collides with the ground below
    if (verticalSpeed < -10 && IsFalling() && collisionFlags == CollisionFlags.Below)
    {
        SendMessage("PlayerFallDamage");
        //Debug.Log("collided below");

    }


    if ((collisionFlags & CollisionFlags.CollidedBelow) != 0)
    {
       // Debug.Log("Collided Below");
    }
   // Debug.Log(collisionFlags);
 	
}
    
    private void ApplyGravity()
    {
        if (isControllable)	// don't move player at all if not controllable.
        {
            // Apply gravity
            bool jumpButton = Input.GetButton("Jump");

            // * When falling down we use controlledDescentGravity (only when holding down jump)
            bool controlledDescent = canControlDescent && verticalSpeed <= 0.0f && jumpButton && jumping;

            // When we reach the apex of the jump we send out a message
            if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0)
            {
                jumpingReachedApex = true;
                SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
            }

            // * When jumping up we don't apply gravity for some time when the user is holding the jump button
            //   This gives more control over jump height by pressing the button longer
           // bool extraPowerJump = IsJumping() && verticalSpeed > 0.0 && jumpButton && transform.position.y < lastJumpStartHeight + extraJumpHeight;

            if (controlledDescent)
            {
                verticalSpeed -= controlledDescentGravity * Time.deltaTime;
            }
            else if (IsGrounded())
            {
                verticalSpeed = 0.0f;
            }
            else
            {
                //ACTUAL GRAVITY
                verticalSpeed -= gravity * Time.deltaTime;
            }
        }
    }

    public float VerticalSpeed()
    {
        return verticalSpeed;
    }
    public float GetSpeed()
    {
        return moveSpeed;
    }
    public bool IsJumping()
    {
      return jumping; 
    }
    public bool IsFalling()
    {
        //will check to see if player is falling and will calculate fall damage depending on height

        //if y value is decreasing at the rate of gravity, character is falling. Set bool to true
        if (verticalSpeed <= -2.0f && isFalling == false && !IsJumping())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float GetRunSpeed()
    {
        return runSpeed;
    }
    public bool HasJumpReachedApex()
    {
        return jumpingReachedApex;
    }   
    public bool IsGroundedWithTimeout()
    {
        return lastGroundedTime + groundedTimeout > Time.time;
    }
    public bool IsMoving () 
    {
	return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5;
    }
    public bool AreMovingKeysDown()
    {
        return areKeysPressed;
    }
    public float LastJumpTime()
    {
        return lastJumpTime;
    }
    public Vector3 MoveDirection()
    {
        return moveDirection;
    }


    
    #endregion


  
}//end class
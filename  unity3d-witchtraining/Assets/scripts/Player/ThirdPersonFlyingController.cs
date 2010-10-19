using UnityEngine;
using System.Collections;



public class ThirdPersonFlyingController : MonoBehaviour
{


    public float flyingSpeed = 15.0f;
    
    private float runFlyMultiplier = 2.1f;
    private float elevateMultipler = 2.0f;
    
    private bool isFlying = false;
    private ThirdPersonController characterController;
    // Move the controller
    CharacterController controller;
    private float jumpOffset = 0.5f;
    
    //helps Lerp work right when doing movements
   // private float a = 0.0f;


   // private bool isMoving = false;
    // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
    //private float lockCameraTimer = 0.0f;
    // The current move direction in x-z
    private Vector3 moveDirection = Vector3.zero;
    // The current x-z move speed
   // private float currSpeed = 0.0f;
    // The speed when walking
    //public float walkSpeed = 3.0f;
    public float rotateSpeed = 100.0f;
    public float speedSmoothing = 10.0f;

    //Class that smoothes input when pressed
    private SmoothInputAxes axes;
    
    //alternate camera when in fly mode
    //private PlayerCamera normalCameraSmoothFollow;
    private CameraManager camManager;
       

    // The last collision flags returned from controller.Move
    private CollisionFlags collisionFlags;

    void Awake() {

        characterController = GetComponent<ThirdPersonController>();
        controller = GetComponent<CharacterController>();

        axes = GetComponent<SmoothInputAxes>();


        moveDirection = transform.TransformDirection(Vector3.forward);

        //assign to Component in main camera
       // normalCameraSmoothFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCamera>();
        camManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraManager>();
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
        
    }


    void FlyingUpdate()
    {

        // ----------------Movement Controls      ---------------------//
      //  Transform cameraTransform = Camera.main.transform;


       //--- find out the direction of the movement
       //first we don't want to rotate if pressing the down button, so make it equal to zero if pressed
      /*  float axesV;
        if (axes.GetV() < 0.0f)
           axesV = 0.0f;
        else
           axesV = axes.GetV();
        */

        //sets target direction by making a vector with the keys that you have pressed.
        // the 1.0f is the vertical value. If you try to turn with only h being pressed, it will act funny
        Vector3 targetDirection = axes.GetH() * transform.right + 0.3f * transform.forward;
       targetDirection = targetDirection.normalized; 

       //creates a vector that will smoothly rotate toward destination. Normalized for good measure    
        moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
        moveDirection = moveDirection.normalized;

        //Does the final rotation along as there is being input pressed (targetDirection)
        if (targetDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);


            

        //--Find out the movement
        //We want to make sure the backward movement is a lot slower than forward movement
        // If he v input input is less than .2 clamp it to that.
        float axesVm;
       
        if (axes.GetV() < -0.3f)
        {
            axesVm = -0.3f;
        }
        else
        {
            axesVm = axes.GetV();
        }

        Vector3 targetDirection2 = axes.GetH() * transform.right + axesVm * transform.forward;
        Vector3 moveAmount = targetDirection2 * flyingSpeed;
        moveAmount *= Time.deltaTime;

        // Pick speed modifier if Run is pressed
        if (Input.GetButton("Run"))
        {
            moveAmount *= runFlyMultiplier;
        }

        collisionFlags = controller.Move(moveAmount);
            



        
            // ----------------Elevate Controls      ---------------------//

            if (Input.GetButton("Rise"))
            {
                //transform.Translate(0.0f, 10.0f * Time.deltaTime, 0.0f);
                Vector3 riseAmount = new Vector3(0.0f, 7.0f * Time.deltaTime, 0.0f);

                if (Input.GetButton("Run"))
                {
                    riseAmount *= elevateMultipler;
                }
                
                
                collisionFlags = controller.Move(riseAmount);
            }

            if (Input.GetButton("Descend"))
            {
                //transform.Translate(0.0f, -10.0f * Time.deltaTime, 0.0f);
                Vector3 descendAmount = new Vector3(0.0f, -7.0f * Time.deltaTime, 0.0f);
                
                if (Input.GetButton("Run"))
                {
                    descendAmount *= elevateMultipler;
                }
                
                collisionFlags = controller.Move(descendAmount);
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

   

}//end class
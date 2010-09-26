using UnityEngine;
using System.Collections;

public class ThirdPersonPlayerAnimation : MonoBehaviour
{
public float runSpeedScale = 1.0f;
public float walkSpeedScale = 1.0f;

private ThirdPersonController characterController;
	
	
    void Start ()
{
	// By default loop all animations
	animation.wrapMode = WrapMode.Loop;

	animation["run"].layer = -1;
	animation["walk"].layer = -1;
	animation["idle"].layer = -2;
	animation.SyncLayer(-1);



    animation["idle"].wrapMode = WrapMode.Loop;

	// The jump animation is clamped and overrides all others
	animation["jump"].layer = 10;
	animation["jump"].wrapMode = WrapMode.ClampForever;

	animation["fall"].layer = 10;	
	animation["fall"].wrapMode = WrapMode.Loop;

	animation["land"].layer = 10;	
	animation["land"].wrapMode = WrapMode.Once;


	// We are in full control here - don't let any other animations play when we start
	animation.Stop();
	animation.Play("idle");


    characterController = GetComponent<ThirdPersonController>();
    
}


void Update ()
{


    if (characterController.IsGrounded())
        {
            animation.Blend("fall", 0.0f, 0.3f);
            animation.Blend("jump", 0.0f, 0.3f);

            //Fade in Run
            if (characterController.GetSpeed() >= (characterController.GetRunSpeed() - 1))
            {
                animation.CrossFade("run");

                //fade out walk
                animation.Blend("walk", 0.0f, 0.3f);
            }

            // Fade in walk
            else if (characterController.GetSpeed() > 0.1f)
            {

                if (characterController.AreMovingKeysDown())
                {
                    animation.CrossFade("walk");
                }
                else
                {
                    animation.Blend("walk", 0.0f, 0.1f);
                }

                // We fade out jumpland realy quick otherwise we get sliding feet
                animation.Blend("run", 0.0f, 0.3f);

               
                
           }

            // Fade out walk and run so just idle remains
            else
            {
                animation.Blend("walk", 0.0f, 0.3f);
                animation.Blend("run", 0.0f, 0.3f);
            }



  


            animation["run"].normalizedSpeed = runSpeedScale;
            animation["walk"].normalizedSpeed = walkSpeedScale;

        }



    if (characterController.IsJumping())
        {

        
            if (!characterController.HasJumpReachedApex() && characterController.IsJumping())
            {
                animation.CrossFade("jump", 0.2f);
                animation.Blend("fall", 0.0f, 0.3f);
            }

            else
            {
                animation.Blend("jump", 0.0f, 0.3f);
                animation.CrossFade("fall", 0.2f);

            }


            animation.Blend("walk", 0.0f, 0.1f);
            animation.Blend("run", 0.0f, 0.1f);

        }




    
}

public void DidLand () {
	animation.Play("land");
}


}//end class
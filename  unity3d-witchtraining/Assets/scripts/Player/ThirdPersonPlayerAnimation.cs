using UnityEngine;
using System.Collections;

public class ThirdPersonPlayerAnimation : MonoBehaviour
{
public float runSpeedScale = 1.0f;
public float walkSpeedScale = 1.0f;

private ThirdPersonController characterController;
private ThirdPersonFlyingController flyingController;
public Transform animationGameObject;	
	
    void Start ()
{
	// By default loop all animations
   // animationGameObject.animation.wrapMode = WrapMode.Loop;

    animationGameObject.animation["run"].layer = -1;
    animationGameObject.animation["run"].wrapMode = WrapMode.Loop;
    animationGameObject.animation["walk"].layer = -1;
    animationGameObject.animation["walk"].wrapMode = WrapMode.Loop;
    animationGameObject.animation["idle"].layer = -2;
    animationGameObject.animation.SyncLayer(-1);

    animationGameObject.animation["fly"].layer = 1;
    animationGameObject.animation["fly"].wrapMode = WrapMode.ClampForever;

    animationGameObject.animation["elevate"].layer = 1;
    animationGameObject.animation["lower"].layer = 1;


    animationGameObject.animation["idle"].wrapMode = WrapMode.Loop;

	// The jump animation is clamped and overrides all others
    animationGameObject.animation["jump"].layer = 10;
    animationGameObject.animation["jump"].wrapMode = WrapMode.ClampForever;

    animationGameObject.animation["fall"].layer = 10;
    animationGameObject.animation["fall"].wrapMode = WrapMode.Loop;

    animationGameObject.animation["land"].layer = 10;
    animationGameObject.animation["land"].wrapMode = WrapMode.Once;

    animationGameObject.animation["fallHit"].layer = 10;
    animationGameObject.animation["fallHit"].wrapMode = WrapMode.Once;




	// We are in full control here - don't let any other animations play when we start
    animationGameObject.animation.Stop();
    animationGameObject.animation.Play("idle");


    characterController = GetComponent<ThirdPersonController>();
    flyingController = GetComponent<ThirdPersonFlyingController>();
    
}


void Update ()
{
    

    if (flyingController.IsFlying())
    {
        FlyingAnimations();
    }
    
    else  if (characterController.IsFalling())
    {
        animationGameObject.animation.CrossFade("fall");
    }
     
    //must be grounded
    else  
        {
            GroundedAnimations();        
       
        }
    
}



private void FlyingAnimations()
{
    
       
    //play flying animation right when flight mode is entered
    animationGameObject.animation.Blend("fall", 0.0f, 0.3f);
    animationGameObject.animation.Blend("jump", 0.0f, 0.3f);
    animationGameObject.animation.Blend("fly", 1.0f, .3f);


    if (Input.GetButton("Rise"))
    {
      animationGameObject.animation.CrossFade("elevate");
    }

    if (Input.GetButton("Descend"))
    {
      animationGameObject.animation.CrossFade("lower");
    }



}


private void GroundedAnimations()
{
    //blend off any residual animations
    animationGameObject.animation.Blend("fall", 0.0f, 0.1f);
    animationGameObject.animation.Blend("jump", 0.0f, 0.1f);
    animationGameObject.animation.Blend("fly", 0.0f, 0.1f);
    animationGameObject.animation.Blend("elevate", 0.0f, 0.1f);
    animationGameObject.animation.Blend("lower", 0.0f, 0.1f);

    //Fade in Run
    if (characterController.GetSpeed() >= (characterController.GetRunSpeed() - 1))
    {
        animationGameObject.animation.CrossFade("run");

        //fade out walk
        animationGameObject.animation.Blend("walk", 0.0f, 0.3f);
    }

    // Fade in walk
    else if (characterController.GetSpeed() > 0.1f)
    {

        if (characterController.AreMovingKeysDown())
        {
            animationGameObject.animation.CrossFade("walk");
        }
        else
        {
            animationGameObject.animation.Blend("walk", 0.0f, 0.1f);
        }

        // We fade out jumpland realy quick otherwise we get sliding feet
        animationGameObject.animation.Blend("run", 0.0f, 0.3f);



    }

    // Fade out walk and run so just idle remains
    else
    {
        animationGameObject.animation.Blend("walk", 0.0f, 0.3f);
       // animationGameObject.animation.Blend("run", 0.0f, 0.3f);
    }

    
    animationGameObject.animation["run"].normalizedSpeed = runSpeedScale;
    animationGameObject.animation["walk"].normalizedSpeed = walkSpeedScale;

    //is Character Jumping
    if (characterController.IsJumping())
    {


        if (!characterController.HasJumpReachedApex() && characterController.IsJumping())
        {
            animationGameObject.animation.CrossFade("jump", 0.2f);
            animationGameObject.animation.Blend("fall", 0.0f, 0.3f);
        }

        else
        {
            animationGameObject.animation.Blend("jump", 0.0f, 0.3f);
            animationGameObject.animation.CrossFade("fall", 0.2f);

        }


        animationGameObject.animation.Blend("walk", 0.0f, 0.1f);
        animationGameObject.animation.Blend("run", 0.0f, 0.1f);

    }

}



public void DidLand () {
    animationGameObject.animation.Play("land");
}

void PlayerFallDamage()
{
    animationGameObject.animation.Play("fallHit");
    Debug.Log("Hit Damage fired");
}


}//end class
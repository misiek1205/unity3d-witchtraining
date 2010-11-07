using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour
{

    private AudioSource audioSource;
    private Animation playerAnimations;

    public AudioClip walkSound;

    public Transform playerModelObject;

    // Use this for initialization
    void Start()
    {
        //create audio source component for sounds to play
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;


       
        //create animation event
               
        AnimationEvent walkSoundEvent = new AnimationEvent();
        walkSoundEvent.functionName = "WalkingSounds";
        walkSoundEvent.time = 139.0f;


        playerModelObject.GetComponent<Animation>()["walk"].clip.AddEvent(walkSoundEvent);
       
    }


    public void WalkingSounds()
    {
        animation.Rewind();
        audioSource.PlayOneShot(walkSound);            
        Debug.Log("Walk Sound Fired");
    }
}

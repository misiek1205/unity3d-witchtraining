using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour
{
	
    private AudioSource audioSource;
    private Animation playerAnimations;

    public AudioClip hitGroundSound;
	

    public GameObject animationObject;
		
	
	public void DidLand () {
		audio.pitch = 1;
		audio.
    audio.PlayOneShot(hitGroundSound);
	}
	
	
	
}
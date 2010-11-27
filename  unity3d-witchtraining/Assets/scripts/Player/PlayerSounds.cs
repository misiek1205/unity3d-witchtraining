using UnityEngine;
using System.Collections;

public class PlayerSounds : MonoBehaviour
{
	
    private AudioSource audioSource;
    private Animation playerAnimations;

    public AudioClip hitGroundSound;
	public AudioClip walkSound;
	public AudioClip runSound;

    public GameObject animationObject;
	private ThirdPersonController thirdPersonController;
	
	private bool isRunning = false;
	private bool isWalking = false;
	private bool isIdle = false;
	
    // Use this for initialization
    void Start()
    {
      
        
		Debug.Log(animationObject.animation["walk"].name);
		//playerManager = GetComponent<PlayerManager>();
		thirdPersonController = GetComponent<ThirdPersonController>();
    }

		
	
	public void DidLand () {
		audio.pitch = 1;
		audio.
    audio.PlayOneShot(hitGroundSound);
	}
	
	
	
}
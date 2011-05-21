using UnityEngine;
using System.Collections;

public class PlayerAnimationEvents : MonoBehaviour
{
	
	
	public AudioClip walkSound;
	public AudioClip runSound;
   
	
	private ThirdPersonPlayerAnimation playerAnimation;
	private string movingState;
	
	void Start()
	{
	audio.loop = false;	
	
	playerAnimation = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonPlayerAnimation>();
	
	}
	
	
	void Update()
	{
	//calculate which state the animation is in
		movingState = playerAnimation.State();
	
	}
	
	
public void walkAnimationEvent()
	{
		if (movingState == "Walk")
		{
		//Debug.Log("Is walking");
		audio.PlayOneShot(walkSound);
		audio.volume = 0.4f;
		}
	}
	
	public void runAnimationEvent()
	{
		if (movingState == "Run")
		{
			//Debug.Log("Is running");
			audio.PlayOneShot(runSound);
			audio.volume = 0.5f;
		}
	}
}

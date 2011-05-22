
using UnityEngine;
using System.Collections;

public class PlayerLookTarget : MonoBehaviour
{
	
	
	private HeadLookController headLookCont;
	
	public GameObject lookObject;
	private Vector3 lookOriginalTransform;
	
	//private ThirdPersonController thirdPersonController;
	private PlayerManager playerManager;
	
	
	public float sensitivityX = 12.0f;
	public float sensitivityY = 2.0f;

	//private float yawMouse = 0F;
	//private float pitchMouse = 0F;
	

	private Vector3 lookDirection;
	private bool wasInAir = false;
	
	// Use this for initialization
	void Start () {
		headLookCont = GetComponent<HeadLookController>();
		playerManager = GetComponent<PlayerManager>();
		//thirdPersonController = GetComponent<ThirdPersonController>();
		
		if (headLookCont==null) Debug.Log("Head Look Controller is null!");
		
		//position when character is not on ground
		lookOriginalTransform = new Vector3(0 , 0, 10);
			

	}
	

	
	void LateUpdate()
	{
			
		
		if (playerManager.IsFlying())
		{		
			wasInAir = true;
		}
		else
		{
			
			if (wasInAir)
			{
			
				Quaternion rotation = Quaternion.Euler(0, 0, 0);
        		Vector3 position = new Vector3(0.0f, 0.0f, 10f) + transform.position;
        
		        lookObject.transform.rotation = rotation;
		        lookObject.transform.position = position;
				
				wasInAir = false;
				
				Debug.Log(lookObject.transform.rotation + "   " + lookObject.transform.position);
			}
			
			
			//yawMouse = Input.GetAxis("Mouse X");
			//pitchMouse = Input.GetAxis("Mouse Y");
			
					
			//updated target location for character to be looking at
			headLookCont.target = lookObject.transform.position;
					
			Vector3 newLookDirection = lookObject.transform.position -  transform.position;
			newLookDirection.Normalize();
			
			playerManager.LookDirection(newLookDirection);
			
		}
	
			
	}
}
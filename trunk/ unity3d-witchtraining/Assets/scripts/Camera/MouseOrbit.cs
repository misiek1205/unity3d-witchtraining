using UnityEngine;
using System.Collections;

public class MouseOrbit : MonoBehaviour
{

public Transform target;
public float distance = 10.0f;

public float xSpeed = 250.0f;
public float ySpeed = 120.0f;

public float yMinLimit = -10f;
public float yMaxLimit = 50f;

public float xMinLimit = -90f;
public float xMaxLimit = 90f;

private float x = 0.0f;
private float y = 0.0f;

//private GameObject player;

void Start () {
    Vector3 angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;

	// Make the rigid body not change rotation
   	if (rigidbody)
		rigidbody.freezeRotation = true;

	//player = GameObject.Find("Player");
}

void LateUpdate () {
    if (target) {
        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y += Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
 		
 		
		//y = ClampAngle(y, yMinLimit, yMaxLimit);
 		//x = ClampAngle(x, xMinLimit, xMaxLimit);
 		    

 		          
 		
        //update for rotation based off character orientation
        // Debug.Log(-y + " " + x);
        
		//	x += player.transform.localRotation.eulerAngles.y;
			
		
		//	Debug.Log(x + "  " + y);	
			
        Quaternion rotation = Quaternion.Euler(-y, x, 0f);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, distance) + target.position;
        

				
		//apply the transformations		
       transform.rotation = rotation;
       transform.position = position;
    }
}

private static float ClampAngle (float angle, float min, float max) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}

}
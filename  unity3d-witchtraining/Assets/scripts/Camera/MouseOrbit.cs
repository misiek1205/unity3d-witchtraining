/*  This script controls the player ground movement aiming
 *  It doesn't change anythign if the player is in the air.
 */


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

private Camera mainCamera;
private Quaternion mainCameraRot;

//private GameObject player;

void Start () {
    Vector3 angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;

	// Make the rigid body not change rotation
   	if (rigidbody)
		rigidbody.freezeRotation = true;

    mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    mainCameraRot = mainCamera.transform.rotation;

    }

void LateUpdate () {

   
    
    
    if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y += Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            Quaternion rotation = Quaternion.Euler(-y, x, 0f);

            //y = ClampAngle(y, yMinLimit, yMaxLimit);
            //x = ClampAngle(x, xMinLimit, xMaxLimit);


            //camera rotation that will move the lookRotation sphere
            Vector3 difAngle;
            difAngle = mainCamera.transform.rotation.eulerAngles - mainCameraRot.eulerAngles;
            Quaternion diffrotation = Quaternion.Euler(0.0f, difAngle.y, 0.0f);




            Vector3 position = diffrotation * rotation * new Vector3(0.0f, 0.0f, distance) + target.position;
           

             //apply the transformations		
           // transform.rotation = rotation;
            transform.position = position;
        }

    /*if camera has rotated, rotate lookDirection sphere the same amount
    if (mainCameraRot != mainCamera.transform.rotation)
    {
        //find difference between two angles
        Vector3 difAngle;
        difAngle = mainCameraRot.eulerAngles - mainCamera.transform.rotation.eulerAngles;

        Quaternion diffrotation = Quaternion.Euler(-difAngle.y, difAngle.x, 0f);

        //transform.position = position;
        Vector3 position = diffrotation * new Vector3(0.0f, 0.0f, distance) + this.transform.position;
        transform.position = position;
    }
    
    */
}

private static float ClampAngle (float angle, float min, float max) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}

private Quaternion extrapolateLinearRotation(Vector3 dir1, Vector3 dir2)
{
    Vector3 dir3 = dir1 - dir2;
    dir3 = dir2 + dir3;
    return Quaternion.FromToRotation(dir2, dir3);
}


}
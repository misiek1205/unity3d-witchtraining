using UnityEngine;
using System.Collections;

public class HairPhysics: MonoBehaviour {

    public Transform headObject;


	// Use this for initialization
	void Start () {

        
        Debug.Log(headObject.GetChildCount());


        for (int i=0; i < headObject.GetChildCount(); i++)
        {
            headObject.GetChild(i).gameObject.AddComponent<Rigidbody>();
            headObject.GetChild(i).gameObject.AddComponent<HingeJoint>();
            headObject.GetChild(i).gameObject.GetComponent<HingeJoint>().connectedBody = headObject.rigidbody;
            headObject.GetChild(i).gameObject.GetComponent<HingeJoint>().useSpring = true;
            headObject.GetChild(i).gameObject.GetComponent<HingeJoint>().useMotor = true;

            
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

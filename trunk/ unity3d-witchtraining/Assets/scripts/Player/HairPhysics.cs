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
            
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

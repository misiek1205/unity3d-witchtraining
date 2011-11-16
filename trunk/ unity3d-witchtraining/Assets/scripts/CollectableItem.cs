using UnityEngine;
using System.Collections;

public class CollectableItem : MonoBehaviour {

	private float bounceMultplier = 3.0f;
	private PlayerManager playerManager;
	
	//remember start position so the bouncing know where it was originally
	private Vector3 startPos;
	
	// Use this for initialization
	void Start () {
		
		playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		//use sin to make bounce up and down
		float timeChange = Mathf.Sin( Time.time * bounceMultplier ) * .2f +  startPos.y ;
		this.transform.position = new Vector3(startPos.x, timeChange,startPos.z);
		
		//slowly rotate
		this.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f) );
		
		
	}
	
	//kill game object and add one to the amount collected 
	void OnTriggerEnter(Collider other) {
		Destroy(this.gameObject);
		
		int newAmount = playerManager.CatsCollected()+ 1;		
		playerManager.CatsCollected(newAmount);
		Debug.Log("cats collected: " + playerManager.CatsCollected());
		
	}
	
}

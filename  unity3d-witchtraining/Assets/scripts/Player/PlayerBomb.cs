using UnityEngine;
using System.Collections;

public class PlayerBomb : MonoBehaviour
{
    public GameObject explosionParticles;
	private PlayerBombThrowing bombThrowingScript;
	
    // Use this for initialization
    void Start()
    {
		bombThrowingScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBombThrowing>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
{

    if (col.gameObject.tag != "Player" && !(bombThrowingScript.IsHoldingBomb()))
    {
        ContactPoint collisionPoint = col.contacts[0];
        Quaternion collisionRot = Quaternion.Euler(collisionPoint.normal);



        Instantiate(explosionParticles, transform.position, collisionRot);
        Destroy(this.gameObject);
    }

}
}

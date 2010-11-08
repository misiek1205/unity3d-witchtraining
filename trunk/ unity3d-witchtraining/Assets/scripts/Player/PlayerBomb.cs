using UnityEngine;
using System.Collections;

public class PlayerBomb : MonoBehaviour
{
    public GameObject explosionParticles;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
{

    if (col.gameObject.tag != "Player")
    {
        ContactPoint collisionPoint = col.contacts[0];
        Quaternion collisionRot = Quaternion.Euler(collisionPoint.normal);



        Instantiate(explosionParticles, transform.position, collisionRot);
        Destroy(this.gameObject);
    }

}
}

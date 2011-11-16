using UnityEngine;
using System.Collections;

public class MouseFlying : MonoBehaviour
{

    public GameObject player;

    private PlayerManager playerMan;
    private Vector3 playerPos;
    //private Vector3 playerLookDirection;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //update player position and look direction so we know how to position the sphere
        playerPos = player.transform.position;
               
        Vector3 finalLocation = playerPos + ( player.transform.forward * 4);

        this.transform.position = finalLocation;
    }
}

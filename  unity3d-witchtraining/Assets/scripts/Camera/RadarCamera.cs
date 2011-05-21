using UnityEngine;
using System.Collections;

public class RadarCamera : MonoBehaviour
{
    
    public Transform player;
    public Transform FOVGUI;
    

    private Quaternion forwardDirection;



    // Use this for initialization
    void Start()
    {

       
    }

    // Update will position camera along same 2d plane as main player
    //this will allow the camera to follow the player exactly
    void Update()
    {
        //get reference to current player. Use character controller since that that doesn't rotate weird when flying
        forwardDirection = player.transform.rotation;

        this.transform.position = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        
        //only need to transform in one direction
        FOVGUI.transform.rotation = new Quaternion(forwardDirection.x, forwardDirection.y, forwardDirection.z, forwardDirection.w);


        //FOVGUI.transform.rotation = Quaternion.LookRotation(new Vector3(startingxRot, player.transform.eulerAngles.y, startingzRot));

        //float yAngle = this.transform.eulerAngles.y;

        //FOVGUI.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, player.transform.eulerAngles.x, player.transform.eulerAngles.z);
        //FOVGUI.transform.rotation = new Quaternion(player.transform.rotation.x, player.transform.rotation.y, player.transform.rotation.z, player.transform.rotation.z);

    }
}

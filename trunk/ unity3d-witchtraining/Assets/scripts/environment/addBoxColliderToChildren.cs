using UnityEngine;
using System.Collections;

public class addBoxColliderToChildren : MonoBehaviour
{

   // private Component combineChildren;
    private bool combineChildrenAttached = false;

    // Use this for initialization
    void Start()
    {
        //access each child that the script is attached to. Only goes one level deep
        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<BoxCollider>();
        }



    }

    void Awake()
    {
        //access each child that the script is attached to. Add level tag to make sure player collides
        foreach (Transform child in transform)
        {
           child.gameObject.tag = "World";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<MeshCollider>() && combineChildrenAttached == false)
        {
            //combineChildren = gameObject.GetComponent<CombineChildren>();
            combineChildrenAttached = true;
        }

        


    }
}

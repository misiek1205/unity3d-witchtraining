using UnityEngine;
using System.Collections;

public class BlobShadow : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(-Vector3.up, transform.parent.forward);
    }
}

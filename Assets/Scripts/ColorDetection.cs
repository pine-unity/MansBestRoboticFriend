using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDetection : MonoBehaviour
{
    // so that only blocks are detected and not walls etc
    LayerMask layerMask;
    // Start is called before the first frame update
    void Awake()
    {
        layerMask = LayerMask.GetMask("Block");
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.forward, out hit, 20f, layerMask))
        {
            Debug.Log("block detected");
        }
        else
        {
            Debug.Log("no block detected");
        }
            
    }
}

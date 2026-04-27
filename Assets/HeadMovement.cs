using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour
{
    [SerializeField] private float rotAngle;
    private float verticalRotScale;
    private float horizontalRotScale;

    void Start()
    {
        rotAngle = 90;
        verticalRotScale = 0; 
        horizontalRotScale = 0;
    }

    void FixedUpdate()
    {
        rotate(); 
    }

    void rotate()
    {
        if (horizontalRotScale > -50 && Input.GetKey(KeyCode.RightArrow))
        {
            horizontalRotScale--;
            transform.Rotate(0, 0, -1 * rotAngle * Time.deltaTime);
        }

        if (horizontalRotScale < 50 && Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalRotScale++;
            transform.Rotate(0, 0, rotAngle * Time.deltaTime);
        }

        if(verticalRotScale > -40 && Input.GetKey(KeyCode.UpArrow)) 
        {   
            verticalRotScale--;
            transform.Rotate(rotAngle * Time.deltaTime, 0, 0);
        }

        if(verticalRotScale < 20 && Input.GetKey(KeyCode.DownArrow))
        { 
            verticalRotScale++;
            transform.Rotate(-1 * rotAngle * Time.deltaTime, 0, 0); 
        }
    
    }
}

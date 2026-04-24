using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControlMode : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angle;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        speed = 50;
        angle = 65;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    // move forward/backward/left/right
    void move(){
    
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = -1 * transform.forward * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = -1 * transform.right * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = transform.right * speed;
        }    


        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            rb.velocity = Vector3.zero;
        }  

        rotate();
         
    }

    void rotate(){
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -1 * angle * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, angle * Time.deltaTime, 0);
        }
    }


    void onCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("block"))
        {
            rb.velocity = Vector3.zero;
        }
    }


}

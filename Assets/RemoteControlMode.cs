using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControlMode : MonoBehaviour
{
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 40;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    // move forward/backward/left/right
    void move(){
        float vertical = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;
        float horizontal = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;

        gameObject.transform.position += new Vector3(horizontal, 0, vertical);
    }

    void rotate(){

    }

}

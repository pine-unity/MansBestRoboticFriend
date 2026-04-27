using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDetection : MonoBehaviour
{
    // so that only blocks are detected and not walls etc
    LayerMask layerMask;
    Color color;

    // Constants
    Color RED;
    Color ORANGE;
    Color YELLOW;
    Color GREEN;
    Color BLUE;
    Color PURPLE;
    Color PINK;
    Color[] ALL_COLORS;

    public Color colorSeen;
    // Start is called before the first frame update
    void Awake()
    {
        RED = new Color(1, 0, 0, 1);
        ORANGE = new Color(1, 0.502f, 0, 1);
        YELLOW = new Color(1, 1, 0.2f, 1);
        GREEN = new Color(0.2f, 1, 0.2f, 1);
        BLUE = new Color(0, 0, 1, 1);
        PURPLE = new Color(0.502f, 0, 1, 1);
        PINK = new Color(1, 0, 1, 1);
        ALL_COLORS = new Color[]{RED, ORANGE, YELLOW, GREEN, BLUE, PURPLE, PINK};

        colorSeen = new Color(0,0,0,1);
        layerMask = LayerMask.GetMask("Block");
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.forward, out hit, 20f, layerMask))
        {
            color = hit.collider.gameObject.GetComponent<Renderer>().sharedMaterial.color;
            colorSeen = color;
            Debug.Log("block detected, color: " + color);
            checkColor();
            
        }
        else
        {
            Debug.Log("no block detected");
        }
            
    }

    void checkColor()
    {
        Color closestColor = new Color(0,0,0,1);
        float maxDistance = 1.5f;
        float distance = 0f;
        for(int i = 0; i < ALL_COLORS.Length; i++)
        {
            float rDiff = Mathf.Abs(colorSeen.r - ALL_COLORS[i].r);
            float gDiff = Mathf.Abs(colorSeen.g - ALL_COLORS[i].g);
            float bDiff = Mathf.Abs(colorSeen.b - ALL_COLORS[i].b);
            

            distance = Mathf.Sqrt(rDiff*rDiff + gDiff*gDiff + bDiff*bDiff);
            Debug.Log("Distance: " + distance);
            if(distance < maxDistance)
            {
                maxDistance = distance;
                closestColor = ALL_COLORS[i];
            }
            
        }
        Debug.Log("Predicted Color: " + closestColor);
    }
}

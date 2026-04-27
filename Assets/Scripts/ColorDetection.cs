using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorDetection : MonoBehaviour
{
    // so that only blocks are detected and not walls etc
    LayerMask layerMask;

    // Constants
    Color RED;
    Color ORANGE;
    Color YELLOW;
    Color GREEN;
    Color BLUE;
    Color PURPLE;
    Color PINK;
    Color[] ALL_COLORS;
    string[] NAMES;

    public TextMeshProUGUI colorText;

    public Color colorSeen;

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
        NAMES = new string[]{"Red", "Orange", "Yellow", "Green", "Blue", "Purple", "Pink"};

        colorSeen = new Color(0,0,0,1);

        layerMask = LayerMask.GetMask("Block");
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.up, out hit, 40f, layerMask))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            colorSeen = hit.collider.gameObject.GetComponent<Renderer>().sharedMaterial.color;
            // Debug.Log("block detected, color: " + color);
            checkColor();  
        }
        else
        {
            colorText.text = "No Color Detected";
            Debug.DrawLine(transform.position, hit.point, Color.blue);
            colorText.fontSharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, new Color(0,0,0,1));
            // Debug.Log("no block detected");
        }        
    }

    void checkColor()
    {
        Color closestColor = new Color(0,0,0,1);
        float maxDistance = 1.5f;
        float distance = 0f;
        string name = "";
        for(int i = 0; i < ALL_COLORS.Length; i++)
        {
            float rDiff = Mathf.Abs(colorSeen.r - ALL_COLORS[i].r);
            float gDiff = Mathf.Abs(colorSeen.g - ALL_COLORS[i].g);
            float bDiff = Mathf.Abs(colorSeen.b - ALL_COLORS[i].b);
            

            distance = Mathf.Sqrt(rDiff*rDiff + gDiff*gDiff + bDiff*bDiff);
            // Debug.Log("Distance: " + distance);
            if(distance < maxDistance)
            {
                maxDistance = distance;
                closestColor = ALL_COLORS[i];
                name = NAMES[i];
            } 
        }
        // Debug.Log("Predicted Color: " + name);
        colorText.text = name;
        colorText.fontSharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, closestColor);
    }
}

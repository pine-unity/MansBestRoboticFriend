using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalityCalculation : MonoBehaviour
{
    [SerializeField] float hyperactivity;
    [SerializeField] float laziness;

    [SerializeField] float totalPraises;
    [SerializeField] float totalScolds;

    float difference;
    float total;
    float normVal;

    string emotion;

    float happyScale;
    float sadScale;
    float angerScale;

    void Start()
    {
        happyScale = 1.0f;
        sadScale = 1.0f;
        angerScale = 1.0f;

        hyperactivity = 500;
        laziness = 500;
        total = hyperactivity + laziness;

        totalPraises = 0;
        totalScolds = 0;
    }

    void Update()
    {
        difference = hyperactivity - laziness;
        normVal = difference / (total + 1);
    }

    // will implement action-specific praises/scolds later
    public void praiseIncrease()
    {
        totalPraises++;

        // make him more happy :3
        if(happyScale <= 0.5)
        {
            happyScale += 0.5f;
        } else if(happyScale < 1)
        {
            float difference = 1 - happyScale;
            happyScale += 1 - difference;
        }

        Debug.Log(totalPraises);
    }

    public void scoldIncrease()
    {
        totalScolds++;

        // make him more ANGRY >:(
        if(angerScale <= 0.5)
        {
            angerScale += 0.5f;
        } else if(angerScale < 1)
        {
            float difference = 1 - angerScale;
            angerScale += 1 - difference;
        }

        Debug.Log(totalScolds);
    }

    // bayesian probability
    float calculateProbability(float praises, float scolds)
    {
        float finalProbability;

        float a = pA();
        float b = pB(praises, scolds);
        float bGivenA = pBGivenA();

        finalProbability = (bGivenA * a) / b;

        return finalProbability;
    }

    // based on personality traits only
    float pA()
    {
        float probability = 1 / (1 + Mathf.Exp(normVal));
        return probability;
    }

    // action-specific
    float pB(float praises, float scolds)
    {
        float probability = (praises + 1) / (praises + scolds + 2);
        return probability; 
    }

    // global praises/scolds
    float pBGivenA()
    {
        float probability = (totalPraises + 1) / (totalPraises + totalScolds + 2);
        return probability; 
    }
}

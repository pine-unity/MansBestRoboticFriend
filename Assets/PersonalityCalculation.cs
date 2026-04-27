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

    // Start is called before the first frame update
    void Start()
    {
        hyperactivity = 500;
        laziness = 500;
        total = hyperactivity + laziness;

        totalPraises = 0;
        totalScolds = 0;
    }

    // Update is called once per frame
    void Update()
    {
        difference = hyperactivity - laziness;
        normVal = difference / (total + 1);
    }

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

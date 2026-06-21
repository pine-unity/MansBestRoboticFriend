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

    [SerializeField] float happyScale;
    [SerializeField] float sadScale;
    [SerializeField] float angerScale;

    // stages 1-5, newborn, baby, child, young, adult
    [SerializeField] int evolutionStage = 1;

    // baseline stages
    string[] stages = {"Newborn", "Baby", "Child", "Young", "Adult"};

    // stages based on personality
    string[] childTypes = {"Lazy Child", "Rumble Child"};
    string[] youngTypes = {"Lazy Boy", "Bad Boy", "Good Boy"};
    string[] adultTypes = {"Lazy Cat", "Rumble Cat", "Lazy Dog", "Rumble Dog"};
    
    /*

    EVOLUTION TIMES (for reference)
    ***************
    2 h 46 mins --> Newborn to Baby
    8 h 20 mins --> Baby to Child
    27 h 46 mins --> Child to Young
    111 h 06 mins --> Young to Adult

    Per AiboHack: "Evolution (or 'Growing') from one stage 
    happens after a certain amount of 'quality time' is spent 
    at the old stage. If you are interacting with the dog, 
    'quality time' passes at the same rate as real time. 
    If you are ignoring the dog, 'quality time' passes slower 
    than real time."

    Source: https://www.aibohack.com/111/evolve.htm

    */

    void Start()
    {
        happyScale = 0.5f;
        sadScale = 0.5f;
        angerScale = 0.5f;

        hyperactivity = 500;
        laziness = 500;
        total = hyperactivity + laziness;

        totalPraises = 0;
        totalScolds = 0;

        StartCoroutine(shiftMoodsToBaseline()); 
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
            happyScale += 0.005f;
        } else if(happyScale < 1)
        {
            float difference = 1 - happyScale;
            happyScale += difference * 0.01f;
            if(happyScale > 1)
            {
                happyScale = 1f;
            }
        }

        // make him less sad
        if(sadScale > 0)
        {
            sadScale -= (angerScale * 0.1f);
            if(sadScale < 0)
            {
                sadScale = 0f;
            }
        }

        // make him less angry
        if(angerScale > 0)
        {
            angerScale -= 0.005f;
            if(angerScale < 0)
            {
                angerScale = 0f;
            }
        }

        Debug.Log(totalPraises);
    }

    public void scoldIncrease()
    {
        totalScolds++;

        // make him more ANGRY >:(
        if(angerScale <= 0.5)
        {
            angerScale += 0.005f;
        } else if(angerScale < 1)
        {
            float difference = 1 - angerScale;
            angerScale += difference * 0.01f;
            if(angerScale > 1)
            {
                angerScale = 1f;
            }
        }

        // if he's too angry, make him more sad
        if(angerScale > 0.5)
        {
            sadScale = sadScale < 1 ? sadScale + (angerScale * 0.1f) : sadScale;
        }

        // make him less happy
        if(happyScale > 0)
        {
            happyScale -= 0.005f;
            if(happyScale < 0)
            {
                happyScale = 0f;
            }
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
        float probability = 1f / (1f + Mathf.Exp(normVal));
        return probability;
    }

    // action-specific
    float pB(float praises, float scolds)
    {
        // Laplace smoothing to prevent division by 0
        float probability = (praises + 1f) / (praises + scolds + 2f);
        return probability; 
    }

    // global praises/scolds
    float pBGivenA()
    {
        float probability = (totalPraises + 1f) / (totalPraises + totalScolds + 2f);
        return probability; 
    }

    IEnumerator shiftMoodsToBaseline()
    {

        while (true)
        {
            happyScale = Mathf.MoveTowards(happyScale, 0.5f, 0.01f);
            sadScale = Mathf.MoveTowards(sadScale, 0.5f, 0.02f);
            angerScale = Mathf.MoveTowards(angerScale, 0.5f, 0.01f);
            yield return new WaitForSeconds(3f);
        }
        
    }
}

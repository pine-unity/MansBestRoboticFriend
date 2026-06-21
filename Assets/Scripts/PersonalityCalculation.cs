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
    
    string personalityType;

    /*

    EVOLUTION TIMES (for reference)
    ***************
    166 mins --> Newborn to Baby
    500 mins --> Baby to Child
    1666 mins --> Child to Young
    6666 mins --> Young to Adult

    Per AiboHack: "Evolution (or 'Growing') from one stage 
    happens after a certain amount of 'quality time' is spent 
    at the old stage. If you are interacting with the dog, 
    'quality time' passes at the same rate as real time. 
    If you are ignoring the dog, 'quality time' passes slower 
    than real time."

    Source: https://www.aibohack.com/111/evolve.htm

    */

    [SerializeField] float qualityTime = 0f;

    Queue<float> interactions = new Queue<float>();
    float timeWindow = 60f;

    [SerializeField] string qualityTimeType = "Slow";

    void Start()
    {
        personalityType = stages[0];
        Debug.Log("Starting Stage: " + personalityType);

        happyScale = 0.5f;
        sadScale = 0.5f;
        angerScale = 0.5f;

        hyperactivity = 500;
        laziness = 500;
        total = hyperactivity + laziness;

        totalPraises = 0;
        totalScolds = 0;

        StartCoroutine(shiftMoodsToBaseline()); 
        StartCoroutine(incrementTime()); 
    }

    void Update()
    {
        difference = hyperactivity - laziness;
        normVal = difference / (total + 1); 
        determineQualityTimeType();
    }

    void determineQualityTimeType()
    {
        if(interactions.Count > 0)
        {
            if(Time.time - interactions.Peek() > timeWindow)
            {
                qualityTimeType = "Slow";
                interactions.Dequeue();
            }
        }

    }

    IEnumerator incrementTime()
    {
        while (true)
        {
            if (qualityTimeType.Equals("Slow"))
            {
                qualityTime += 0.25f; 
            } else if (qualityTimeType.Equals("Normal"))
            {
                qualityTime += 1;
            }
            yield return new WaitForSeconds(1f);
        }
    
    }

    // will implement action-specific praises/scolds later
    public void praiseIncrease()
    {
        qualityTimeType = "Normal Time";
        totalPraises++;
        interactions.Enqueue(Time.time);

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
        qualityTimeType = "Normal Time";
        totalScolds++;
        interactions.Enqueue(Time.time);

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

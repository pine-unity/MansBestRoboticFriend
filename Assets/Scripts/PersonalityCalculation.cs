using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalityCalculation : MonoBehaviour
{
    // basic personality traits
    [SerializeField] float hyperactivity;
    [SerializeField] float laziness;

    // interactions
    [SerializeField] float totalPraises;
    [SerializeField] float totalScolds;

    // values used for calculating bayesian probability
    float difference;
    float total;
    float normVal;

    // related to the robot's current feelings
    string emotion;
    [SerializeField] float happyScale;
    [SerializeField] float sadScale;
    [SerializeField] float angerScale;

    // stages 1-5, newborn, baby, child, young, adult
    [SerializeField] int evolutionStage;
    // current stage name as a string 
    [SerializeField] string evoStageStr;

    // baseline stages
    string[] stages = {"Newborn", "Baby", "Child", "Young", "Adult"};

    // stages based on personality
    string[] childTypes = {"Lazy Child", "Rumble Child"};
    string[] youngTypes = {"Lazy Boy", "Bad Boy", "Good Boy"};
    string[] adultTypes = {"Lazy Cat", "Rumble Cat", "Lazy Dog", "Rumble Dog"};
    
    [SerializeField] string personalityType;

    // Determining factor to decide whether a Baby
    // turns into a Lazy Child or a Rumble Child
    float shakenIndex;

    // Determines evolution between Child and Young,
    // and Young and Adult. Essentially indicates whether the majority
    // of the current stage was spent in a predominantly "blue" (or cool-colored)
    // room.
    bool blueRoom;

    // Determines whether to evolve a Lazy Boy 
    // to a Lazy Cat or Rumble Cat (Youth to Adult)
    float lSkill;

    // These flags ensure the game
    // doesn't try to evolve to Child/Young/Adult
    // several times
    bool evolvedToChild;
    bool evolvedToYoung;
    bool evolvedToAdult;

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

    [SerializeField] float qualityTime;

    Queue<float> interactions = new Queue<float>();
    float timeWindow = 60f;

    [SerializeField] string qualityTimeType;

    void Start()
    {
        evolvedToChild = false;
        evolvedToYoung = false;
        evolvedToAdult = false;

        shakenIndex = 0f;
        blueRoom = false;
        lSkill = 0f;

        qualityTime = 0f;
        qualityTimeType = "Slow";

        evolutionStage = 0;
        personalityType = "Undetermined";

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
        evoStageStr = stages[evolutionStage];
    }

    void ageUp()
    {
        if(qualityTime < 166)
        {
            // Newborn
            evolutionStage = 0;
        }  
        else if (qualityTime >= 166 && qualityTime < 500)
        {
            // Baby
            evolutionStage = 1;
        } 
        else if (qualityTime >= 500 && qualityTime < 1666)
        {
            // Child
            evolutionStage = 2;
        } 
        else if (qualityTime >= 1666 && qualityTime < 6666)
        {
            // Young
            evolutionStage = 3;
        } 
        else
        {
            // Adult
            evolutionStage = 4;
        }


        determinePersonality();
        
    }

    void determinePersonality()
    {
        if(evolutionStage == 2 && !evolvedToChild)
        {
            evolvedToChild = true;
            // Baby to Child stage
            if(shakenIndex >= 5.0)
            {
                // Rumble Child
                personalityType = childTypes[1];
            } 
            else
            {
                // Lazy Child
                personalityType = childTypes[0];
            }
            
        } 
        else if(evolutionStage == 3 && !evolvedToYoung)
        {
            evolvedToYoung = true;
            // Child to Young stage
            if (blueRoom)
            {
                if (personalityType.Equals(childTypes[0]))
                {
                    // Lazy Boy
                    personalityType = youngTypes[0];
                } 
                else
                {
                    // Good Boy
                    personalityType = youngTypes[2];
                }
                blueRoom = false;
                
            } 
            else
            {
                // Bad Boy
                personalityType = youngTypes[1];
            }
            
            
        } 
        else if(evolutionStage == 4 && !evolvedToAdult)
        {
            evolvedToAdult = true;
            // Young to Adult stage
            if (personalityType.Equals(youngTypes[0]))
            {
                if(lSkill < 5.0)
                {
                    // Lazy Cat
                    personalityType = adultTypes[0];
                } 
                else
                {
                    // Rumble Cat
                    personalityType = adultTypes[1];
                }
            } 
            else if(personalityType.Equals(youngTypes[1]))
            {
                if (blueRoom)
                {
                    // Lazy Dog
                   personalityType = adultTypes[2]; 
                   blueRoom = false;
                }
                else
                {
                    // Rumble Cat
                   personalityType = adultTypes[1]; 
                }

            } 
            else
            {
                if (blueRoom)
                {
                    // Rumble Dog
                    personalityType = adultTypes[3];
                    blueRoom = false;
                }
                else
                {
                    // Lazy Dog
                    personalityType = adultTypes[2];
                }
            }

        } 
        else if(evolutionStage == 0)
        {
            // Newborn or Baby stages
           personalityType = "Undetermined"; 
        }
    
    }

    // determines whether to count time slower or faster 
    // based on user interaction with the robot
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

    // increases the time, to advance the robot 
    // through the evolution stages naturally
    IEnumerator incrementTime()
    {
        
        if (qualityTimeType.Equals("Slow"))
        {
            qualityTime += 0.25f; 
        } 
        else if (qualityTimeType.Equals("Normal"))
        {
            qualityTime += 1;
        }
        ageUp();
        yield return new WaitForSeconds(1f);
        

        StartCoroutine(incrementTime());
    
    }

    // will implement action-specific praises/scolds later
    public void praiseIncrease()
    {
        qualityTimeType = "Normal";
        totalPraises++;
        interactions.Enqueue(Time.time);

        // make him more happy :3
        if(happyScale <= 0.5)
        {
            happyScale += 0.005f;
        } 
        else if(happyScale < 1)
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

        Debug.Log("Total praises: " + totalPraises);
    }

    public void scoldIncrease()
    {
        qualityTimeType = "Normal";
        totalScolds++;
        interactions.Enqueue(Time.time);

        // make him more ANGRY >:(
        if(angerScale <= 0.5)
        {
            angerScale += 0.005f;
        } 
        else if(angerScale < 1)
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

        Debug.Log("Total scolds: " + totalScolds);

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
    
    // ensures the robot doesn't always stay at the same mood forever when 
    // interactions are lacking
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

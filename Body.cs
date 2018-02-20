using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour {

    //child objects of BodyPart types
    //will be added to this at runtime
    //key is name, value is bodypart
    //this makes getting each part O(1)
    //bc this is a hashmap
    public Dictionary<string, BodyPart> bodyParts;

    //to account for blood loss
    public float maxBlood = 100;
    public float baseBleedDamage = 2;
    private float blood;
    private float bloodLossRate;

    //legs
    public float movementSpeed = 5;
    private float moveSpeed;
    //hands
    public float manipulationFactor = 1;
    private float manFact;
    //eyes
    public float sightFactor = 1;
    private float seeFact;
    //ears
    public float hearingFactor = 1;
    private float hearFact;
    //may include nose, mouth, and brain at a later time

    private Rigidbody rigid;

	// Use this for initialization
	void Start () {
        bodyParts = new Dictionary<string, BodyPart>();
        List<BodyPart> parts = new List<BodyPart>();
        getAllBodyParts(gameObject, ref parts);
        foreach(BodyPart part in parts)
        {
            bodyParts.Add(part.name, part);
        }
        blood = maxBlood;
        bloodLossRate = 0;
        moveSpeed = movementSpeed;
        manFact = manipulationFactor;
        seeFact = sightFactor;
        hearFact = hearingFactor;
        rigid = GetComponent<Rigidbody>();
	}

    //using the ref keyword allows a parameter to
    //use an object's reference rather than the object itself
    private void getAllBodyParts(GameObject g, ref List<BodyPart> parts)
    {
        BodyPart part = g.GetComponent<BodyPart>();
        if (part != null)
            parts.Add(part);

        foreach(Transform t in g.transform)
            getAllBodyParts(t.gameObject, ref parts);
        
    }
	
	// Update is called once per frame
	void Update () {
        gaugeHealth();
        bleed();
        float bloodMultiplier = blood / maxBlood;
        if (bloodMultiplier > 0)
        {
            move(bloodMultiplier);
        }
        else
            die();
	}

    //handles blood loss
    private void bleed()
    {
        blood -= bloodLossRate * Time.deltaTime;
    }

    private void clearFactors()
    {
        moveSpeed = 0;
        manFact = 0;
        seeFact = 0;
        hearFact = 0;
    }

    //handles body part bleeding
    //true if bleeding just caused
    //false otherwise
    private bool tryBleed(BodyPart part)
    {
        if (part.justDamaged())
        {
            bloodLossRate += baseBleedDamage * part.percentBloodCapacity;
            return true;
        }

        return false;
    }

    //returns true if found part with name
    //sets part factors
    //returns false if not
    //dont do anything
    private bool setPartAbilities(string name, ref float efficiencyFactor, float factor, int totalOfSameType)
    {
        BodyPart part;
        if(bodyParts.TryGetValue(name, out part)){
            if (part != null)
            {
                efficiencyFactor += part.getEfficiency() * factor / totalOfSameType;
            }
            return true;
        }

        return false;
    }

    //updates abilities and blood loss rates
    //based upon health of body parts
    private void gaugeHealth()
    {
        //every part needs to be able to bleed,
        //regardless of whether or not it affects
        //any action rates
        foreach (BodyPart part in bodyParts.Values)
        {
            //print(part);
            tryBleed(part);
        }

        //explicitly handle the effects of body parts here
        clearFactors();
        setPartAbilities("left_arm", ref manFact, manipulationFactor, 2);
        setPartAbilities("right_arm", ref manFact, manipulationFactor, 2);
        setPartAbilities("left_leg", ref moveSpeed, movementSpeed, 2);
        setPartAbilities("right_leg", ref moveSpeed, movementSpeed, 2);
        //head functions as eyes for now
        setPartAbilities("head", ref seeFact, sightFactor, 1);
        //no ears implemented yet
        hearFact = hearingFactor;
    }

    private void move(float actionMultiplier)
    {

        float x = -Input.GetAxis("Horizontal");
        float y = -Input.GetAxis("Vertical");
        if (x != 0 || y != 0)
        {
            //this is bad, fix to do non jerky movement
            //and stop erroneously faster diagonal movement
            rigid.velocity = actionMultiplier * moveSpeed * (transform.forward * y + transform.right * x + transform.up * rigid.velocity.y);      
        }
        else
            rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
        
    }

    //for obvious things
    //temporary implementation
    private void die()
    {
        Destroy(this);
    }
}

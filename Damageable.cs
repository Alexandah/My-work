using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    public float maxHealth = 100;
    private float currentHealth;

    //1 is completely healthy,
    //0 is dead
    private float healthRatio;

    private bool damaged;
    private float maxSecondsSince;
    private float secondsSince;

    public HashSet<Harmful> invulnerableTo;


	//dont override
	void Start () {
        currentHealth = maxHealth;
        healthRatio = 1;
        damaged = false;
        maxSecondsSince = 1;
        secondsSince = 0;
	}
	
	//dont override
	void Update () {
        customUpdate();

        if (damaged)
        {
            secondsSince += Time.deltaTime;
        }
        if (secondsSince >= maxSecondsSince)
        {
            damaged = false;
            secondsSince = 0;
        }

        //abs to prevent momentarily
        //negative ratios if damage taken
        //is obscenely large
        healthRatio = System.Math.Abs(currentHealth / maxHealth);
        if (currentHealth <= 0)
            die();
	}

    //to be overrided by child
    //classes as need be
    public virtual void customUpdate()
    {

    }

    //dont override
    public bool justDamaged()
    {
        if (damaged)
        {
            damaged = false;
            return true;
        }
        else
            return false;
    }

    //dont override
    public float getHealthRatio()
    {
        return healthRatio;
    }

    //can override
    public void damage(float amt)
    {
        currentHealth -= amt;
    }

    //can override
    public void die()
    {
        Destroy(this.gameObject);
    }

    //dont override
    private void OnCollisionEnter(Collision collision)
    {
        //stop checking if collision cant be harmful
        Harmful hurt = collision.gameObject.GetComponent<Harmful>();
        if (hurt == null)
            return;

        //don't get damaged if it cant hurt you
        if (invulnerableTo.Contains(hurt))
            return;

        //account for recent damage
        damaged = true;
        secondsSince = 0;

        damage(hurt.getHarm());


    }
}

using UnityEngine;

public class Harmful : MonoBehaviour{

    public bool proportionalToVelocity;
    
    //only access directly pre-runtime
    public readonly float harm = 10;

    private float realHarm;

    private Rigidbody rigid;

    void Start()
    {
        realHarm = harm;
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (proportionalToVelocity)
        {
            realHarm = harm * rigid.velocity.magnitude;
        }
    }

    public float getHarm()
    {
        return realHarm;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : Damageable {

    //do not alter during runtime
    public string partName;

    //a number from 0.0 to 1.0 representing
    //how much blood loss damage to this
    //body part will cause proportionally
    //"percentages" of all parts do not
    //need to add to 100%

    //do not alter during runtime
    public float percentBloodCapacity;

    public float getEfficiency()
    {
        return getHealthRatio();
    }

}

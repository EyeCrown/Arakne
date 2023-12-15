using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede : Enemy
{
    public AK.Wwise.Event movingCaterpillar;
    // Start is called before the first frame update
    void Start()
    {
        movingCaterpillar.Post(gameObject);
    }

}

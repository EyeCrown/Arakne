using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    #region ATTRIBUTES
    [Header("Movement")]
    [SerializeField] private float minHangingDistance;
    [SerializeField] private float maxHangingDistance;
    private float hangingDistance;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        Random.Range(minHangingDistance, maxHangingDistance);
    }



    #region MOVEMENT
    protected override void Idle() {
        
    }
    #endregion

}

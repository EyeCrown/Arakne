using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{
    public AK.Wwise.Event SpawnSpider;
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
        SpawnSpider.Post(gameObject);
    }



    #region MOVEMENT
    protected override void Idle() {
        
    }
    #endregion

}

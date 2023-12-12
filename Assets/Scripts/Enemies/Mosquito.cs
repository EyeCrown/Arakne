using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mosquito : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    #region MOVEMENTS
    protected override void Idle()
    {
        if(Mathf.Abs(idleDistance) > idleRange)
        {
            float distance = idleSpeed * Time.deltaTime;
            idleDistance += distance;
            idleSpeed = -idleSpeed;
            transform.parent.position = new Vector3(transform.parent.position.x + distance, transform.parent.position.y);
        }
    }
    #endregion
}

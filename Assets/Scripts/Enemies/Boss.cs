using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform ballSpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region EVENT HANDLER
    protected override void HitHandler(int damage)
    {
        Debug.Log("Boss got hit");
        animator.SetInteger("Progress", animator.GetInteger("Progress")+1);

    }
    #endregion
}

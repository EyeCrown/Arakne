using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Gameplay Attributes")]
    [SerializeField] protected float fallSpeed;
    [SerializeField] protected float idleSpeed;
    [SerializeField] protected float idleRange;
    [SerializeField] protected float wiggleSpeed;
    [SerializeField] protected float wiggleRange;
    [SerializeField] protected int health;
    [SerializeField] protected int scorePerHealthPoint;
    protected float idleDistance;
    #endregion

    #region EVENTS
    public UnityEvent<int> Hit;
    #endregion

    #region UNITY API
    void Awake()
    {
        Hit.AddListener(HitHandler);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Fall();
        Idle();
        Wiggle();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //TODO player hit
        }
    }

    #endregion

    #region EVENT HANDLERS
    void HitHandler(int dammage)
    {
        if (health < dammage) {
            dammage = health; 
        }
        //TODO notify the game manager here score = dammage * scorePerHealthPoint
        health -= dammage;
        if (health <= 0)
        {
            //TODO Die here.
            Destroy(gameObject);
        }
    }


    #endregion

    #region MOVEMENTS
    protected virtual void Fall()
    {
        transform.position = transform.position + Vector3.down * fallSpeed * Time.deltaTime;
    }
    protected virtual void Idle() { }
    protected virtual void Wiggle()
    {
        Vector2 randomdirection = Random.insideUnitSphere;
        transform.localPosition = transform.position + new Vector3(randomdirection.x, randomdirection.y);
        if(transform.localPosition.magnitude > wiggleRange)
        {
            transform.localPosition = transform.localPosition.normalized * wiggleRange;
        }
        
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Gameplay Attributes")]
    [SerializeField] protected float fallSpeed = 7;
    [SerializeField] protected float idleSpeed;
    [SerializeField] protected float idleRange;
    [SerializeField] protected float wiggleSpeed;
    [SerializeField] protected float wiggleRange;
    [SerializeField] protected int health = 10;
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
            collision.gameObject.GetComponent<PlayerController>().Hit.Invoke();
        } else if (collision.gameObject.CompareTag("MapEnd"))
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region EVENT HANDLERS
    protected virtual void HitHandler(int damage)
    {
        Debug.Log("Enemy: hit");
        if (health < damage) {
            damage = health; 
        }
        GameManager.Instance.ScoreChange.Invoke(damage * scorePerHealthPoint);
        health -= damage;
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
        //Vector2 randomdirection = Random.insideUnitSphere;
        //transform.localPosition = transform.position + new Vector3(randomdirection.x, randomdirection.y);
        //if(transform.localPosition.magnitude > wiggleRange)
        //{
        //    transform.localPosition = transform.localPosition.normalized * wiggleRange;
        //}
        
    }
    #endregion
}

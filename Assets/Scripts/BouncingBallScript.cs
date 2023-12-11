using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class BouncingBallScript : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Ball Movement")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedLimit;

    [Header("Ball Gameplay")]
    [SerializeField] private int health = 2;
    [SerializeField] private int dammage = 1;
    [SerializeField] private int power = 0;
    [SerializeField] private int pass = 0;
    [SerializeField] private bool bouncingMode = true;
    public GameObject target;
    #endregion


    #region UNITY API
    // Start is called before the first frame update
    void Start()
    {
        //rigidBody.AddForce(transform.up * speed);
    }

    // Update is called once per frame
    void Update()
    {
        BallMove();
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ball Collision");
        //int layerMask = ~0;
        //if (collision.gameObject.CompareTag("Wall") || (collision.gameObject.CompareTag("Enemy") && bouncingMode))
        //{
        //    foreach (ContactPoint contact in collision.contacts)
        //    {
        //        Vector3 reflectVec = Vector3.Reflect(transform.up, contact.normal);
        //        reflectVec.z = 0;
        //        transform.up = reflectVec.normalized;
        //        if(collision.gameObject.CompareTag("Enemy"))
        //        {
        //            speed = speed + speed * speedMultiplier;
        //        }
        //    }
        //}
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, collision.transform.position, out hit, (transform.position - collision.transform.position).magnitude,layerMask))
        //{
        //    Debug.Log("Raycast hit");
        //    Vector3 reflectVec = Vector3.Reflect(transform.up, hit.normal);
        //    reflectVec.z = 0;
        //    transform.up = reflectVec.normalized;
        //}

    }
    #endregion

    #region METHODS
    private void BallMove()
    {
        if(bouncingMode)
        {
            BallPredictBounces();
        } else
        {
            if(target != null)
            {
                MoveTowardTarget();
            } else
            {
                Debug.Log("No target set for the ball");
            }
        }

       
    }

    private void BallPredictBounces()
    {
        int layerMask = ~0;
        float travelDistance = speed * Time.deltaTime;
        RaycastHit hit;
        int bounces = 0;
        int maxbounces = 10;

        //Predicting all bounces that can happens during the frame. 
        while (travelDistance > 0 && bounces < maxbounces)
        {
            if (Physics.SphereCast(transform.position, transform.localScale.y / 2, transform.up, out hit, travelDistance, layerMask)
                && (hit.collider.gameObject.CompareTag("Wall") || (hit.collider.gameObject.CompareTag("Enemy") && bouncingMode)))
            {
                Debug.Log("ShpereCast hit");
                Vector3 reflectVec = Vector3.Reflect(transform.up, hit.normal);
                reflectVec.z = 0;
                transform.up = reflectVec.normalized;
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                      ApplySpeedMultiplier();
                }
                CheckCollisions(hit.distance);
                transform.position = transform.position + transform.up * hit.distance;
                travelDistance -= hit.distance;
                bounces++;
            }
            else
            {
                CheckCollisions(travelDistance);
                transform.position += transform.up * travelDistance;
                travelDistance = 0;
            }
            Debug.Log("Travel Distance remaining:" + travelDistance);
        }
    }

    private void MoveTowardTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        transform.up = direction.normalized;
        float length = speed * Time.deltaTime < direction.magnitude ? speed * Time.deltaTime : direction.magnitude;
        CheckCollisions(length);
        transform.position = transform.up * length;
    }


    private void CheckCollisions(float length)
    {
        int layerMask = ~0;
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, transform.localScale.y / 2, transform.up, length, layerMask);
        foreach(RaycastHit hit in hits)
        {
            if(hit.transform.gameObject.CompareTag("Enemy"))
            {
                CollideEnemy(hit.transform.gameObject);
            } else if(hit.transform.gameObject.CompareTag("Boss"))
            {
                CollideBoss(hit.transform.gameObject);
            } else if(hit.transform.gameObject.CompareTag("Player"))
            {
                CollidePlayer(hit.transform.gameObject);
            }
        }
    }

    private void CollideEnemy(GameObject enemy)
    {
        health--;
        //TODO Event collide enemy
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void CollidePlayer(GameObject enemy)
    {
        //TODO Event collide player
        Destroy(gameObject);
    }

    private void CollideBoss(GameObject enemy)
    {
        //TODO Event Collide Boss
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 start = transform.position;
        Vector3 direction = transform.up;
        RaycastHit hit;

        for (int i = 0; i < 4; i++)
        {
            if (Physics.SphereCast(start, 0.5f, direction, out hit, Mathf.Infinity, ~0))
            {
                Gizmos.DrawLine(start, start + direction.normalized * hit.distance);
                Gizmos.DrawSphere(start + direction.normalized * hit.distance, 0.5f);
                start = start + direction.normalized * hit.distance;
                direction = Vector3.Reflect(direction, hit.normal);
            }
        }


        }



        public void ApplySpeedMultiplier()
        {
            speed = speed + speed * speedMultiplier;
            if (speed > speedLimit)
            {
                speed = speedLimit;
            }
        }

    #endregion

}




using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class BouncingBallScript : MonoBehaviour
{
    [Header("Ball movement")]
    [SerializeField] private float speed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedLimit;

    [Header("Ball Gameplay")]
    [SerializeField] private bool bouncingMode = true;
    public GameObject target;

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



    private void BallMove()
    {
        int layerMask = ~0;
        float travelDistance = speed * Time.deltaTime;
        RaycastHit hit;
        //Predicting all bounces that can happens during the frame. 
        while (travelDistance > 0)
        {
            if (Physics.SphereCast(transform.position + transform.up * 0.02f, transform.localScale.y / 2, transform.up, out hit, travelDistance, layerMask))
            {
                Debug.Log("ShpereCast hit");
                if (hit.collider.gameObject.CompareTag("Wall") || (hit.collider.gameObject.CompareTag("Enemy") && bouncingMode))
                {
                    Vector3 reflectVec = Vector3.Reflect(transform.up, hit.normal);
                    reflectVec.z = 0;
                    transform.up = reflectVec.normalized;
                    if (hit.collider.gameObject.CompareTag("Enemy") && speed < speedLimit)
                    {
                        speed = speed + speed * speedMultiplier;
                        if(speed > speedLimit)
                        {
                            speed = speedLimit;
                        }
                    }
                    transform.position = transform.position + transform.up*hit.distance;
                    travelDistance -= hit.distance;

                }
            }
            else
            {
                transform.position +=  transform.up * travelDistance;
                travelDistance = 0;
            }
            Debug.Log("Travel Distance remaining:" + travelDistance);

        }
    }

    private void CollidingEnemy(GameObject enemy)
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 start = transform.position;
        Vector3 direction = transform.up;
        RaycastHit hit;

            if (Physics.SphereCast(start, 0.5f, direction, out hit, Mathf.Infinity, ~0))
            {
                Gizmos.DrawLine(start, start + direction.normalized * hit.distance);
                Gizmos.DrawSphere(start + direction.normalized*hit.distance, 0.5f);
            }
      

    }


}




using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;




public class BouncingBallScript : MonoBehaviour
{
    #region ATTRIBUTES
    [Header("Ball Movement")]
    [SerializeField] private float speed = 10;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedLimit;
    private GameObject target;
    [SerializeField] private BallMode mode = BallMode.bouncing;

    [Header("Ball Gameplay")]
    [SerializeField] private int power = 0;
    [SerializeField] private int maxPower = 1;
    [SerializeField] private int pass = 0;
    [SerializeField] private int health;
    private bool canHitPlayer = true;

    [Header("Sound")]
    public AK.Wwise.Event ThrowSound;
    public AK.Wwise.Event PassSound;
    public AK.Wwise.Event BounceSound;

    public enum BallMode
    {
        bouncing,
        homing,
        grabbed,
        fall
    }
    #endregion

    #region EVENTS
    public UnityEvent Grab;
    public UnityEvent<Vector3> Throw;
    public UnityEvent<GameObject> Pass;
    #endregion

    #region UNITY API
    // Start is called before the first frame update
    void Start()
    {
        Grab.AddListener(GrabHandler);
        Throw.AddListener(ThrowHandler);
        Pass.AddListener(PassHandler);

        //mode = BallMode.fall;
        //transform.up = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    {
        BallMove();
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 start = transform.position;
        Vector3 direction = transform.up;
        RaycastHit hit;

        for (int i = 0; i < health; i++)
        {
            if (Physics.SphereCast(start, 0.5f, direction, out hit, Mathf.Infinity, 1))
            {
                Gizmos.DrawLine(start, start + direction.normalized * hit.distance);
                Gizmos.DrawSphere(start + direction.normalized * hit.distance, 0.5f);
                start = start + direction.normalized * hit.distance;
                direction = Vector3.Reflect(direction, hit.normal);
            }
        }
    }
    #endregion

    #region METHODS
    private void BallMove()
    {
        switch (mode)
        {
            case BallMode.bouncing:
                PredictBounces();
                break;
            case BallMode.homing:
                if (target != null)
                {
                    MoveTowardTarget();
                }
                else
                {
                    Debug.Log("No target set for the ball");
                }
                break;
            case BallMode.grabbed:
                break;
            case BallMode.fall:
                Fall();
                break;
            default:
                break;
        }

       
    }

    private void PredictBounces()
    {
        int layerMask = 1;
        float travelDistance = speed * Time.deltaTime;
        RaycastHit hit;
        int bounces = 0;
        int maxbounces = 10;

        //Predicting all bounces that can happens during the frame. 
        while (travelDistance > 0 && bounces < maxbounces)
        {
            if (Physics.SphereCast(transform.position, transform.localScale.y / 2, transform.up, out hit, travelDistance, layerMask)
                && (hit.collider.gameObject.CompareTag("Wall") || (hit.collider.gameObject.CompareTag("Enemy") && mode == BallMode.bouncing)))
            {
                //Debug.Log("ShpereCast hit");
                Vector3 reflectVec = Vector3.Reflect(transform.up, hit.normal);
                reflectVec.z = 0;
                transform.up = reflectVec.normalized;
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    CollideEnemy(hit.collider.gameObject);
                } else if(hit.collider.gameObject.CompareTag("Wall"))
                {
                    CollideWall();
                }
                BounceSound.Post(gameObject);
                CheckCollisions(hit.distance);
                TranslateForward(hit.distance);
                travelDistance -= hit.distance;
                bounces++;
            }
            else
            {
                CheckCollisions(travelDistance);
                TranslateForward(travelDistance);
                travelDistance = 0;
            }
            //Debug.Log("Travel Distance remaining:" + travelDistance);
        }
    }

    private void MoveTowardTarget()
    {
        Vector3 direction = target.transform.position - transform.position;
        transform.up = direction.normalized;
        float distance = speed * Time.deltaTime < direction.magnitude ? speed * Time.deltaTime : direction.magnitude;
        CheckCollisions(distance);
        TranslateForward(distance);
    }

    private void Fall()
    {
        TranslateForward(speed * Time.deltaTime);
    }

    private void TranslateForward(float distance)
    {
        transform.position += transform.up * distance;
    }

    private void CheckCollisions(float length)
    {
        int layerMask = ~4;
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

    public void ApplySpeedMultiplier()
    {
        speed = speed + speed * speedMultiplier;
        if (speed > speedLimit)
        {
            speed = speedLimit;
        }
    }

    #endregion


    #region COLLISION HANDLERS
    private void CollideEnemy(GameObject enemy)
    {
        if(mode == BallMode.fall)
        {
            return;
        }
        health--;
        Enemy enemyHit = enemy.GetComponent<Enemy>();
        if (enemyHit)
        {
            enemyHit.Hit.Invoke(power);
        }
        if(health <= 0)
        {
            //TODO death effect
            Destroy(gameObject);
        }
    }

    private void CollidePlayer(GameObject player)
    {
        if(!canHitPlayer)
        { return; }
        //TODO Event collide player
        player.GetComponent<PlayerController>().Hit.Invoke();
        Destroy(gameObject);
    }

    private void CollideBoss(GameObject boss)
    {
        //TODO Event Collide Boss
        Destroy(gameObject);
    }

    private void CollideWall()
    {
        health--;
        if (health <= 0)
        {
            //TODO death effect
            Destroy(gameObject);
        }
    }

    #endregion


    #region EVENT HANDLERS

    private void GrabHandler()
    {
       // Debug.Log("Ball: GrabHandler before: " + mode);
        /*if ((int)mode != (int)BallMode.fall || (int)mode != (int)BallMode.homing)
        {
            return;
        }*/
        if (mode == BallMode.fall || mode == BallMode.homing)
        mode = BallMode.grabbed;

        //Debug.Log("Ball: GrabHandler after: " + mode);
    }
    private void ThrowHandler(Vector3 direction)
    {
        Debug.Log("Ball: ThrowHandler");
        if ((int)mode != (int)BallMode.grabbed)
        {
            return;
        }
        StartCoroutine(ThrowCoroutine());
        transform.up = direction;
        mode = BallMode.bouncing;
        ThrowSound.Post(gameObject);
    }
    private void PassHandler(GameObject newTarget)
    {
        if((int)mode != (int)BallMode.fall && (int)mode != (int)BallMode.homing)
        {
            return;
        }
        //AK.SoundEngine.SetRTPCValue("PassCount", power);
        ApplySpeedMultiplier();
        target = newTarget;
        mode = BallMode.homing;
    }
    #endregion

    IEnumerator ThrowCoroutine()
    {
        canHitPlayer = false;

        yield return new WaitForSeconds(0.5f);
        canHitPlayer = true;
    }
}




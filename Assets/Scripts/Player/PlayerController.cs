using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    #region ATTRIBUTES
    // Visible values (made for GD)
    [SerializeField]
    [Tooltip("Player's Inertia")]
    [Range(1f, 10f)]
    private float inertia; // range 1 to 10

    [SerializeField]
    [Tooltip("Player's actual speed, lower it is, faster it is")]
    [Range(1f, 100f)]
    private float speed; // lower it is, faster it is | range 1 to 100

    [SerializeField] private int health = 3;

    // Cooldowns
    [SerializeField]
    [Tooltip("Time to aim before shooting")]
    private float timeToShoot;
    [SerializeField]
    [Tooltip("Invulnerabiltiy time after taking damage")]
    private float invincibilityTime;


    [SerializeField] private Animator animator;

    // Hidden values
    private Vector3 movements;
    private Vector3 velocity;

    public bool isAlive { get; private set; }
    public bool isHittable { get; private set; }
    private bool canMove;

    private GameObject viewfinder;
    private BallDetector ballDetector;

    private Transform bottomLeftBorder;
    private Transform topRightBorder;

    public int ID { get; private set; }
    #endregion

    #region EVENTS
    public UnityEvent Hit;
    #endregion

    #region UNITY API
    void Start()
    {
        Revive();
        canMove = true;

        Hit.AddListener(HitHandler);

        DontDestroyOnLoad(gameObject);

        viewfinder = transform.Find("Pivot").gameObject;
        viewfinder.SetActive(false);
        ballDetector = GetComponentInChildren<BallDetector>();

        bottomLeftBorder = GameObject.Find("BottomLeftBorder").transform;
        topRightBorder = GameObject.Find("TopRightBorder").transform;
    }

    void Update()
    {
        if (canMove) 
            DoMovements();

        viewfinder.transform.up = movements.normalized;
    }


    private void OnCollisionEnter(Collision collision)
    {
        movements = Vector3.zero;
    }

    #endregion

    #region METHODS
    public void Initialize(int id)
    {
        ID = id;
        Revive();
    }

    private void DoMovements()
    {
        Vector3 nextPosition = transform.position + movements * inertia;

        if (nextPosition.x < bottomLeftBorder.position.x)
            nextPosition.x = bottomLeftBorder.position.x;

        if (nextPosition.x > topRightBorder.position.x)
            nextPosition.x = topRightBorder.position.x;

        if (nextPosition.y < bottomLeftBorder.position.y)
            nextPosition.y = bottomLeftBorder.position.y;

        if (nextPosition.y > topRightBorder.position.y)
            nextPosition.y = topRightBorder.position.y;

        transform.position = Vector3.SmoothDamp(transform.position, nextPosition, ref velocity, speed * Time.deltaTime);
    }

    private void DoPass()
    {
        GameObject otherPlayer = GameManager.Instance.GetOtherPlayer(ID);
        if (otherPlayer != null)
            ballDetector.ball.GetComponent<BouncingBallScript>().Pass.Invoke(otherPlayer);
        else
            DoShoot();
    }

    private void DoShoot()
    {
        isHittable = false;
        canMove = false;
        viewfinder.SetActive(true);

        if (ballDetector.ball != null)
            ballDetector.ball.GetComponent<BouncingBallScript>().Grab.Invoke();

        StartCoroutine(ShootCoroutine());
    }

    private void TakeDamage()
    {
        if (isHittable)
        {
            StartCoroutine(InvicibilityCoroutine());
            health--;

            if (health <= 0)
                Die();
        }
    }

    private void Revive()
    {
        StartCoroutine(InvicibilityCoroutine());

        isAlive = true;
        health = GameManager.Instance.maxHealth;
    }

    private void Die()
    {
        Debug.Log("Player die");
        isAlive = false;
        isHittable = false;
        GameManager.Instance.PlayerDie.Invoke(ID);
        //TODO: put anim dead here

    }
    #endregion

    #region INPUTS
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movements = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0f);
        }
        else if (context.canceled)
        {
            movements = Vector3.zero;
        }
    }

    public void Pass(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isAlive && ballDetector.ball)
            {
                DoPass();
            }
            else
                Debug.Log("Pass: Ball is missing");
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isAlive && ballDetector.ball)
            {
                DoShoot();
            }
            else
                Debug.Log("Shoot: Ball is missing");
        }

        if (context.canceled && !canMove)
            canMove = true;
    }
    #endregion;

    #region EVENT HANDLERS
    public void HitHandler()
    {
        if (isAlive)
            TakeDamage();
        else
            Revive();
        animator.SetInteger("HealthPoint", health);
    }
    #endregion

    #region COROUTINES
    IEnumerator ShootCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        yield return new WaitForSeconds(timeToShoot);
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        
        if (ballDetector.ball != null)
        {
            Vector3 direction;
            if (movements != Vector3.zero)
                direction = movements;
            else
                direction = Vector3.up;
            ballDetector.ball.GetComponent<BouncingBallScript>().Throw.Invoke(direction);

        }

        viewfinder.SetActive(false);
        isHittable = true;
    }

    IEnumerator InvicibilityCoroutine()
    {
        isHittable = false;
        yield return new WaitForSeconds(invincibilityTime);
        isHittable = true;
    }
    #endregion
}

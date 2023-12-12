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
    [Tooltip("Recovery time before shoot again")]
    private float timeToShoot;
    [SerializeField]
    [Tooltip("Invulnerabiltiy time after taking damage")]
    private float invincibilityTime;

    // Hidden values
    private Vector3 movements;
    private Vector3 velocity;

    private bool isAlive;
    public bool isHittable { get; private set; }
    private bool canMove;

    private GameObject viewfinder;
    private BallDetector ballDetector;

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
    }

    void Update()
    {
        if (canMove) 
            DoMovements();

        viewfinder.transform.up = movements.normalized;
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
        StartCoroutine(ShootCoroutine());
    }


    public void HitHandler()
    {
        if (isAlive)
            TakeDamage();
        else
            Revive();
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
    }
    #endregion;

    #region COROUTINES
    IEnumerator ShootCoroutine()
    {
        
        isHittable = false;
        canMove = false;
        viewfinder.SetActive(true);

        ballDetector.ball.GetComponent<BouncingBallScript>().Grab.Invoke();

        yield return new WaitForSeconds(timeToShoot);

        ballDetector.ball.GetComponent<BouncingBallScript>().Throw.Invoke(movements);

        viewfinder.SetActive(false);
        canMove = true;
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

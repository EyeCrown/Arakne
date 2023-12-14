using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
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
    private float hittableReloadTime;
    [SerializeField]
    [Tooltip("Time before player can perform action (Shoot/Pass)")]
    private float doActionReloadTime;


    [Header("Animation and FX")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Color throwColor;
    [SerializeField] private Color passColor;

    // Hidden values
    private Vector3 movements;
    private Vector3 velocity;

    public bool isAlive { get; private set; }
    public bool isHittable { get; private set; }

    private bool canMove;
    private bool canDoAction;

    private bool isShooting;

    private GameObject viewfinder;
    private BallDetector ballDetector;

    private Transform bottomLeftBorder;
    private Transform topRightBorder;


    public int ID { get; private set; }
    #endregion

    #region EVENTS
    public UnityEvent<bool> Hit;
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player was hit by an enemy");
            Hit.Invoke(false);
        } else
        {
            movements = Vector3.zero;
        }
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

    private void DoPass(GameObject ball)
    {
        Debug.Log("Enter pass " + ball.GetComponent<BouncingBallScript>().mode);
        if (ball.GetComponent<BouncingBallScript>().mode == BouncingBallScript.BallMode.bouncing &&
            ball.GetComponent<BouncingBallScript>().mode == BouncingBallScript.BallMode.grabbed)
        {
            //Debug.Log("Alors d�gage !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            return;
        }
        Debug.Log("Do pass");
        GameObject otherPlayer = GameManager.Instance.GetOtherPlayer(ID);
        Debug.Log(otherPlayer.name);
        if (otherPlayer != null)
        {
            ballDetector.ball.GetComponent<BouncingBallScript>().Pass.Invoke(otherPlayer);
            particle.startColor = passColor;
            particle.Play();
        }
        else
        {
            DoShoot(ball);
            Debug.Log("Shoot du coup");
        }
    }

    private void DoShoot(GameObject ball)
    {
        if ((int)ball.GetComponent<BouncingBallScript>().mode == (int)BouncingBallScript.BallMode.homing
            || (int)ball.GetComponent<BouncingBallScript>().mode == (int)BouncingBallScript.BallMode.fall)
        {
            Debug.Log("Shoot");
            isShooting = true;
            isHittable = false;
            
            viewfinder.SetActive(true);

            if (ball != null)
                ball.GetComponent<BouncingBallScript>().Grab.Invoke();

            StartCoroutine(ShootCoroutine(ball));
        }

        
    }

    private void TakeDamage()
    {
        if (isHittable)
        {
            StartCoroutine(HittableCoroutine());
            health--;
            Debug.Log("Player take damage");
        }
        if (health <= 0)
            Die();
    }

    private void Revive()
    {
        StartCoroutine(HittableCoroutine());
        isAlive = true;
        health = GameManager.Instance.maxHealth;
        canDoAction = true;
        isShooting = false;
    }

    private void Die()
    {
        Debug.Log("Player die");
        isAlive = false;
        isHittable = false;
        GameManager.Instance.PlayerDie.Invoke(ID);
        //TODO: put anim dead here

    }

    public void SetAnimatorController(AnimatorController controller)
    {
        animator.runtimeAnimatorController = controller;
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
        if (isAlive && canDoAction && context.performed)
        {
            StartCoroutine(DoActionCoroutine());
            
            if (ballDetector.ball)
            {
                DoPass(ballDetector.ball);


            }
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (isAlive && canDoAction && context.performed)
        {
            if (canMove)
                canMove = false;
            StartCoroutine(DoActionCoroutine());

            if (ballDetector.ball)
                DoShoot(ballDetector.ball);
        }

        if (context.canceled && !isShooting)
            canMove = true;
    }
    #endregion;

    #region EVENT HANDLERS
    public void HitHandler(bool fromBouncingBall)
    {
        if (isAlive)
            TakeDamage();
        else if (fromBouncingBall)
            Revive();
        animator.SetInteger("HealthPoint", health);
    }
    #endregion

    #region COROUTINES
    IEnumerator ShootCoroutine(GameObject ball)
    {
        yield return new WaitForSeconds(timeToShoot);
        
        if (!canMove && isShooting)
            canMove = true;

        if (ball != null)
        {
            Vector3 direction;
            if (movements != Vector3.zero)
                direction = movements;
            else
                direction = Vector3.up;
            ball.GetComponent<BouncingBallScript>().Throw.Invoke(direction);
            particle.startColor = throwColor;
            particle.Play();

        }

        viewfinder.SetActive(false);
        isHittable = true;
        isShooting = false;
        
    }

    IEnumerator HittableCoroutine()
    {
        isHittable = false;
        yield return new WaitForSeconds(hittableReloadTime);
        isHittable = true;
    }

    IEnumerator DoActionCoroutine()
    {
        canDoAction = false;
        yield return new WaitForSeconds(doActionReloadTime);
        canDoAction = true;
    }

    #endregion
}

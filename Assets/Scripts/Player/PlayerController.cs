using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField] private int hp = 3;

    // Cooldowns
    [SerializeField]
    private float timeToShoot;


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

    #region UNITY API
    void Start()
    {
        isAlive = true;
        isHittable = true;
        canMove = true;

        DontDestroyOnLoad(gameObject);

        viewfinder = transform.Find("Pivot").gameObject;
        viewfinder.SetActive(false);
        ballDetector = GetComponentInChildren<BallDetector>();
    }

    void Update()
    {
        if (isAlive)
        {
            if (canMove) 
                DoMovements();
        }

        viewfinder.transform.up = movements.normalized;
    }
    #endregion

    #region METHODS
    public void Initialize(int id)
    {
        ID = id;
    }

    private void DoMovements()
    {
        Vector3 nextPosition = transform.position + movements * inertia;
        transform.position = Vector3.SmoothDamp(transform.position, nextPosition, ref velocity, speed * Time.deltaTime);
    }

    public void TakeDamage()
    {
        hp--;
        if (hp < 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player die");
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
            if (ballDetector.ball)
            {
                Debug.Log("Pass: Do something with " + ballDetector.ball.name);
                ballDetector.ball.GetComponent<BouncingBallScript>().Pass.Invoke(GameManager.Instance.GetOtherPlayer(ID));
            }
            else
                Debug.Log("Pass: Ball is missing");
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (ballDetector.ball)
            {
                //Debug.Log("Shoot: Do something with " + ballDetector.ball.name);
                StartCoroutine(ShootCoroutine());
            }
            else
                Debug.Log("Shoot: Ball is missing");
        }
    }
    #endregion;

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

}

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
    //[SerializeField]
    private float passCooldown;


    // Hidden values
    private Vector3 movements;
    private Vector3 velocity;

    private bool isAlive;

    private BallDetector ballDetector;
    #endregion

    #region UNITY API
    void Start()
    {
        isAlive = true;
        DontDestroyOnLoad(gameObject);

        ballDetector = GetComponentInChildren<BallDetector>();
    }

    void Update()
    {
        if (isAlive)
        {
            DoMovements();
        }
        

    }


    #endregion

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
            Debug.Log("Pass: Action is performed");
            if (ballDetector.ball)
            {
                Debug.Log("Pass: Do something with " + ballDetector.ball.name);
            }
            else
                Debug.Log("Pass: Ball is missing");
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Shoot: Action is performed");
            if (ballDetector.ball)
            {
                Debug.Log("Shoot: Do something with " + ballDetector.ball.name);
            }
            else
                Debug.Log("Shoot: Ball is missing");
        }
    }
    #endregion
}

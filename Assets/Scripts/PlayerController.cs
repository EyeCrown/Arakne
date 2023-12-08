using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Visible values (made for GD)
    // TODO: make it more readable for others
    [SerializeField] private float inertia; // range 1 to 10
    [SerializeField] private float speed; // lower it is, faster it is | range 1 to 100
    
    // Hidden values
    private Vector3 movements;
    private Vector3 velocity;

    void Start()
    {
        
    }

    void Update()
    {
        DoMovements();

    }


    private void DoMovements()
    {
        Vector3 nextPosition = transform.position + movements * inertia;
        transform.position = Vector3.SmoothDamp(transform.position, nextPosition, ref velocity, speed * Time.deltaTime);
    }


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
            Debug.Log("Pass: Do something");
        }
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Shoot: Do something");
        }
    }
}

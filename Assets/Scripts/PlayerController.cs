using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 movements;


    void Start()
    {
        
    }

    void Update()
    {
        
    }


    void Move(InputAction.CallbackContext context)
    {
        
    }

    void Pass(InputAction.CallbackContext context)
    {
        Debug.Log("Pass: Do something");
    }

    void Shoot(InputAction.CallbackContext context)
    {
        Debug.Log("Shoot: Do something");
    }
}

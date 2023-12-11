using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDetector : MonoBehaviour
{
    public GameObject ball { get; private set; }


    void Start()
    {
        ball = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            ball = other.gameObject;
        }
        else
        {
            Debug.Log(other.name + " doesn't have a tag 'Ball'");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ball = null;
    }
}

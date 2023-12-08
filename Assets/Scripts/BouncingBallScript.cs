using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBallScript : MonoBehaviour
{
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, hit.transform, out hit, (transform.position - hit.transform).magnitude, layerMask))
        {
            Vector3 reflectVec = Vector3.Reflect(transform.forward, hit.normal);
            transform.forward = reflectVec.normalized();
        }

    }
}

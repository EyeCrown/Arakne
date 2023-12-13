using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class States : MonoBehaviour
{
    [SerializeField] int etat = 0;


 

    // Start is called before the first frame update
    void Start()
    {
    

    }

    // Update is called once per frame
    void Update()
    {
        if (etat > 2)
        {
            
            Debug.Log("sup 2");

        }

        if (etat < 2)
        {

            Debug.Log("inf 2");
        }
    }
}

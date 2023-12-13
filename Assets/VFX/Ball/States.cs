using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class States : MonoBehaviour
{
    [SerializeField] int etat = 0;
    [SerializeField] SpriteRenderer render_1;
    [SerializeField] SpriteRenderer render_2;
    [SerializeField] TrailRenderer trail;
    [SerializeField] ParticleSystem particle;

    [SerializeField] Color color_1;
    [SerializeField] Color color_2;
    [SerializeField] Color color_3;
    [SerializeField] Color color_4;
    [SerializeField] Color color_5;
    [SerializeField] Color color_6;
 

    // Start is called before the first frame update
    void Start()
    {
    

    }

    // Update is called once per frame
    void Update()
    {
        //switch (etat)
        //{
        //    case 0:
        //        render_1;
        //        break;
        //    default:
        //        break;
        //}
        if (etat == 0)
        {
            ChangeColor(color_1);
         
        }

        else if (etat == 1)
        {

            ChangeColor(color_2);
        }
        else if (etat == 2)
        {

            ChangeColor(color_3);
        }
        else if (etat == 3)
        {

            ChangeColor(color_4);
        }
        else if (etat == 4)
        {

            ChangeColor(color_5);
        }
        else if (etat == 5)
        {

            ChangeColor(color_6);
        }
       
    }
    void ChangeColor(Color color)
    {
        render_1.material.EnableKeyword("_EMISSION");
        render_1.material.SetColor("_EmisColor", color);
        render_2.material.SetColor("_EmisColor", color);
        trail.startColor = color;
        particle.startColor = color;
    }
}

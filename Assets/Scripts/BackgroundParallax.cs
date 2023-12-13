using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [System.Serializable] public class LayerAndSpeed{
        public GameObject gameObject;
        public float speed;
    }

    [SerializeField] private List<LayerAndSpeed> layers = new List<LayerAndSpeed>();


    void Start()
    {

    }


    void Update()
    {
        foreach(LayerAndSpeed layer in layers)
        {
            //TODO Find a way to make it work without translation the solution under doesn't work
            layer.gameObject.GetComponent<SpriteRenderer>().material.mainTextureOffset += new  Vector2(0,layer.gameObject.GetComponent<SpriteRenderer>().material.mainTextureOffset.x + layer.speed/200f * Time.deltaTime);
            //layer.gameObject.transform.position += layer.gameObject.transform.up * layer.speed * Time.deltaTime;
        }
    }
}

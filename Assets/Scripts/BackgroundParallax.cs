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
            layer.gameObject.GetComponent<SpriteRenderer>().material.mainTextureOffset += new  Vector2(layer.gameObject.GetComponent<SpriteRenderer>().material.mainTextureOffset.x + layer.speed * Time.deltaTime,0);
        }
    }
}

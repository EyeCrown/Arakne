using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    #region ATTRIBUTES
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    #endregion

    #region EVENTS
    public UnityEvent<int> Hit;
    #endregion

    #region UNITY API
    private void Awake()
    {
        Hit.AddListener(HitHandler);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region EVENT HANDLERS
    void HitHandler(int damage)
    {
        //TODO notify the game manager here
        health -= damage;
        if (health <= 0) {

        }
    }
    #endregion
}

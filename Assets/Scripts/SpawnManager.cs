using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject enemy;
    private float timerWitness = 0;
    private int timer = 0;
    private bool timerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //lance le timer
        if (Input.GetKeyDown("space"))
        {
            timerOn = true;
        }

        //met à jour le timer
        if(timerOn)
        {
            timerWitness += Time.deltaTime;
        }

        timer = Mathf.RoundToInt(timerWitness);

        if (timer == 3)
        {
            SpawnEnemy(-2);
            SpawnEnemy(0);
            SpawnEnemy(2);
        }

        if (timer == 5)
        {
            SpawnEnemy(0);
        }

        if (timer == 7)
        {
            SpawnEnemy(-2);
            SpawnEnemy(2);
        }
    }


    void SpawnEnemy(int xPos)
    {
        Instantiate(enemy, new Vector3(xPos, 3, 0), Quaternion.identity);
    }
}

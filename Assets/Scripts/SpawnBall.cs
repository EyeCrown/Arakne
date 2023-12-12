using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    
    public GameObject ball;
    public GameObject enemy;

    public Transform limit;

    private void Start()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
    {
        Vector3 position = new Vector3(Random.Range(transform.position.x, limit.position.x), transform.position.y, 0f);
        Instantiate(ball, position, transform.rotation);
        Vector3 enemyPos = new Vector3(Random.Range(transform.position.x, limit.position.x), transform.position.y, 0f);
        Instantiate(enemy, enemyPos, transform.rotation);
        yield return new WaitForSeconds(3f);

        StartCoroutine(spawn());
    }
}

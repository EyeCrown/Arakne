using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] protected float ballSpawnTime = 5;
    [SerializeField] protected float ballAnimationTime = 1;
    public AK.Wwise.Event BossHit;
    public AK.Wwise.Event BossLastHit;

    // Start is called before the first frame update
    void Start()
    {
        ballSpawnPoint.transform.position = new Vector3(ballSpawnPoint.transform.position.x, ballSpawnPoint.transform.position.y);
        ballSpawnPoint.up = Vector3.down;
        StartCoroutine(BallSpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Die()
    {
        GameManager.Instance.GameWin();
    }

    #region EVENT HANDLER
    protected override void HitHandler(int damage)
    {
        health -= damage;
        Debug.Log("Boss got hit");
        animator.SetInteger("Progress", 8 - (health*8)/maxHealth);
        GameManager.Instance.ScoreChange.Invoke(damage * scorePerHealthPoint);
        BossHit.Post(gameObject);
        if (health <= 0)
        {   
            Die();
            BossLastHit.Post(gameObject);
        }
    }
    #endregion


    IEnumerator AnimateSpawnBallCoroutine()
    {
        animator.SetTrigger("ThrowBall");
        yield return new WaitForSeconds(ballAnimationTime);
        Instantiate(ballPrefab, ballSpawnPoint.position,ballSpawnPoint.rotation);
    }

    IEnumerator BallSpawnCoroutine()
    {
        if (GameManager.Instance.ballCount <= 0)
        {
            StartCoroutine(AnimateSpawnBallCoroutine());
        }
        //StartCoroutine(AnimateSpawnBallCoroutine());
        yield return new WaitForSeconds(ballSpawnTime);

        StartCoroutine(BallSpawnCoroutine());
    }
}

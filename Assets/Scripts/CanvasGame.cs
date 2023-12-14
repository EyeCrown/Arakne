using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGame : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject looseScreen;
    [SerializeField] private GameObject panelScoreScreen;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject boss;
    [SerializeField] private Slider bossHealthBar;
    //[SerializeField] private Slider player1Bar;
    //[SerializeField] private Slider player2hBar;


    private void Start()
    {
        Debug.Log("Canvas start");
        GameManager.Instance.canvas = this;
        UpdateScore(GameManager.Instance.score);
        winScreen.SetActive(false);
        looseScreen.SetActive(false);
        scoreText.text = "0";

        bossHealthBar.maxValue = boss.GetComponent<Boss>().GetHealth();
        bossHealthBar.minValue = 0;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void DisplayWinScreen()
    {
        winScreen.SetActive(true);
        winScreen.GetComponent<score_display>().SetScoreToText();
        scoreText.enabled = false;
        panelScoreScreen.SetActive(false);
    }

    public void DisplayLooseScreen()
    {
        looseScreen.SetActive(true);
        looseScreen.GetComponent<score_display>().SetScoreToText();
        scoreText.enabled = false;
        panelScoreScreen.SetActive(false);
    }


    public void UpdateBossHealth()
    {
        bossHealthBar.value = boss.GetComponent<Boss>().GetHealth();
    }

}

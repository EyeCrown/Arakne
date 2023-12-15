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

    [SerializeField] private Slider[] playersBar;

    [SerializeField] private GameObject[] heartP1;
    [SerializeField] private GameObject[] heartP2;

    [SerializeField] public TextMeshProUGUI multiplicatorTMP;
    [SerializeField] public TextMeshProUGUI timerTMP;
    [SerializeField] public GameObject timer;
    [SerializeField] public GameObject healthBar;
    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;



    private void Start()
    {
        Debug.Log("Canvas start");
        GameManager.Instance.canvas = this;
        UpdateScore(GameManager.Instance.score);
        winScreen.SetActive(false);
        looseScreen.SetActive(false);
        SetActiveGameplayUI(true);
        scoreText.text = "0";

        bossHealthBar.maxValue = boss.GetComponent<Boss>().GetHealth();
        bossHealthBar.minValue = 0;

        foreach (Slider slider in playersBar)
        {
            slider.minValue = 0;
            slider.maxValue = GameManager.Instance.maxHealth;
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void DisplayWinScreen()
    {
        winScreen.SetActive(true);
        winScreen.GetComponent<score_display>().SetScoreToText();
        SetActiveGameplayUI(false);
    }

    public void DisplayLooseScreen()
    {
        looseScreen.SetActive(true);
        looseScreen.GetComponent<score_display>().SetScoreToText();
        SetActiveGameplayUI(false);
    }

    void SetActiveGameplayUI(bool state)
    {
        scoreText.enabled = state;
        timer.SetActive(state);
        player1.SetActive(state);
        player2.SetActive(state);
        healthBar.SetActive(state);
        panelScoreScreen.SetActive(state);
    }
    public void UpdateBossHealth()
    {
        bossHealthBar.value = boss.GetComponent<Boss>().GetHealth();
    }

    public void UpdatePlayerHealth(int id, int hp)
    {
        Debug.Log("Switch : " + hp);
        switch (hp)
        {
            case 0:
                if (id == 0)
                {
                    heartP1[0].SetActive(false);
                    heartP1[1].SetActive(false);
                    heartP1[2].SetActive(false);
                }
                else
                {
                    heartP2[0].SetActive(false);
                    heartP2[1].SetActive(false);
                    heartP2[2].SetActive(false);
                }
                break;
            case 1:
                if (id == 0)
                {
                    heartP1[1].SetActive(false);
                    heartP1[2].SetActive(false);
                }
                else
                {
                    heartP2[1].SetActive(false);
                    heartP2[2].SetActive(false);
                }
                break;
            case 2:
                if (id == 0)
                {
                    heartP1[2].SetActive(false);
                }
                else
                {
                    heartP2[2].SetActive(false);
                }
                break;
            case 3:
                if (id == 0)
                {
                    heartP1[0].SetActive(true);
                    heartP1[1].SetActive(true);
                    heartP1[2].SetActive(true);
                }
                else
                {
                    heartP2[0].SetActive(true);
                    heartP2[1].SetActive(true);
                    heartP2[2].SetActive(true);
                }
                break;
            default:
                Debug.LogError("UpdatePlayerHealth: Error invalid health > " + hp);
                break;
        }
    }

    public void UpdateMultiplier(int value)
    {
        multiplicatorTMP.text = "x" + value;
    }

    public void UpdateTimer(float value)
    {
        float minutes = value / 60f;
        float seconds = value % 60f;
        timerTMP.text = Mathf.FloorToInt(minutes) + ":" + Mathf.FloorToInt(seconds);
    }


}

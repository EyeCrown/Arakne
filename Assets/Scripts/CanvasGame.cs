using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasGame : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject looseScreen;
    [SerializeField] private GameObject panelScoreScreen;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        Debug.Log("Canvas start");
        GameManager.Instance.canvas = this;
        UpdateScore(GameManager.Instance.score);
        winScreen.SetActive(false);
        looseScreen.SetActive(false);
        scoreText.text = "0";
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

}

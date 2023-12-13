using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;


public class GameManager : MonoBehaviour
{
    public int score { get; private set; }
    public int maxHealth = 3;
    public int multiplier = 1;
    public int ballCount = 0;


    [SerializeField] private List<Transform> spawnPositions;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject looseScreen;
    [SerializeField] private TextMeshProUGUI scoreText;

    #region EVENTS
    public UnityEvent<int> ScoreChange;
    public UnityEvent<int> PlayerDie;
    #endregion

    public GameObject[] players { get; set; }

    //private PlayerInputManager playerInputManager;

    private string gameScene = "GameTestScene";

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        ScoreChange.AddListener(ScoreChangeHandler);
        PlayerDie.AddListener(PlayerDieHandler);
        players = new GameObject[2];
    }

    void Start()
    {

    }

    public void ChangeScene()
    {
        Debug.Log("Change scene");

        SceneManager.LoadScene(gameScene);
    }

    public GameObject GetOtherPlayer(int playerId)
    {
        if (players.Length <= 1)
            return null;
        else
            return players[GetOtherPlayerId(playerId)];
    }

    private int GetOtherPlayerId(int myId)
    {
        return myId == 0 ? 1 : 0;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);

        winScreen = GameObject.Find("/Canvas/WinScreenEndGame");
        winScreen.SetActive(false);
        looseScreen = GameObject.Find("/Canvas/LooseScreenEndGame");
        looseScreen.SetActive(false);

        scoreText = GameObject.Find("/Canvas/UI_ScoreText").GetComponent<TextMeshProUGUI>();
        Debug.Log(scoreText.text);

        spawnPositions[0] = GameObject.Find("SpawnPosJ1").transform;
        players[0].transform.position = spawnPositions[0].position;

        spawnPositions[1] = GameObject.Find("SpawnPosJ2").transform;
        players[1].transform.position = spawnPositions[1].position;


        Debug.Log("___GAME START___");

        score = 0;
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        Debug.Log("___GAME OVER___");
        Debug.Log("Score: " + score);
        looseScreen.SetActive(true);
        //TODO: Make game over
    }

    public void GameWin()
    {
        Debug.Log("___GAME WIN___");
        Debug.Log("Score: " + score);
        winScreen.SetActive(true);
    }

    #region EVENT HANDLERS
    private void ScoreChangeHandler(int points)
    {
        Debug.Log("GameManager: Score += " + points);
        score += points;
        scoreText.text = score.ToString();
    }

    private void PlayerDieHandler(int idPlayer)
    {
        if (!players[idPlayer].GetComponent<PlayerController>().isAlive 
            && !players[GetOtherPlayerId(idPlayer)].GetComponent<PlayerController>().isAlive)
        {
            GameOver();
        }
    }
    #endregion
}

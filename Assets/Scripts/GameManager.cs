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

    private TextMeshProUGUI scoreText;

    [SerializeField] private List<Transform> spawnPositions;

    #region EVENTS
    public UnityEvent<int> ScoreChange;
    public UnityEvent<int> PlayerDie;
    #endregion

    public GameObject[] players { get; private set; }

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
        scoreText = GameObject.Find("UI_ScoreText")?.GetComponent<TextMeshProUGUI>();
        //StartGame();
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

        players[0].transform.position = spawnPositions[0].position;
        players[1].transform.position = spawnPositions[1].position;

        players[0].GetComponent<PlayerController>().Initialize(0);
        players[1].GetComponent<PlayerController>().Initialize(1);

        Debug.Log("___GAME START___");

        score = 0;
        scoreText.text = score.ToString();
    }

    private void GameOver()
    {
        Debug.Log("___GAME OVER___");
        Debug.Log("Score: " + score);
        //TODO: Make game over
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

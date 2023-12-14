using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using UnityEditor.Animations;

public class GameManager : MonoBehaviour
{
    public int score { get; private set; }
    public int maxHealth = 3;
    public int multiplier = 1;
    public int ballCount = 0;
    public AK.Wwise.Event ContainerMusic;
    public CanvasGame canvas;
    #region EVENTS
    public UnityEvent<int> ScoreChange;
    public UnityEvent<int> PlayerDie;
    #endregion

    public GameObject[] players { get; set; }
    [SerializeField] private AnimatorController[] animators;
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
        ContainerMusic.Post(gameObject);
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

    public void SetAnimator(int idPlayer)
    {
        players[idPlayer].GetComponent<PlayerController>().SetAnimatorController(animators[idPlayer]);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        
        Vector3 spawnPosJ0 = new Vector3(-5, -10, 0);
        players[0].transform.position = spawnPosJ0;

        Vector3 spawnPosJ1 = new Vector3(5, -10, 0);
        players[1].transform.position = spawnPosJ1;

        Debug.Log("___GAME START___");
        score = 0;
    }

    public void GameOver()
    {
        Debug.Log("___GAME OVER___");
        Debug.Log("Score: " + score);
        canvas.DisplayLooseScreen();
        //TODO: Make game over
    }

    public void GameWin()
    {
        Debug.Log("___GAME WIN___");
        Debug.Log("Score: " + score);
        canvas.DisplayWinScreen();

    }

    #region EVENT HANDLERS
    private void ScoreChangeHandler(int points)
    {
        Debug.Log("GameManager: Score += " + points);
        score += points;
        canvas.UpdateScore(score);
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

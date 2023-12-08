using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    


    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        playerInputManager.EnableJoining();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (Globals.gameState != Globals.GameState.OnMenu) return; // Re-activated player on respawn triggers OnPlayerJoined

        Debug.Log("Player " + playerInput.playerIndex + " joined");

        if (playerInputManager.playerCount == playerInputManager.maxPlayerCount)
            ChangeScene();
    }

    public void ChangeScene()
    {
        playerInputManager.DisableJoining();
        Debug.Log("Change scene");
        string sceneToLoad = "PlayerTestScene";
        SceneManager.LoadScene(sceneToLoad);
    }
}

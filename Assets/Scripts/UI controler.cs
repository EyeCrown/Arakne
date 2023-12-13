using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UIcontroler : MonoBehaviour
{
    
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject playPanel;

    [SerializeField] EventSystem eventsystem;
    
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject playButton;

    //[SerializeField] GameObject GameManager;

    //[SerializeField] AudioSource audioSource;

    private PlayerInputManager playerInputManager;

    private void Start()
    {
        //controls = GetComponent<GameObject>();
        controlsPanel.SetActive(false);  
        creditsPanel.SetActive(false);
        //GameManager.SetActive(false);
        playPanel.SetActive(false);

        //audioSource.Play();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        
    }


    public void OpenControlsPanel()
    {
        controlsPanel.SetActive(true);
        eventsystem.SetSelectedGameObject(backButton);
    }

    public void OpencreditsPanel()
    {
        creditsPanel.SetActive(true);
        eventsystem.SetSelectedGameObject(backButton);

    }

    public void ClosePanel()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        playPanel.SetActive(false);
        eventsystem.SetSelectedGameObject(playButton);

        playerInputManager.DisableJoining();
    }


    public void OpenPlayPanel()
    {
        playPanel.SetActive(true);
        playerInputManager.EnableJoining();

        if (true)
        {
            Debug.Log("Players can join");
        }
        //SceneManager.LoadSceneAsync(2);
        eventsystem.SetSelectedGameObject(backButton);

    }

    public void QuitGame()
   {
      Application.Quit();
      UnityEditor.EditorApplication.isPlaying = false;
   }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (Globals.gameState != Globals.GameState.OnMenu) return; // Re-activated player on respawn triggers OnPlayerJoined
        
        Debug.Log("Player " + playerInput.playerIndex + " joined");

        if (playerInputManager.playerCount == playerInputManager.maxPlayerCount)
        {
            playerInputManager.DisableJoining();
            GameManager.Instance.StartGame();
        }
            
    }
}

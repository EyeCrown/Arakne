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
    
    [SerializeField] GameObject backButtonFromCredits;
    [SerializeField] GameObject backButtonFromControls;
    [SerializeField] GameObject playButton;

    [SerializeField] GameObject[] buttonObjectList;

    //[SerializeField] AudioSource audioSource;

    private PlayerInputManager playerInputManager;

    private void Start()
    {
        controlsPanel.SetActive(false);  
        creditsPanel.SetActive(false);
        playPanel.SetActive(false);

        //audioSource.Play();
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        
    }

    public void OpenPlayPanel()
    {
        playPanel.SetActive(true);
        playerInputManager.EnableJoining();
        DesactivateHomeButtons();
    }

    public void OpenControlsPanel()
    {
        controlsPanel.SetActive(true);
        eventsystem.SetSelectedGameObject(backButtonFromControls);
        DesactivateHomeButtons();
    }

    public void OpencreditsPanel()
    {
        creditsPanel.SetActive(true);
        eventsystem.SetSelectedGameObject(backButtonFromCredits);
        DesactivateHomeButtons();
    }

    public void ClosePanelFromCredits()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        playPanel.SetActive(false);
        eventsystem.SetSelectedGameObject(playButton);

        playerInputManager.DisableJoining();
        ActivateHomeButtons();
    }

    public void ClosePanelFromControls()
    {
        controlsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        playPanel.SetActive(false);
        eventsystem.SetSelectedGameObject(playButton);

        playerInputManager.DisableJoining();
        ActivateHomeButtons();
    }

    public void QuitGame()
    {
      Application.Quit();
      //UnityEditor.EditorApplication.isPlaying = false;
    }

    private void ActivateHomeButtons()
    {
        foreach (GameObject button in buttonObjectList)
        {
            button.SetActive(true);
        }
    }
    private void DesactivateHomeButtons()
    {
        foreach (GameObject button in buttonObjectList)
        {
            button.SetActive(false);
        }
    }



    public void OnPlayerJoined(PlayerInput playerInput)
    {
        if (Globals.gameState != Globals.GameState.OnMenu) return; // Re-activated player on respawn triggers OnPlayerJoined
        
        GameManager.Instance.players[playerInput.playerIndex] = playerInput.gameObject;
        GameManager.Instance.players[playerInput.playerIndex].GetComponent<PlayerController>().Initialize(playerInput.playerIndex);
        GameManager.Instance.SetAnimator(playerInput.playerIndex);

        if (playerInputManager.playerCount == playerInputManager.maxPlayerCount)
        {
            playerInputManager.DisableJoining();
            GameManager.Instance.StartGame();
        }
    }
}

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

    [SerializeField] GameObject GameManager;

    //[SerializeField] AudioSource audioSource;
    

    private void Start()
    {
        //controls = GetComponent<GameObject>();
        controlsPanel.SetActive(false);  
        creditsPanel.SetActive(false);
        GameManager.SetActive(false);
        playPanel.SetActive(false);

        //audioSource.Play();

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
        controlsPanel.SetActive(false );
        creditsPanel.SetActive(false);
        playPanel.SetActive(false);
        eventsystem.SetSelectedGameObject(playButton);
       
    }


    public void OpenPlayPanel()
    {
        playPanel.SetActive(true);
        GameManager.SetActive(true);

        if (GameManager == true)
        {
            Debug.Log("Game Manager is activated");
        }
        //SceneManager.LoadSceneAsync(2);
        //eventsystem.SetSelectedGameObject(backButton);

    }

    public void QuitGame()
   {
      Application.Quit();
      UnityEditor.EditorApplication.isPlaying = false;
   }
}

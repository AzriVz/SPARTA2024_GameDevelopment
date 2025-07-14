using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject option; 
    [SerializeField] private GameObject pauseButton; 
    [SerializeField] private Texture2D[] pauseSprite;
    bool isPause;

    private void Start()
    {
        AudioManager.instance.PlayMusic("TitleScreen");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else Time.timeScale = 1;
    }
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0); 
    }
    public void Pause()
    {
        if (!isPause)
        {
            pauseButton.GetComponent<RawImage>().texture = pauseSprite[0];
            isPause = true;
            option.SetActive(true);
        }
        else
        {
            pauseButton.GetComponent<RawImage>().texture = pauseSprite[1];
            isPause = false;
            option.SetActive(false);
        }
    }
    public void PlayGame()
    {
        var ie = FindFirstObjectByType<DialogueRunner>();    
        var au = AudioManager.instance;
        au.PlayMusic("Perkenalan");
        ie.StartDialogue("Prolog");
    }
    
    [YarnCommand("LoadScene")]
    public static void LoadScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}

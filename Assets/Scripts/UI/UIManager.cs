using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public float currentTime = 0.0f;
    int m_framesCount = 0;
    public Text timerText;
    public Text gameResult;

    [SerializeField] List<GameObject> dontDestroyUIList;

    private void Start()
    {
        ResetUI();
        LoadUI();
    }

    private void Update()
    {
        var gameStatus = GameManager.Get().GetGameStatus();
        if (gameStatus == GameManager.GameStatus.InProgress)
        {
            m_framesCount++;
            currentTime += Time.deltaTime;
            UpdateTimer();
        }
        else if(gameStatus == GameManager.GameStatus.Ended)
        {
            UpdateGameResult();
        }    
    }

    public void Awake()
    {
        foreach(var uiElement in dontDestroyUIList)
        {
            if (uiElement)
            {
                DontDestroyOnLoad(uiElement);
            }
        }
    }

    private void UpdateTimer()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        if (timerText)
        {
            timerText.text = time.ToString(@"mm\:ss\:fff");
        }
    }

    private void UpdateGameResult()
    {
        if (!gameResult)
        {
            return;
        }

        if(gameResult.text.Length != 0)
        {
            return;
        }

        gameResult.text = "Game has been finished by destroying all escapers \n";
        gameResult.text += ("Game time is: " + timerText.text + "\n");

        var pursuerCounter = 1;
        var pursuers = GameManager.Get().GetPusuers();
        foreach(var pursuer in pursuers)
        {
            if(!pursuer)
            {
                continue;
            }

            var value = pursuer.GetTravelDistance();
            gameResult.text += ("Pursuer " + pursuerCounter.ToString() + ": \n");
            gameResult.text += ("completed distance - " + pursuer.GetTravelDistance().ToString() + "\n");
            pursuerCounter++;
        }
    }

    public void OnStartButtonClicked()
    {
        GameManager.Get().StartGame();
        ResetUI();
    }

    private void ResetUI()
    {
        currentTime = 0.0f;
        m_framesCount = 0;

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        if (timerText)
        {
            timerText.text = time.ToString(@"mm\:ss\:fff");
        }

        if (gameResult)
        {
            gameResult.text = "";
        }

    }

    private void LoadUI()
    {
        var pursuers = GameManager.Get().GetPusuers();
    }
    
    public void OnAddEscaperClicked()
    {
        //GameManager.Get().AddEscaper();
    }

    public void OnDropDownChanged(Dropdown dropDown)
    {
        switch (dropDown.value)
        {
            case 0:
                SceneManager.LoadScene("MainScene");
                break;
            case 1:
                SceneManager.LoadScene("Scene2");
                break;
            default:
                // code block
                break;
        }
        ResetUI();
    }

    Player GetPursuer()
    {
        var pursuers = GameManager.Get().GetPusuers();

        if (pursuers.Count == 0)
        {
            return null;
        }

        return pursuers[0];
    }

}

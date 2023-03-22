using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float currentTime = 0.0f;
    int m_framesCount = 0;
    public Text timerText;
    public Text gameResult;

    private void Start()
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        if (timerText)
        {
            timerText.text = time.ToString(@"mm\:ss\:fff");
        }
        if(gameResult)
        {
            gameResult.text = "";
        }
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
    }

    public void OnAddEscaperClicked()
    {
        //GameManager.Get().AddEscaper();
    }
}

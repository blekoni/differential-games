using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text timerText;
    public Text gameResult;

    [SerializeField] List<GameObject> dontDestroyUIList;
    [SerializeField] GameObject m_startButton;
    [SerializeField] GameObject m_stopButton;
    [SerializeField] GameObject m_resetButton;

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
            UpdateTimer();
        }
        else if(gameStatus == GameManager.GameStatus.Ended)
        {
            UpdateGameResult();
        }

        if (m_startButton)
        {
            m_startButton.SetActive(gameStatus != GameManager.GameStatus.InProgress);
        }

        if (m_stopButton)
        {
            m_stopButton.SetActive(gameStatus == GameManager.GameStatus.InProgress);
        }

        if(m_resetButton)
        {
            m_resetButton.SetActive(gameStatus == GameManager.GameStatus.Ended);
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
        TimeSpan time = TimeSpan.FromSeconds(GameManager.Get().GetGameTime());
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

        gameResult.gameObject.SetActive(true);

        var gamRes = GameManager.Get().GetGameResult();

        gameResult.text = " Game has been ";
        if(gamRes.result == GameManager.FinishGame.AllEscapersDestroyed)
        {
            gameResult.text += "finished since all escapers were destroyed.\n ";
        }
        else if (gamRes.result == GameManager.FinishGame.EscaperOutOfZone)
        {
            gameResult.text += "finished since all escapers are out of game area.\n ";
        }
        else if(gamRes.result == GameManager.FinishGame.OutOfTime)
        {
            gameResult.text += "finished since no time left.\n ";
        }
        else if (gamRes.result == GameManager.FinishGame.StoppedByUser)
        {
            gameResult.text += "stopped by user.\n ";
        }
        gameResult.text += ("Game time is: " + timerText.text + "\n ");
        gameResult.text += ("Distance completed by pursuer: " + gamRes.distanceCompByPursuer.ToString() + "\n ");
        gameResult.text += ("Distance completed by escaper: " + gamRes.distanceCompByEscaper.ToString() + "\n ");
    }

    public void OnStartButtonClicked()
    {
        GameManager.Get().StartGame();
        ResetUI();
    }

    public void OnStopButtonClicked()
    {
        GameManager.Get().StopGame(GameManager.FinishGame.StoppedByUser);
        //ResetUI();
    }

    public void OnResetButtonClicked()
    {
        GameManager.Get().ResetGame();
        ResetUI();
    }


    private void ResetUI()
    {
        if (gameResult)
        {
            gameResult.gameObject.SetActive(false);
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
}

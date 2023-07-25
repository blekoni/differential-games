using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text timerText;
    [SerializeField] private GameResultUI m_gameResult;

    private void Start()
    {
        ResetUI();
    }

    private void Update()
    {
        var gameStatus = GameManager.Get().GetGameStatus();
        if (gameStatus == GameManager.GameStatus.InProgress)
        {
            UpdateTimer();
        }
    }

    public void Awake()
    {
        m_instance = this;
    }

    private void UpdateTimer()
    {
        TimeSpan time = TimeSpan.FromSeconds(GameManager.Get().GetCurrentGameTime());
        if (timerText)
        {
            timerText.text = time.ToString(@"mm\:ss\:fff");
        }
    }

    public void ShowGameResult(GameManager.GameResult gameResult)
    {
        m_gameResult.ShowResult(gameResult);
    }

    public void HideGameResult()
    {
        m_gameResult.HideResult();
    }

    public void ResetUI()
    {
        m_gameResult.HideResult();
    }

    private static UIManager m_instance;

    public static UIManager Get()
    {
        return m_instance;
    }
}

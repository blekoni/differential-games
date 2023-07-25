using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Text timerText;
    [SerializeField] private GameResultUI m_gameResultUI;
    [SerializeField] private PlayerUI m_playerUI;


    private void Start()
    {
        HideGameResult();
    }

    private void Update()
    {
        var gameStatus = GameManager.Get().GetGameStatus();
        if (gameStatus == GameManager.GameStatus.InProgress)
        {
            UpdateTimer();
        }
    }

    private void Awake()
    {
        Debug.Assert(m_gameResultUI);
        Debug.Assert(m_playerUI);
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
        m_gameResultUI.ShowResult(gameResult);
    }

    public void HideGameResult()
    {
        m_gameResultUI.HideResult();
    }

    public void ShowPlayerUI(GameObject player)
    {
        m_playerUI.Show(player);
    }

    public void HidePlayerUI()
    {
        m_playerUI.Hide();
    }
}

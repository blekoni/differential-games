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
    [SerializeField] private SettingsUI m_settingsUI;

    private void Start()
    {
        HideGameResult();
    }

    private void Update()
    {
    }

    private void Awake()
    {
        Debug.Assert(m_gameResultUI);
        Debug.Assert(m_playerUI);
        Debug.Assert(m_settingsUI);
    }

    public void UpdateTimer(float gameTime)
    {
        TimeSpan time = TimeSpan.FromSeconds(gameTime);
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

    public void SetStartButtonActive(bool flag)
    {
        m_settingsUI.SetStartButtonActive(flag);
    }

    public void ResetUI()
    {
        HideGameResult();
        HidePlayerUI();
        UpdateTimer(0.0f);
    }

    public void RefreshUI()
    {
        m_playerUI.RefreshUI();
    }
}

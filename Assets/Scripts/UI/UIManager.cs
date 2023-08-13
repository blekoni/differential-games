using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TimerUI m_timerUI;
    [SerializeField] private GameResultUI m_gameResultUI;
    [SerializeField] private PlayerUI m_playerUI;
    [SerializeField] private SettingsUI m_settingsUI;

    private void Start()
    {
        HideGameResult();
    }

    private void Awake()
    {
        Debug.Assert(m_gameResultUI);
        Debug.Assert(m_playerUI);
        Debug.Assert(m_settingsUI);
        Debug.Assert(m_timerUI);
    }

    public void UpdateTimer(float gameTime)
    {
        m_timerUI.UpdateTimer(gameTime);
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
        m_settingsUI.RefreshUI();
    }
}

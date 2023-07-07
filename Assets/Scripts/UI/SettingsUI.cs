using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] Dropdown m_dropdown;
    [SerializeField] Button m_startButton;
    [SerializeField] InputField m_timeInput;

    public static SettingsUI m_instance;

    private void Start()
    {
        m_timeInput.gameObject.active = false;
    }

    private void Update()
    {
        var gameStatus = GameManager.Get().GetGameStatus();
        UpdateStartButton(gameStatus);
    }

    private void UpdateStartButton(GameManager.GameStatus gameStatus)
    {
        switch(gameStatus)
        {
            case GameManager.GameStatus.NotStarted:
                m_startButton.GetComponentInChildren<Text>().text = "Start";
                break;
            case GameManager.GameStatus.InProgress:
                m_startButton.GetComponentInChildren<Text>().text = "Stop";
                break;
            case GameManager.GameStatus.Ended:
                m_startButton.GetComponentInChildren<Text>().text = "Reset";
                break;
            default:
                break;
        }
    }

    public void OnStartButtonClicked()
    {
        var gameStatus = GameManager.Get().GetGameStatus();
        switch (gameStatus)
        {
            case GameManager.GameStatus.NotStarted:
                GameManager.Get().StartGame();
                UIManager.Get().ResetUI();
                break;
            case GameManager.GameStatus.InProgress:
                GameManager.Get().StopGame(GameManager.FinishGame.StoppedByUser);
                break;
            case GameManager.GameStatus.Ended:
                GameManager.Get().ResetGame();
                UIManager.Get().ResetUI();
                break;
            default:
                break;
        }
    }

    public void OnDropDownChanged(Dropdown dropDown)
    {
        if(!dropDown || !m_timeInput)
        {
            return;
        }

        switch (dropDown.value)
        {
            case 0:
                m_timeInput.gameObject.active = false;
                GameManager.Get().SetGameType(GameManager.GameType.TypicalGame);
                break;
            case 1:
                m_timeInput.gameObject.active = true;
                m_timeInput.text = GameManager.Get().GetGameTime().ToString();
                GameManager.Get().SetUntilTimeGameType(Convert.ToInt32(m_timeInput.text));
                break;
            case 2:
                m_timeInput.gameObject.active = false;
                GameManager.Get().SetGameType(GameManager.GameType.UntilOutOfZone);
                break;
            default:
                break;
        }
    }

    public void OnTimeInputChanged(InputField inputField)
    {
        var str = inputField.text;
        if (str.Length == 0)
        {
            return;
        }

        var toInteger = Convert.ToInt32(str);
        if (toInteger == null)
        {
            return;
        }

        GameManager.Get().SetUntilTimeGameType(toInteger);
    }

}

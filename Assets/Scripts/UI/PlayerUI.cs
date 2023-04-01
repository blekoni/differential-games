using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] InputField m_positionInputX;
    [SerializeField] InputField m_positionInputY;
    [SerializeField] Slider m_slider;
    [SerializeField] Dropdown m_dropdown;

    [SerializeField] Player m_pursuer;
    private void Start()
    {
        m_pursuer = GetPursuer();
        SetPlayerInfo();
    }

    private void SetPlayerInfo()
    {
        var position = m_pursuer.transform.position;
        m_positionInputX.text = position.x.ToString();
        m_positionInputY.text = position.z.ToString();
        m_slider.value = m_pursuer.GetSpeed();
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

    public void OnDropDownPursuerChanged(Dropdown dropDown)
    {
        var pursuer = GetPursuer();
        if (pursuer == null)
        {
            return;
        }

        switch (dropDown.value)
        {
            case 0:
                //SceneManager.LoadScene("MainScene");
                break;
            case 1:
                //SceneManager.LoadScene("Scene2");
                break;
            default:
                // code block
                break;
        }
    }

    public void OnPositionYChange(InputField fieldX)
    {
        var str = fieldX.text;
        if (str.Length == 0)
        {
            return;
        }
        var toDouble = Convert.ToDouble(str);
        if (toDouble == null)
        {
            return;
        }

        var pursuer = GetPursuer();
        if (pursuer != null)
        {
            pursuer.transform.position = new Vector3(pursuer.transform.position.x, pursuer.transform.position.y, (float)toDouble);
        }
    }

    public void OnPositionXChange(InputField fieldX)
    {
        var str = fieldX.text;
        if (str.Length == 0)
        {
            return;
        }
        var toDouble = Convert.ToDouble(str);
        if (toDouble == null)
        {
            return;
        }

        var pursuer = GetPursuer();
        if (pursuer != null)
        {
            pursuer.transform.position = new Vector3((float)toDouble, pursuer.transform.position.y, pursuer.transform.position.z);
        }
    }

    public void OnSpeedChange(Slider slider)
    {
        var pursuer = GetPursuer();
        if (pursuer != null)
        {
            pursuer.SetSpeed(slider.value);
        }
    }


}

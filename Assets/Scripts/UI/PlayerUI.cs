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
    [SerializeField] Text m_playerName;

    [SerializeField] Text m_additionalText;
    [SerializeField] InputField m_additionalInputX;
    [SerializeField] InputField m_additionalInputY;

    Player m_player;

    private void Start()
    {
    }

    public void Show(GameObject obj)
    {
        var player = obj.GetComponent<Player>();
        ShowPlayerInfo(player);
        gameObject.SetActive(true);

        m_player = player;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        m_player = null;
    }

    private void ShowPlayerInfo(Player player)
    {
        if (!player)
        {
            return;
        }

        m_playerName.text = player.tag;

        var position = player.transform.position;
        m_positionInputX.text = position.x.ToString();
        m_positionInputY.text = position.z.ToString();
        m_slider.value = player.GetSpeed();

        UpdateBehaviour(player);
        UpdateAdditional(player);
    }

    private void UpdateBehaviour(Player player)
    {
        if (!player || !m_dropdown)
        {
            return;
        }

        m_dropdown.ClearOptions();

        Behavior.BehaviorType behaviorType = player.GetBehaviorType();

        if (player.CompareTag("Pursuer"))
        {
            List<string> options = new List<string> { "Simple pursuit", "Parallel pursuit" };
            m_dropdown.AddOptions(options);

            if(behaviorType == Behavior.BehaviorType.SimplePursuit)
            {
                m_dropdown.SetValueWithoutNotify(0);
            }
            else if(behaviorType == Behavior.BehaviorType.ParallelPursuit)
            {
                m_dropdown.SetValueWithoutNotify(1);
            }

        }
        else
        {
            List<string> options = new List<string> { "Simple escape", "Static direction", "Escape from area" };
            m_dropdown.AddOptions(options);

            if (behaviorType == Behavior.BehaviorType.EscapeFromClosestPursuer)
            {
                m_dropdown.SetValueWithoutNotify(0);
            }
            else if (behaviorType == Behavior.BehaviorType.EscapeInStaticDirection)
            {
                m_dropdown.SetValueWithoutNotify(1);
            }
            else if (behaviorType == Behavior.BehaviorType.EscapeFromArea)
            {
                m_dropdown.SetValueWithoutNotify(2);
            }
        }
    }

    private void UpdateAdditional(Player player)
    {
        if (!player || !m_additionalText || !m_additionalInputX || !m_additionalInputY)
        {
            return;
        }

        m_additionalText.text = "";
        m_additionalInputX.gameObject.active = false;
        m_additionalInputY.gameObject.active = false;

        if (player.CompareTag("Escaper"))
        {
            if (player.GetBehaviorType() == Behavior.BehaviorType.EscapeInStaticDirection)
            {
                m_additionalText.text = "Direction:";
                m_additionalInputX.gameObject.active = true;
                m_additionalInputY.gameObject.active = true;

                var escapeInStaticDir = GetEscapeInStaticDirection();
                if (escapeInStaticDir != null)
                {
                    var dir = escapeInStaticDir.GetDirection();
                    m_additionalInputX.text = dir.x.ToString();
                    m_additionalInputY.text = dir.y.ToString();
                }
            }
        }
    }

    private EscapeInStaticDirection GetEscapeInStaticDirection()
    {
        if(!m_player)
        {
            return null;
        }

        if(m_player.GetBehaviorType() != Behavior.BehaviorType.EscapeInStaticDirection)
        {
            return null;
        }

        var behavior = m_player.GetBehavior();
        if (behavior == null)
        {
            return null;
        }
    
        return (EscapeInStaticDirection)behavior;
    }

    public void OnDropDownChanged(Dropdown dropDown)
    {
        if (!m_player || !dropDown)
        {
            return;
        }

        if (m_player.CompareTag("Pursuer"))
        {
            if(dropDown.value == 0)
            {
                m_player.SetBehavior(Behavior.BehaviorType.SimplePursuit);
            }
            else if (dropDown.value == 1)
            {
                m_player.SetBehavior(Behavior.BehaviorType.ParallelPursuit);
            }
        }
        else
        {
            if (dropDown.value == 0)
            {
                m_player.SetBehavior(Behavior.BehaviorType.EscapeFromClosestPursuer);
                GameManager.Get().SetGameType(GameManager.GameType.UntilTime);
            }
            else if (dropDown.value == 1)
            {
                m_player.SetBehavior(Behavior.BehaviorType.EscapeInStaticDirection);
                GameManager.Get().SetGameType(GameManager.GameType.UntilTime);
            }
            else if (dropDown.value == 2)
            {
                m_player.SetBehavior(Behavior.BehaviorType.EscapeFromArea);
                GameManager.Get().SetGameType(GameManager.GameType.UntilOutOfZone);
            }
        }

      
        UpdateAdditional(m_player);
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

        if (m_player != null)
        {
            m_player.SetPosition(new Vector2(m_player.transform.position.x, (float)toDouble));
           // m_player.transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, (float)toDouble);
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

        if (m_player != null)
        {
            m_player.SetPosition(new Vector2((float)toDouble, m_player.transform.position.z));
            //m_player.transform.position = new Vector3((float)toDouble, m_player.transform.position.y, m_player.transform.position.z);
        }
    }

    public void OnAdditionalYChange(InputField fieldX)
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

        var escapeInStaticDir = GetEscapeInStaticDirection();
        if (escapeInStaticDir != null)
        {
            escapeInStaticDir.SetDirection(new Vector2(escapeInStaticDir.GetDirection().x, (float)toDouble));
        }
    }

    public void OnAdditionalXChange(InputField fieldX)
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

        var escapeInStaticDir = GetEscapeInStaticDirection();
        if (escapeInStaticDir != null)
        {
            escapeInStaticDir.SetDirection(new Vector2((float)toDouble, escapeInStaticDir.GetDirection().y));
        }
    }

    public void OnSpeedChange(Slider slider)
    {
        if (m_player != null)
        {
            m_player.SetSpeed(slider.value);
        }
    }

}

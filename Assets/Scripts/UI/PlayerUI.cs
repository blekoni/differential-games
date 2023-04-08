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

    Player m_player;

    public static PlayerUI m_instance;

    private void Start()
    {
    }

    private void Awake()
    {
        m_instance = this;
    }

    public static PlayerUI Get()
    {
        return m_instance;
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
    }

    private void UpdateBehaviour(Player player)
    {
        if (!player || !m_dropdown)
        {
            return;
        }

        m_dropdown.ClearOptions();

        if (player.CompareTag("Pursuer"))
        {
            List<string> options = new List<string> { "Simple pursuit", "Parallel pursuit" };
            m_dropdown.AddOptions(options);

        }
        else
        {
            List<string> options = new List<string> { "Simple escape", "Static vector", "Escape from area" };
            m_dropdown.AddOptions(options);
        }

        m_dropdown.SetValueWithoutNotify(player.GetBehavior());
    }

    public void OnDropDownChanged(Dropdown dropDown)
    {
        if (!m_player || !dropDown)
        {
            return;
        }

        Debug.Log(dropDown.value);
        m_player.SetBehavior(dropDown.value);
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
            m_player.transform.position = new Vector3(m_player.transform.position.x, m_player.transform.position.y, (float)toDouble);
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
            m_player.transform.position = new Vector3((float)toDouble, m_player.transform.position.y, m_player.transform.position.z);
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

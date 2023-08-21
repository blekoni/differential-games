using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Text m_text;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(m_text);
    }

    public void UpdateTimer(float gameTime)
    {
        TimeSpan time = TimeSpan.FromSeconds(gameTime);
        m_text.text = time.ToString(@"mm\:ss\:fff");
    }
}

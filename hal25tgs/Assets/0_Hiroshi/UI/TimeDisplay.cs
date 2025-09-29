using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TimerText;
    [SerializeField] private Timer m_Timer;
    [SerializeField] bool m_TurnTimer = false;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        if (m_TurnTimer)
        {
            m_Timer = TurnManager.Instance.GetTurnTimer();
        }
        else
        {
            m_Timer = TurnManager.Instance.GetGameTimer();
        }
    }
    private void FixedUpdate()
    {
        Init();
    }
    private void Update()
    {
        TimerDispView();
    }

    private void TimerDispView()
    {
        float time = m_Timer.GetNowTime();
        if (time < 0.0f) return;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        m_TimerText.text = $"{minutes:00}:{seconds:00}"; // "00:00" ‚Å•\Ž¦
    }
}
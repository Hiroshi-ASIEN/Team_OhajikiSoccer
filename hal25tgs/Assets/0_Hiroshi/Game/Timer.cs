using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 設定時間のタイプ
public enum TimeSetType
{
   Second,  // 秒
   Minute,  // 分
}
public class Timer : MonoBehaviour
{
    [Header("何のタイマーかメモ用")]
    [SerializeField] private string m_TimerName;

    [Header("設定する時間単位")]
    [SerializeField] private TimeSetType m_Type = TimeSetType.Second;

    [Header("タイマーに設定する時間")]
    [SerializeField] private float m_MaxTime = 0.0f; // カウントする時間

    [Header("イベントを発生させる時間")]
    [SerializeField] private float m_EventTime = 0.0f;

    private float m_NowTime = 0.0f; // 現在の時間

    private bool m_Active = false;

    private bool m_Evented = false;
    public event Action OnEventTime;    // 指定した時間になったらイベントを発生させる    
    public event Action OnTimerEnd; // このタイマーが終了した際に発生するイベント

    private void Awake()
    {
        SetTime();
        m_NowTime = m_MaxTime;
        Debug.Log(m_TimerName.ToString() + "Awake" + m_NowTime.ToString());
    }
    private void Update()
    {
        TimeCount();    // タイマーカウント
    }

    private void FixedUpdate()
    {
        
    }

    // タイマーカウント関数
    private void TimeCount()
    {
        if (!m_Active) return;

        m_NowTime -= Time.deltaTime;

        if (!m_Evented)
        {
            if (m_EventTime >= m_NowTime)
            {
                OnEventTime?.Invoke();
                m_Evented = true;
            }
        }

        if (m_NowTime <= 0.0f)
        {
            OnTimerEnd?.Invoke();
        }

    }

    private void SetTime()
    {
        switch (m_Type)
        {
            case TimeSetType.Second:
                m_MaxTime *= 1.0f;
                break;

            case TimeSetType.Minute:
                m_MaxTime *= 60.0f;
                break;
        }
    }

    public void SetMaxTimer(float _maxTime)
    {
        m_MaxTime = _maxTime;
        Debug.Log(m_TimerName.ToString() + "時間設定" + m_MaxTime.ToString());
    }
    // タイマー開始
    public void TimerStart()
    {
        m_Active = true;
        m_NowTime = m_MaxTime;
        Debug.Log(m_TimerName.ToString() + "起動" + m_NowTime.ToString());
    }
    // タイマー終了したか取得関数
    public bool TimerEnd()
    {
        if (m_NowTime > 0.0f) return false;
        m_Active = false;

        return true;
    }

    // タイマーリセット
    public void ReSetTimer()
    {
        m_NowTime = m_MaxTime;
    }

    // 設定されているタイマー時間取得
    public float GetMaxTime()
    {
        return m_MaxTime;
    }

    // 現在のタイマー取得
    public float GetNowTime()
    {
        return m_NowTime;
    }

    public void StopTimer()
    {
        m_Active = false;
    }

    public void ReStartTimer()
    {
        m_Active = true;
    }

    public void SyncTime(float _time)
    {
        m_NowTime = _time;
    }
}
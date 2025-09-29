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
    [Header("設定する時間単位")]
    [SerializeField] private TimeSetType m_Type = TimeSetType.Second;

    [Header("タイマーに設定する時間")]
    [SerializeField] private float m_MaxTime = 0.0f; // カウントする時間
    private float m_NowTime = 0.0f; // 現在の時間

    private bool m_Active = false;

    private void Start()
    {
        SetTime();
        m_NowTime = m_MaxTime;
    }

    private void Update()
    {
        TimeCount();    // タイマーカウント
    }

    // タイマーカウント関数
    private void TimeCount()
    {
        if (!m_Active) return;

        m_NowTime -= Time.deltaTime;
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
    }
    // タイマー開始
    public void TimerStart()
    {
        m_Active = true;
        m_NowTime = m_MaxTime;
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
}
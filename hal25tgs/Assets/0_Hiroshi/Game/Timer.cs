using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �ݒ莞�Ԃ̃^�C�v
public enum TimeSetType
{
   Second,  // �b
   Minute,  // ��
}
public class Timer : MonoBehaviour
{
    [Header("���̃^�C�}�[�������p")]
    [SerializeField] private string m_TimerName;

    [Header("�ݒ肷�鎞�ԒP��")]
    [SerializeField] private TimeSetType m_Type = TimeSetType.Second;

    [Header("�^�C�}�[�ɐݒ肷�鎞��")]
    [SerializeField] private float m_MaxTime = 0.0f; // �J�E���g���鎞��

    [Header("�C�x���g�𔭐������鎞��")]
    [SerializeField] private float m_EventTime = 0.0f;

    private float m_NowTime = 0.0f; // ���݂̎���

    private bool m_Active = false;

    private bool m_Evented = false;
    public event Action OnEventTime;    // �w�肵�����ԂɂȂ�����C�x���g�𔭐�������    
    public event Action OnTimerEnd; // ���̃^�C�}�[���I�������ۂɔ�������C�x���g

    private void Awake()
    {
        SetTime();
        m_NowTime = m_MaxTime;
        Debug.Log(m_TimerName.ToString() + "Awake" + m_NowTime.ToString());
    }
    private void Update()
    {
        TimeCount();    // �^�C�}�[�J�E���g
    }

    private void FixedUpdate()
    {
        
    }

    // �^�C�}�[�J�E���g�֐�
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
        Debug.Log(m_TimerName.ToString() + "���Ԑݒ�" + m_MaxTime.ToString());
    }
    // �^�C�}�[�J�n
    public void TimerStart()
    {
        m_Active = true;
        m_NowTime = m_MaxTime;
        Debug.Log(m_TimerName.ToString() + "�N��" + m_NowTime.ToString());
    }
    // �^�C�}�[�I���������擾�֐�
    public bool TimerEnd()
    {
        if (m_NowTime > 0.0f) return false;
        m_Active = false;

        return true;
    }

    // �^�C�}�[���Z�b�g
    public void ReSetTimer()
    {
        m_NowTime = m_MaxTime;
    }

    // �ݒ肳��Ă���^�C�}�[���Ԏ擾
    public float GetMaxTime()
    {
        return m_MaxTime;
    }

    // ���݂̃^�C�}�[�擾
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
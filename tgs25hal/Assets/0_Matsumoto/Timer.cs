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
    [Header("�ݒ肷�鎞�ԒP��")]
    [SerializeField] private TimeSetType m_Type = TimeSetType.Second;

    [Header("�^�C�}�[�ɐݒ肷�鎞��")]
    [SerializeField] private float m_MaxTime = 0.0f; // �J�E���g���鎞��
    private float m_NowTime = 0.0f; // ���݂̎���

    private bool m_Active = false;

    private void Start()
    {
        SetTime();
        m_NowTime = m_MaxTime;
    }

    private void Update()
    {
        TimeCount();    // �^�C�}�[�J�E���g
    }

    // �^�C�}�[�J�E���g�֐�
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
    // �^�C�}�[�J�n
    public void TimerStart()
    {
        m_Active = true;
        m_NowTime = m_MaxTime;
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
}
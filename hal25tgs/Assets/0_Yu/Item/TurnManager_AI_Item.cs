using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager_AI_Item : MonoBehaviour
{
    [SerializeField] private Timer_AI m_Timer;
    [SerializeField] private EnemyAI_Item[] m_Enemies;

    [Header("���Ԑݒ�")]
    [SerializeField] private float m_PlayPhaseTime;
    [SerializeField] private float m_ActivePhaseTime;
    [SerializeField] private float m_BurstPhaseTime;

    [SerializeField] private CreatePlayer m_CreatePlayer;

    private bool m_Active = false;
    private bool m_Burst = false;   // �o�[�X�g�^�C������

    // Start is called before the first frame update
    void Start()
    {
        m_Timer.SetMaxTimer(m_PlayPhaseTime);
        m_Timer.TimerStart();
        Debug.Log("����t�F�[�Y�J�n");

        foreach (var e in m_Enemies)
        {
            e.PrepareMove();
            Debug.Log("Prepare");
        }
    }

    // Update is called once per frame
    void Update()
    {
        TurnSwitch();
    }

    private void TurnSwitch()
    {
        if (!m_Timer.TimerEnd()) return;

        if (m_Active)
        {
            // ��ړ��t�F�[�Y�I����
            // ����^�C���ɐؑ�
            PhaseSwitch(false, false, m_PlayPhaseTime);

            foreach(var e in m_Enemies)
            {
                e.FacePlayer();
                e.PrepareMove();
            }

            Debug.Log("����t�F�[�Y");
        }
        else if (!m_Active)
        {
            // ����t�F�[�Y�I����

            if (m_Burst)
            {
                // �o�[�X�g�t�F�[�Y�ɐؑ�
                PhaseSwitch(true, true, m_BurstPhaseTime);
                Debug.Log("�o�[�X�g�^�C��");

                return;
            }

            // ���^�C���ɐؑ�
            PhaseSwitch(true,false,m_ActivePhaseTime);
            Debug.Log("���t�F�[�Y");
        }
    }

    // ��󑀍쎞�Ԓ����擾
    public bool GetPlayTimeTurn()
    { 
        return !m_Active;
    }

    public Timer_AI GetUseTimer()
    {
        return m_Timer;
    }

    private void PhaseSwitch(bool _active,bool _burst,float _time)
    {
        m_Active = _active;
        m_Burst = _burst;
        m_Timer.SetMaxTimer(_time);
        m_Timer.TimerStart();   // �^�C�}�[�N��

        // ����^�C���ɐؑւ̏ꍇ�͂����ŏI��
        if (!m_Active) return;

        foreach(var enemy in m_Enemies)
        {
            enemy.Move();
        }
        
    }

    public bool IsBurstTimeActive()
    {
        // �o�[�X�g�^�C�������\��
        return m_Burst && m_Active;
    }

    // �o�[�X�g�^�C���N���i�^�[�����ꎞ�p�~�j
    public void BurstTimeStart()
    {
        m_Burst = true;
    }
}

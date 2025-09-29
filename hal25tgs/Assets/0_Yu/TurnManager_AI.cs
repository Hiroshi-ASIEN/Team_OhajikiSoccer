using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager_AI : MonoBehaviour
{
    public enum TURN_STATE
    {
        PLAY_TURN = 0,
        ACTIVE_TURN,
        STADIUMSWAP_INTERVAL,
        BURST_TURN,

        GAME_END,
        TURN_INTERVAL,

        TURN_TYPE_MAX
    };

    static public TurnManager_AI Instance { get; private set; }  // �V���O���g����

    //�G�l�~�[
    //[SerializeField] private EnemyAI[] m_Enemies;
    private AITeamObject m_AITeam;

    [Header("�Q�[���S�̃^�C�}�[")]
    [SerializeField] private Timer_AI m_Timer;

    [Header("�^�[���v���^�C�}�[")]
    [SerializeField] private Timer m_TurnTimer;

    [Header("�����I�Ք��莞��")]
    [SerializeField] private float m_FinalPhase;

    [Header("���Ԑݒ�")]
    [Header("���Ԑݒ�")]
    [SerializeField] private float m_PlayTurnTime = 10.0f; // ����^�[��
    [SerializeField] private float m_ActiveTurnTime = 5.0f;   // ���^�[��
    [SerializeField] private float m_BurstTurnTime = 20.0f;    // ���A���^�C�����[�h
    [SerializeField] private float m_TurnIntervalTime = 3.0f;    // �^�[���Ԃ̑ҋ@����
    [SerializeField] private float m_StadiumSwapIntervalTime = 5.0f;    // �X�e�[�W�؂�ւ����̑ҋ@����

    [SerializeField] private CreatePlayer m_CreatePlayer;

    private bool m_Active = false;
    private bool m_Burst = false;   // �o�[�X�g�^�C������

    // Start is called before the first frame update
    void Start()
    {
        m_Timer.SetMaxTimer(m_PlayTurnTime);
        m_Timer.TimerStart();
        Debug.Log("����t�F�[�Y�J�n");

        //foreach (var e in m_Enemies)
        //{
        //    e.PrepareMove();
        //    Debug.Log("Prepare");
        //}

        m_AITeam = GameObject.FindFirstObjectByType<AITeamObject>();
        m_AITeam.AssignItemChasers();
        m_AITeam.PrepareAllEnemies();
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
            PhaseSwitch(false, false, m_PlayTurnTime);

            //foreach(var e in m_Enemies)
            //{
            //    e.FacePlayer();
            //    e.PrepareMove();
            //}

            m_AITeam.FaceAllEnemies();
            m_AITeam.AssignItemChasers();
            m_AITeam.PrepareAllEnemies();

            Debug.Log("����t�F�[�Y");
        }
        else if (!m_Active)
        {
            // ����t�F�[�Y�I����

            if (m_Burst)
            {
                // �o�[�X�g�t�F�[�Y�ɐؑ�
                PhaseSwitch(true, true, m_BurstTurnTime);
                Debug.Log("�o�[�X�g�^�C��");

                return;
            }

            // ���^�C���ɐؑ�
            PhaseSwitch(true,false, m_ActiveTurnTime);
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

        //foreach(var enemy in m_Enemies)
        //{
        //    enemy.Move();
        //}

        m_AITeam.ExecuteAllEnemies();
        
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

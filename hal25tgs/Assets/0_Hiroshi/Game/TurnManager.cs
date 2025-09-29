using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TurnManager;
using static UnityEngine.ParticleSystem;

// �Z�b�g�^�[���𓯊��֐��ɂ���
// �Z�b�g�^�[�����Ń^�C�}�[�N���s��
// �Z�b�g�^�[�����ĂԊ֐����N���C�A���g�}�l�[�W���[�������^�C�}�[�ɃC�x���g�o�^
// �C�x���g�őS�v���C���[�̓����֐����Ă�Ń^�[���J��

public class TurnManager : MonoBehaviourPun
{
    // �^�[���̃^�C�v
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

    static public TurnManager Instance { get; private set; }  // �V���O���g����

    [Header("�Q�[���S�̃^�C�}�[")]
    [SerializeField] private Timer m_GameTimer;
    // �Q�[���S�̃^�C�}�[�I���@���@����^�[���C�x���g����������@�Q�[���I��

    [Header("�^�[���v���^�C�}�[")]
    [SerializeField] private Timer m_TurnTimer;

    [Header("�����I�Ք��莞��")]
    [SerializeField] private float m_FinalPhase;    // �I�ՂƂ��Ĉ�������

    [Header("���Ԑݒ�")]
    [SerializeField] private float m_PlayTurnTime = 10.0f; // ����^�[��
    [SerializeField] private float m_ActiveTurnTime = 5.0f;   // ���^�[��
    [SerializeField] private float m_BurstTurnTime = 20.0f;    // ���A���^�C�����[�h
    [SerializeField] private float m_TurnIntervalTime = 0.0f;    // �^�[���Ԃ̑ҋ@����
    [SerializeField] private float m_StadiumSwapIntervalTime = 1.0f;    // �X�e�[�W�؂�ւ����̑ҋ@����

    [SerializeField] private int m_TurnCount = 0;

    private TeamObject m_Team;

    private bool m_NextBurst = false;   // ���^�[���o�[�X�g�^�C���ɂ��邩

    private bool m_StageSwap = false;   // �X�e�[�W�؂�ւ�����������
    private bool m_GameEnd = false;   // �Q�[���I���ł��邩

    private TURN_STATE m_CurrentTurn = TURN_STATE.TURN_INTERVAL;   // ���݂̃^�[���^�C�v
    private TURN_STATE m_NextTurn = TURN_STATE.PLAY_TURN;   // ���̃^�[���^�C�v

    // �C�x���g�ꗗ
    // ======================================================================================================
    public event Action<TURN_STATE> OnTurnChanged;  // �^�[���ؑ֎��ɔ�������C�x���g
    public event Action OnFinalPhase;   // �Q�[���I�Ղɍ����|���������ɔ�������C�x���g
    public event Action OnGameEnd;   // �Q�[���I������������C�x���g
    // ======================================================================================================

    [SerializeField] private bool m_BurstTest = false;
    private bool m_Active = false;  // �����\��

    [SerializeField] AITeamObject m_AITeam;

    private float m_ErrorCheckTimer = 1.0f;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("���� TurnManager �����݂��Ă��܂��B�d���������̂�j�����܂��B");
            Destroy(this.gameObject);
            return;
        }


        OnTurnChanged += TurnCount; // �^�[���ؑփC�x���g�������ɑ���^�[���ł���΃^�[�������Z�֐����s
    }
    private void Start()
    {
        /// <summary>
        /// �^�[������J�n
        /// �Q�[���}�l�[�W���[���ŁA�Q�[�����J�n���ꂽ����s
        /// </summary>
//        TurnStart();

        if (!PhotonNetwork.IsConnected)
        {
            m_AITeam = GameObject.FindFirstObjectByType<AITeamObject>();
            m_AITeam.AssignItemChasers();
            m_AITeam.PrepareAllEnemies();
        }
    }
    private void FixedUpdate()
    {
        //        TurnTimerCheck();
        if (m_Team != null)
        { 
GameObject obj = GameObject.FindWithTag("TeamObject");
            m_Team =obj.GetComponent<TeamObject>();
        }
    }

    private void TurnTimerCheck()
    {
        if (TimerError())
        {
            TurnChanged();
//            SetNextTurn(m_CurrentTurn, 3.0f);
        }
    }

    private bool TimerError()
    {
        if (m_ErrorCheckTimer == m_TurnTimer.GetNowTime())
        {
            return true;
        }

        m_ErrorCheckTimer=m_TurnTimer.GetNowTime();

        return false;
    }
    /// <summary>
    /// �C�x���g�N���E�ǉ��E�폜�n
    /// </summary>
        // �^�[���Ǘ��^�C�}�[�I���C�x���g�������^�[���ؑ֊֐��g�ݍ���
    private void SetTurnLoop()
    {
        if (PhotonNetwork.IsConnected)
        {
            // �}�X�^�[�N���C�A���g�łȂ���Ή������Ȃ�
            if (!PhotonNetwork.IsMasterClient) return;
        }

        m_TurnTimer.OnTimerEnd += TurnChanged;  // �^�[���p�^�C�}�[�̏I���C�x���g�Ƀ^�[���؂�ւ��֐��ǉ�
        m_GameTimer.OnEventTime += FinalPhaseEvent;     // �Q�[���S�̃^�C�}�[�̏I�Վ��Ԕ����C�x���g�ɁA�I�Փ˓��C�x���g�N���֐��ǉ�
    }

// �I�Փ˓��C�x���g�N���֐��ďo�p�֐�
    private void FinalPhaseEvent()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("PhotonFinalPhaseEvent", RpcTarget.All);
        }
        else
        {
            PhotonFinalPhaseEvent();
        }
    }

    // �I�Փ˓��C�x���g�N��
    [PunRPC]
    private void PhotonFinalPhaseEvent()
    {
        OnFinalPhase?.Invoke();
        Debug.Log("�Q�[���I�ՃC�x���g�����I");
    }

    // �Q�[���S�̃^�C�}�[�I���C�x���g�������ɂ��̃N���X����C�x���g�𔭐�������
    private void SetGameTimer()
    {
        m_GameTimer.TimerStart();
        m_GameTimer.StopTimer();
        m_GameTimer.OnTimerEnd += GameTimerEnd;
    }

    private void OnDisable()
    {
        m_TurnTimer.OnTimerEnd -= TurnChanged;
        m_GameTimer.OnTimerEnd -= GameTimerEnd;
        m_GameTimer.OnEventTime -= FinalPhaseEvent;

        OnTurnChanged -= TurnCount;
    }

    // �^�[���؂�ւ������E�񓯊��֐��Ăяo���p�֐�
    private void TurnChanged()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            photonView.RPC("AllPlayerTurnChanged", RpcTarget.All);
        }
        else
        {
            AllPlayerTurnChanged();
        }
    }

    /// <summary>
    /// �^�[���ؑ�
    /// ���� �� ��ړ�or���A���^�C�� �� ���� ���[�v
    /// </summary>
    [PunRPC]
    private void AllPlayerTurnChanged()
    {
        if (m_CurrentTurn >= TURN_STATE.PLAY_TURN &&
            m_CurrentTurn <= TURN_STATE.BURST_TURN)
        {
            // ���݃^�[�������^�[���ɍX�V����B�i���݂��C���^�[�o���ɂ���j
            ChangedPlayerTurn();// ���^�[�����C���^�[�o���ȊO�ɂ���
            StopGameTimer();    // �S�̃^�C�}�[��~
        }
        else
        {
            // ���݂��C���^�[�o���̂Ƃ�
            /*
            if (m_StageSwap)
            {
                // �X�e�[�W�ؑփt���Otrue�Ȃ�"���݂̓X�e�[�W�ؑփC���^�[�o����"
                m_StageSwap = false;    // �t���O��false�ɂ���
                SetTurn();                 // ���݃^�[�������^�[���ɍX�V����B�i���݂��C���^�[�o���ɂ���j
                SetNextTurn(TURN_STATE.PLAY_TURN, m_TurnIntervalTime);  // ���^�[�������^�[���ɂ���
                return;
            }*/

            SetTurn();  // ���݃^�[�������^�[���ɍX�V����B�i���݂��C���^�[�o���ȊO�ɂ���j
            SetNextTurn(TURN_STATE.TURN_INTERVAL, m_TurnIntervalTime);            // ���^�[�����C���^�[�o���ɂ���
            RestartGameTimer(); // �S�̃^�C�}�[�ĊJ
        }
    }

    // ���^�[�����C���^�[�o�����X�e�[�W�؂�ւ��̂Ƃ�
    private void ChangedPlayerTurn()
    {
        if (m_StageSwap)
        {
            // �X�e�[�W�؂�ւ��t���O��true�Ȃ�A
            // �ʏ�C���^�[�o�����͂���ŃX�e�[�W�؂�ւ��C���^�[�o���ɂ���
            SetTurn();  // ���̃^�[�������݂̃^�[���ɐؑ�
            SetNextTurn(TURN_STATE.STADIUMSWAP_INTERVAL, m_StadiumSwapIntervalTime);
            m_StageSwap = false;
        }
        // ���݂�����^�[���̂Ƃ�
        else if (m_CurrentTurn == TURN_STATE.PLAY_TURN)
        {
            // ���݂��C���^�[�o���^�[���ɂ���
            SetTurn();  // ���̃^�[�������݂̃^�[���ɐؑ�

            // ���A���^�C�����[�h�t���Otrue�Ȃ玟�^�[�������A���^�C�����[�h��
            if (m_NextBurst)
            {
                Debug.Log("1");
                SetNextTurn(TURN_STATE.BURST_TURN, m_BurstTurnTime);
                m_NextBurst = false;
                Debug.Log("2");
            }
            else
            {
                // �����łȂ���Ύ��^�[�������^�[����
                SetNextTurn(TURN_STATE.ACTIVE_TURN, m_ActiveTurnTime);
            }

        }
        else
        {
            // �Q�[���S�̃^�C�}�[���I�����Ă��āA���^�[�����I��������I��
            if (m_GameEnd)
            {
                // �^�[���ؑփC�x���g����
                OnGameEnd?.Invoke();
                GameEnd();
                return;
            }
            else
            {
                // ���^�[���łȂ���΁A���^�[�������^
                // �[����
                SetTurn();  // ���̃^�[�������݂̃^�[���ɐؑ�
                SetNextTurn(TURN_STATE.PLAY_TURN, m_PlayTurnTime);
            }
        }
    }

    // ���̃^�[���̐ݒ�
    private void SetNextTurn(TURN_STATE _nextState, float _nextTime)
    {
        m_NextTurn = _nextState;    // ���̃^�[����ݒ�

        // �^�[���ؑփC�x���g����
        OnTurnChanged?.Invoke(m_CurrentTurn);
        m_TurnTimer.TimerStart();   // �^�C�}�[�N��

        m_TurnTimer.SetMaxTimer(_nextTime); // ���̃^�[���̎��Ԃ�ݒ�
    }

    // ���̃^�[���ݒ�����݂̃^�[���ɔ��f���ċN��
    [PunRPC]
    private void SetTurn()
    {
        m_CurrentTurn = m_NextTurn;  // ���݂̃^�[����ؑ�
        Debug.Log("���݂̃^�[��:" + m_CurrentTurn.ToString());



        if (m_CurrentTurn == TURN_STATE.ACTIVE_TURN)
        {
            if (m_Team == null)
            {
                Debug.LogError("m_Team �� null");
                return;
            }

            var players = m_Team.GetPlayersArray();
            if (players == null)
            {
                Debug.LogError("GetPlayersArray() �� null");
                return;
            }

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] == null)
                {
                    Debug.LogError($"players[{i}] �� null");
                    continue;
                }

                PlayerMove playerMove = players[i].GetComponent<PlayerMove>();
                if (playerMove == null)
                {
                    Debug.LogError($"players[{i}] �� PlayerMove �R���|�[�l���g���t���Ă��܂���");
                    continue;
                }

                // �v���C���[��ŁA�C�x���g�������Ɏ��s��������
                // �o�[�X�g�C�x���g�������F�o�[�X�g�t���O���v���C���[�ɕt�^���A�o�[�X�g�C�x���g��������true�ɂ��āAtrue�̂Ƃ��̓v���C���[����シ�����s
                // �o�[�X�g�ȊO�C�x���g�������ɂ�false�ɂ��Ă����Ƃ��H

                playerMove.Move();
                if (!PhotonNetwork.IsConnected)
                {
                    m_AITeam.ExecuteAllEnemies();
                }
            }
        }
        else if(m_CurrentTurn == TURN_STATE.PLAY_TURN)
        {
            if (!PhotonNetwork.IsConnected)
            {
                Debug.Log("AISet");
                m_AITeam.FaceAllEnemies();
                m_AITeam.AssignItemChasers();
                m_AITeam.PrepareAllEnemies();
            }
        }


        //
        // ��ԉ��̊֐�������������\��
        switch (m_CurrentTurn)
        {
            case TURN_STATE.PLAY_TURN:
                m_Active = false;
                m_NextBurst = false;
                break;

            case TURN_STATE.ACTIVE_TURN:
                m_Active = true;
                m_NextBurst = false;
                break;

            case TURN_STATE.BURST_TURN:
                m_Active = true;
                m_NextBurst = true;
                break;
        }
    }

    // �^�[���Ǘ����I��������
    private void GameEnd()
    {
        m_CurrentTurn = TURN_STATE.GAME_END;
        m_NextTurn = TURN_STATE.GAME_END;
        m_NextBurst = false;

        m_TurnTimer.StopTimer();
        Debug.Log(m_CurrentTurn.ToString());
    }

    // �^�[������J�n�p������
    [PunRPC]
    private void TurnInit()
    {
        m_CurrentTurn = TURN_STATE.TURN_INTERVAL;
        m_NextTurn = TURN_STATE.TURN_INTERVAL;
        // �ŏ��Ƀ^�C�}�[�N�����Ȃ��悤�ɂ��Ă���
        m_TurnTimer.StopTimer();
        // ���̃^�[��
        SetNextTurn(TURN_STATE.TURN_INTERVAL, m_TurnIntervalTime);
        SetTurn();  // �����Ń^�C�}�[�N������
                    //               TurnChanged();
        SetNextTurn(TURN_STATE.PLAY_TURN, m_PlayTurnTime);
        SetTurnLoop();  // �^�C�}�[�I���C�x���g�������Ƀ^�[���ؑւ����[�v����悤�ݒ�
        SetGameTimer();

        m_TurnCount = 0;

        m_AITeam = GameObject.FindFirstObjectByType<AITeamObject>();
        m_AITeam.AssignItemChasers();
        m_AITeam.PrepareAllEnemies();

        Debug.Log("�^�[������������");
    }

    private void TurnCount(TURN_STATE _state)
    {
        if (_state != TURN_STATE.PLAY_TURN) return;
        m_TurnCount++;
    }

    private void StopGameTimer()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("PhotonSyncGameTimer", RpcTarget.All, m_GameTimer.GetNowTime());
                photonView.RPC("PhotonStopGameTimer", RpcTarget.All);
            }
        }
        else
        {
            PhotonStopGameTimer();
        }
    }

    private void RestartGameTimer()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("PhotonSyncGameTimer", RpcTarget.All, m_GameTimer.GetNowTime());
                photonView.RPC("PhotonRestartGameTimer", RpcTarget.All);
            }
        }
        else
        {
            PhotonRestartGameTimer();
        }
    }
    // �O������ݒ�n ===================================================

    /// <summary>
    /// �^�[������J�n
    /// �Q�[���}�l�[�W���[���ŁA�Q�[�����J�n���ꂽ����s
    /// </summary>
    public void TurnStart()
    {
        Debug.Log("TurnStart�N��");
        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            photonView.RPC("TurnInit", RpcTarget.All);
        }
        else
        {
            TurnInit();


        }

        Debug.Log("�^�[���X�^�[�g");
    }

    // �X�R�A����ȂǂŃQ�[�����I���ɂȂ����ہA�Ăяo���Ǝ��ɑ���^�[���ɂȂ����ہA
    // �Q�[���S�̃^�C�}�[���I��������t���O
    // �^�[���Ǘ����I��������
    public void GameTimerEnd()
    {
        m_GameEnd = true;
    }


    // �����^�[���I�����Ƀo�[�X�g�^�C���N���i�^�[�����ꎞ�p�~�j
    public void BurstTimeStart()
    {
        m_NextBurst = true;
        Debug.Log("burst active");
    }

    public void StageSwap()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            { 
                photonView.RPC("PunStageSwap",RpcTarget.All);
            }
        }
        else
        {
            PunStageSwap();
        }
    }

    [PunRPC]
    public void PunStageSwap()
    {
        m_StageSwap = true;
    }

    public void SetTeamObject(TeamObject _teamObject)
    {
        m_Team = _teamObject;
    }


    // �擾�n ===================================================

    // ���݂̃^�[���̏�Ԏ擾
    public TURN_STATE GetCurrentTurnState()
    {
        return m_CurrentTurn;
    }

    // ���̃^�[���̏�Ԏ擾
    public TURN_STATE GetNextTurnState()
    {
        /*
        if (m_CurrentTurn == TURN_STATE.ACTIVE_TURN)
        {
            // ���݃^�[�������^�[���ŁA
            // �o�[�X�g�^�C���t���O��true�������玟�^�[���̓o�[�X�g�^�C�����Ƌ�����
            if (m_NextBurst) return TURN_STATE.BURST_TURN;
        }*/
        return m_NextTurn;
    }

    // ���̋��^�[�����o�[�X�g�^�C���ɂȂ邩�ǂ���
    public bool IsNextTurnBurst()
    {
        return m_NextBurst;
    }
    // ���݂̃^�[���̎c�莞�Ԏ擾
    public Timer GetTurnTimer()
    {
        return m_TurnTimer;
    }
    // ���݂̃Q�[���S�̂̎c�莞�Ԏ擾
    public Timer GetGameTimer()
    {
        return m_GameTimer;
    }

    public int GetTurnCount()
    { 
        return m_TurnCount;
    }

    

    // Photon�����n ===================================================
    [PunRPC]
    void SyncTurn(int turn)
    {
        m_CurrentTurn = (TURN_STATE)turn;
    }

    [PunRPC]
    void PhotonSyncGameTimer(float _time)
    {
        m_GameTimer.SyncTime(_time);
    }
    [PunRPC]
    void PhotonStopGameTimer()
    {
        m_GameTimer.StopTimer();
    }
    [PunRPC]
    void PhotonRestartGameTimer()
    { 
        m_GameTimer.ReStartTimer();
    }

    /// <summary>
    /// �ȉ������\��
    /// </summary>
    /// <returns></returns>

    private void TurnSwitch()
    {
        if (!m_TurnTimer.TimerEnd()) return;

        if (m_Active)
        {
            // ��ړ��t�F�[�Y�I����
            // ����^�C���ɐؑ�
            PhaseSwitch(false, false, m_PlayTurnTime);

        }
        else if (!m_Active)
        {
            // ����t�F�[�Y�I����

            if (m_NextBurst)
            {
                // �o�[�X�g�t�F�[�Y�ɐؑ�
                PhaseSwitch(true, true, m_BurstTurnTime);

                return;
            }

            // ���^�C���ɐؑ�
            PhaseSwitch(true, false, m_ActiveTurnTime);
        }
    }

    // ���A���^�C�����[�h�ƃ^�[�����[�h�ύX
    private void PhaseSwitch(bool _active, bool _burst, float _time)
    {
        m_Active = _active;
        m_NextBurst = _burst;
        m_TurnTimer.SetMaxTimer(_time);
        m_TurnTimer.TimerStart();   // �^�C�}�[�N��

        // ����^�C���ɐؑւ̏ꍇ�͂����ŏI��
        if (!m_Active) return;

        //for (int i = 0; i < m_CreatePlayer.m_Players.Length; i++)
        //{
        //    // �v���C���[���������X�N���v�g����v���C���[���[�u�擾
        //    PlayerMove playerMove = m_CreatePlayer.m_Players[i].GetComponent<PlayerMove>();
        //    playerMove.Move();
        //}


        //        for(int i = 0; i < m_Team.GetPlayers().Count;i++)
        for (int i = 0; i < m_Team.GetPlayersArray().Length; i++)
        {
            //            PlayerMove playerMove = m_Team.GetPlayers()[i].GetComponent<PlayerMove>();
            PlayerMove playerMove = m_Team.GetPlayersArray()[i].GetComponent<PlayerMove>();
            playerMove.Move();
        }
    }
    // ��󑀍쎞�Ԓ����擾
    public bool IsPlayTimeTurn()
    {
        return !m_Active;
    }

    // ���^�[�����擾
    public bool IsActiveTimeTurn()
    {
        return m_Active;
    }

    public bool IsBurstTimeActive()
    {
        // �o�[�X�g�^�C�������\��
        return m_NextBurst && m_Active;
    }
    /*
    private void TurnSwap()
    {
        if (m_NextTurn > TURN_STATE.TURN_TYPE_MAX)
        {
            m_CurrentTurn++;
            m_NextTurn++;
            Debug.Log(m_CurrentTurn.ToString());
        }
        else
        {
            TURN_STATE state = m_CurrentTurn;
            m_CurrentTurn = m_NextTurn;
            m_NextTurn = state + 2;
        }
        // �^�[���ؑփC�x���g����
        OnTurnChanged?.Invoke(m_CurrentTurn);
        m_TurnTimer.TimerStart();   // �^�C�}�[�N��
    }
    */

    private void Update()
    {
        if (m_BurstTest)
        {
            m_NextBurst = true;
        }
        if (!m_NextBurst)
        {
            m_BurstTest = false;
        }
    }
}

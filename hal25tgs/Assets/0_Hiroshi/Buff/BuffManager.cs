using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BuffManager : MonoBehaviourPun
{
    [Header("�o�t�^�C�v���Ɋe�X�N���v�g���A�^�b�`")]
    [SerializeField] private Buff_Base[] m_BuffList = new Buff_Base[(int)BUFF_TYPE.BUFF_TYPEMAX];
 
    private BUFF_TYPE m_CurrentBuffType;    // ���݂̃o�t�^�C�v
    private Buff_Base m_CurrentBuff;    // ���݂̃o�t

    public event Action<BUFF_TYPE> OnBuffEvent;  // �o�t�N�����ɔ�������C�x���g


    [SerializeField] private bool m_Test = false;
    [SerializeField] private BUFF_TYPE m_TestType = BUFF_TYPE.BUFF_GHOST;
    // Start is called before the first frame update
    void Start()
    {
        TurnManager.Instance.OnFinalPhase += BuffEventTime; // �ŏI�t�F�[�Y�ɂȂ�����o�t������
    }

    private void OnDisable()
    {
        TurnManager.Instance.OnFinalPhase -= BuffEventTime;
        ScoreManager.Instance.OnScoreChanged -= RandomBuffEffect;
    }

    // �Q�[���I�ՃC�x���g�������ɓo�^����֐�
    // �o�t�������͂��߂�
    private void BuffEventTime()
    {
//        SetBuffEffect(BUFF_TYPE.BUFF_BALL_BOOST);
        RandomBuffEffect();
        ScoreManager.Instance.OnScoreChanged += RandomBuffEffect;  // ���_���Ƀo�t�؂�ւ��悤�ɂ���
    }

    // �o�t�^�C�v�����_���I�o
    private void RandomBuffEffect()
    {
        // �o�t�����肵�A���݂̃o�t�ɐؑւăo�t���ʔ���
        BUFF_TYPE buffType = (BUFF_TYPE)UnityEngine.Random.Range(1, m_BuffList.Length);

        Debug.Log(buffType.ToString());

        if (PhotonNetwork.IsConnected)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            photonView.RPC("SetBuffEffect", RpcTarget.All, buffType);
        }
        else
        {
            SetBuffEffect(buffType);
        }
    }

    [PunRPC]
    // �o�t�^�C�v�ݒ�
    private void SetBuffEffect(BUFF_TYPE _type)
    {
        // ���݉����̃o�t������΁A��ɂ���̏I������
        if (m_CurrentBuff)
        {
            m_CurrentBuff.BuffDeactivate();
        }

        m_CurrentBuffType = _type;
        m_CurrentBuff = m_BuffList[(int)m_CurrentBuffType];
        m_CurrentBuff.BuffActivate();

        // �o�t���������C�x���g����
        OnBuffEvent?.Invoke(m_CurrentBuffType);
    }

    private void EndBuff()
    {
        m_CurrentBuff = null;
        m_CurrentBuffType = BUFF_TYPE.BUFF_NONE;
    }

    private void TestBuffSet()
    {
        photonView.RPC("SetBuffEffect", RpcTarget.All, m_TestType);
    }

    private void Update()
    {
        if (m_Test)
        {
            TestBuffSet();
            m_Test = false;
        }
        if(Input.GetKeyUp(KeyCode.I)) RandomBuffEffect();
    }

    public void ChangeBuff()
    {
        RandomBuffEffect();
    }
}

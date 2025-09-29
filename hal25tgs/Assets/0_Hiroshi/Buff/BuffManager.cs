using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BuffManager : MonoBehaviourPun
{
    [Header("バフタイプ順に各スクリプトをアタッチ")]
    [SerializeField] private Buff_Base[] m_BuffList = new Buff_Base[(int)BUFF_TYPE.BUFF_TYPEMAX];
 
    private BUFF_TYPE m_CurrentBuffType;    // 現在のバフタイプ
    private Buff_Base m_CurrentBuff;    // 現在のバフ

    public event Action<BUFF_TYPE> OnBuffEvent;  // バフ起動時に発生するイベント


    [SerializeField] private bool m_Test = false;
    [SerializeField] private BUFF_TYPE m_TestType = BUFF_TYPE.BUFF_GHOST;
    // Start is called before the first frame update
    void Start()
    {
        TurnManager.Instance.OnFinalPhase += BuffEventTime; // 最終フェーズになったらバフが発生
    }

    private void OnDisable()
    {
        TurnManager.Instance.OnFinalPhase -= BuffEventTime;
        ScoreManager.Instance.OnScoreChanged -= RandomBuffEffect;
    }

    // ゲーム終盤イベント発生時に登録する関数
    // バフ発生しはじめる
    private void BuffEventTime()
    {
//        SetBuffEffect(BUFF_TYPE.BUFF_BALL_BOOST);
        RandomBuffEffect();
        ScoreManager.Instance.OnScoreChanged += RandomBuffEffect;  // 得点時にバフ切り替わるようにする
    }

    // バフタイプランダム選出
    private void RandomBuffEffect()
    {
        // バフを決定し、現在のバフに切替てバフ効果発動
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
    // バフタイプ設定
    private void SetBuffEffect(BUFF_TYPE _type)
    {
        // 現在何かのバフがあれば、先にそれの終了処理
        if (m_CurrentBuff)
        {
            m_CurrentBuff.BuffDeactivate();
        }

        m_CurrentBuffType = _type;
        m_CurrentBuff = m_BuffList[(int)m_CurrentBuffType];
        m_CurrentBuff.BuffActivate();

        // バフ発動したイベント発生
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

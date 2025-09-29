using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class WarpGimmick : MonoBehaviour
{
    //このターン通過したプレイヤーたち　毎ターンリセット...
    //っていいたいところだけど、ワープの真上で止まったらどうする？
    //次のターンの開始時にワープの上にいてもok
    [Header("加速")]
    [SerializeField] private float m_AddPower = 1.25f;

    [Header("ワープ単体（子オブジェクト）")]
    [SerializeField] private WarpUnit m_warpUnitA;
    [SerializeField] private WarpUnit m_warpUnitB;

    //触れたプレイヤーのリスト　プレイヤーが急に消えないことを望む
    List<PlayerObject> m_playerList = new List<PlayerObject>();
    List<EnemyAI> m_enemyList = new List<EnemyAI>();


    bool m_isDetectingStayCollision = false;
    int m_detectCount = 0;


    //勢いを付けないと、同じ座標に出ちゃう
    //public event Action ;   // ゲーム終盤に差し掛かった時に発生するイベント

    //こいつは、子がプレイヤーと触れたことを、子から送信されないとあかん


    private void Awake()
    {
        TurnManager.Instance.OnTurnChanged += AddTurnWarp;
    }

    private void OnDestroy()
    {
        TurnManager.Instance.OnTurnChanged -= AddTurnWarp;
    }


    //ターンが経過したよって教える（TurnManagerから使用）
    public void AddTurnWarp(TurnManager.TURN_STATE _state)
    {
        if (_state == TurnManager.TURN_STATE.ACTIVE_TURN)
        {
            //プレイヤーのリスト　リセット
            ClearPlayerList();
            ClearEnemyList();

            m_isDetectingStayCollision = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ワープの親のセットを試みました");

        //登録
        if (m_warpUnitA)
            m_warpUnitA.SetWarpGimmick(this);
        else
            Debug.LogWarning("子がいません");
        if (m_warpUnitB)
            m_warpUnitB.SetWarpGimmick(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_isDetectingStayCollision)
        {
            if (m_detectCount > 3)
            {
                m_isDetectingStayCollision = false;
                m_detectCount = 0;
            }
            m_detectCount++;
        }
    }



    //触れたプレイヤーを受け取り、ワープさせる
    public void RecievePlayer(PlayerObject _player, WarpUnit _warpUnit)
    {
        if (m_warpUnitA == null)
        {
            Debug.LogWarning("子オブジェクトがなく、ワープが出来ません");
            return;
        }
        if (m_warpUnitB == null)
        {
            Debug.LogWarning("子オブジェクトがなく、ワープが出来ません");
            return;
        }


        if (m_playerList.Contains(_player))
        {
            //すでにある
            Debug.Log("すでに同ターン内でワープさせました");
            return;
        }

        //プレイヤーが自身のシーンで生成したかどうか
        if (_player.gameObject.TryGetComponent<PhotonView>(out var photonView))
        {
            if(photonView.IsMine==false)
            {
                Debug.Log("外部で生成されたプレイヤーはワープさせません");
                return;
            }
        }
        else
        {
            Debug.LogWarning("プレイヤーにPhotonViewがついていません");
        }

        //追加して
        m_playerList.Add(_player);

        //もう一方へ移動
        if (m_warpUnitA == _warpUnit)
        {
            Vector3 pos = m_warpUnitB.gameObject.transform.position;
            pos.y = _player.gameObject.transform.position.y;
            _player.gameObject.transform.position = pos;
        }
        else
        {
            Vector3 pos = m_warpUnitA.gameObject.transform.position;
            pos.y = _player.gameObject.transform.position.y;
            _player.gameObject.transform.position = pos;
        }

        //加速
        if (_player.gameObject.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 vel = rb.velocity;
            vel += vel.normalized * m_AddPower;

            rb.velocity = vel;
        }

        Debug.Log("ワープを遂行しました");
    }

    void ClearPlayerList()
    {
        Debug.LogWarning("プレイヤーのリストをリセットしました");
        m_playerList.Clear();
    }

    //触れたエネミーを受け取り、ワープさせる
    public void RecieveEnemy(EnemyAI _enemy, WarpUnit _warpUnit)
    {
        if (m_warpUnitA == null)
        {
            Debug.LogWarning("子オブジェクトがなく、ワープが出来ません");
            return;
        }
        if (m_warpUnitB == null)
        {
            Debug.LogWarning("子オブジェクトがなく、ワープが出来ません");
            return;
        }


        if (m_enemyList.Contains(_enemy))
        {
            //すでにある
            Debug.Log("すでに同ターン内でワープさせました");
            return;
        }

        //追加して
        m_enemyList.Add(_enemy);

        //もう一方へ移動
        if (m_warpUnitA == _warpUnit)
        {
            _enemy.gameObject.transform.position = m_warpUnitB.gameObject.transform.position;
        }
        else
        {
            _enemy.gameObject.transform.position = m_warpUnitA.gameObject.transform.position;
        }

        //加速
        if (_enemy.gameObject.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 vel = rb.velocity;
            vel += vel.normalized * m_AddPower;

            rb.velocity = vel;
        }
    }

    void ClearEnemyList()
    {
        m_enemyList.Clear();
    }

    public bool GetIsDetectingStayCollision()
    {
        return m_isDetectingStayCollision;
    }

}

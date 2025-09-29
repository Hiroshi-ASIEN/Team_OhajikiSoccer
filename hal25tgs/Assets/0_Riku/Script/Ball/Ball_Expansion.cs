using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PhotonView))]

public class Ball_Expansion : MonoBehaviourPun
{
    // 管理フラグ
    private bool m_EnableExpansion;

    [SerializeField] private float m_ExpandMul = 2.0f;  // 巨大化倍率
    [SerializeField] private float m_ExpansionTime = 3.0f;  // 巨大化時間
    private float m_ElapsedTime;
    private Vector3 m_CurrentScale;
    private bool m_IsExpanded;
    private GameObject m_Ball;

    // Start is called before the first frame update
    void Start()
    {
        m_Ball = GameObject.FindGameObjectWithTag("Ball");

        // 初期サイズ保存
        m_CurrentScale = m_Ball.transform.localScale;

        //マスターの場合のみ拡大を開始する
        if (PhotonNetwork.IsMasterClient)
            ExpandStart();
        else if(PhotonNetwork.InRoom==false)
        {
            //非通信状態ならば　拡大を開始する
            ExpandStart();
        }
        else
        {
            //自決する
            Destroy(this.gameObject);
            Debug.Log("Ball_Expansionは自決しました");
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_ElapsedTime += Time.deltaTime;

        // 巨大化
        if (m_EnableExpansion && !m_IsExpanded)
        {
            //photonView.RPC("Expand", RpcTarget.All);
            Expand();
        }

        // m_ExpansionTime経ったらサイズを戻す
        if (m_ElapsedTime > m_ExpansionTime && m_IsExpanded)
        {
            //photonView.RPC("Contract", RpcTarget.All);
            Contract();
        }
    }

    private void Expand()
    {
        m_IsExpanded = true;
        m_ElapsedTime = 0.0f;

        // 巨大化
        ApplyScale(m_CurrentScale * m_ExpandMul);
    }

    private void Contract()
    {
        m_EnableExpansion = false;
        m_IsExpanded = false;

        // 縮小化
        ApplyScale(m_CurrentScale);


        //自決する
        Destroy(this.gameObject);
        Debug.Log("Ball_Expansionは自決しました");
    }

    public void ExpandStart()
    {
        m_EnableExpansion = true;
    }

    // スケール変更後の共通処理
    void ApplyScale(Vector3 newScale)
    {
        m_Ball.transform.localScale = newScale;

        //// コライダー再有効化（スケール変更の反映）
        //Collider col = m_Ball.GetComponent<Collider>();
        //col.enabled = false;
        //col.enabled = true;

        //// Rigidbody再設定
        //Rigidbody rb = m_Ball.GetComponent<Rigidbody>();

        //// 必要に応じて質量と慣性を調整
        //float scaleFactor = newScale.x; // 均等スケーリング前提
        //rb.mass = m_BaseMass * Mathf.Pow(scaleFactor, 3); // 体積に比例
        //rb.ResetInertiaTensor(); // 回転挙動のズレ防止



    }
}


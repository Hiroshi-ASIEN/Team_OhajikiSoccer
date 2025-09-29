using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static TurnManager;

public class Swap : MonoBehaviour
{
    public enum STADIUM_TYPE
    {
        NORMAL = 0, // 基本
        LIGHTNING,  // 雷
        DOUBLEGOAL, // ゴール２倍
        HOLE,       // 穴
        FROST,      // 氷
        WARP,       // ワープ

        STADIUM_TYPE_MAX,
    }

    public class Stadiums
    {
        [SerializeField] public STADIUM_TYPE m_StadiumType; // このスタジアムタイプ
        [SerializeField] public string m_PrefabName; // このスタジアムのプレハブ名
    };


    [Header("全スタジアム設定")]
    [SerializeField] private Stadiums[] m_Stadiums;

    [Header("スタジアム出現位置")]
    [SerializeField] private Vector3 m_Position = new Vector3(0.0f, 0.0f, 0.0f);

    [Header("現在のスタジアム")]
    [SerializeField] private GameObject m_CurrentStadium;

    private string m_CreateObjectName;    // スタジアム切り替えプレハブ用変数

    [SerializeField] bool m_IsSwap = false;         // 切り替えフラグ

    private STADIUM_TYPE m_NextType = STADIUM_TYPE.LIGHTNING;                // 次に切り替えるスタジアムタイプ
    private STADIUM_TYPE m_CurrentType;
    private TurnManager m_TurnManager;  // ターンマネージャー取得用

    public event Action<STADIUM_TYPE> OnStadiumChanged;  // スタジアム切替時に発生するイベント

    [SerializeField] private Swap m_StageSwap;

    public GameObject m_tagetPlayer; // キーパーの座標を取得

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient) return;  // マスタークライアントでなければなにもしない

        // ターンマネージャーを取得して、ターン時に発生するイベントにステージ切替関数を登録しておく
        m_TurnManager = TurnManager.Instance;
        m_TurnManager.OnTurnChanged += hSwap;

        m_StageSwap.OnStadiumChanged += KeeperFlyAway;

    }
    // Start is called before the first frame update
    void Start()
    {
        // 現在のスタジアム(最初に設定されているスタジアム)を生成しておく
        Instantiate(m_CurrentStadium, gameObject.transform.position, Quaternion.identity);
    }

    void OnDisable()
    {
        m_TurnManager.OnTurnChanged -= hSwap;
        m_StageSwap.OnStadiumChanged -= KeeperFlyAway;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_IsSwap) return;  // 切り替えフラグが無ければ戻る

        // 次のスタジアムを生成しておく
        Instantiate(m_CurrentStadium, gameObject.transform.position, Quaternion.identity);
        // 現在のスタジアムを破棄
        Destroy(gameObject);
    }

    // 切替するかチェック関数
    private bool SwapCheck(TURN_STATE _state)
    {
        if (!PhotonNetwork.IsMasterClient) return false;  // マスタークライアントでなければなにもしない

        if (!m_IsSwap) return false;   // 切替フラグがtrueでなければ切替ない
        if (_state != TURN_STATE.STADIUMSWAP_INTERVAL) return false;  // ステージ切替

        // 次のタイプが間違っているか、現在のスタジアムタイプと同じなら切替ない
        if (m_NextType >= STADIUM_TYPE.STADIUM_TYPE_MAX || m_NextType == m_CurrentType) return false;

        return true;
    }

    private void hSwap(TURN_STATE _state)
    {
        if (!SwapCheck(_state)) return; //チェックしてfalseなら切替ない

        for (int i = 0; i < m_Stadiums.Length; i++)
        {
            if (m_NextType == m_Stadiums[i].m_StadiumType)
            {
                m_CreateObjectName = m_Stadiums[i].m_PrefabName; // 生成用変数に次のスタジアムのプレハブ登録
                m_CurrentType = m_NextType;                          // 現在のスタジアムを引数のタイプに変更
                break;
            }

            if (i == m_Stadiums.Length) return;   // 見つからなかった場合は切替ない
        }

        // 現在のスタジアムを削除する用変数に移す
        GameObject destroyObject = m_CurrentStadium;

        // 次のスタジアムを生成して現在のスタジアムに登録
        m_CurrentStadium = PhotonNetwork.Instantiate(m_CreateObjectName, m_Position, Quaternion.identity);

        // 先ほどまで使っていたスタジアムを削除
        PhotonNetwork.Destroy(destroyObject);

        return;
    }

    public void SetNextStadium(STADIUM_TYPE _type)
    {
        m_NextType = _type;
        m_IsSwap = true;
    }

    public void RandomSetNextStadium()
    {
        int type = UnityEngine.Random.Range(0, (int)STADIUM_TYPE.STADIUM_TYPE_MAX);
        m_NextType = (STADIUM_TYPE)type;
        m_IsSwap = true;
    }

    private void KeeperFlyAway(Swap.STADIUM_TYPE _type)
    { 
        if (_type == Swap.STADIUM_TYPE.DOUBLEGOAL && m_tagetPlayer != null)
        {
            Vector3 newPosition = m_tagetPlayer.transform.position + new Vector3(0f, 0f, 20f);
            m_tagetPlayer.transform.position = newPosition;
        }
    }
}
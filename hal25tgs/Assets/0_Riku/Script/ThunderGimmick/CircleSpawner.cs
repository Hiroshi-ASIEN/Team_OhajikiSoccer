using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using static TurnManager;


public class CircleSpawner : MonoBehaviourPun
{
    private enum CircleState
    {
        NONE,
        SHOW,
        HIDE,
    }

    public GameObject m_CirclePrefab;
    private TurnManager m_Timer;

    [SerializeField]private float m_SpawnTime = 3.0f;
    private bool m_CirclesVisible = false;
    private List<GameObject> m_ActiveCircles = new List<GameObject>();
    private List<GameObject> m_DestroyCircles = new List<GameObject>();

    private Bounds m_GroundBounds;

    private CircleState m_State = CircleState.NONE;

    void Start()
    {
        m_Timer = TurnManager.Instance;

        // イベント追加
        m_Timer.OnTurnChanged += ShowTurn;
        m_Timer.OnTurnChanged += HideTurn;

        // タグが "Ground" のオブジェクトの Bounds を取得
        GameObject ground = GameObject.FindGameObjectWithTag("Ground");

        if (ground != null)
        {
            Collider groundCollider = ground.GetComponent<Collider>();
            if (groundCollider != null)
            {
                m_GroundBounds = groundCollider.bounds;
            }
            else
            {
                Debug.LogError("GroundオブジェクトにColliderがありません。");
            }
        }
        else
        {
            Debug.LogError("Groundタグが付いたオブジェクトが見つかりません。");
        }
    }

    void Update()
    {

        foreach (GameObject circle in m_DestroyCircles)
        {
            if (!circle.GetComponent<CircleEffectTrigger>().GetDestroyFlg()) continue;

            m_DestroyCircles.Remove(circle);
        }

            float cycleTime = m_Timer.GetTurnTimer().GetNowTime();

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (cycleTime < m_SpawnTime && m_State == CircleState.SHOW)
                {
                    ShowCircles(); // ← マスターだけ実行！
                    photonView.RPC("NotifyCirclesVisible", RpcTarget.OthersBuffered);
                    m_State = CircleState.NONE;
                }
                else if (cycleTime < 0.5 && m_State == CircleState.HIDE)
                {
                    photonView.RPC("HideCircles", RpcTarget.AllBuffered);
                    m_State = CircleState.NONE;
                }
            }
        }
        else
        {
            if (cycleTime < m_SpawnTime && m_State == CircleState.SHOW)
            {
                ShowCircles(); // ← マスターだけ実行！
                NotifyCirclesVisible();
                m_State = CircleState.NONE;
            }
            else if (cycleTime < 0.5 && m_State == CircleState.HIDE)
            {
                HideCircles();
                m_State = CircleState.NONE;
            }
        }

    }

    void ShowCircles()
    {
        if (m_CirclesVisible) return;

        int count = Random.Range(3, 6);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = GetRandomPositionOnGround();
            GameObject circle = SingleMultiUtility.Instantiate("Circle", pos, Quaternion.Euler(90f, 0f, 0f));
            m_ActiveCircles.Add(circle);
        }

        m_CirclesVisible = true;
    }

    [PunRPC]
    void NotifyCirclesVisible()
    {
        m_CirclesVisible = true;
    }


    [PunRPC]
    void HideCircles()
    {
        foreach (GameObject circle in m_ActiveCircles)
        {
            if (circle != null)
            {
                var fader = circle.GetComponent<CircleFader>();
                if (fader != null)
                {
                    fader.photonView.RPC("StartFadeOutAndDestroy", RpcTarget.AllBuffered);
                    m_DestroyCircles.Add(circle);
                }
            }
        }

        m_ActiveCircles.Clear();
        m_CirclesVisible = false;
    }


    Vector3 GetRandomPositionOnGround()
    {
        float x = Random.Range(m_GroundBounds.min.x, m_GroundBounds.max.x);
        float z = Random.Range(m_GroundBounds.min.z, m_GroundBounds.max.z);
        float y = m_GroundBounds.max.y + 0.1f; // 少し上に浮かせる

        return new Vector3(x, y, z);
    }

    private void ShowTurn(TURN_STATE _state)
    {
        if (_state != TURN_STATE.PLAY_TURN) return;

        m_State = CircleState.SHOW;
    }

    private void HideTurn(TURN_STATE _state)
    {
        if (_state != TURN_STATE.ACTIVE_TURN) return;

        m_State = CircleState.HIDE;
    }

    private void OnDestroy()
    {
        foreach (GameObject circle in m_ActiveCircles)
        {
            circle.GetComponent<CircleEffectTrigger>().AllDefault();
            SingleMultiUtility.Destroy(circle); // Photon/Single 両対応
        }
        foreach (GameObject circle in m_DestroyCircles)
        {
            circle.GetComponent<CircleEffectTrigger>().AllDefault();
            SingleMultiUtility.Destroy(circle); // Photon/Single 両対応
        }
    }
}

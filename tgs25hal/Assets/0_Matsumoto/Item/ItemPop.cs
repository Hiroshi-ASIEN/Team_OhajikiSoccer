using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemPop : MonoBehaviourPun
{
    [SerializeField] private Timer m_PopTimer;   // あいてむ生成時間
    [SerializeField] private GameObject[] m_Items;
    [SerializeField] private Vector3[] m_Positions;
    [SerializeField] private TurnManager m_TurnManager;

    private bool m_Pop = false;

    // Start is called before the first frame update
    void Start()
    {
        m_PopTimer.TimerStart();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_TurnManager.GetPlayTimeTurn())
        {
            m_Pop = false;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (m_Pop) return;

                m_Pop = true;
                int selItem = RandomSet(m_Items.Length);
                int popPos = RandomSet(m_Positions.Length);

                Debug.Log("ランダム設定");
                photonView.RPC("CreateItem", RpcTarget.All, selItem, popPos);
            }
        }
    }


    private int RandomSet(int maxLength)
    {
        return Random.Range(0,maxLength);    // ランダム生成
    }

    [PunRPC]
    void CreateItem(int _selItem,int _popPos)
    {
        Instantiate(m_Items[_selItem], m_Positions[_popPos],Quaternion.identity);

        Debug.Log("アイテム生成");
        // タイマー再設定
        m_PopTimer.ReSetTimer();
        m_PopTimer.TimerStart();
    }
}

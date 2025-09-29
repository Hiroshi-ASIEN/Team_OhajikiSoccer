using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(RayCastPosition))]

public class GimmickPutter : MonoBehaviourPunCallbacks
{
    //設置したいプレハブ
    GameObject m_prefab = null;

    //現在設置中か
    ITEM_STATE m_state = ITEM_STATE.JUDGING;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //設置中ならば...
        if (m_state == ITEM_STATE.RUNNING)
        {
            //付属のコンポーネント、（別オブジェクトにでもする？）で移動



            //ボタンが押されたら設置する
            if (Input.GetMouseButtonDown(0))
            {
                Put();
            }

            //右ボタンが押されたらキャンセル（仮）
            if (Input.GetMouseButtonDown(1))
            {
                CancelPutting();
            }
        }
    }

    private void FixedUpdate()
    {

    }


    //設置開始
    public void StartPutting()
    {
        m_state = ITEM_STATE.RUNNING;
    }


    private void Put()
    {
        //回転はいま考慮しない！設置後かな
        //SingleMultiUtility.Instantiate(m_prefab.name, this.transform.position, Quaternion.identity);
        //↑これだめだ！

        if (!PhotonNetwork.IsConnected)
        {
            // シングルプレイ時はResourcesから読み込み
            GameObject prefab = Resources.Load<GameObject>(m_prefab.name);
            if (prefab == null)
            {
                Debug.LogWarning($"プレハブが見つかりません: {m_prefab.name}");
                m_state = ITEM_STATE.FAILURE;
                return;
            }

            GameObject.Instantiate(m_prefab, this.transform.position, Quaternion.identity);
        }
        else
        {
            // マルチプレイ時なら生成
            PhotonNetwork.Instantiate(m_prefab.name, this.transform.position, Quaternion.identity);
        }


        m_state = ITEM_STATE.SUCCESS;

        Debug.Log("プレハブを設置しました");

    }


    //中断する(これをボタン側から呼ぶのかな？)
    public void CancelPutting()
    {
        m_state = ITEM_STATE.FAILURE;
    }
    

    //ステートを取得する　これをインベントリ？が使用する
    public ITEM_STATE GetState()
    {
        return m_state;
    }

    public void SetPrefab(GameObject _prefab)
    {
        m_prefab = _prefab;
    }
}

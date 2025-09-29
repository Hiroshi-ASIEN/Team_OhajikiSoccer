using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//Playerに付けるスクリプト
public class PlayerMove : MonoBehaviourPunCallbacks
{
    public Vector3 m_Velocity;
    Rigidbody m_Rb;
    public Material m_PlayerMaterial;
    public Material m_EnemyMaterial;
    GameManager_S m_GameManager_S;

    LineRenderer m_LineRenderer;

    //======================================
    //2025/06/27 河上　追記
    //Boostアイテムに対応するための、速度の割合
    //Boostを使用するとここが1.2fに変更される
    float m_SpeedRate = 1.0f;
    //======================================

    // Start is called before the first frame update
    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();

        GameObject obj = GameObject.Find("GameObject");

        m_GameManager_S = obj.GetComponent<GameManager_S>();

        if (photonView.IsMine)
        {
            tag = "Player";
            enabled = true;
            if (m_GameManager_S.AssignPlayersToTeams() == TeamName.TeamA)
            {
                GetComponent<MeshRenderer>().material = m_PlayerMaterial;
            }
            else
            {
                GetComponent<MeshRenderer>().material = m_EnemyMaterial;
            }
        }
        else
        {
            tag = "Untagged";
            enabled = false;
            if (m_GameManager_S.AssignPlayersToTeams() == TeamName.TeamA)
            {
                GetComponent<MeshRenderer>().material = m_EnemyMaterial;
            }
            else
            {
                GetComponent<MeshRenderer>().material = m_PlayerMaterial;
            }
        }

        //ソロプレイ時にtagをPlayerにする
        if(!PhotonNetwork.IsConnected)
        {
            tag = "Player";
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.enabled = false;
        if (m_Velocity.x != 0.0f && m_Velocity.z != 0.0f)
        {
            if(tag == "Player")
            {
                this.transform.rotation = Quaternion.LookRotation(m_Velocity);
            }
        }

        //======================================
        //2025/06/27 河上　追記　
        //計算式の末尾に * m_SpeedRateを追記
        m_Rb.AddForce(m_Velocity * 0.25f * m_SpeedRate * m_Rb.mass, ForceMode.Impulse);


        m_Velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }



    //======================================
    //2025/06/24 河上　追記
    //スピードレートのゲッター、セッター（Item_Boost_Newestから呼ばれる）
    public void SetSpeedRate(float rate)
    {
        m_SpeedRate = rate;
    }
    public float GetSpeedRate()
    {
        return m_SpeedRate;
    }
    //======================================

}

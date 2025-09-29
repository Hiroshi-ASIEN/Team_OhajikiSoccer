using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Move : MonoBehaviourPunCallbacks
{
    public Vector3 m_Velocity;
    Rigidbody m_Rb;
    public Material m_PlayerMaterial;
    public Material m_EnemyMaterial;
    GameManager_S m_GameManager_S;

    public float m_PlayerSpeed = 1f;

    LineRenderer m_LineRenderer;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PMove()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.enabled = false;
        m_Rb.AddForce(m_Velocity * m_PlayerSpeed, ForceMode.Impulse);
        m_Velocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

}
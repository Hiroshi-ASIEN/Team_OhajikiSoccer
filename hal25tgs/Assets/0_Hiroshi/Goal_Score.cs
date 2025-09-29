using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goal_Score : MonoBehaviourPun
{
    [Header("得点が入るチーム")]
    [SerializeField] TeamName m_ScoringTeam;
    [SerializeField] GameObject m_GoalVFX;

    private GameObject m_Ball;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            m_Ball = GameObject.Find("Ball");
            if (PhotonNetwork.IsConnected)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("GoalIn", RpcTarget.All);
                }
            }
            else
            {
                GoalIn();
            }
        }
    }

    [PunRPC]
    private void GoalIn()
    {

        Debug.Log($"ゴール！ {m_ScoringTeam} に得点！");
        ScoreManager.Instance.AddScore(m_ScoringTeam);
        m_Ball.GetComponent<Rigidbody>().isKinematic = true;  // 物理演算無効化----追記----
        m_Ball.transform.position = new Vector3(0.0f, 5.0f, 0.0f);
        m_Ball.GetComponent<Rigidbody>().isKinematic = false; // 物理演算有効化----追記----
        SingleMultiUtility.Instantiate(m_GoalVFX.name, this.transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        m_Ball = other.gameObject;

        if (PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("GoalIn", RpcTarget.All);
            }
        }
        else
        {
            GoalIn();
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goal_Score : MonoBehaviourPun
{
    [Header("���_������`�[��")]
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

        Debug.Log($"�S�[���I {m_ScoringTeam} �ɓ��_�I");
        ScoreManager.Instance.AddScore(m_ScoringTeam);
        m_Ball.GetComponent<Rigidbody>().isKinematic = true;  // �������Z������----�ǋL----
        m_Ball.transform.position = new Vector3(0.0f, 5.0f, 0.0f);
        m_Ball.GetComponent<Rigidbody>().isKinematic = false; // �������Z�L����----�ǋL----
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

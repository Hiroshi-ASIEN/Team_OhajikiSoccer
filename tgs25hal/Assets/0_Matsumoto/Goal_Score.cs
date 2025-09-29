using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goal_Score : MonoBehaviourPun
{
    [Header("���_������`�[��")]
    [SerializeField] TeamName m_ScoringTeam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    private void GoalIn()
    {
        Debug.Log($"�S�[���I {m_ScoringTeam} �ɓ��_�I");
        ScoreManager.Instance.AddScore(m_ScoringTeam);
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        ball.transform.position = new Vector3(0.0f, 5.0f, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        photonView.RPC("GoalIn",RpcTarget.All);
    }
}

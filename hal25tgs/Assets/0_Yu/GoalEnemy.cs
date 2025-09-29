using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GoalEnemy : MonoBehaviour
{
    [Header("得点が入るチーム")]
    [SerializeField] TeamName m_ScoringTeam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GoalIn()
    {
        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        ball.GetComponent<Rigidbody>().isKinematic = true;  // 物理演算無効化----追記----
        ball.transform.position = new Vector3(0.0f, 5.0f, 0.0f);
        ball.GetComponent<Rigidbody>().isKinematic = false; // 物理演算有効化----追記----

        // test
        //if (!ball.GetComponent<Ball_Expansion>().m_EnableExpansion)
        //    ball.GetComponent<Ball_Expansion>().m_EnableExpansion = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        GoalIn();
    }
}

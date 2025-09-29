using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 製作者　河上尚哉
/// 
/// はっとりくんが作ってくれたItem_Boostの設計思想をそのままに
/// プレイヤー全体に適用、かつエネミーが使用できるようにした。
/// </summary>

public class Item_Boost_Newest : MonoBehaviour
{
    [SerializeField] private float m_BoostPower = 1.2f;  // ブースト力
    [SerializeField] private float m_BoostTime = 10.0f;  // ブースト時間
    float m_secondCount = 0.0f;

    bool m_isBoosting = false;

    //Move.csのm_PlayerSpeedは、割合なので、ここを変更する

    //保有している　リスト
    private List<PlayerMove> m_playerMoveList;
    private List<EnemyAI> m_enemyMoveList;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (m_isBoosting == false)
            return;

        m_secondCount += Time.deltaTime;
        if (m_secondCount >= m_BoostTime)
        {
            BoostEnd();

            //自決する
            Destroy(this.gameObject);
            Debug.Log("Boostは自決しました");
        }
    }


    public void BoostStartPlayer(List<PlayerMove> moves)
    {
        //チームメンバー全員に対して、m_PlayerSpeedを変更する
        m_playerMoveList = moves;

        foreach (PlayerMove move in m_playerMoveList)
        {
            move.SetSpeedRate(m_BoostPower);
        }

        m_isBoosting=true;
    }

    public void BoostStartEnemy(List<EnemyAI> enemyAIs)
    {
        //エネミーの場合はチームメンバー全員に対して、m_SpeedRateを変更する
        m_enemyMoveList = enemyAIs;

        foreach (EnemyAI enemyAI in m_enemyMoveList)
        {
            enemyAI.SetSpeedRate(m_BoostPower);
        }

        m_isBoosting = true;
    }

    void BoostEnd()
    {
        Debug.Log("ブーストを終了。");


        foreach (PlayerMove move in m_playerMoveList)
        {
            move.SetSpeedRate(1.0f);
        }
        foreach (EnemyAI enemyAI in m_enemyMoveList)
        {
            enemyAI.SetSpeedRate(1.0f);
        }

    }


}

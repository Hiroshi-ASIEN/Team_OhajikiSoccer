using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// ブーストアイテム
/// </summary>
//----------------------------------------------------------------

public class ItemEffect_Boost : ItemEffect_Base
{
    [SerializeField] GameObject m_boost;


    [Header("エネミーが使用するまでの秒数")]
    [SerializeField] float m_secondToUseByEnemy = 3.0f;
    float m_secondCount = 0.0f;




    void Start()
    {

    }

    public override ITEM_STATE Effect()
    {
        if (m_boost)
        {
            //味方だけにしか効果がないならばPhotonを経由せずに作ってよいのでは
            GameObject instance = Instantiate(m_boost, this.transform.position, Quaternion.identity);
            //GameObject instance = PhotonNetwork.Instantiate(m_boost.name, this.transform.position, Quaternion.identity);

            if (instance.TryGetComponent<Item_Boost_Newest>(out var boost))
            {
                if (m_itemObject)
                {
                    PlayerObject[] playerArray =
                        m_itemObject.GetInventory().GetTeamObject().GetPlayersArray();

                    //そこからMoveスクリプトを取得　まって、PlayerにMoveついてねぇ！
                    List<PlayerMove> playerMoves = new List<PlayerMove>();

                    for (int i = 0; i < playerArray.Length; i++)
                    {
                        playerMoves.Add(playerArray[i].gameObject.GetComponent<PlayerMove>());
                    }


                        boost.BoostStartPlayer(playerMoves);
                }
            }


            Debug.Log("ブーストを試みました。");
        }
        else
            Debug.LogWarning("ブーストプレハブがありません");



        return ITEM_STATE.SUCCESS;
    }


    public override ITEM_STATE EffectByEnemy()
    {
        //上と違い、Photonではない純粋な生成
        if (m_boost)
        {
            GameObject instance = Instantiate(m_boost, this.transform.position, Quaternion.identity);

            if(instance.TryGetComponent<Item_Boost_Newest>(out var boost))
            {
                if(m_itemObject)
                {
                    List<EnemyAI> enemyAIs =
                        m_itemObject.GetInventory().GetAITeamObject().GetEnemyAIs();
                    
                    boost.BoostStartEnemy(enemyAIs);
                }
            }


            Debug.Log("ブーストを試みました。");
        }
        else
            Debug.LogWarning("ブーストプレハブがありません");


        return ITEM_STATE.SUCCESS;
    }



    public override bool ConsiderUsingByEnemy()
    {
        m_secondCount += Time.deltaTime;
        if (m_secondCount >= m_secondToUseByEnemy)
        {
            return true;
        }

        return false;
    }

}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// ボール拡大アイテム
/// </summary>
//----------------------------------------------------------------

public class ItemEffect_ExpandBall : ItemEffect_Base
{
    //Ball_Expansionスクリプトがついたプレハブを生成するかぁ
    [SerializeField] GameObject m_ballExpander;

    [Header("エネミーが使用するまでの秒数")]
    [SerializeField] float m_secondToUseByEnemy = 3.0f;
    float m_secondCount = 0.0f;

    void Start()
    {
        //m_ballExpansion = GetComponent<Ball_Expansion>();

    }

    public override ITEM_STATE Effect()
    {
        if (m_ballExpander)
        {
            GameObject instance = SingleMultiUtility.Instantiate(m_ballExpander.name, this.transform.position, Quaternion.identity);
            Debug.Log("ボール拡大を試みました。");
        }
        else
            Debug.LogWarning("ボール拡大プレハブがありません");



        return ITEM_STATE.SUCCESS;
    }


    public override ITEM_STATE EffectByEnemy()
    {
        //上と違い、Photonではない純粋な生成
        if (m_ballExpander)
        {
            GameObject instance = Instantiate(m_ballExpander, this.transform.position, Quaternion.identity);
            Debug.Log("ボール拡大を試みました。");
        }
        else
            Debug.LogWarning("ボール拡大プレハブがありません");


        return ITEM_STATE.SUCCESS;
    }



    public override bool ConsiderUsingByEnemy()
    {
        m_secondCount += Time.deltaTime;
        if(m_secondCount >= m_secondToUseByEnemy)
        {
            return true;
        }

        return false;
    }

}

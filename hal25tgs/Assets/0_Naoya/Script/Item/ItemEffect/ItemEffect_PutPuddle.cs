using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// 水たまり設置アイテム　（水たまりそのものではないので注意）
/// </summary>
//----------------------------------------------------------------

public class ItemEffect_PutPuddle : ItemEffect_Base
{
    //これ、インスタンスじゃなくてもいいな。新規生成でも可
    [Header("ギミック設置者のインスタンス（子オブジェ？）")]
    [SerializeField] private GimmickPutter m_gimmickPutter;

    //プレハブ
    [Header("水たまりのプレハブ")]
    [SerializeField] private GameObject m_puddle;


    [Header("エネミーが使用するまでの秒数")]
    [SerializeField] float m_secondToUseByEnemy = 3.0f;
    float m_secondCount = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        //最初はアクティブオフにする
        if (m_gimmickPutter)
            m_gimmickPutter.gameObject.SetActive(false);
        else
            Debug.LogWarning("ItemEffect_PutPuddleにGimmickPutterが登録されていません！");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(m_gimmickPutter)
        {
            //ステート更新　こいつがItemObjectに送られて、Inventoryに送られる（遠いなぁ）
            m_state = m_gimmickPutter.GetState();

            if(m_state==ITEM_STATE.FAILURE)
            {
                //アクティブを停止
                m_gimmickPutter.gameObject.SetActive(false);
            }

        }
        
    }

    public override ITEM_STATE Effect()
    {
        if(!m_puddle)
        {
            Debug.LogWarning("ItemEffect_PutPuddleに水たまりのプレハブが登録されていません！");
            return ITEM_STATE.FAILURE;
        }

        //ギミックプッターにプレハブを登録
        if (m_gimmickPutter)
        {
            //アクティブを開始
            m_gimmickPutter.gameObject.SetActive(true);
            m_gimmickPutter.SetPrefab(m_puddle);
            m_gimmickPutter.StartPutting();
        }
        else
        {
            Debug.LogWarning("ItemEffect_PutPuddleにGimmickPutterが登録されていません！");
            return ITEM_STATE.FAILURE;
        }

        Debug.Log("水たまり設置開始を試みました。");
        m_state = ITEM_STATE.RUNNING;
        return ITEM_STATE.RUNNING;
    }


    public override ITEM_STATE EffectByEnemy()
    {
        if (!m_puddle)
        {
            Debug.LogWarning("ItemEffect_PutPuddleに水たまりのプレハブが登録されていません！");
            return ITEM_STATE.FAILURE;
        }


        //ボールを探して。左側に？設置する　今は適当に
        Vector3 pos = Vector3.zero;
        pos = GameObject.FindGameObjectWithTag("Ball").transform.position;
        pos += Vector3.left;

        GameObject newGameObject =
            Instantiate(m_puddle, pos, Quaternion.identity) as GameObject;

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

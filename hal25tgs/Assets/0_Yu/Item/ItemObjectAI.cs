using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ITEM_STATE_AI
{
    SUCCESS,    //使用完了
    FAILURE,    //使用失敗
    RUNNING,    //使用中

    JUDGING,    //結果判定中
}


//こいつが、効果、三次元物体、2次元物体を管理する
public class ItemObjectAI : MonoBehaviour
{
 
    [SerializeField] ItemEffect_Base m_effect;  //効果

    [SerializeField] Item3DObjectAI m_3Dobj;  //3D部分（子供）
    [SerializeField] Item2DObjectAI m_2Dobj;  //UI部分（子供）

    ItemInventory m_inventory = null;

    //取得されたかどうか
    bool m_isGot = false;


    void Start()
    {
        m_2Dobj.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        //3Dの検索
        if (m_3Dobj != null)
        {

            //3Dが触れたか
            PlayerObject player = m_3Dobj.GetCollidedPlayer();
            if (player != null)
            {
                //切り替え
                Destroy(m_3Dobj.gameObject);
                m_2Dobj.gameObject.SetActive(true);

                //プレイヤーからチームにアクセスして追加
                //player.GetTeamObject().AddItemToInventory(this);

                //取得された
                m_isGot=true;

            }
        }

        //2Dの検索
        if(m_2Dobj.GetIsUsed())
        {
            //インベントリに、使用開始の合図を送る、削除もむこうで行う
            if (m_inventory)
            {
                ITEM_STATE_AI state = ITEM_STATE_AI.JUDGING;
                //state = m_inventory.UseItemMaster(this);

                //失敗したら、使わなかったことにする
                if (state == ITEM_STATE_AI.FAILURE)
                    m_2Dobj.UseFailed();
            }
            else
                Debug.LogWarning("アイテムにインベントリが登録されていません！");



            ////効果発動（今、こっちで効果出してるけど、インベントリで行ってもいい。例えば、中断処理を組みたいときとか）
            //if (m_effect)
            //    m_effect.Effect();


            ////インベントリから破棄する
            //if(m_inventory)
            //    m_inventory.RemoveItem(this);

            ////こいつを削除する
            //Destroy(this.gameObject);
        }

    }



    //チームの登録
    public void SetInventory(ItemInventory itemInventory)
    {
        m_inventory = itemInventory;
    }

    public RectTransform Get2DobjRectTransform() { 
        return m_2Dobj.GetRectTransform(); 
    }

    public ITEM_STATE Effect()
    {
        if (m_effect)
            return m_effect.Effect();


        Debug.LogWarning("アイテムにエフェクトが登録されていません！");
        return ITEM_STATE.FAILURE;
    }


    //例えば、設置型のアイテムのように、Effectで速攻結果が出ない場合、インベントリはここを参照して結果を見る
    public ITEM_STATE GetItemState()
    {
        if (m_effect)
            return m_effect.GetState();


        Debug.LogWarning("アイテムにエフェクトが登録されていません！");
        return ITEM_STATE.FAILURE;
    }


    public bool GetIsGot()
    {
        return m_isGot;
    }
}

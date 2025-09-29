using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]

//----------------------------------------------------------------
/// <summary>
/// アイテムの効果の基底クラス
/// </summary>
//----------------------------------------------------------------

public abstract class ItemEffect_Base : MonoBehaviourPun
{
    //============================================================
    // 変数
    //============================================================
    //状態
    protected ITEM_STATE m_state = ITEM_STATE.JUDGING;
    //親のアイテム
    protected ItemObject m_itemObject;

    //============================================================
    // 関数
    //============================================================
    public abstract ITEM_STATE Effect();

    public ITEM_STATE GetState() { return m_state; }

    public void SetItemObject(ItemObject itemObject) { m_itemObject = itemObject; }


    //エネミーが使用した場合の効果　みずたまりのように、変わる場合はこれを継承先で変える
    public virtual ITEM_STATE EffectByEnemy() { return Effect(); }

    //エネミーが使用するかを考える　継承先に考えさせることで、if分を簡単にしたい。
    public virtual bool ConsiderUsingByEnemy() { return false; }


}

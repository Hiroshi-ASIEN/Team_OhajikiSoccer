using Photon.Pun.Demo.Cockpit;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// アイテムの効果の派生クラスのサンプル
/// </summary>
//----------------------------------------------------------------

public class ItemEffect_DerivedSample : ItemEffect_Base
{
    public override ITEM_STATE Effect()
    {
        //使用例
        Debug.Log("Effect!!!!!!!!!!!!!");

        return ITEM_STATE.SUCCESS;


        //failure
        //return ITEM_STATE.FAILURE;
    }


    public override ITEM_STATE EffectByEnemy()
    {
        Debug.Log("EffectByEnemy!!!!!!!!!!!!");

        return ITEM_STATE.SUCCESS;
    }

    public override bool ConsiderUsingByEnemy()
    {
        //以下はサンプル
        //毎フレーム、10%の確率で使用するか否かを検討している。
        int random = Random.Range(0, 10);

        if(random == 1)
            return true;

        return false;
    }

}

using Photon.Pun.Demo.Cockpit;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// �A�C�e���̌��ʂ̔h���N���X�̃T���v��
/// </summary>
//----------------------------------------------------------------

public class ItemEffect_DerivedSample : ItemEffect_Base
{
    public override ITEM_STATE Effect()
    {
        //�g�p��
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
        //�ȉ��̓T���v��
        //���t���[���A10%�̊m���Ŏg�p���邩�ۂ����������Ă���B
        int random = Random.Range(0, 10);

        if(random == 1)
            return true;

        return false;
    }

}

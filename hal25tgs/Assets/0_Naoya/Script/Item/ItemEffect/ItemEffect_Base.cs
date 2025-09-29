using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]

//----------------------------------------------------------------
/// <summary>
/// �A�C�e���̌��ʂ̊��N���X
/// </summary>
//----------------------------------------------------------------

public abstract class ItemEffect_Base : MonoBehaviourPun
{
    //============================================================
    // �ϐ�
    //============================================================
    //���
    protected ITEM_STATE m_state = ITEM_STATE.JUDGING;
    //�e�̃A�C�e��
    protected ItemObject m_itemObject;

    //============================================================
    // �֐�
    //============================================================
    public abstract ITEM_STATE Effect();

    public ITEM_STATE GetState() { return m_state; }

    public void SetItemObject(ItemObject itemObject) { m_itemObject = itemObject; }


    //�G�l�~�[���g�p�����ꍇ�̌��ʁ@�݂����܂�̂悤�ɁA�ς��ꍇ�͂�����p����ŕς���
    public virtual ITEM_STATE EffectByEnemy() { return Effect(); }

    //�G�l�~�[���g�p���邩���l����@�p����ɍl�������邱�ƂŁAif�����ȒP�ɂ������B
    public virtual bool ConsiderUsingByEnemy() { return false; }


}

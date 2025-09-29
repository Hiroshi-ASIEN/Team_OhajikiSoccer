using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------
/// <summary>
/// �{�[���g��A�C�e��
/// </summary>
//----------------------------------------------------------------

public class ItemEffect_ExpandBall : ItemEffect_Base
{
    //Ball_Expansion�X�N���v�g�������v���n�u�𐶐����邩��
    [SerializeField] GameObject m_ballExpander;

    [Header("�G�l�~�[���g�p����܂ł̕b��")]
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
            Debug.Log("�{�[���g������݂܂����B");
        }
        else
            Debug.LogWarning("�{�[���g��v���n�u������܂���");



        return ITEM_STATE.SUCCESS;
    }


    public override ITEM_STATE EffectByEnemy()
    {
        //��ƈႢ�APhoton�ł͂Ȃ������Ȑ���
        if (m_ballExpander)
        {
            GameObject instance = Instantiate(m_ballExpander, this.transform.position, Quaternion.identity);
            Debug.Log("�{�[���g������݂܂����B");
        }
        else
            Debug.LogWarning("�{�[���g��v���n�u������܂���");


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

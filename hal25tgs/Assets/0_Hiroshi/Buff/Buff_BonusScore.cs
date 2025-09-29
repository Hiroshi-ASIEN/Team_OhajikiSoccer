using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_BonusScore : Buff_Base
{
    [Header("���_�����_�ɂ��邩")]
    [SerializeField] int m_BonusScore = 2;

    private int m_InitScorePoint = 1;
    private ScoreManager m_ScoreManager;

    // Start is called before the first frame update
    void Start()
    {
        m_ScoreManager = ScoreManager.Instance;
    }


    public override void BuffActivate()
    {
        Debug.Log("�o�t�F���_�㏸");
        // ��̓I�Ȍ��ʂ����̉��ɋL�q
        m_InitScorePoint=m_ScoreManager.GetAddScorePoint(); // �ʏ펞�ɉ��Z����链�_���擾
        SetScore(m_BonusScore); // ���Z����链�_�����㏸
    }

    public override void BuffDeactivate()
    {
        SetScore(m_InitScorePoint);    // ���_��ʏ�l�ɖ߂�
    }

    private void SetScore(int _score)
    {
        m_ScoreManager.SetAddScorePoint(_score);
    }
}

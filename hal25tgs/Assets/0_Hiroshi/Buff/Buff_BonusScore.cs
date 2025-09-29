using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_BonusScore : Buff_Base
{
    [Header("得点を何点にするか")]
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
        Debug.Log("バフ：得点上昇");
        // 具体的な効果をこの下に記述
        m_InitScorePoint=m_ScoreManager.GetAddScorePoint(); // 通常時に加算される得点数取得
        SetScore(m_BonusScore); // 加算される得点数を上昇
    }

    public override void BuffDeactivate()
    {
        SetScore(m_InitScorePoint);    // 得点を通常値に戻す
    }

    private void SetScore(int _score)
    {
        m_ScoreManager.SetAddScorePoint(_score);
    }
}

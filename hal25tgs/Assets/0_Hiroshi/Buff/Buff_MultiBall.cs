using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Buff_MultiBall : Buff_Base
{
    [SerializeField] private GameManager_S m_GameManager;
    [Header("ボールの出現位置")]
    [SerializeField] private GameObject[] m_PopPositions;
//    [SerializeField] private Vector3[] m_PopPositions;
    [Header("ボールが増える最大数")]
    [SerializeField] private int m_MaxPopCount;

//    [SerializeField] 
    private string m_BallPrefabName = "Ball";

    private List<GameObject> m_Balls = new List<GameObject>();

    private void Start()
    {
        m_GameManager.GetBallObject().gameObject.name = m_BallPrefabName;
    }
    public override void BuffActivate()
    {
        Debug.Log("バフ：ボール複数出現");
        // 具体的な効果をこの下に記述
        PopBall();
    }

    public override void BuffDeactivate()
    {
        foreach (var ball in m_Balls)
        {
            if (ball != null)
            {
                SingleMultiUtility.Destroy(ball);
                Debug.Log("ボール削除");
            }
        }
        m_Balls.Clear();
    }

    private void PopBall()
    {
        m_Balls.Clear();  // 念のためバフ再使用時の前回分を削除

        int ballCount = Random.Range(1, m_MaxPopCount); // 1～最大数
        List<int> selectedIndexes = new List<int>();

        for (int i = 0; i < ballCount; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, m_PopPositions.Length);
            } while (selectedIndexes.Contains(index));
            selectedIndexes.Add(index);

            Vector3 spawnPos = m_PopPositions[index].transform.position;
            GameObject ball = SingleMultiUtility.Instantiate(m_BallPrefabName, spawnPos, Quaternion.identity);

            m_Balls.Add(ball);  // 作成したボールを登録
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Buff_MultiBall : Buff_Base
{
    [SerializeField] private GameManager_S m_GameManager;
    [Header("�{�[���̏o���ʒu")]
    [SerializeField] private GameObject[] m_PopPositions;
//    [SerializeField] private Vector3[] m_PopPositions;
    [Header("�{�[����������ő吔")]
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
        Debug.Log("�o�t�F�{�[�������o��");
        // ��̓I�Ȍ��ʂ����̉��ɋL�q
        PopBall();
    }

    public override void BuffDeactivate()
    {
        foreach (var ball in m_Balls)
        {
            if (ball != null)
            {
                SingleMultiUtility.Destroy(ball);
                Debug.Log("�{�[���폜");
            }
        }
        m_Balls.Clear();
    }

    private void PopBall()
    {
        m_Balls.Clear();  // �O�̂��߃o�t�Ďg�p���̑O�񕪂��폜

        int ballCount = Random.Range(1, m_MaxPopCount); // 1�`�ő吔
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

            m_Balls.Add(ball);  // �쐬�����{�[����o�^
        }
    }

}

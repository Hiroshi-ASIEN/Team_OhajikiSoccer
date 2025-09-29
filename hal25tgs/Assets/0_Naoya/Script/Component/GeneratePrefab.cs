using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �C���X�y�N�^�[�ɐݒ肳�ꂽ�v���n�u�𐶐�����R���|�[�l���g
/// </summary>
public class GeneratePrefab : MonoBehaviour
{
    //============================================================
    // �ϐ�
    //============================================================
    //------------------
    // ������
    //------------------
    [Header("��������v���n�u�@���v�ݒ聦")]
    [SerializeField] private GameObject m_blockPrefab;

    [Header("�����ɐ��������ہA�Œ�ł������ׂ�����")]
    [SerializeField] private float m_distance = 1.0f;

    //------------------
    // �v���C�x�[�g
    //------------------
    //�����ʒu�̃��X�g
    List<Vector3> m_generatePositions = new List<Vector3>();


    //============================================================
    // �֐�
    //============================================================




    /// <summary>
    /// �w�肳�ꂽ���������������݂܂��B
    /// �͈͎͂��g�̑̐ς͈͓̔��i��]�͖��z��j
    /// </summary>
    /// <returns>���������ł�����</returns>
    public int GeneratePrefabInBoxVolume(int generateNum)
    {
        m_generatePositions = GetRandomPositions(generateNum);

        //��������
        for (int i = 0; i < m_generatePositions.Count; i++)
        {
            GameObject newGameObject =
                Instantiate(m_blockPrefab, m_generatePositions[i], Quaternion.identity) as GameObject;
        }
        return 0;
    }





    /// <summary>
    /// ���g�̖ʐς̊ԂŁA�Œ�ł�distance���ɓ���Ȃ������ŁA
    /// �ꏊ���擾����
    /// </summary>
    /// <param name="howManyPos"></param>
    /// <returns></returns>
    private List<Vector3> GetRandomPositions(int howManyPos)
    {
        List<Vector3> vector3s = new List<Vector3>();

        float leftX;
        float rightX;

        leftX = this.transform.position.x - this.transform.lossyScale.x * 0.5f;
        rightX = this.transform.position.x + this.transform.lossyScale.x * 0.5f;

        float topY;
        float bottomY;

        topY = this.transform.position.y + this.transform.lossyScale.y * 0.5f;
        bottomY = this.transform.position.y - this.transform.lossyScale.y * 0.5f;

        for (int i = 0; i < howManyPos; i++)
        {
            //�܂��̓����_������
            float posX;
            float posY;

            //�������[�v�|������1000�񃋁[�v
            for (int a = 0; a < 1000; a++)
            {
                posX = Random.Range(leftX, rightX);
                posY = Random.Range(bottomY, topY);

                bool isOK = true;

                //�O�̈ʒu���擾���Ĕ�r
                for (int j = 0; j < vector3s.Count; j++)
                {
                    //�������󂢂Ă邩�ǂ���
                    if (vector3s[j].x + m_distance >= posX
                        && posX >= vector3s[j].x - m_distance)
                    {
                        if (vector3s[j].y + m_distance >= posY
                               && posY >= vector3s[j].y - m_distance)
                        {
                            //�ߋ��Ɉ�ł����߂������烊�g���C
                            isOK = false;
                            break;
                        }
                    }
                }


                //�������N���A������ʉ�
                if (isOK == true)
                {
                    vector3s.Add(new Vector2(posX, posY));
                    break;
                }
            }
        }
        return vector3s;
    }

}

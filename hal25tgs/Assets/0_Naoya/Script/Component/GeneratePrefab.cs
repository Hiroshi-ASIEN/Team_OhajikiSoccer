using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インスペクターに設定されたプレハブを生成するコンポーネント
/// </summary>
public class GeneratePrefab : MonoBehaviour
{
    //============================================================
    // 変数
    //============================================================
    //------------------
    // 調整可
    //------------------
    [Header("生成するプレハブ　※要設定※")]
    [SerializeField] private GameObject m_blockPrefab;

    [Header("同時に生成される際、最低でも離れるべき距離")]
    [SerializeField] private float m_distance = 1.0f;

    //------------------
    // プライベート
    //------------------
    //生成位置のリスト
    List<Vector3> m_generatePositions = new List<Vector3>();


    //============================================================
    // 関数
    //============================================================




    /// <summary>
    /// 指定された数だけ生成を試みます。
    /// 範囲は自身の体積の範囲内（回転は未想定）
    /// </summary>
    /// <returns>いくつ生成できたか</returns>
    public int GeneratePrefabInBoxVolume(int generateNum)
    {
        m_generatePositions = GetRandomPositions(generateNum);

        //生成する
        for (int i = 0; i < m_generatePositions.Count; i++)
        {
            GameObject newGameObject =
                Instantiate(m_blockPrefab, m_generatePositions[i], Quaternion.identity) as GameObject;
        }
        return 0;
    }





    /// <summary>
    /// 自身の面積の間で、最低でもdistance内に入らない距離で、
    /// 場所を取得する
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
            //まずはランダム生成
            float posX;
            float posY;

            //無限ループ怖いから1000回ループ
            for (int a = 0; a < 1000; a++)
            {
                posX = Random.Range(leftX, rightX);
                posY = Random.Range(bottomY, topY);

                bool isOK = true;

                //前の位置を取得して比較
                for (int j = 0; j < vector3s.Count; j++)
                {
                    //距離が空いてるかどうか
                    if (vector3s[j].x + m_distance >= posX
                        && posX >= vector3s[j].x - m_distance)
                    {
                        if (vector3s[j].y + m_distance >= posY
                               && posY >= vector3s[j].y - m_distance)
                        {
                            //過去に一つでもだめだったらリトライ
                            isOK = false;
                            break;
                        }
                    }
                }


                //検査をクリアしたら通過
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

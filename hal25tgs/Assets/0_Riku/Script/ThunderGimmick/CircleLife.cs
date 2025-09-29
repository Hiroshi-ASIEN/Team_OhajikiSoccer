using UnityEngine;

public class CircleLife : MonoBehaviour
{
    private float m_LifeTime = 0f;
    public float LifeTime => m_LifeTime; // 外部参照用（読み取り専用）

    void Update()
    {
        m_LifeTime += Time.deltaTime;
    }

    // リセットしたい場合（再利用時など）
    public void ResetLifeTime()
    {
        m_LifeTime = 0f;
    }
}

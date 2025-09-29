using UnityEngine;

public class CircleLife : MonoBehaviour
{
    private float m_LifeTime = 0f;
    public float LifeTime => m_LifeTime; // �O���Q�Ɨp�i�ǂݎ���p�j

    void Update()
    {
        m_LifeTime += Time.deltaTime;
    }

    // ���Z�b�g�������ꍇ�i�ė��p���Ȃǁj
    public void ResetLifeTime()
    {
        m_LifeTime = 0f;
    }
}

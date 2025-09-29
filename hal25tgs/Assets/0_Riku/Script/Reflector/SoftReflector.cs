using UnityEngine;

public class SoftReflector : MonoBehaviour
{
    Mesh m_Mesh;
    Vector3[] m_OriginalVertices;
    Vector3[] m_DeformedVertices;
    Vector3 m_LastImpactPoint;
    float m_ImpactTime;
    bool m_HasImpact = false;

    [SerializeField] private float m_WaveAmplitude = 0.05f;   // �g�̑傫���i�h��̋����j
    [SerializeField] private float m_WaveFrequency = 15.0f;    // �g�̎��g���i�����^�ׂ����j
    [SerializeField] private float m_WaveDamping = 1.0f;       // �����̑����i���ԂƋ��ɏ����鑬�x�j
    [SerializeField] private float m_WaveSpread = 0.5f;        // �g�̍L����i�e���͈́j

    void Start()
    {
        m_Mesh = GetComponent<MeshFilter>().mesh;
        m_OriginalVertices = m_Mesh.vertices;
        m_DeformedVertices = new Vector3[m_OriginalVertices.Length];
        m_OriginalVertices.CopyTo(m_DeformedVertices, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        m_LastImpactPoint = transform.InverseTransformPoint(collision.contacts[0].point);
        m_ImpactTime = Time.time;
        m_HasImpact = true;
    }

    void Update()
    {
        if (!m_HasImpact) return;

        float t = Time.time - m_ImpactTime;

        for (int i = 0; i < m_OriginalVertices.Length; i++)
        {
            Vector3 v = m_OriginalVertices[i];
            float dist = Vector3.Distance(v, m_LastImpactPoint);

            // �g�̉e���͈͂𐧌��i�����̒��_�قǉe�����������Ȃ�j
            float attenuation = Mathf.Exp(-dist * m_WaveSpread);

            // �����g + ����
            float wave = Mathf.Sin(t * m_WaveFrequency - dist * 10f) * m_WaveAmplitude * Mathf.Exp(-t * m_WaveDamping) * attenuation;

            // Z�����ɔg
            m_DeformedVertices[i] = v + Vector3.forward * wave;
        }

        m_Mesh.vertices = m_DeformedVertices;
        m_Mesh.RecalculateNormals();
    }
}

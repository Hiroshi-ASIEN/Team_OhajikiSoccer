using UnityEngine;

public class SoftReflector : MonoBehaviour
{
    Mesh m_Mesh;
    Vector3[] m_OriginalVertices;
    Vector3[] m_DeformedVertices;
    Vector3 m_LastImpactPoint;
    float m_ImpactTime;
    bool m_HasImpact = false;

    [SerializeField] private float m_WaveAmplitude = 0.05f;   // ”g‚Ì‘å‚«‚³i—h‚ê‚Ì‹­‚³j
    [SerializeField] private float m_WaveFrequency = 15.0f;    // ”g‚Ìü”g”i‘¬‚³^×‚©‚³j
    [SerializeField] private float m_WaveDamping = 1.0f;       // Œ¸Š‚Ì‘¬‚³iŠÔ‚Æ‹¤‚ÉÁ‚¦‚é‘¬“xj
    [SerializeField] private float m_WaveSpread = 0.5f;        // ”g‚ÌL‚ª‚èi‰e‹¿”ÍˆÍj

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

            // ”g‚Ì‰e‹¿”ÍˆÍ‚ğ§ŒÀi‰“‚­‚Ì’¸“_‚Ù‚Ç‰e‹¿‚ª¬‚³‚­‚È‚éj
            float attenuation = Mathf.Exp(-dist * m_WaveSpread);

            // ³Œ·”g + Œ¸Š
            float wave = Mathf.Sin(t * m_WaveFrequency - dist * 10f) * m_WaveAmplitude * Mathf.Exp(-t * m_WaveDamping) * attenuation;

            // Z•ûŒü‚É”g
            m_DeformedVertices[i] = v + Vector3.forward * wave;
        }

        m_Mesh.vertices = m_DeformedVertices;
        m_Mesh.RecalculateNormals();
    }
}

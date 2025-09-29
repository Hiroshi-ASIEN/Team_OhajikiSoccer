using UnityEngine;
using Photon.Pun;

public class CircleFader : MonoBehaviourPun
{
    private enum FadeState
    {
        None,
        In,
        Out,
    }

    private FadeState m_State = FadeState.None;

    [SerializeField]private float m_FadeTime = 2.0f;
    [SerializeField]private float m_DestroyTime = 2.0f;
    private float m_Elapsed = 0f;
    private Material m_Mat;
    private Collider m_Col;

    void Start()
    {
        m_Col = GetComponent<Collider>();
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            m_Mat = new Material(renderer.material);
            renderer.material = m_Mat;
        }

        if (m_Mat != null)
        {
            // 透明で開始
            Color color = m_Mat.color;
            color.a = 0f;
            m_Mat.color = color;

            m_State = FadeState.In; // フェードイン開始
            m_Elapsed = 0f;
        }
    }

    [PunRPC]
    public void StartFadeOutAndDestroy()
    {
        if (m_State != FadeState.Out && m_Mat != null)
        {
            m_State = FadeState.Out;
            m_Elapsed = 0f; // フェードアウト再開始
        }
    }

    void Update()
    {
        if (m_Mat == null || m_State == FadeState.None)
            return;

        m_Elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(m_Elapsed / m_FadeTime);

        if (m_State == FadeState.In)
        {
            Color color = m_Mat.color;
            color.a = Mathf.Lerp(0f, 1f, t);
            m_Mat.color = color;

            if (t >= 1f)
            {
                m_Col.enabled = true;
                m_State = FadeState.None;
                m_Elapsed = 0f; // 次のためにリセット
            }
        }
        else if (m_State == FadeState.Out)
        {
            Color color = m_Mat.color;
            color.a = Mathf.Lerp(1f, 0f, t);
            m_Mat.color = color;

            if (t >= 1f)
                m_Col.enabled = false;

            if (m_Elapsed >= m_DestroyTime && photonView.IsMine)
            {
                SingleMultiUtility.Destroy(gameObject); // Photon/Single 両対応
            }
        }
    }
}

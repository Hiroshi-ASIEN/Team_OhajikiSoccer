using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public enum FadeMode 
    {
        In=0,
        Out,
    };

    [Header("自動切換えするか")]
    [SerializeField] private bool m_AutoFade = true;

    [Header("フェード in / out")]
    [SerializeField] private FadeMode m_Fade;

    [SerializeField] UnityEngine.Color m_FadeColor = new UnityEngine.Color(0.0f, 0.0f, 0.0f, 1.0f);
    Image m_Panel;

    [SerializeField] bool m_IsStart = false;
    bool m_IsEnd = false;

    void Start()
    {
        m_Panel = GetComponent<Image>();
        m_Panel.color = m_FadeColor;
    }

    void FixedUpdate()
    {
        Fade();
    }

    private void Fade()
    {
        if (!m_IsStart) return;

        switch (m_Fade)
        {
            case FadeMode.In:
                m_FadeColor.a -= 0.05f;
                if (m_FadeColor.a <= 0.0f)
                {
                    m_FadeColor.a = 0.0f;
                    m_Panel.color = m_FadeColor;
                    m_IsEnd = true;
                }
                m_Panel.color = m_FadeColor;
                break;

            case FadeMode.Out:
                m_FadeColor.a += 0.05f;
                if (1.0f <= m_FadeColor.a)
                {
                    m_FadeColor.a = 1.0f;
                    m_Panel.color = m_FadeColor;
                    m_IsEnd = true;
                }
                m_Panel.color = m_FadeColor;
                break;
        }

        if (!m_IsEnd) return;

        m_IsStart = false;

        if (!m_AutoFade) return;

        if (m_Fade == FadeMode.Out)
        {
            m_Fade = FadeMode.In;
        }
        else if (m_Fade == FadeMode.In)
        {
            m_Fade = FadeMode.Out;
            this.gameObject.SetActive(false);
        }

    }

    public void FadeStart() 
    {
        m_IsStart = true;
        m_IsEnd = false;
    }
    public bool IsFadeEnd() { return m_IsEnd; }

    public void SetFadeMode(FadeMode _fade)
    {
        m_Fade = _fade;

        if (m_Fade == FadeMode.In) m_FadeColor.a = 1.0f;
        else if (m_Fade == FadeMode.Out)  m_FadeColor.a = 0.0f;
    }
}
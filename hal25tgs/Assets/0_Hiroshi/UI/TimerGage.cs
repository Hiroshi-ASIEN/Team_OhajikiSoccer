using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimerGage : MonoBehaviour
{
    [Header("ターン残り時間表示ゲージバー")]
    [SerializeField] private Slider m_Slider;
    [Header("ゲージバーに反映させるタイマー")]
    [SerializeField] protected Timer m_Timer;

    [Header("矢印操作時ゲージ色")]
    [SerializeField] private Color m_Color;

    private float m_CurrentValue;
    private float m_MaxValue;

    private bool m_IsActive = false;
    private bool m_Rainbow = false;

    public Color m_Test;

    private Image m_FillImage;
    private float m_Hue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        InitGage();
    }

   public void Update()
    {
        Rainbow();

        if (!m_IsActive) return;

        m_CurrentValue = m_Timer.GetNowTime() / m_MaxValue;
        SetSliderValue(m_CurrentValue);
    }

    private void SetSliderValue(float _value)
    {
        m_Slider.value = _value;
        Canvas.ForceUpdateCanvases();
    }

    protected void EndGage()
    {
        m_CurrentValue = 0.0f;
        SetSliderValue(m_CurrentValue);

        m_IsActive = false;
    }

    private void SetGage()
    {
        m_MaxValue = m_Timer.GetMaxTime();
        m_CurrentValue = m_MaxValue;
    }

    public void StartGage()
    {
        SetGage();
        m_IsActive = true;
        Debug.Log("start");
    }

    // 初期化・イベント登録
    protected void InitGage()
    {
        SetGage();
        m_Slider = this.gameObject.GetComponent<Slider>();
        m_FillImage = m_Slider.fillRect.GetComponent<UnityEngine.UI.Image>();
        m_FillImage.color = m_Color;
    }

    private void Rainbow()
    {
        if (!m_Rainbow) return;

        // 色相を時間で変化
        m_Hue += Time.deltaTime * 0.5f;
        if (m_Hue > 1f) m_Hue -= 1f;

//        Color rainbowColor = m_Test;
        Color rainbowColor = Color.HSVToRGB(m_Hue, -m_Hue, m_Hue*0.7f);


        float t = Time.time * 2.0f; // 色変化のスピードを調整

        float r = Mathf.Sin(t) * 0.5f + 0.5f; // 0~1 に正規化
        float g = Mathf.Sin(t + 2f) * 0.5f + 0.5f;
        float b = Mathf.Sin(t + 4f) * 0.5f + 0.5f;

        m_FillImage.color = new Color(r, g, b, 1f);
//        m_FillImage.color = rainbowColor;
    }

    public void StartRainbow()
    {
        m_Rainbow = true;
        m_CurrentValue = m_MaxValue / m_MaxValue;
    }

    public void EndRainbow()
    {
        m_Rainbow = false;
        m_FillImage.color = m_Color;
    }
}
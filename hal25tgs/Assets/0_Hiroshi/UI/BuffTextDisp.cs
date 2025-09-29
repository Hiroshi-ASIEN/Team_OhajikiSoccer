using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffTextDisp : MonoBehaviour
{
    [Serializable]
    public class IntervalText
    {
        [Header("ターンに応じて表示するテキスト")]
        [SerializeField] public BUFF_TYPE m_BuffType;
        [SerializeField] private string m_Text;
        [SerializeField] private Color m_Color;

        private TextMeshProUGUI m_TextUGUI;

        public void Disp(BUFF_TYPE _type)
        {
            if (_type != m_BuffType) return;

            m_TextUGUI.text = m_Text;
            m_TextUGUI.color = m_Color;
            Debug.Log(m_Text);
        }

        public void SetTMPUGUI(TextMeshProUGUI textUGUI)
        {
            m_TextUGUI = textUGUI;
        }
    };

    [SerializeField] private IntervalText[] m_IntervalText;
    [SerializeField] private TextMeshProUGUI m_TextUGUI;

    [SerializeField]private BuffManager m_BuffManager;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        m_BuffManager.OnBuffEvent += Disp;
        for (int i = 0; i < m_IntervalText.Length; i++)
        {
            m_IntervalText[i].SetTMPUGUI(m_TextUGUI);
        }
    }


    private void OnDisable()
    {
        m_BuffManager.OnBuffEvent -= Disp;
    }

    public void Disp(BUFF_TYPE _type)
    {
        // テキストを一旦削除
            UnDisp();

        // インターバル中に、ターンに応じたテキストを表示
        for (int i = 0; i < m_IntervalText.Length; i++)
        {
            m_IntervalText[i].Disp(_type);
        }
    }

    private void UnDisp()
    {
        m_TextUGUI.text = null;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("text表示");
            Disp(m_IntervalText[0].m_BuffType);
        }
    }
}
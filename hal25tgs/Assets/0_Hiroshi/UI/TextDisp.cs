using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDisp : MonoBehaviour
{
    [Serializable]
    public class IntervalText
    {
        [Header("ターンに応じて表示するテキスト")]
        [SerializeField] public TurnManager.TURN_STATE m_TurnState;
        [SerializeField] private string m_Text;
        [SerializeField] private Color m_Color;

        private TextMeshProUGUI m_TextUGUI;

        public void Disp(TurnManager.TURN_STATE _state)
        {
            if (_state != m_TurnState) return;

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

    private TurnManager m_TurnManager;

    // Start is called before the first frame update
    void Start()
    {

    }
    private void Init()
    {
        m_TurnManager = TurnManager.Instance;
        m_TurnManager.OnTurnChanged += Disp;
        for (int i = 0; i < m_IntervalText.Length; i++)
        {
            m_IntervalText[i].SetTMPUGUI(m_TextUGUI);
        }
    }
    private void FixedUpdate()
    {
        if (!m_TurnManager)
        {
            Init();
        }
    }
    private void OnDisable()
    {
        m_TurnManager.OnTurnChanged -= Disp;
    }

    public void Disp(TurnManager.TURN_STATE _state)
    {
        TurnManager.TURN_STATE nextState = m_TurnManager.GetNextTurnState();

        // 現在がインターバルでなければ（次がインターバル）なら現在はテキストは消す
        if (nextState == TurnManager.TURN_STATE.TURN_INTERVAL)
        {
            UnDisp();
            return;
        }

        // インターバル中に、ターンに応じたテキストを表示
        for (int i = 0; i < m_IntervalText.Length; i++)
        {
            m_IntervalText[i].Disp(nextState);
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
            Disp(m_IntervalText[0].m_TurnState);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private Timer m_Timer;
    [Header("���쎞�ԂƓ��쎞��")]
    [SerializeField] private float m_PlayTime;
    [SerializeField] private float m_ActiveTime;

    private bool m_Active = false;
    // Start is called before the first frame update
    void Start()
    {
        m_Timer.SetMaxTimer(m_PlayTime);
        m_Timer.TimerStart();
    }

    // Update is called once per frame
    void Update()
    {
        TurnSwitch();
    }

    private void TurnSwitch()
    {
        if (!m_Timer.TimerEnd()) return;

        if (m_Active)
        {
            m_Active = false;
            m_Timer.SetMaxTimer(m_PlayTime);
            m_Timer.TimerStart();
        }
        else if (!m_Active)
        {
            m_Active = true;
            m_Timer.SetMaxTimer(m_ActiveTime);
            m_Timer.TimerStart();
        }
    }

    // ���쎞�Ԓ����擾
    public bool GetPlayTimeTurn()
    { 
    return !m_Active;
    }
}

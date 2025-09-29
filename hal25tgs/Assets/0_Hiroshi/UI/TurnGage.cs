using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TurnGage : TimerGage
{
    [Header("ゲージとして表示させたいターン")]
    [SerializeField] TurnManager.TURN_STATE m_DispTurn;
    private TurnManager m_TurnManager;

    // Start is called before the first frame update
    void Start()
    {
        m_TurnManager = TurnManager.Instance;
        m_TurnManager.OnTurnChanged += RestartGage;
        m_Timer = m_TurnManager.GetTurnTimer();

        InitGage();
        EndGage();
        Debug.Log("ゲージ設定完了");
    }

    private void OnDisable()
    {
        m_TurnManager.OnTurnChanged -= RestartGage;
    }

    public void RestartGage(TurnManager.TURN_STATE _state)
    {
        EndGage();

        if (_state == TurnManager.TURN_STATE.BURST_TURN)
        {
            StartRainbow();
        }
        else if (m_DispTurn != _state)
        {
            EndRainbow();
            EndGage();
            return;
        }
        StartGage();
    }

}
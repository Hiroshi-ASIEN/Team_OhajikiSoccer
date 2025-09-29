using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_RealTime : Buff_Base
{
    public override void BuffActivate()
    {
        Debug.Log("バフ：リアルタイム");
        // 具体的な効果をこの下に記述
        TurnManager.Instance.BurstTimeStart();

        TurnManager.Instance.OnTurnChanged += EndRealTime;
    }

    public override void BuffDeactivate()
    {
        TurnManager.Instance.OnTurnChanged -= EndRealTime;
    }

    public void EndRealTime(TurnManager.TURN_STATE _state)
    {
        if (_state == TurnManager.TURN_STATE.BURST_TURN) return;

        this.GetComponent<BuffManager>().ChangeBuff();
    }
}

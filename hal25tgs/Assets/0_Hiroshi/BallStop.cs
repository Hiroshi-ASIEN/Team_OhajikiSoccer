using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallStop : MonoBehaviour
{
    private Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        TurnManager.Instance.OnTurnChanged += BallStopEvent;
    }

    private void OnDisable()
    {
        TurnManager.Instance.OnTurnChanged -= BallStopEvent;
    }

    // �^�[���}�l�[�W���[�̃C�x���g�o�^���ăC���^�[�o�����ɃI��
    void BallStopEvent(TurnManager.TURN_STATE _state)
    {
        if (_state == TurnManager.TURN_STATE.TURN_INTERVAL)
        {
            SetBallKinematic(true);
        }
        else
        {
            SetBallKinematic(false);
        }
    }

    void SetBallKinematic(bool _bool)
    {
        this.gameObject.GetComponent<Rigidbody>().isKinematic = _bool;
    }

}

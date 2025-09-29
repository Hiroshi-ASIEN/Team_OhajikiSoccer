using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTriggerMove : ObjectMove
{
    [SerializeField] private StageSwap m_StageSwap;
    // Start is called before the first frame update

    private void Start()
    {
        m_Object = this.gameObject;
        m_StageSwap.OnStadiumChanged += StadiumCamera;
    }

    private void OnDisable()
    {
        m_StageSwap.OnStadiumChanged -= StadiumCamera;
    }

    public void StadiumCamera(StageSwap.STADIUM_TYPE _type)
    {
        switch (_type)
        {
            case StageSwap.STADIUM_TYPE.NORMAL:
                SetAll(0);
                break;
            case StageSwap.STADIUM_TYPE.DOUBLEGOAL:
                SetAll(0);
                break;
            case StageSwap.STADIUM_TYPE.LIGHTNING:
                SetAll(1);
                break;
            case StageSwap.STADIUM_TYPE.FROST:
                SetAll(1);
                break;
            case StageSwap.STADIUM_TYPE.WARP:
                SetAll(2);
                break;
            case StageSwap.STADIUM_TYPE.HOLE:
                SetAll(2);
                break;
        }
    }
}

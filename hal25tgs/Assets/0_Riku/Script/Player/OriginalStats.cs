using Photon.Pun;
using UnityEngine;
using static TurnManager;

public class OriginalStats : MonoBehaviourPun
{

    private Vector3 m_OriginalScale;
    private float m_OriginalMass;
    private bool m_ItemEffectActive = true;
    private bool m_IsActiveTurn = false;

    private Rigidbody m_Rb;

    float m_TurnCount = -1.0f;

    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();

        m_OriginalScale = transform.localScale;
        m_OriginalMass = m_Rb.mass;

        TurnManager.Instance.OnTurnChanged += Active;
    }

    private void Update()
    {
        if (m_IsActiveTurn)
        {
            m_ItemEffectActive = false;
            m_IsActiveTurn = false;
        }
    }

    private void Active(TURN_STATE _state)
    {
        if (_state != TURN_STATE.ACTIVE_TURN)
        {
            m_IsActiveTurn = false;
            m_ItemEffectActive = true;
        }
        else
            m_IsActiveTurn = true;
    }

    //[PunRPC]
    public void ItemEffectActive()
    {
        m_ItemEffectActive = true;
    }

    public bool GetItemEffectActive()
    {
        return m_ItemEffectActive;
    }

    public Vector3 GetOriginalScale()
    {
        return m_OriginalScale;
    }

    public float GetOriginalMass()
    {
        return m_OriginalMass;
    }

    public void SetTurnCount(float turnCount)
    {
        m_TurnCount = turnCount;
    }

    public float GetTurnCount()
    {
        return m_TurnCount;
    }

}

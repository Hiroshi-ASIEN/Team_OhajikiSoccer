using UnityEngine;
using static StageSwap;

public class PlayerFriction : MonoBehaviour
{
    [SerializeField] private float m_IceDrag = 0.08f;
    [SerializeField] private float m_DefaultDrag = 0.8f;
    private Rigidbody m_Rb;

    bool m_IsFrost;

    void Start()
    {
        PlayerObject playerObject = this.GetComponent<PlayerObject>();
        TeamObject teamObject = playerObject.GetTeamObject();
        GameManager_S gameManager_S = teamObject.GetGameManager();
        gameManager_S.gameObject.GetComponent<StageSwap>().OnStadiumChanged += StageFrostCheck;

        m_Rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (m_IsFrost)
        {
            m_Rb.drag = m_IceDrag;
        }
        else
        {
            m_Rb.drag = m_DefaultDrag;
        }
    }

    private void StageFrostCheck(STADIUM_TYPE _state)
    {
        if (_state != STADIUM_TYPE.FROST)
            m_IsFrost = false;
        else
            m_IsFrost = true;
    }

}

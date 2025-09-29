using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pitfall : MonoBehaviour
{
    List<GameObject> m_PlayerList = new List<GameObject>();

    [Header("TeamA:�ߊl�ꏊ")]
    [SerializeField] GameObject m_JailA;
    [Header("TeamB:�ߊl�ꏊ")]
    [SerializeField] GameObject m_JailB;

    [Header("TeamA:�o���ꏊ")]
    [SerializeField] List<GameObject> m_AppearA;
    [Header("TeamB:�o���ꏊ")]
    [SerializeField] List<GameObject> m_AppearB;

    TurnManager m_TurnManager;

    // Start is called before the first frame update
    void Start()
    {
        
        m_TurnManager = TurnManager.Instance;
        m_TurnManager.OnTurnChanged += ReplacePosition;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //���I�u�W�F�N�g�ɐG���ƟB�̂Ȃ��Ɉړ�
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //�Ԃ�����Player�^�O�I�u�W�F�N�g��List�ɒǉ�
            m_PlayerList.Add(other.gameObject);

            //TeamName�ɂ���Ă��ꂼ��̟B�Ɉړ�
            if(other.gameObject.GetComponent<PlayerObject>().GetTeamName() == TeamName.TeamA)
            {
                other.gameObject.transform.position = m_JailA.transform.position;
            }
            else if(other.gameObject.GetComponent<PlayerObject>().GetTeamName() == TeamName.TeamB)
            {
                other.gameObject.transform.position = m_JailB.transform.position;
            }
        }
    }

    //�o���ꏊ����o��
    //���Ăяo�����^�C�~���O��PLAY_TURN�̎�
    public void ReplacePosition(TurnManager.TURN_STATE _turn)
    {
        //PLAY_TURN�ȊO�ł͌Ăяo����Ă������ɂʂ���
        if(_turn != TurnManager.TURN_STATE.PLAY_TURN)
        {
            return;
        }

        foreach (var player in m_PlayerList)
        {
            if(player.GetComponent<PlayerObject>().GetTeamName() == TeamName.TeamA)
            {
                int index = Random.Range(0, m_AppearA.Count - 1);
                player.transform.position = m_AppearA[index].transform.position;
            }
            else if(player.GetComponent<PlayerObject>().GetTeamName() == TeamName.TeamB)
            {
                int index = Random.Range(0, m_AppearB.Count - 1);
                player.transform.position = m_AppearB[index].transform.position;

            }
        }
        Debug.Log(m_PlayerList);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    //�`�[���̎Q�Ƃ�ۗL�i����SerializeField�Ȃ̂̓e�X�g�̂��߁B�ŏI�I�ɂ͂�߂�j
    [SerializeField] TeamObject m_teamObject;
    TeamName m_TeamName = TeamName.None;


    void Start()
    {
        
    }

    void Update()
    {

    }


    //�`�[���̏���^����
    public TeamObject GetTeamObject() { 

        if (m_teamObject == null)
        {
        }

        return m_teamObject; 
    }

    /// <summary>
    /// �`�[���̓o�^�@�i�v���C���[�N���G�C�g�̒i�K�Őݒ肷��B�`�[���I�u�W�F�N�g����Ă΂��i����j�j
    /// </summary>
    /// <param name="_team"></param>
    public void SetTeamObject(TeamObject _team)
    {
        m_teamObject = _team;
        m_teamObject.AddPlayer(this);
    }


    public void SetTeamName(TeamName teamName)
    {
        m_TeamName = teamName;
        if(m_TeamName == TeamName.TeamA)
        {
            Debug.Log("TeamA�ł���I");
        }
        else if(m_TeamName==TeamName.TeamB)
        {
            Debug.Log("TeamB�ł���I");
        }
    }

    public TeamName GetTeamName()
    {
        return m_TeamName;
    }
}

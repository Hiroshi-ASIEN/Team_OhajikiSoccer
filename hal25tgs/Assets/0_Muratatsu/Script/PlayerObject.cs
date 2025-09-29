using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    //チームの参照を保有（今はSerializeFieldなのはテストのため。最終的にはやめる）
    [SerializeField] TeamObject m_teamObject;
    TeamName m_TeamName = TeamName.None;


    void Start()
    {
        
    }

    void Update()
    {

    }


    //チームの情報を与える
    public TeamObject GetTeamObject() { 

        if (m_teamObject == null)
        {
        }

        return m_teamObject; 
    }

    /// <summary>
    /// チームの登録　（プレイヤークリエイトの段階で設定する。チームオブジェクトから呼ばれる（未定））
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
            Debug.Log("TeamAですよ！");
        }
        else if(m_TeamName==TeamName.TeamB)
        {
            Debug.Log("TeamBですよ！");
        }
    }

    public TeamName GetTeamName()
    {
        return m_TeamName;
    }
}

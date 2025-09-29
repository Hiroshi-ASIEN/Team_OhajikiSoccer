using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelectionSolo : MonoBehaviour
{

    [SerializeField] private SceneChanger m_SceneChanger;

    [SerializeField] private UnityEngine.UI.Button m_TeamA_Button;    // チーム選択ボタン
    [SerializeField] private UnityEngine.UI.Button m_TeamB_Button;    // チーム選択ボタン
    [SerializeField] private GameObject m_Character1_Button;
    [SerializeField] private GameObject m_Character2_Button;
    [SerializeField] private GameObject m_Character3_Button;
    [SerializeField] private GameObject m_Character4_Button;
    [SerializeField] private GameObject m_Ready_Button;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectTeamA()
    {
        SetPlayerTeam(TeamName.TeamA);
        m_Character1_Button.SetActive(true);
        m_Character2_Button.SetActive(true);
        m_Character3_Button.SetActive(true);
        m_Character4_Button.SetActive(true);
    }

    public void SelectTeamB()
    {
        SetPlayerTeam(TeamName.TeamB);
        m_Character1_Button.SetActive(true);
        m_Character2_Button.SetActive(true);
        m_Character3_Button.SetActive(true);
        m_Character4_Button.SetActive(true);
    }

    private void SetPlayerTeam(TeamName _teamName)
    {

        if (_teamName == TeamName.TeamA)
        {
            //            m_StatusText.text = $"Team : Tokyo"; // UIに反映
            Debug.Log($"チーム選択: {_teamName}");
        }
        else if (_teamName == TeamName.TeamB)
        {
            //            m_StatusText.text = $"Team : Urawa"; // UIに反映
            Debug.Log($"チーム選択: {_teamName}");
        }

        ScoreManager.Instance.SetSoloTeamNeme(_teamName);
    }

    public void SelectCharacter1()
    {
        SelectCharacter(Character.Character1);
        m_Ready_Button.SetActive(true);
    }
    public void SelectCharacter2()
    {
        SelectCharacter(Character.Character2);
        m_Ready_Button.SetActive(true);
    }
    public void SelectCharacter3()
    {
        SelectCharacter(Character.Character3);
        m_Ready_Button.SetActive(true);
    } 
    public void SelectCharacter4()
    {
        SelectCharacter(Character.Character4);
        m_Ready_Button.SetActive(true);
    }

    private void SelectCharacter(Character _character)
    {
        ScoreManager.Instance.SetSoloCharacter(_character);
    }

}

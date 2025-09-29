using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviourPunCallbacks
{
    GameObject m_Player;
    public GameObject[] m_Players;

    //Team(TeamObject)完成時はこっちかな(仮)？
    /*
    Team m_TeamA;
    Team m_TeamB;
    */
    GameManager_S m_GameManager_S;

    //テスト用Player生成
    public Vector3 playerPosition1 = Vector3.zero;
    public Vector3 playerPosition2 = Vector3.zero;
    public Vector3 playerPosition3 = Vector3.zero;
    public Vector3 playerPosition4 = Vector3.zero;
    public Vector3 enemyPosition1 = Vector3.zero;
    public Vector3 enemyPosition2 = Vector3.zero;
    public Vector3 enemyPosition3 = Vector3.zero;
    public Vector3 enemyPosition4 = Vector3.zero;
    public Vector3 keeperPosition1 = Vector3.zero;
    public Vector3 keeperPosition2 = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        //仮作成
        /*
        m_TeamA = gameObject.AddComponent<Team>();
        m_TeamB = gameObject.AddComponent<Team>();
        */

        m_GameManager_S = GetComponent<GameManager_S>();
        /*
        StartCoroutine(CreatePlayer2(playerPosition1, enemyPosition1));
        StartCoroutine(CreatePlayer2(playerPosition2, enemyPosition2));
        StartCoroutine(CreatePlayer2(playerPosition3, enemyPosition3));
        StartCoroutine(CreatePlayer2(playerPosition4, enemyPosition4));
        StartCoroutine(CreateKeeper(keeperPosition1, keeperPosition2));
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    private void CreatePlayer1()
    {

        if (m_GameManager_S.AssignPlayersToTeams() == TeamName.TeamA)
        {
            m_Player = PhotonNetwork.Instantiate("Player1", playerPosition1, Quaternion.identity);
        }
        else
        {
            m_Player = PhotonNetwork.Instantiate("Player1", enemyPosition1, Quaternion.identity);
        }
        m_Players = GameObject.FindGameObjectsWithTag("Player");



        
        Vector3 position = new Vector3(Random.Range(-3f, 3f), 0.5f, 0.0f);
        player = PhotonNetwork.Instantiate("Player1", position, Quaternion.identity);
        Debug.Log(player.tag);
        players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(players.Length);
        
    }
    */

    IEnumerator CreatePlayer2(Vector3 playerPosition, Vector3 enemyPosition)
    {
        if (m_GameManager_S.AssignPlayersToTeams() == TeamName.TeamA)
        {
            m_Player = PhotonNetwork.Instantiate("Player1", playerPosition, Quaternion.identity);
        }
        else
        {
            m_Player = PhotonNetwork.Instantiate("Player1", enemyPosition, Quaternion.identity);
        }
        m_Players = GameObject.FindGameObjectsWithTag("Player");
        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator CreateKeeper(Vector3 playerPosition, Vector3 enemyPosition)
    {
        if (m_GameManager_S.AssignPlayersToTeams() == TeamName.TeamA)
        {
            m_Player = PhotonNetwork.Instantiate("Keeper", playerPosition, Quaternion.identity);
        }
        else
        {
            m_Player = PhotonNetwork.Instantiate("Keeper", enemyPosition, Quaternion.identity);
        }
        m_Players = GameObject.FindGameObjectsWithTag("Player");
        yield return new WaitForSeconds(0.2f);
    }

}

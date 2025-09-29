using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviourPunCallbacks
{

    GameObject player;
    public GameObject[] players;

    GameManager_S gameManager_S;
   // public Material playerMaterial;
    //public Material enemyMaterial;


    public Vector3 playerPosition1 = Vector3.zero;
    public Vector3 playerPosition2 = Vector3.zero;
    public Vector3 playerPosition3 = Vector3.zero;
    public Vector3 playerPosition4 = Vector3.zero;
    public Vector3 enemyPosition1 = Vector3.zero;
    public Vector3 enemyPosition2 = Vector3.zero;
    public Vector3 enemyPosition3 = Vector3.zero;
    public Vector3 enemyPosition4 = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        gameManager_S = GetComponent<GameManager_S>();
        StartCoroutine(CreatePlayer2(playerPosition1, enemyPosition1));
        StartCoroutine(CreatePlayer2(playerPosition2, enemyPosition2));
        StartCoroutine(CreatePlayer2(playerPosition3, enemyPosition3));
        StartCoroutine(CreatePlayer2(playerPosition4, enemyPosition4));
        /*
        Invoke("CreatePlayer1", 0.2f);
        Invoke("CreatePlayer1", 0.2f);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlayer1()
    {

        if (gameManager_S.AssignPlayersToTeams() == TeamName.TeamA)
        {
            player = PhotonNetwork.Instantiate("Player1", playerPosition1, Quaternion.identity);
            //player.GetComponent<MeshRenderer>().material = playerMaterial;
        }
        else
        {
            player = PhotonNetwork.Instantiate("Player1", enemyPosition1, Quaternion.identity);
            //player.GetComponent<MeshRenderer>().material = enemyMaterial;
        }
        players = GameObject.FindGameObjectsWithTag("Player");



        /*
        Vector3 position = new Vector3(Random.Range(-3f, 3f), 0.5f, 0.0f);
        player = PhotonNetwork.Instantiate("Player1", position, Quaternion.identity);
        Debug.Log(player.tag);
        players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(players.Length);
        */
    }

    IEnumerator CreatePlayer2(Vector3 playerPosition, Vector3 enemyPosition)
    {
        if (gameManager_S.AssignPlayersToTeams() == TeamName.TeamA)
        {
            player = PhotonNetwork.Instantiate("Player1", playerPosition, Quaternion.identity);
        }
        else
        {
            player = PhotonNetwork.Instantiate("Player1", enemyPosition, Quaternion.identity);
        }
        players = GameObject.FindGameObjectsWithTag("Player");


        yield return new WaitForSeconds(0.2f);
    }

}

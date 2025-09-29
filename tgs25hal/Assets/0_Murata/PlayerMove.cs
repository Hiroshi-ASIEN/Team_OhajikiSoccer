using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    public Vector3 velocity;
    Rigidbody rb;
    public Material playerMaterial;
    public Material enemyMaterial;
    GameManager_S gameManager_S;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject obj = GameObject.Find("GameObject");

        gameManager_S = obj.GetComponent<GameManager_S>();

        if (photonView.IsMine)
        {
            tag = "Player";
            enabled = true;
            if (gameManager_S.AssignPlayersToTeams() == TeamName.TeamA)
            {
                GetComponent<MeshRenderer>().material = playerMaterial;
            }
            else
            {
                GetComponent<MeshRenderer>().material = enemyMaterial;
            }
        }
        else
        {
            tag = "Untagged";
            enabled = false;
            if (gameManager_S.AssignPlayersToTeams() == TeamName.TeamA)
            {
                GetComponent<MeshRenderer>().material = enemyMaterial;
            }
            else
            {
                GetComponent<MeshRenderer>().material = playerMaterial;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move()
    {
        Debug.Log("ä÷êîåƒÇ—èoÇµ");
        rb.AddForce(velocity * 0.25f, ForceMode.Impulse);
    }
}

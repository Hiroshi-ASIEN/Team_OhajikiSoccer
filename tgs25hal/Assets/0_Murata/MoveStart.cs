using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStart : MonoBehaviourPunCallbacks
{
    bool move = false;

    //GameObject[] objs;
    CreatePlayer script;

    GameObject Timer;
    TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        //objs = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log(objs.Length);
        Timer = GameObject.Find("TurnManager");
        turnManager = Timer.GetComponent<TurnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    move = true;
        //}
    }

    private void FixedUpdate()
    {

        if (!turnManager.GetPlayTimeTurn() && !move)
        {
            script = GetComponent<CreatePlayer>();
            for(int i = 0; i < script.players.Length;i++)
            {
                PlayerMove playerMove = script.players[i].GetComponent<PlayerMove>();
                playerMove.Move();
            }
            move = true;
        }
        if(turnManager.GetPlayTimeTurn() && move)
        {
            move = false;
        }
    }
}

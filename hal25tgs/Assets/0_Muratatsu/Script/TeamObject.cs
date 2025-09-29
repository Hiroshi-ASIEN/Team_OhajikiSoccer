using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class TeamObject : MonoBehaviourPunCallbacks
{
    //各プレイヤーの参照を保有
    GameObject m_Player;
    //List<PlayerObject> m_playersList;
    PlayerObject[] m_Players;

    [Header("アイテムのインベントリのインスタンス")]
    [SerializeField] ItemInventory m_itemInventory;

    //ゲームマネージャーの参照を保有（ゲームマネージャー側から登録される）
    GameManager_S m_GameManager = null;

    [Header("TeamAPlayer")]
    public Vector3 LeftPlayerPosition1 = Vector3.zero;
    public Vector3 LeftPlayerPosition2 = Vector3.zero;
    public Vector3 LeftPlayerPosition3 = Vector3.zero;
    public Vector3 LeftPlayerPosition4 = Vector3.zero;
    public Vector3 LeftKeeperPosition = Vector3.zero;

    [Header("TeamBPlayer")]
    public Vector3 RightPlayerPosition1 = Vector3.zero;
    public Vector3 RightPlayerPosition2 = Vector3.zero;
    public Vector3 RightPlayerPosition3 = Vector3.zero;
    public Vector3 RightPlayerPosition4 = Vector3.zero;
    public Vector3 RightKeeperPosition = Vector3.zero;

    [SerializeField] Character m_MyCharacter;
    [SerializeField] Character m_YourCharacter;
    void Start()
    {
        m_GameManager = GameObject.FindObjectOfType<GameManager_S>();
        Debug.Log(m_GameManager);
        m_Players = new PlayerObject[5];

        //自分が生成したTeamManager以外は実行しない
        if(photonView.IsMine)
        {
            //m_playersList = new List<PlayerObject>();
            CreatePlayerMulti();
            /*
            if (m_itemInventory)
                m_itemInventory.SetTeam(this);

            //ここでプレイヤーのインスタンスを生成しつつ、
            //PlayerObject::SetTeamObjectをしてもいいかも？
            StartCoroutine(CreatePlayer(LeftPlayerPosition1, RightPlayerPosition1));
            StartCoroutine(CreatePlayer(LeftPlayerPosition2, RightPlayerPosition2));
            StartCoroutine(CreatePlayer(LeftPlayerPosition3, RightPlayerPosition3));
            StartCoroutine(CreatePlayer(LeftPlayerPosition4, RightPlayerPosition4));
            StartCoroutine(CreateKeeper(LeftKeeperPosition, RightKeeperPosition));
            for (int i = 0; i < m_Players.Length; i++)
            {
                if (!m_Players[i])
                {
                    continue;
                }
                m_Players[i].SetTeamObject(this);
            }

            TurnManager.Instance.SetTeamObject(this);   // ターンマネージャーにチームオブジェクト登録
            */
        }



        //************************************************************
        //追記　
        //ここに記述を移動　下に書くとなぜか呼ばれなかったので
        //************************************************************
        if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
        {
            Debug.Log("アイテムインベントリ（左）のキャンバス生成を試みました");

            m_itemInventory.SetInventoryPositionAndInit(InventoryPos.Left);
        }
        else
        {
            Debug.Log("アイテムインベントリ（右）のキャンバス生成を試みました");

            m_itemInventory.SetInventoryPositionAndInit(InventoryPos.Right);
        }




        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("yobareta");
            CreatePlayerSolo();
            TurnManager.Instance.SetTeamObject(this);   // ターンマネージャーにチームオブジェクト登録
        }

        //相手のPlayerと相手のTeamManagerを紐づけ
        TeamObject enemyTeamObject = null;
        List<PlayerObject> enemyObject = null;
        enemyObject = new List<PlayerObject>();

        PhotonView[] views = GameObject.FindObjectsOfType<PhotonView>();
        foreach(PhotonView view in views)
        {
            if (view.Owner != PhotonNetwork.LocalPlayer)
            {
                if (view.gameObject.GetComponent<TeamObject>() != null)
                {
                    enemyTeamObject = view.gameObject.GetComponent<TeamObject>();
                }
                if(view.gameObject.GetComponent<PlayerObject>() != null)
                {
                    enemyObject.Add(view.gameObject.GetComponent<PlayerObject>());
                }
            }
        }

        foreach(PlayerObject obj in enemyObject)
        {
            obj.SetTeamObject(enemyTeamObject);
        }

        //-------------------------------------------------------

        //インベントリの左右決定（正常に動作しない）これは、クライアントがAなら、Aしか返さない関数だから。
        /*
        if (m_GameManager.IsTeamA(this))
            m_itemInventory.SetInventoryPositionAndInit(InventoryPos.Left);
        else
            m_itemInventory.SetInventoryPositionAndInit(InventoryPos.Right);
        */

    }

    void Update()
    {


        
    }





    /// <summary>
    /// インベントリにアイテムを追加する　（プレイヤーから呼ばれる）
    /// </summary>
    /// <param name="_item"></param>
    public void AddItemToInventory(ItemObject _item)
    {
        if (m_itemInventory)
        {
            Debug.Log("AddItem関数が呼ばれました。");
            m_itemInventory.AddItem(_item);
        }

    }

    //public List<PlayerObject> GetPlayers()
    //{
    //    return m_playersList;
    //}
    public PlayerObject[] GetPlayersArray()
    {
        return m_Players;
    }

    public GameManager_S GetGameManager()
    { 
        return m_GameManager; 
    }

    //ゲームマネージャーから登録
    public void SetGameManager(GameManager_S _manager)
    {
        m_GameManager = _manager;
    }

    //ゲームマネージャーから使用
    public void UseItemByGameManager(int _itemIndex)
    {
        if (m_itemInventory)
            m_itemInventory.UseItemUnmaster(_itemIndex);
    }


    //プレイヤー生成
    IEnumerator CreatePlayer(Character character, Vector3 playerPosition, Vector3 enemyPosition)
    {
        Debug.Log("Player生成");
        if (m_GameManager.AssignPlayersToTeams() == TeamName.TeamA)
        {
            switch(character)
            {
                case Character.Character1:
                    m_Player = SingleMultiUtility.InstantiateForClient("Player1", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                case Character.Character2:
                    m_Player = SingleMultiUtility.InstantiateForClient("Player2", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                case Character.Character3:
                    m_Player = SingleMultiUtility.InstantiateForClient("Player3", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                case Character.Character4:
                    m_Player = SingleMultiUtility.InstantiateForClient("Player4", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
            }
            m_Player.GetComponent<PlayerObject>().SetTeamName(TeamName.TeamA);
        }
        else
        {
            switch (character)
            {
                case Character.Character1:
                    m_Player = SingleMultiUtility.InstantiateForClient("Player1", enemyPosition, Quaternion.Euler(0.0f,-90.0f,0.0f));
                    break;
                case Character.Character2:
                    m_Player = SingleMultiUtility.InstantiateForClient("Player2", enemyPosition, Quaternion.Euler(0.0f,-90.0f,0.0f));
                    break;
                case Character.Character3:
                    m_Player = SingleMultiUtility.InstantiateForClient("Player3", enemyPosition, Quaternion.Euler(0.0f,-90.0f,0.0f));
                    break;
                case Character.Character4:
                    m_Player = SingleMultiUtility.InstantiateForClient("Player4", enemyPosition, Quaternion.Euler(0.0f,-90.0f,0.0f));
                    break;
            }

            m_Player.GetComponent<PlayerObject>().SetTeamName(TeamName.TeamB);
        }
        //m_playersList.Add(m_Player.GetComponent<PlayerObject>());
        int index = GetPlayersUnusedNum();
        m_Players[index] = m_Player.GetComponent<PlayerObject>();

        
        yield return new WaitForSeconds(0.2f);
    }
    //キーパー生成
    IEnumerator CreateKeeper(Character character, Vector3 playerPosition, Vector3 enemyPosition)
    {
        if (m_GameManager.AssignPlayersToTeams() == TeamName.TeamA)
        {
            switch (character)
            {
                case Character.Character1:
                    m_Player = SingleMultiUtility.InstantiateForClient("Keeper1", playerPosition, Quaternion.identity);
                    break;
                case Character.Character2:
                    m_Player = SingleMultiUtility.InstantiateForClient("Keeper2", playerPosition, Quaternion.identity);
                    break;
                case Character.Character3:
                    m_Player = SingleMultiUtility.InstantiateForClient("Keeper3", playerPosition, Quaternion.identity);
                    break;
                case Character.Character4:
                    m_Player = SingleMultiUtility.InstantiateForClient("Keeper4", playerPosition, Quaternion.identity);
                    break;
            }

            m_Player.GetComponent<PlayerObject>().SetTeamName(TeamName.TeamA);
        }
        else
        {
            switch (character)
            {
                case Character.Character1:
                    m_Player = SingleMultiUtility.InstantiateForClient("Keeper1", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
                case Character.Character2:
                    m_Player = SingleMultiUtility.InstantiateForClient("Keeper2", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
                case Character.Character3:
                    m_Player = SingleMultiUtility.InstantiateForClient("Keeper3", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
                case Character.Character4:
                    m_Player = SingleMultiUtility.InstantiateForClient("Keeper4", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
            }
            m_Player.GetComponent<PlayerObject>().SetTeamName(TeamName.TeamB);
        }
        //m_playersList.Add(m_Player.GetComponent<PlayerObject>());
        int index = GetPlayersUnusedNum();
        m_Players[index] = m_Player.GetComponent<PlayerObject>();

        yield return new WaitForSeconds(0.2f);
    }

    public void CreatePlayerMulti()
    {
        if (m_itemInventory)
            m_itemInventory.SetTeam(this);

        m_MyCharacter = ScoreManager.Instance.m_CharacterA;
        Debug.Log("Multi" + m_MyCharacter);

        //ここでプレイヤーのインスタンスを生成しつつ、
        //PlayerObject::SetTeamObjectをしてもいいかも？
        StartCoroutine(CreatePlayer(m_MyCharacter, LeftPlayerPosition1, RightPlayerPosition1));
        StartCoroutine(CreatePlayer(m_MyCharacter, LeftPlayerPosition2, RightPlayerPosition2));
        StartCoroutine(CreatePlayer(m_MyCharacter, LeftPlayerPosition3, RightPlayerPosition3));
        StartCoroutine(CreatePlayer(m_MyCharacter, LeftPlayerPosition4, RightPlayerPosition4));
        StartCoroutine(CreateKeeper(m_MyCharacter, LeftKeeperPosition, RightKeeperPosition));
        /*
        for(int i = 0; i < m_playersList.Count; i++)
        {
            m_playersList[i].SetTeamObject(this);
        }
        */
        for (int i = 0; i < m_Players.Length; i++)
        {
            if (!m_Players[i])
            {
                continue;
            }
            m_Players[i].SetTeamObject(this);
        }

        TurnManager.Instance.SetTeamObject(this);   // ターンマネージャーにチームオブジェクト登録

        m_GameManager.SetTeamUI();

    }

    //配列の使ってない場所探索
    //全て使ってたら-1
    public int GetPlayersUnusedNum()
    {
        int num = -1;
        for(int i = 0; i < m_Players.Length; i++)
        {
            if (!m_Players[i])
            {
                return i;
            }
        }

        return num;
    }

    public void AddPlayer(PlayerObject _playerObject)
    {
        if (m_Players == null)
        {
            Debug.LogError("m_PlayerListがnullです！");
        }

        if (_playerObject == null)
        {
            Debug.LogError("_playerObjectがnullです");
        }

        for (int i = 0; i < m_Players.Length; i++)
        {
            if (!m_Players[i])
            {
                m_Players[i] = _playerObject;
            }
        }
    }


    //プレイヤー生成
    IEnumerator CreateSoloPlayer(Character character,Vector3 playerPosition, Vector3 enemyPosition)
    {
        Debug.Log("Player生成");
        if (m_GameManager.AssignPlayersToTeams() == TeamName.TeamA)
        {
            switch(character)
            {
                case Character.Character1:
                    m_Player = SingleMultiUtility.Instantiate("Player1", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                case Character.Character2:
                    m_Player = SingleMultiUtility.Instantiate("Player2", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                case Character.Character3:
                    m_Player = SingleMultiUtility.Instantiate("Player3", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                case Character.Character4:
                    m_Player = SingleMultiUtility.Instantiate("Player4", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
            }
            m_Player.GetComponent<PlayerObject>().SetTeamName(TeamName.TeamA);
        }
        else
        {
            switch (character)
            {
                case Character.Character1:
                    m_Player = SingleMultiUtility.Instantiate("Player1", enemyPosition, Quaternion.Euler(0.0f, -90.0f, 0.0f));
                    break;
                case Character.Character2:
                    m_Player = SingleMultiUtility.Instantiate("Player2", enemyPosition, Quaternion.Euler(0.0f, -90.0f, 0.0f));
                    break;
                case Character.Character3:
                    m_Player = SingleMultiUtility.Instantiate("Player3", enemyPosition, Quaternion.Euler(0.0f, -90.0f, 0.0f));
                    break;
                case Character.Character4:
                    m_Player = SingleMultiUtility.Instantiate("Player4", enemyPosition, Quaternion.Euler(0.0f, -90.0f, 0.0f));
                    break;
            }

            m_Player.GetComponent<PlayerObject>().SetTeamName(TeamName.TeamB);
        }
        //m_playersList.Add(m_Player.GetComponent<PlayerObject>());
        int index = GetPlayersUnusedNum();
        m_Players[index] = m_Player.GetComponent<PlayerObject>();


        yield return new WaitForSeconds(0.2f);
    }
    IEnumerator CreateSoloKeeper(Character character, Vector3 playerPosition, Vector3 enemyPosition)
    {
        if (m_GameManager.AssignPlayersToTeams() == TeamName.TeamA)
        {
            switch (character)
            {
                case Character.Character1:
                    m_Player = SingleMultiUtility.Instantiate("Keeper1", playerPosition, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    break;
                case Character.Character2:
                    m_Player = SingleMultiUtility.Instantiate("Keeper2", playerPosition, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    break;
                case Character.Character3:
                    m_Player = SingleMultiUtility.Instantiate("Keeper3", playerPosition, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    break;
                case Character.Character4:
                    m_Player = SingleMultiUtility.Instantiate("Keeper4", playerPosition, Quaternion.Euler(0.0f, 0.0f, 0.0f));
                    break;
            }
            m_Player.GetComponent<PlayerObject>().SetTeamName(TeamName.TeamA);
        }
        else
        {
            switch (character)
            {
                case Character.Character1:
                    m_Player = SingleMultiUtility.Instantiate("Keeper1", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
                case Character.Character2:
                    m_Player = SingleMultiUtility.Instantiate("Keeper2", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
                case Character.Character3:
                    m_Player = SingleMultiUtility.Instantiate("Keeper3", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
                case Character.Character4:
                    m_Player = SingleMultiUtility.Instantiate("Keeper4", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
            }
            m_Player.GetComponent<PlayerObject>().SetTeamName(TeamName.TeamB);
        }
        //m_playersList.Add(m_Player.GetComponent<PlayerObject>());
        int index = GetPlayersUnusedNum();
        m_Players[index] = m_Player.GetComponent<PlayerObject>();

        yield return new WaitForSeconds(0.2f);
    }




    public void CreatePlayerSolo()
    {
        if (m_itemInventory)
            m_itemInventory.SetTeam(this);

        m_MyCharacter = ScoreManager.Instance.m_SoloCharacter;
        Debug.Log("MyCharacter" +  m_MyCharacter);
        StartCoroutine(CreateSoloPlayer(m_MyCharacter,LeftPlayerPosition1, RightPlayerPosition1));
        StartCoroutine(CreateSoloPlayer(m_MyCharacter,LeftPlayerPosition2, RightPlayerPosition2));
        StartCoroutine(CreateSoloPlayer(m_MyCharacter,LeftPlayerPosition3, RightPlayerPosition3));
        StartCoroutine(CreateSoloPlayer(m_MyCharacter,LeftPlayerPosition4, RightPlayerPosition4));
        StartCoroutine(CreateSoloKeeper(m_MyCharacter,LeftKeeperPosition, RightKeeperPosition));
        /*
        for(int i = 0; i < m_playersList.Count; i++)
        {
            m_playersList[i].SetTeamObject(this);
        }
        */
        for (int i = 0; i < m_Players.Length; i++)
        {
            if (!m_Players[i])
            {
                continue;
            }
            m_Players[i].SetTeamObject(this);
        }

        m_GameManager.SetTeamUI();
    }

    public Character GetCharacter()
    {
        if(PhotonNetwork.IsConnected)
        {
            photonView.RPC("SetCharacterRPC", RpcTarget.All, (int)m_MyCharacter);
        }
        return m_MyCharacter;
    }

    public Character GetCharacter2()
    {
        return m_MyCharacter;
    }

    [PunRPC]
    public void SetCharacterRPC(int characterIndex)
    {
        m_MyCharacter = (Character)characterIndex;
        //m_YourCharacter = (Character)characterIndex;
    }

}

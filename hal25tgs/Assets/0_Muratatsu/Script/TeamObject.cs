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
    //�e�v���C���[�̎Q�Ƃ�ۗL
    GameObject m_Player;
    //List<PlayerObject> m_playersList;
    PlayerObject[] m_Players;

    [Header("�A�C�e���̃C���x���g���̃C���X�^���X")]
    [SerializeField] ItemInventory m_itemInventory;

    //�Q�[���}�l�[�W���[�̎Q�Ƃ�ۗL�i�Q�[���}�l�[�W���[������o�^�����j
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

        //��������������TeamManager�ȊO�͎��s���Ȃ�
        if(photonView.IsMine)
        {
            //m_playersList = new List<PlayerObject>();
            CreatePlayerMulti();
            /*
            if (m_itemInventory)
                m_itemInventory.SetTeam(this);

            //�����Ńv���C���[�̃C���X�^���X�𐶐����A
            //PlayerObject::SetTeamObject�����Ă����������H
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

            TurnManager.Instance.SetTeamObject(this);   // �^�[���}�l�[�W���[�Ƀ`�[���I�u�W�F�N�g�o�^
            */
        }



        //************************************************************
        //�ǋL�@
        //�����ɋL�q���ړ��@���ɏ����ƂȂ����Ă΂�Ȃ������̂�
        //************************************************************
        if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
        {
            Debug.Log("�A�C�e���C���x���g���i���j�̃L�����o�X���������݂܂���");

            m_itemInventory.SetInventoryPositionAndInit(InventoryPos.Left);
        }
        else
        {
            Debug.Log("�A�C�e���C���x���g���i�E�j�̃L�����o�X���������݂܂���");

            m_itemInventory.SetInventoryPositionAndInit(InventoryPos.Right);
        }




        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("yobareta");
            CreatePlayerSolo();
            TurnManager.Instance.SetTeamObject(this);   // �^�[���}�l�[�W���[�Ƀ`�[���I�u�W�F�N�g�o�^
        }

        //�����Player�Ƒ����TeamManager��R�Â�
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

        //�C���x���g���̍��E����i����ɓ��삵�Ȃ��j����́A�N���C�A���g��A�Ȃ�AA�����Ԃ��Ȃ��֐�������B
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
    /// �C���x���g���ɃA�C�e����ǉ�����@�i�v���C���[����Ă΂��j
    /// </summary>
    /// <param name="_item"></param>
    public void AddItemToInventory(ItemObject _item)
    {
        if (m_itemInventory)
        {
            Debug.Log("AddItem�֐����Ă΂�܂����B");
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

    //�Q�[���}�l�[�W���[����o�^
    public void SetGameManager(GameManager_S _manager)
    {
        m_GameManager = _manager;
    }

    //�Q�[���}�l�[�W���[����g�p
    public void UseItemByGameManager(int _itemIndex)
    {
        if (m_itemInventory)
            m_itemInventory.UseItemUnmaster(_itemIndex);
    }


    //�v���C���[����
    IEnumerator CreatePlayer(Character character, Vector3 playerPosition, Vector3 enemyPosition)
    {
        Debug.Log("Player����");
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
    //�L�[�p�[����
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

        //�����Ńv���C���[�̃C���X�^���X�𐶐����A
        //PlayerObject::SetTeamObject�����Ă����������H
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

        TurnManager.Instance.SetTeamObject(this);   // �^�[���}�l�[�W���[�Ƀ`�[���I�u�W�F�N�g�o�^

        m_GameManager.SetTeamUI();

    }

    //�z��̎g���ĂȂ��ꏊ�T��
    //�S�Ďg���Ă���-1
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
            Debug.LogError("m_PlayerList��null�ł��I");
        }

        if (_playerObject == null)
        {
            Debug.LogError("_playerObject��null�ł�");
        }

        for (int i = 0; i < m_Players.Length; i++)
        {
            if (!m_Players[i])
            {
                m_Players[i] = _playerObject;
            }
        }
    }


    //�v���C���[����
    IEnumerator CreateSoloPlayer(Character character,Vector3 playerPosition, Vector3 enemyPosition)
    {
        Debug.Log("Player����");
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

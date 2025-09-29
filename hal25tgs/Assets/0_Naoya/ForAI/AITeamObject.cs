using Photon.Pun.Demo.PunBasics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


/// <summary>
/// Enemy全員を保有して管理するクラス
/// </summary>


public class AITeamObject : MonoBehaviour
{
    //Enemyたち
    [SerializeField] List<EnemyAI> m_EnemyList = new List<EnemyAI>();
    private KeeperAI m_Keeper;

    private ItemObject[] m_Items;

    private EnemyAI m_ItemChaser;
    

    [Header("アイテムのインベントリのインスタンス")]
    [SerializeField] ItemInventory m_ItemInventory;

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

    [Header("UI")]
    [SerializeField] Image m_TeamAUI;
    [SerializeField] Image m_TeamBUI;
    [SerializeField] List<Sprite> m_Sprite;


    // Start is called before the first frame update
    void Start()
    {
        CreateEnemySolo();

        //Enemyを登録
        AssignEnemies();

        m_Keeper = GameObject.FindObjectOfType<KeeperAI>();

        if (m_ItemInventory)
        {
            m_ItemInventory.SetAITeam(this);
        }
           
        //インベントリの左右決定　（デバッグで見えるようにしているだけ。実際には表示しない）
        m_ItemInventory.SetInventoryPositionAndInit(InventoryPos.Right);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        //エネミーはここで考える
        if (m_ItemInventory)
        {
            m_ItemInventory.ConsiderUsingItemForEnemy();
        }
    }

    void AssignEnemies()
    {
        //全てのエネミーを探して登録する
        //FindGameObjectは重い処理なので、
        //m_EnemyListを[SerializeField]にしてヒエラルキーから登録してもよい。

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject gameObject in gameObjects)
        {
            if(gameObject.TryGetComponent<EnemyAI>(out var enemy))
            {
                enemy.SetTeam(this);
                m_EnemyList.Add(enemy);
            }
        }
    }

    public void AddItemToInventory(ItemObject _item)
    {
        if (m_ItemInventory)
        {
            Debug.Log("AddItem関数が呼ばれました。");
            m_ItemInventory.AddItem(_item);
        }
        else
        {
            Debug.LogWarning("エネミーのチームに、インベントリが登録されていません。");
        }
    }

    public List<EnemyAI> GetEnemyAIs() 
    { 
        return m_EnemyList;
    }

    public void PrepareAllEnemies()
    {
        foreach (var e in m_EnemyList)
        {
            e.PrepareMove(e.m_IsItemChaser ? e.m_TargeItem : null);
            Debug.Log("Prepare moving");
        }

        if(m_Keeper)
        {
            m_Keeper.PrepareMove();
        }
       
    }

    public void ExecuteAllEnemies()
    {
        foreach (var e in m_EnemyList)
        {
            e.Move();
        }

        if (m_Keeper)
        {
            m_Keeper.Move();
        }
        
    }

    public void FaceAllEnemies()
    {
        foreach (var e in m_EnemyList)
        {
            e.FacePlayer();
        } 
    }

    public void AssignItemChasers()
    {
        FindAllItems();

        foreach (var e in m_EnemyList)
        {
            e.m_IsItemChaser = true;
            e.m_TargeItem = null;
        }


        float bestDist = float.MaxValue;

        foreach(var item in m_Items)
        {
            if(item == null)
            {
                continue;
            }

            Vector3 ip = item.transform.position;

            foreach (var e in m_EnemyList)
            {
                float d = Vector3.Distance(e.transform.position, ip);
                if(d < bestDist)
                {
                    bestDist = d;
                    m_ItemChaser = e;
                }
            }
        }

        if(m_ItemChaser != null)
        {
            ItemObject bestItem = null;
            bestDist = float.MaxValue;

            foreach(var item in m_Items)
            {
                if (item == null)
                {
                    continue;
                }

                float d = Vector3.Distance(m_ItemChaser.transform.position, item.transform.position);

                if(d < bestDist)
                {
                    bestDist = d;
                    bestItem = item;
                }

                m_ItemChaser.m_IsItemChaser = true;
                m_ItemChaser.m_TargeItem = bestItem;
            }
        } 
    }

    private void FindAllItems()
    {
        m_Items = GameObject.FindObjectsOfType<ItemObject>();
    }

    IEnumerator CreateAIEnemy(Character character,Vector3 playerPosition, Vector3 enemyPosition)
    {
        if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
        {
            switch(character)
            {
                case Character.Character1:
                    SingleMultiUtility.Instantiate("AIENEMY1", enemyPosition, Quaternion.Euler(0.0f, -90.0f, 0.0f));
                    break;
                case Character.Character2:
                    SingleMultiUtility.Instantiate("AIENEMY2", enemyPosition, Quaternion.Euler(0.0f, -90.0f, 0.0f));
                    break;
                case Character.Character3:
                    SingleMultiUtility.Instantiate("AIENEMY3", enemyPosition, Quaternion.Euler(0.0f, -90.0f, 0.0f));
                    break;
                case Character.Character4:
                    SingleMultiUtility.Instantiate("AIENEMY4", enemyPosition, Quaternion.Euler(0.0f, -90.0f, 0.0f));
                    break;
            }
        }
        else if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamB)
        {
            switch (character)
            {
                case Character.Character1:
                    SingleMultiUtility.Instantiate("AIENEMY1", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                case Character.Character2:
                    SingleMultiUtility.Instantiate("AIENEMY2", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                case Character.Character3:
                    SingleMultiUtility.Instantiate("AIENEMY3", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
                case Character.Character4:
                    SingleMultiUtility.Instantiate("AIENEMY4", playerPosition, Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;
            }
        }

        yield return new WaitForSeconds(0.2f);
    }
    IEnumerator CreateAIKeeper(Character character, Vector3 playerPosition, Vector3 enemyPosition)
    {
        if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
        {
            switch (character)
            {
                case Character.Character1:
                    SingleMultiUtility.Instantiate("AIKEEPER1", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
                case Character.Character2:
                    SingleMultiUtility.Instantiate("AIKEEPER2", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
                case Character.Character3:
                    SingleMultiUtility.Instantiate("AIKEEPER3", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
                case Character.Character4:
                    SingleMultiUtility.Instantiate("AIKEEPER4", enemyPosition, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                    break;
            }
        }
        else if (ScoreManager.Instance.m_SoloTeamName == TeamName.TeamB)
        {
            switch (character)
            {
                case Character.Character1:
                    SingleMultiUtility.Instantiate("AIKEEPER1", playerPosition, Quaternion.identity);
                    break;
                case Character.Character2:
                    SingleMultiUtility.Instantiate("AIKEEPER2", playerPosition, Quaternion.identity);
                    break;
                case Character.Character3:
                    SingleMultiUtility.Instantiate("AIKEEPER3", playerPosition, Quaternion.identity);
                    break;
                case Character.Character4:
                    SingleMultiUtility.Instantiate("AIKEEPER4", playerPosition, Quaternion.identity);
                    break;
            }
        }

        yield return new WaitForSeconds(0.2f);
    }

    public void CreateEnemySolo()
    {
        Character character = ScoreManager.Instance.m_SoloCharacter;
        Character aiCharacter;
        do
        {
            int max = Enum.GetNames(typeof(Character)).Length;
            int num = UnityEngine.Random.Range(0, max);
            aiCharacter = (Character)Enum.ToObject(typeof(Character), num);
        } while (character == aiCharacter);
        ScoreManager.Instance.m_AICharacter = aiCharacter;
        StartCoroutine(CreateAIEnemy(aiCharacter,LeftPlayerPosition1, RightPlayerPosition1));
        StartCoroutine(CreateAIEnemy(aiCharacter,LeftPlayerPosition2, RightPlayerPosition2));
        StartCoroutine(CreateAIEnemy(aiCharacter,LeftPlayerPosition3, RightPlayerPosition3));
        StartCoroutine(CreateAIEnemy(aiCharacter,LeftPlayerPosition4, RightPlayerPosition4));
        StartCoroutine(CreateAIKeeper(aiCharacter,LeftKeeperPosition, RightKeeperPosition));

        if(ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
        {
            int num = (int)aiCharacter;
            m_TeamBUI.sprite = m_Sprite[num];
        }
        else
        {
            int num = (int)aiCharacter;
            m_TeamAUI.sprite = m_Sprite[num];
        }

    }

}

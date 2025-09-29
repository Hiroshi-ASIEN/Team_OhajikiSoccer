using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Keeper_ChangePlayer : MonoBehaviour
{
    // 切り替えたい対象のGameObject設定
    public GameObject targetObject; 
    public GameObject targetObject2;
    //private bool NowScene = false;

    //public string targetSceneName = "Doublegoal"; 

    // Update is called once per frame
    void Update()
    {

        //if (!NowScene && SceneManager.GetActiveScene().name == "Doblegoal")
        //{
        //    NowScene = true;

        //    Keeper keeper = targetObject.GetComponent<Keeper>();
        //    Move move = targetObject2.GetComponent<Move>();

        //    if (keeper != null)
        //    {
        //        // オンオフ切り替え
        //        keeper.enabled = !keeper.enabled;
        //        Debug.Log("キーパースクリプトの状態 " + (keeper.enabled ? "enabled" : "disabled"));
        //    }

        //    if (move != null)
        //    {
        //        // オンオフ切り替え
        //        move.enabled = !move.enabled;
        //        Debug.Log("ムーブスクリプトの状態 " + (move.enabled ? "enabled" : "disabled"));
        //    }
        //}

        // キーで変更
        if (Input.GetKeyDown(KeyCode.C))
        {
            Keeper keeper = targetObject.GetComponent<Keeper>();
            Move move = targetObject2.GetComponent<Move>();

            if (keeper != null)
            {
                // オンオフ切り替え
                keeper.enabled = !keeper.enabled;
                Debug.Log("キーパースクリプトの状態 " + (keeper.enabled ? "enabled" : "disabled"));
            }

            if (move != null)
            {
                // オンオフ切り替え
                move.enabled = !move.enabled;
                Debug.Log("ムーブスクリプトの状態 " + (move.enabled ? "enabled" : "disabled"));
            }

        }
    }
}

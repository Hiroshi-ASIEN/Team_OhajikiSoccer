using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Keeper_ChangePlayer : MonoBehaviour
{
    // �؂�ւ������Ώۂ�GameObject�ݒ�
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
        //        // �I���I�t�؂�ւ�
        //        keeper.enabled = !keeper.enabled;
        //        Debug.Log("�L�[�p�[�X�N���v�g�̏�� " + (keeper.enabled ? "enabled" : "disabled"));
        //    }

        //    if (move != null)
        //    {
        //        // �I���I�t�؂�ւ�
        //        move.enabled = !move.enabled;
        //        Debug.Log("���[�u�X�N���v�g�̏�� " + (move.enabled ? "enabled" : "disabled"));
        //    }
        //}

        // �L�[�ŕύX
        if (Input.GetKeyDown(KeyCode.C))
        {
            Keeper keeper = targetObject.GetComponent<Keeper>();
            Move move = targetObject2.GetComponent<Move>();

            if (keeper != null)
            {
                // �I���I�t�؂�ւ�
                keeper.enabled = !keeper.enabled;
                Debug.Log("�L�[�p�[�X�N���v�g�̏�� " + (keeper.enabled ? "enabled" : "disabled"));
            }

            if (move != null)
            {
                // �I���I�t�؂�ւ�
                move.enabled = !move.enabled;
                Debug.Log("���[�u�X�N���v�g�̏�� " + (move.enabled ? "enabled" : "disabled"));
            }

        }
    }
}

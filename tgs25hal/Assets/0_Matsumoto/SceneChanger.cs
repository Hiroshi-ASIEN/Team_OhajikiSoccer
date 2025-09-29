using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Timer m_SceneChangeTimer;

    [SerializeField] private string m_NextSceneName;    // 遷移先のシーン名

    private bool m_Active = false;

    // Update is called once per frame
    void Update()
    {
        SceneChangeTimer();
    }

    public void IsActive()
    {
        m_Active = true;
        if (m_SceneChangeTimer != null)
        {
            m_SceneChangeTimer.TimerStart();    // 遷移までのタイマー開始
        }
    }

    private void SceneChangeTimer()
    {
        if (!m_Active) return;

        if (m_SceneChangeTimer != null)
        {
            if (!m_SceneChangeTimer.TimerEnd()) return; // タイマー終了していなければ戻る
        }

        NextSceneChange();
    }
    private void NextSceneChange()
    {
        m_Active = false;
        PhotonNetwork.LoadLevel(m_NextSceneName);
//        SceneManager.LoadScene(m_NextSceneName);
    }

}

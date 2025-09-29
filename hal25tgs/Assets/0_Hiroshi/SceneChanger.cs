using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Timer m_SceneChangeTimer;
    [SerializeField] private FadeInOut m_Fade;

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
        m_Fade.gameObject.SetActive(true);
//        m_Fade.SetFadeMode(FadeInOut.FadeMode.Out);
        m_Fade.FadeStart();
    }

    private void SceneChangeTimer()
    {
        if (!m_Active) return;

        if (m_SceneChangeTimer != null)
        {
            if (!m_SceneChangeTimer.TimerEnd()) return; // タイマー終了していなければ戻る
        }

        if (!m_Fade.IsFadeEnd()) return;

        NextSceneChange();
    }
    private void NextSceneChange()
    {
        m_Active = false;
        PhotonNetwork.LoadLevel(m_NextSceneName);
//        SceneManager.LoadScene(m_NextSceneName);
    }

}

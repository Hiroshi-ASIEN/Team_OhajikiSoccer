using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// チームの名前
public enum TeamName
{
    None = -1,
    TeamA,
    TeamB
}

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager Instance { get; private set; }  // シングルトン化

    [SerializeField] private int m_AddScorePoint = 1;   // 追加されるスコア
    [SerializeField] private int m_CompleteScore = 100;

    [SerializeField] private string m_NextSceneName;    // 遷移先のシーン名

    // チームごとのスコア管理用Dictionary
    private Dictionary<TeamName, int> m_Scores = new Dictionary<TeamName, int>();

    public event Action OnScoreChanged; // スコア変更時に発生するイベント

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;

            // シーン遷移しても破壊されないようにする。
            DontDestroyOnLoad(this.gameObject);

            // スコアの初期化
            m_Scores[TeamName.TeamA] = 0;
            m_Scores[TeamName.TeamB] = 0;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
    }

    private void CheckGameEnd()
    {
        // すべてのチームのスコアを確認
        foreach (var score in m_Scores.Values)
        {
            if (score >= m_CompleteScore)
            {
                SceneManager.LoadScene(m_NextSceneName);
                return;
            }
        }
    }
    public void AddScore(TeamName _team)
    {
        if (m_Scores.ContainsKey(_team))
        {
            m_Scores[_team] += m_AddScorePoint;
            CheckGameEnd();
            OnScoreChanged?.Invoke(); // イベントにて、スコアが変わったことを通知
        }
    }
    // 指定したチームのスコアを取得
    public int GetScore(TeamName team)
    {
        return m_Scores.ContainsKey(team) ? m_Scores[team] : 0;
    }

    // スコアをリセット（試合開始時などに使用）
    public void ResetScores()
    {
        m_Scores[TeamName.TeamA] = 0;
        m_Scores[TeamName.TeamB] = 0;
        OnScoreChanged?.Invoke(); // イベントにて、スコアが変わったことを通知
    }
}
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

public enum Character
{
    Character1,
    Character2,
    Character3,
    Character4,
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

    public Character m_CharacterA;//マルチ用
    public Character m_CharacterB;

    public TeamName m_SoloTeamName; //ソロ用
    public Character m_SoloCharacter;
    public Character m_AICharacter;

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


    private void CheckGameEnd()
    {
        // すべてのチームのスコアを確認
        foreach (var score in m_Scores.Values)
        {
            if (score >= m_CompleteScore)
            {
                SceneManager.LoadScene(m_NextSceneName);
                Debug.Log("ゲームエンド");
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

    // 加算される得点を設定
    public void SetAddScorePoint(int _setPoint)
    {
        m_AddScorePoint = _setPoint;
    }

    // 現在の加算される得点を取得
    public int GetAddScorePoint()
    {
        return m_AddScorePoint;
    }

    public void SetSoloTeamNeme(TeamName name)
    {
        m_SoloTeamName = name;
    }

    public void SetSoloCharacter(Character character)
    {
        m_SoloCharacter = character;
    }

    public void SetMultiCharacter(Character character)
    {
        m_CharacterA = character;
        Debug.Log("CharacterA" + m_CharacterA);
    }

    public int GetMaxScore()
    {
        return m_CompleteScore;
    }
}
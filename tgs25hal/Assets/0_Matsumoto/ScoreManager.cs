using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// �`�[���̖��O
public enum TeamName
{
    None = -1,
    TeamA,
    TeamB
}

public class ScoreManager : MonoBehaviour
{
    static public ScoreManager Instance { get; private set; }  // �V���O���g����

    [SerializeField] private int m_AddScorePoint = 1;   // �ǉ������X�R�A
    [SerializeField] private int m_CompleteScore = 100;

    [SerializeField] private string m_NextSceneName;    // �J�ڐ�̃V�[����

    // �`�[�����Ƃ̃X�R�A�Ǘ��pDictionary
    private Dictionary<TeamName, int> m_Scores = new Dictionary<TeamName, int>();

    public event Action OnScoreChanged; // �X�R�A�ύX���ɔ�������C�x���g

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;

            // �V�[���J�ڂ��Ă��j�󂳂�Ȃ��悤�ɂ���B
            DontDestroyOnLoad(this.gameObject);

            // �X�R�A�̏�����
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
        // ���ׂẴ`�[���̃X�R�A���m�F
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
            OnScoreChanged?.Invoke(); // �C�x���g�ɂāA�X�R�A���ς�������Ƃ�ʒm
        }
    }
    // �w�肵���`�[���̃X�R�A���擾
    public int GetScore(TeamName team)
    {
        return m_Scores.ContainsKey(team) ? m_Scores[team] : 0;
    }

    // �X�R�A�����Z�b�g�i�����J�n���ȂǂɎg�p�j
    public void ResetScores()
    {
        m_Scores[TeamName.TeamA] = 0;
        m_Scores[TeamName.TeamB] = 0;
        OnScoreChanged?.Invoke(); // �C�x���g�ɂāA�X�R�A���ς�������Ƃ�ʒm
    }
}
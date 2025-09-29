using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TeamAScoreText; // �`�[��A�̃X�R�A�\���p
    [SerializeField] private TextMeshProUGUI m_TeamBScoreText; // �`�[��B�̃X�R�A�\���p

    private void Start()
    {
        // �X�R�A���ς�邽�тɍX�V����
        ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
        UpdateScoreText(); // �����\����ݒ�
    }

    private void UpdateScoreText()
    {
        // ScoreManager ����X�R�A���擾���ĕ\��
        m_TeamAScoreText.text = $"{ScoreManager.Instance.GetScore(TeamName.TeamA)}";
        m_TeamBScoreText.text = $"{ScoreManager.Instance.GetScore(TeamName.TeamB)}";
    }

    private void OnDestroy()
    {
        // �C�x���g���������ă��������[�N��h��
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreText;
        }
    }
}

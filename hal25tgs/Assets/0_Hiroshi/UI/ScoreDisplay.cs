using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Unity.VisualScripting;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TeamAScoreText; // チームAのスコア表示用
    [SerializeField] private TextMeshProUGUI m_TeamBScoreText; // チームBのスコア表示用

    private void Start()
    {
        // スコアが変わるたびに更新する
        ScoreManager.Instance.OnScoreChanged += UpdateScoreText;
        UpdateScoreText(); // 初期表示を設定
    }

    private void UpdateScoreText()
    {
        Debug.Log(ScoreManager.Instance.gameObject.name.ToString());
        Debug.Log(ScoreManager.Instance.GetScore(TeamName.TeamA).ToString());
        if (m_TeamAScoreText == null || m_TeamBScoreText == null)
        {
            Debug.LogError("ScoreDisplay: Textオブジェクトが設定されていません。");
            return;
        }        // ScoreManager からスコアを取得して表示
        m_TeamAScoreText.text = $"{ScoreManager.Instance.GetScore(TeamName.TeamA)}";
        m_TeamBScoreText.text = $"{ScoreManager.Instance.GetScore(TeamName.TeamB)}";
    }

    private void OnDestroy()
    {
        // イベントを解除してメモリリークを防ぐ
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnScoreChanged -= UpdateScoreText;
        }
    }
}

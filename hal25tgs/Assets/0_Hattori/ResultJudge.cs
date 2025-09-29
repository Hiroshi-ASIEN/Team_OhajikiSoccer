using UnityEngine;
using UnityEngine.UI; // Imageコンポーネントを使用するために必要
using TMPro;
using Unity.VisualScripting; // TextMeshProを使用するために必要

public class ResultManager : MonoBehaviour
{
    // 勝敗結果を表示するUIのImageコンポーネント
    [SerializeField] private Image m_ResultImageA;
    [SerializeField] private Image m_ResultImageB;
    [SerializeField] private Image m_ResultImageDraw;

    // 各勝敗結果に対応するSprite
    [SerializeField] private Sprite m_WinSprite;
    [SerializeField] private Sprite m_LoseSprite;
    [SerializeField] private Sprite m_DrawSprite;

    private TeamName m_WinTeam;
    void Start()
    {
        // ScoreManagerのインスタンスが存在しない場合のエラー処理
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManagerのインスタンスが見つかりません。");
            if (m_ResultImageA != null)
            {
                // エラー時の代替画像を設定するか、非表示にするなどの処理
                m_ResultImageA.gameObject.SetActive(false);
            }
            return;
        }

        // ScoreManagerから各チームのスコアを取得
        int scoreA = ScoreManager.Instance.GetScore(TeamName.TeamA);
        int scoreB = ScoreManager.Instance.GetScore(TeamName.TeamB);

        // 勝敗を判定し、表示するSpriteを設定
        if (scoreA > scoreB)
        {
            m_WinTeam = TeamName.TeamA;

            m_ResultImageA.sprite = m_WinSprite;
            m_ResultImageB.sprite = m_LoseSprite;
            m_ResultImageDraw.gameObject.SetActive(false);
        }
        else if (scoreB > scoreA)
        {
            m_WinTeam = TeamName.TeamB;

            m_ResultImageA.sprite = m_LoseSprite;
            m_ResultImageB.sprite = m_WinSprite;
            m_ResultImageDraw.gameObject.SetActive(false);
        }
        else
        {
            m_WinTeam = TeamName.None;

            m_ResultImageDraw.sprite = m_DrawSprite;
            m_ResultImageA.gameObject.SetActive(false);
            m_ResultImageB.gameObject.SetActive(false);
            m_ResultImageDraw.gameObject.SetActive(true);
        }

        // 勝利敗北チーム画像設定
        this.GetComponent<ResultCharacter>().SetCharacterImages(m_WinTeam);
    }

    public TeamName GetWinTeam()
    {
        return m_WinTeam;
    }
}
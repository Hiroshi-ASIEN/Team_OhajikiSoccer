using UnityEngine;
using UnityEngine.UI; // Image�R���|�[�l���g���g�p���邽�߂ɕK�v
using TMPro;
using Unity.VisualScripting; // TextMeshPro���g�p���邽�߂ɕK�v

public class ResultManager : MonoBehaviour
{
    // ���s���ʂ�\������UI��Image�R���|�[�l���g
    [SerializeField] private Image m_ResultImageA;
    [SerializeField] private Image m_ResultImageB;
    [SerializeField] private Image m_ResultImageDraw;

    // �e���s���ʂɑΉ�����Sprite
    [SerializeField] private Sprite m_WinSprite;
    [SerializeField] private Sprite m_LoseSprite;
    [SerializeField] private Sprite m_DrawSprite;

    private TeamName m_WinTeam;
    void Start()
    {
        // ScoreManager�̃C���X�^���X�����݂��Ȃ��ꍇ�̃G���[����
        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager�̃C���X�^���X��������܂���B");
            if (m_ResultImageA != null)
            {
                // �G���[���̑�։摜��ݒ肷�邩�A��\���ɂ���Ȃǂ̏���
                m_ResultImageA.gameObject.SetActive(false);
            }
            return;
        }

        // ScoreManager����e�`�[���̃X�R�A���擾
        int scoreA = ScoreManager.Instance.GetScore(TeamName.TeamA);
        int scoreB = ScoreManager.Instance.GetScore(TeamName.TeamB);

        // ���s�𔻒肵�A�\������Sprite��ݒ�
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

        // �����s�k�`�[���摜�ݒ�
        this.GetComponent<ResultCharacter>().SetCharacterImages(m_WinTeam);
    }

    public TeamName GetWinTeam()
    {
        return m_WinTeam;
    }
}
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCharacter : MonoBehaviour
{
    [SerializeField] private Image m_TeamAImage;
    [SerializeField] private Image m_TeamBImage;

    [SerializeField] private Sprite[] m_WinSprite;
    [SerializeField] private Sprite[] m_LoseSprite;

    // èüóòîsñkÉ`Å[ÉÄâÊëúê›íË
    public void SetCharacterImages(TeamName _winTeam)
    {
        if (PhotonNetwork.IsConnected)
        {
            switch (_winTeam)
            {
                case TeamName.TeamA:
                    m_TeamAImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_CharacterA];
                    m_TeamBImage.sprite = m_LoseSprite[(int)ScoreManager.Instance.m_CharacterB];
                    break;

                case TeamName.TeamB:
                    m_TeamAImage.sprite = m_LoseSprite[(int)ScoreManager.Instance.m_CharacterA];
                    m_TeamBImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_CharacterB];
                    break;

                case TeamName.None:
                    m_TeamAImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_CharacterA];
                    m_TeamBImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_CharacterB];
                    break;
            }
        }
        else
        {
            if(ScoreManager.Instance.m_SoloTeamName == TeamName.TeamA)
            {
                switch (_winTeam)
                {
                    case TeamName.TeamA:
                        m_TeamAImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_SoloCharacter];
                        m_TeamBImage.sprite = m_LoseSprite[(int)ScoreManager.Instance.m_AICharacter];
                        break;

                    case TeamName.TeamB:
                        m_TeamAImage.sprite = m_LoseSprite[(int)ScoreManager.Instance.m_SoloCharacter];
                        m_TeamBImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_AICharacter];
                        break;

                    case TeamName.None:
                        m_TeamAImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_SoloCharacter];
                        m_TeamBImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_AICharacter];
                        break;
                }
            }
            else
            {
                switch (_winTeam)
                {
                    case TeamName.TeamA:
                        m_TeamAImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_AICharacter];
                        m_TeamBImage.sprite = m_LoseSprite[(int)ScoreManager.Instance.m_SoloCharacter];
                        break;

                    case TeamName.TeamB:
                        m_TeamAImage.sprite = m_LoseSprite[(int)ScoreManager.Instance.m_AICharacter];
                        m_TeamBImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_SoloCharacter];
                        break;

                    case TeamName.None:
                        m_TeamAImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_AICharacter];
                        m_TeamBImage.sprite = m_WinSprite[(int)ScoreManager.Instance.m_SoloCharacter];
                        break;
                }

            }
        }

    }
}



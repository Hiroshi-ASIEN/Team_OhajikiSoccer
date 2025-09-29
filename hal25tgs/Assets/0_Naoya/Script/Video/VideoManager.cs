using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;




public class VideoManager : MonoBehaviour
{
    [Header("�r�f�I�̃C���X�^���X")]
    [SerializeField] VideoPlayer m_videoPlayer;
    
    [Header("�X�L�b�v�{�^���̃C���X�^���X")]
    [SerializeField] Button m_skipButton;

    [Header("�Đ�����܂ł̑ҋ@�b��")]
    [SerializeField] float m_delaySecond = 1.0f;

    bool m_hasEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        //�Ȃɂ��̋L�q���@�@�C�x���g���H
        this.m_skipButton.onClick.AddListener(() =>
        {
            //�ꎞ��~����
            this.m_videoPlayer.Stop();
            m_hasEnded = true;
            Debug.Log("������I�����܂���");
        });



        Invoke(nameof(PlayVideo), m_delaySecond);
    }


    // Update is called once per frame
    void Update()
    {
        //���Ԍo�߂𒲂ׂ�




        //�I�����������ׂ�
        if(m_videoPlayer.time >= m_videoPlayer.length)
        {
            this.m_hasEnded = true;
        }
    }

    void PlayVideo()
    {
        this.m_videoPlayer.Play();

        Debug.Log("����Đ������݂܂���");
    }

    /// <summary>
    /// �I��������ǂ����𒲂ׂ�
    /// </summary>
    public bool GetHasEnded()
    {
        return m_hasEnded;
    }

}

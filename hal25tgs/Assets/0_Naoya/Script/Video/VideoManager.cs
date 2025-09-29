using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;




public class VideoManager : MonoBehaviour
{
    [Header("ビデオのインスタンス")]
    [SerializeField] VideoPlayer m_videoPlayer;
    
    [Header("スキップボタンのインスタンス")]
    [SerializeField] Button m_skipButton;

    [Header("再生するまでの待機秒数")]
    [SerializeField] float m_delaySecond = 1.0f;

    bool m_hasEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        //なにこの記述方法　イベントか？
        this.m_skipButton.onClick.AddListener(() =>
        {
            //一時停止して
            this.m_videoPlayer.Stop();
            m_hasEnded = true;
            Debug.Log("動画を終了しました");
        });



        Invoke(nameof(PlayVideo), m_delaySecond);
    }


    // Update is called once per frame
    void Update()
    {
        //時間経過を調べる




        //終了したか調べる
        if(m_videoPlayer.time >= m_videoPlayer.length)
        {
            this.m_hasEnded = true;
        }
    }

    void PlayVideo()
    {
        this.m_videoPlayer.Play();

        Debug.Log("動画再生を試みました");
    }

    /// <summary>
    /// 終わったかどうかを調べる
    /// </summary>
    public bool GetHasEnded()
    {
        return m_hasEnded;
    }

}

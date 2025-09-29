using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using UnityEngine.Video;

public class TurnMovie : MonoBehaviour
{
    [SerializeField] private RawImage m_Video;
    [SerializeField] private VideoPlayer m_VideoPlayer;
    [SerializeField] private GameObject m_Camera;
    [SerializeField] private int3 m_Scale;
    [SerializeField] private VideoClip m_VideoClip;

    // Start is called before the first frame update
    void Start()
    {
        // この下編集する
        RenderTexture renderTexture;
        renderTexture = new RenderTexture(m_Scale.x, m_Scale.y, m_Scale.z);

        m_Video.material.mainTexture = renderTexture;

        if (m_VideoPlayer == null) m_VideoPlayer = m_Camera.AddComponent<VideoPlayer>();

        m_VideoPlayer.renderMode = VideoRenderMode.RenderTexture;
        m_VideoPlayer.targetTexture = renderTexture;
        m_VideoPlayer.errorReceived += ErrorReceived;
        m_VideoPlayer.prepareCompleted += PrepareCompleted;
        m_VideoPlayer.Prepare();
        m_VideoPlayer.isLooping = false;
        m_VideoPlayer.aspectRatio = VideoAspectRatio.Stretch;
        m_VideoPlayer.targetCameraAlpha = 1;
        m_VideoPlayer.loopPointReached += EndReached;

        m_VideoPlayer.source = VideoSource.VideoClip; // 動画ソースの設定
        m_VideoPlayer.clip = m_VideoClip;

        m_VideoPlayer.isLooping = true;   // ループの設定
    }
    public void Play()
    {
        if (m_VideoPlayer.isPlaying) return;

            m_VideoPlayer.Play(); // 動画を再生する。
    }

    public void VPControl()
    {
        var videoPlayer = GetComponent<VideoPlayer>();

        if (!videoPlayer.isPlaying) // ボタンを押した時の処理
            videoPlayer.Play(); // 動画を再生する。
        else
            videoPlayer.Pause();	// 動画を一時停止する。
    }

    // エラー発生時に呼ばれる
    private void ErrorReceived(VideoPlayer vp, string message)
    {
        Debug.Log("エラー発生");
        vp.errorReceived -= ErrorReceived;
        vp.prepareCompleted -= PrepareCompleted;
        Destroy(m_VideoPlayer);
        vp = null;
    }

    //　動画の読み込みが完了したら呼ばれる
    void PrepareCompleted(VideoPlayer vp)
    {
        vp.prepareCompleted -= PrepareCompleted;
        Debug.Log("ロード完了");
        vp.Play();
    }

    // 動画再生完了時に呼ばれる
    private void OnDisable()
    {
        
    }
    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        Debug.Log("再生完了");
        vp.errorReceived -= ErrorReceived;
        Destroy(m_VideoPlayer);
        m_VideoPlayer = null;
        // 動画再生完了時の処理
    }
}

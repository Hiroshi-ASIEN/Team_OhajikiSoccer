using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


//���p����
//https://qiita.com/broken55/items/f235497c7b4b2a46b926

//���̃X�N���v�g�͎g�p���Ȃ��@�����܂ŎQ�l�̂���


public class VideoControl : MonoBehaviour
{
    //[SerializeField] VideoPlayer videoPlayer;

    //[SerializeField] Button playButton;
    //[SerializeField] Button stopButton;
    //[SerializeField] Button pauseButton;

    ////[SerializeField] Slider slider;
    ////[SerializeField] Text timeText;

    //void Start()
    //{
    //    //�Ȃɂ��̋L�q���@�@�C�x���g���H
    //    this.playButton.onClick.AddListener(() =>
    //    {
    //        //�Đ�����
    //        //�ꎞ��~���̏ꍇ�͍ĊJ����
    //        this.videoPlayer.Play();
    //    });

    //    this.stopButton.onClick.AddListener(() =>
    //    {
    //        //��~����
    //        this.videoPlayer.Stop();
    //    });

    //    this.pauseButton.onClick.AddListener(() =>
    //    {
    //        //�ꎞ��~����
    //        this.videoPlayer.Pause();
    //    });

    //    //this.slider.onValueChanged.AddListener(value =>
    //    //{
    //    //    //�X���C�_�[�̏ꏊ�ɍ��킹�čĐ�����ꏊ��ς���
    //    //    this.videoPlayer.time = this.videoPlayer.length * value;
    //    //});
    //}

    //void Update()
    //{
    //    //�X���C�_�[���Đ����Ԃɍ��킹�Đi�s������
    //    //SetValueWithoutNotify���g����onValueChanged�̃R�[���o�b�N���Ă΂�Ȃ�
    //    //��UI�ő��삵���C�x���g�������擾���邱�Ƃ��ł���
    //    //this.slider.SetValueWithoutNotify((float)(this.videoPlayer.time / this.videoPlayer.length));

    //    //���ݍĐ����Ԃ�\��
    //    //this.timeText.text = $"{(int)this.videoPlayer.time / 60:D2}:{(int)this.videoPlayer.time % 60:D2}";
    //}
}
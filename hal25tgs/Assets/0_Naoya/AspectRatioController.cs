using UnityEditor.Rendering;
using UnityEngine;

//�Q�l�T�C�g
//https://qiita.com/AzureBlue/items/f88342bbba3f5d67d230

[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // �ڕW�̃A�X�y�N�g��

    [SerializeField] private Vector2 aspectVec_ = new Vector2(1920, 1080);  // �Q�Ɖ𑜓x
    [SerializeField] private float pixelPerUnit_ = 100; // �摜��PixelPerUnit
    private float currentAspect_ = 0.0f;          // ���݂̱��߸Ĕ�

    void Start()
    {
        UpdateViewport();
    }

    void Update()
    {
        UpdateViewport();
    }

    void UpdateViewport()
    {
        Camera cam = Camera.main;

        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // ���^�[�{�b�N�X�i�㉺�ɍ��сj
            Rect rect = new Rect(0.0f, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
            cam.rect = rect;
        }
        else
        {
            // �s���[�{�b�N�X�i���E�ɍ��сj
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = new Rect((1.0f - scaleWidth) / 2.0f, 0.0f, scaleWidth, 1.0f);
            cam.rect = rect;
        }
    }


    // ��ׂ̕\���̈�̒���
    private void SetCameraViewArea()
    {
        Camera cam = Camera.main;


        // ��׻��ނ̒���
        cam.orthographicSize = aspectVec_.y / pixelPerUnit_ / 2;

        // �ޭ��߰Ă̒���
        float baseAspect = aspectVec_.y / aspectVec_.x;     // ��̱��߸Ĕ�

        if (baseAspect <= currentAspect_)
        {
            // ��ʂ��c�ɒ����ꍇ
            float bgScale = aspectVec_.x / Screen.width;

            // viewportRect�̏c��
            float tmpHeight = aspectVec_.y / (Screen.height * bgScale);

            // viewportRect��ݒ�
            cam.rect =
             new Rect(0.0f, (1.0f - tmpHeight) / 2, 1.0f, tmpHeight);
        }
        else
        {
            // ��ʂ����ɒ����ꍇ
            float bgScale = aspectVec_.y / Screen.height;

            // viewportRect�̉���
            float tmpWidth = aspectVec_.x / (Screen.width * bgScale);

            // viewportRect��ݒ�
            cam.rect =
             new Rect((1.0f - tmpWidth) / 2, 0.0f, tmpWidth, 1.0f);
        }
    }

    // ViewportRect�̒���
    private void SetViewportRect()
    {
        Camera cam = Camera.main;

        float baseAspect = aspectVec_.y / aspectVec_.x;     // ��̱��߸Ĕ�

        if (baseAspect > currentAspect_)
        {
            // ��ʂ����ɍL���ꍇ�A��̱��߸Ĕ�ɍ��킹�Ē�������
            float bgScale = aspectVec_.y / Screen.height;

            // viewportRect�̕�
            float tmpWidth = aspectVec_.x / (Screen.width * bgScale);

            // viewportRect��ݒ�
            cam.rect = new Rect(
             (1.0f - tmpWidth) / 2, 0.0f, tmpWidth, 1.0f);
        }
        else
        {
            // ��ʂ��c�ɒ����ꍇ�A��ʂ̒����ɱ��߸Ĕ��Ǐ]������
            float bgScale = currentAspect_ / baseAspect;

            // ��ׂ̻��ނ��c�̒����ɍ��킹�Đݒ肵�Ȃ���
            cam.orthographicSize *= bgScale;

            // viewportRect��ݒ�(�͈͓��S�Ă�\��)
            cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        }
    }

}

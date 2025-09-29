using UnityEditor.Rendering;
using UnityEngine;

//参考サイト
//https://qiita.com/AzureBlue/items/f88342bbba3f5d67d230

[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    public float targetAspect = 16f / 9f; // 目標のアスペクト比

    [SerializeField] private Vector2 aspectVec_ = new Vector2(1920, 1080);  // 参照解像度
    [SerializeField] private float pixelPerUnit_ = 100; // 画像のPixelPerUnit
    private float currentAspect_ = 0.0f;          // 現在のｱｽﾍﾟｸﾄ比

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
            // レターボックス（上下に黒帯）
            Rect rect = new Rect(0.0f, (1.0f - scaleHeight) / 2.0f, 1.0f, scaleHeight);
            cam.rect = rect;
        }
        else
        {
            // ピラーボックス（左右に黒帯）
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = new Rect((1.0f - scaleWidth) / 2.0f, 0.0f, scaleWidth, 1.0f);
            cam.rect = rect;
        }
    }


    // ｶﾒﾗの表示領域の調整
    private void SetCameraViewArea()
    {
        Camera cam = Camera.main;


        // ｶﾒﾗｻｲｽﾞの調整
        cam.orthographicSize = aspectVec_.y / pixelPerUnit_ / 2;

        // ﾋﾞｭｰﾎﾟｰﾄの調整
        float baseAspect = aspectVec_.y / aspectVec_.x;     // 基準のｱｽﾍﾟｸﾄ比

        if (baseAspect <= currentAspect_)
        {
            // 画面が縦に長い場合
            float bgScale = aspectVec_.x / Screen.width;

            // viewportRectの縦幅
            float tmpHeight = aspectVec_.y / (Screen.height * bgScale);

            // viewportRectを設定
            cam.rect =
             new Rect(0.0f, (1.0f - tmpHeight) / 2, 1.0f, tmpHeight);
        }
        else
        {
            // 画面が横に長い場合
            float bgScale = aspectVec_.y / Screen.height;

            // viewportRectの横幅
            float tmpWidth = aspectVec_.x / (Screen.width * bgScale);

            // viewportRectを設定
            cam.rect =
             new Rect((1.0f - tmpWidth) / 2, 0.0f, tmpWidth, 1.0f);
        }
    }

    // ViewportRectの調整
    private void SetViewportRect()
    {
        Camera cam = Camera.main;

        float baseAspect = aspectVec_.y / aspectVec_.x;     // 基準のｱｽﾍﾟｸﾄ比

        if (baseAspect > currentAspect_)
        {
            // 画面が横に広い場合、基準のｱｽﾍﾟｸﾄ比に合わせて調整する
            float bgScale = aspectVec_.y / Screen.height;

            // viewportRectの幅
            float tmpWidth = aspectVec_.x / (Screen.width * bgScale);

            // viewportRectを設定
            cam.rect = new Rect(
             (1.0f - tmpWidth) / 2, 0.0f, tmpWidth, 1.0f);
        }
        else
        {
            // 画面が縦に長い場合、画面の長さにｱｽﾍﾟｸﾄ比を追従させる
            float bgScale = currentAspect_ / baseAspect;

            // ｶﾒﾗのｻｲｽﾞを縦の長さに合わせて設定しなおす
            cam.orthographicSize *= bgScale;

            // viewportRectを設定(範囲内全てを表示)
            cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        }
    }

}

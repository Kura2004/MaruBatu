using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteColorBlender : MonoBehaviour
{
    [SerializeField]
    private PostProcessVolume postProcessVolume; // PostProcessingのVolumeの参照

    private Vignette vignette;

    [SerializeField]
    private Color colorA = Color.white; // 1つ目の色
    [SerializeField]
    private Color colorB = Color.black; // 2つ目の色
    [SerializeField, Range(0f, 1f)]
    private float blendFactor = 0.5f;  // 補完の割合（0がcolorA、1がcolorB）

    [SerializeField]
    private Color blendedColor; // 補完された色

    private void Start()
    {
        // PostProcessing Volume から Vignette エフェクトを取得
        if (postProcessVolume.profile.TryGetSettings(out Vignette vignetteEffect))
        {
            vignette = vignetteEffect;
        }
        else
        {
            Debug.LogWarning("Vignette effect is not set in the PostProcessVolume.");
        }

        // 初期値として中間色を設定
        UpdateVignetteColor();
    }

    private void Update()
    {
        // 実行中に補完割合が変更された場合、中間色を更新する
        UpdateVignetteColor();
    }

    /// <summary>
    /// 2色の中間色をVignetteに適用し、インスペクターに表示
    /// </summary>
    public void UpdateVignetteColor()
    {
        if (vignette != null)
        {
            // Color.Lerpを使用して2つの色の中間色を取得
            blendedColor = Color.Lerp(colorA, colorB, blendFactor);

            // VignetteのColorプロパティに適用
            vignette.color.value = blendedColor;
        }
    }
}


using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;  // Post-processingの名前空間

public class VignetteEffectController : SingletonMonoBehaviour<VignetteEffectController>
{
    [Header("Vignette Effect Settings")]
    [SerializeField] private PostProcessVolume postProcessVolume; // インスペクターで設定するPostProcessVolume
    [SerializeField] private float maxIntensity = 1f; // インスペクターで編集可能な最大Intensity
    [SerializeField] private float increaseDuration = 2f; // maxIntensityに向かうアニメーションの時間（秒）
    [SerializeField] private float decreaseDuration = 2f; // 元に戻るアニメーションの時間（秒）
    [SerializeField] private float waitDuration = 1f; // アニメーション間の待機時間（秒）
    [SerializeField] private AnimationType increaseType = AnimationType.Exponential; // 増加のアニメーションタイプ
    [SerializeField] private AnimationType decreaseType = AnimationType.Logarithmic; // 減少のアニメーションタイプ

    private Vignette vignette;
    private bool isEffectActive = false;
    private Coroutine animationCoroutine;

    public enum AnimationType
    {
        Linear,
        Exponential,
        Logarithmic
    }

    // 初期化
    private void Start()
    {
        if (postProcessVolume != null)
        {
            // Vignetteの設定を取得
            if (!postProcessVolume.profile.TryGetSettings(out vignette))
            {
                Debug.LogError("PostProcessVolumeにVignette設定が見つかりません。正しいプロファイルが設定されているか確認してください。");
            }
        }
        else
        {
            Debug.LogError("PostProcessVolumeが設定されていません。インスペクターで設定してください。");
        }
        StopVignetteEffect();  // 初期状態でエフェクトを停止
    }

    // エフェクトのOn/Offを切り替えるメソッド
    public void ToggleVignetteEffect()
    {
        if (isEffectActive)
        {
            StopVignetteEffect();
        }
        else
        {
            StartVignetteEffect();
        }
    }

    // エフェクトを開始する
    public void StartVignetteEffect()
    {
        if (vignette != null && animationCoroutine == null)
        {
            isEffectActive = true;
            animationCoroutine = StartCoroutine(AnimateVignetteIntensity());
        }
    }

    // エフェクトを停止する
    public void StopVignetteEffect()
    {
        if (vignette != null)
        {
            isEffectActive = false;
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);  // コルーチンを停止
                animationCoroutine = null;  // コルーチン変数をリセット
            }
            vignette.intensity.value = 0;  // Intensityをリセット
            Debug.Log("エフェクトを停止しました");
        }
    }

    // VignetteのIntensityをアニメーションで永続的に増減させる
    private IEnumerator AnimateVignetteIntensity()
    {
        while (true)
        {
            // IntensityをmaxIntensityに向かって増加させるアニメーション
            yield return StartCoroutine(AnimateToValue(maxIntensity, increaseDuration, increaseType));

            // アニメーションの間に待機
            yield return new WaitForSeconds(waitDuration);

            // Intensityを0に向かって減少させるアニメーション
            yield return StartCoroutine(AnimateToValue(0, decreaseDuration, decreaseType));

        }
    }

    // Intensityを目標値までアニメーションさせる
    private IEnumerator AnimateToValue(float targetValue, float duration, AnimationType animationType)
    {
        float startValue = vignette.intensity.value;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // アニメーションタイプに応じた補間
            switch (animationType)
            {
                case AnimationType.Linear:
                    vignette.intensity.value = Mathf.Lerp(startValue, targetValue, t);
                    break;
                case AnimationType.Exponential:
                    vignette.intensity.value = Mathf.Lerp(startValue, targetValue, Mathf.Pow(t, 2));  // t^2で加速
                    break;
                case AnimationType.Logarithmic:
                    vignette.intensity.value = Mathf.Lerp(startValue, targetValue, Mathf.Log10(t + 1) / Mathf.Log10(2)); // 対数で減速
                    break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最後に目標値をセットして終了
        vignette.intensity.value = targetValue;
    }
}

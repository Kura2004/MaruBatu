using UnityEngine;
using DG.Tweening;

public class FadeOutEffect : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 2f; // アルファ値が0になるまでの時間（秒）

    [SerializeField]
    private float targetAlpha = 1f; // フェードアウト開始時のアルファ値

    private SpriteRenderer spriteRenderer; // SpriteRendererの参照

    private void Awake()
    {
        // SpriteRendererコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component is missing on the GameObject.");
        }
        else
        {
        }
    }

    /// <summary>
    /// フェードアウトを開始するメソッド
    /// </summary>
    public void StartFadeOut()
    {
        if (spriteRenderer != null)
        {
            // グレーの色に対してアルファ値を補完的に0に減少させる
            Color targetColor = new Color(0.5f, 0.5f, 0.5f, targetAlpha);

            // DoTweenを使用してアルファ値を0まで減少させる
            spriteRenderer.DOColor(targetColor, fadeDuration)
                          .SetEase(Ease.Linear);
        }
    }
}


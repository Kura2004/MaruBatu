using UnityEngine;
using DG.Tweening;

public class FadeInEffect : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 2f; // アルファ値が指定の値になるまでの時間（秒）

    [SerializeField]
    private float startAlpha = 1f; //最初のα値

    private SpriteRenderer spriteRenderer; // SpriteRendererの参照

    [SerializeField]
    bool OnStart = true;

    private void Start()
    {
        // SpriteRendererコンポーネントを取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer component is missing on the GameObject.");
        }
        else
        {
            // グレーに設定し、アルファ値を0に設定しておく
            Color color = new Color(0.5f, 0.5f, 0.5f, startAlpha);
            spriteRenderer.color = color;
        }

        if (OnStart)
            StartFadeIn();
    }

    /// <summary>
    /// フェードインを開始するメソッド
    /// </summary>
    public void StartFadeIn()
    {
        if (spriteRenderer != null)
        {
            // 現在の色を取得
            Color currentColor = Color.white;

            // DoTweenを使用してアルファ値を補完的に1まで増加させる
            spriteRenderer.DOColor(new Color(currentColor.r, currentColor.g, currentColor.b, 1f), fadeDuration)
                          .SetEase(Ease.Linear);

            // DoTweenを使用してアルファ値を指定した値まで増加させる
            spriteRenderer.DOColor(currentColor, fadeDuration)
                          .SetEase(Ease.Linear);
        }
    }
}

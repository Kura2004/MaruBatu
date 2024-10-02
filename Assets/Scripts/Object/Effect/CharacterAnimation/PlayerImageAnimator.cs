using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PlayerImageAnimator : MonoBehaviour
{
    [SerializeField] private float moveDuration = 1f; // 補完にかかる時間
    [SerializeField] private float colorDuration = 1f;

    [Header("X軸設定")]
    [SerializeField] private float xMovementDistance = 5f; // X座標の移動距離
    [SerializeField] private Ease xEaseType = Ease.Linear; // X座標のイージングの種類

    [Header("Z軸設定")]
    [SerializeField] private float zMovementDistance = 5f; // Z座標の移動距離
    [SerializeField] private Ease zEaseType = Ease.Linear; // Z座標のイージングの種類

    [Header("スプライト設定")]
    [SerializeField] private List<SpriteRenderer> spriteRenderers; // 複数のSpriteRendererを保持するリスト

    private void Start()
    {
        // 初期色を黒に設定
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.black;
            }
        }
    }

    // アニメーションを開始
    public void StartMovement()
    {
        MoveXPositive(); // X座標の正方向に移動
        MoveZPositive(); // Z座標の正方向に移動
        ChangeSpritesColorToWhite(); // スプライトの色を白に変更
        Invoke(nameof(StopMovement), moveDuration);
    }

    // アニメーションを停止
    public void StopMovement()
    {
        // Tweenを停止する処理を追加することができます
        DOTween.Kill(transform); // このオブジェクトに関連する全Tweenを停止
    }

    // X座標の正方向に移動
    private void MoveXPositive()
    {
        float targetX = transform.localPosition.x + xMovementDistance; // 現在のX座標に移動距離を足す

        transform.DOLocalMoveX(targetX, moveDuration)
            .SetEase(xEaseType)
            .OnComplete(null);
    }

    // X座標の負方向に移動
    private void MoveXNegative()
    {
        float targetX = transform.localPosition.x - xMovementDistance; // 現在のX座標から移動距離を引く

        transform.DOLocalMoveX(targetX, moveDuration)
            .SetEase(xEaseType)
            .OnComplete(null);
    }

    // Z座標の正方向に移動
    private void MoveZPositive()
    {
        float targetZ = transform.localPosition.z + zMovementDistance; // 現在のZ座標に移動距離を足す

        transform.DOLocalMoveZ(targetZ, moveDuration / 5.0f)
            .SetEase(zEaseType)
            .OnComplete(MoveZNegative); // 負方向に移動
    }

    // Z座標の負方向に移動
    private void MoveZNegative()
    {
        float targetZ = transform.localPosition.z - zMovementDistance; // 現在のZ座標から移動距離を引く

        transform.DOLocalMoveZ(targetZ, moveDuration / 5.0f)
            .SetEase(zEaseType)
            .OnComplete(MoveZPositive); // 正方向に移動
    }

    // スプライトの色を白に変化させる
    private void ChangeSpritesColorToWhite()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.DOColor(Color.white, colorDuration)
                    .SetEase(Ease.InExpo); // 指定した時間で白に補完
            }
        }
    }
}

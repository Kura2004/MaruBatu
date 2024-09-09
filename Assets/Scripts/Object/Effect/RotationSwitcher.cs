using UnityEngine;
using DG.Tweening;

public class RotationSwitcher : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    [SerializeField] private RotationAxis rotationAxis = RotationAxis.X; // 回転する軸
    [SerializeField] private float rotationAngle = 45f; // 回転角度
    [SerializeField] private float duration = 2f; // 回転にかかる時間
    [SerializeField] private float delay = 0.5f; // 回転再開の遅延時間
    [SerializeField] private Ease rotationEase = Ease.Linear; // 回転のイージング

    private float rotationSpeed;
    private float currentRotation;
    private bool isRotating = false;

    private void Start()
    {
        // 初期回転速度を設定
        rotationSpeed = rotationAngle / duration;
        currentRotation = 0f;
        StartRotation();
    }

    private void StartRotation()
    {
        if (isRotating)
            return;

        isRotating = true;
        Rotate();
    }

    private void Rotate()
    {
        float endRotation = currentRotation + rotationAngle;
        Tween rotationTween = CreateRotationTween(endRotation);

        rotationTween.OnComplete(() =>
        {
            // 回転が完了した後に遅延させる
            DOVirtual.DelayedCall(delay, () =>
            {
                // 正負を切り替える
                rotationAngle = -rotationAngle;
                currentRotation = endRotation;
                Rotate();
            });
        });
    }

    private Tween CreateRotationTween(float endRotation)
    {
        Vector3 rotationVector = Vector3.zero;

        switch (rotationAxis)
        {
            case RotationAxis.X:
                rotationVector = new Vector3(rotationAngle, 0, 0);
                break;
            case RotationAxis.Y:
                rotationVector = new Vector3(0, rotationAngle, 0);
                break;
            case RotationAxis.Z:
                rotationVector = new Vector3(0, 0, rotationAngle);
                break;
        }

        return transform.DORotate(rotationVector, duration, RotateMode.LocalAxisAdd)
            .SetEase(rotationEase);
    }
}

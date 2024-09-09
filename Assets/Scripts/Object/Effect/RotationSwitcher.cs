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

    [SerializeField] private RotationAxis rotationAxis = RotationAxis.X; // âÒì]Ç∑ÇÈé≤
    [SerializeField] private float rotationAngle = 45f; // âÒì]äpìx
    [SerializeField] private float duration = 2f; // âÒì]Ç…Ç©Ç©ÇÈéûä‘
    [SerializeField] private float delay = 0.5f; // âÒì]çƒäJÇÃíxâÑéûä‘
    [SerializeField] private Ease rotationEase = Ease.Linear; // âÒì]ÇÃÉCÅ[ÉWÉìÉO

    private float rotationSpeed;
    private float currentRotation;
    private bool isRotating = false;

    private void Start()
    {
        // èâä˙âÒì]ë¨ìxÇê›íË
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
            // âÒì]Ç™äÆóπÇµÇΩå„Ç…íxâÑÇ≥ÇπÇÈ
            DOVirtual.DelayedCall(delay, () =>
            {
                // ê≥ïâÇêÿÇËë÷Ç¶ÇÈ
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

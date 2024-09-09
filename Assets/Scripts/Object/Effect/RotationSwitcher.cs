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

    [SerializeField] private RotationAxis rotationAxis = RotationAxis.X; // ��]���鎲
    [SerializeField] private float rotationAngle = 45f; // ��]�p�x
    [SerializeField] private float duration = 2f; // ��]�ɂ����鎞��
    [SerializeField] private float delay = 0.5f; // ��]�ĊJ�̒x������
    [SerializeField] private Ease rotationEase = Ease.Linear; // ��]�̃C�[�W���O

    private float rotationSpeed;
    private float currentRotation;
    private bool isRotating = false;

    private void Start()
    {
        // ������]���x��ݒ�
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
            // ��]������������ɒx��������
            DOVirtual.DelayedCall(delay, () =>
            {
                // ������؂�ւ���
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

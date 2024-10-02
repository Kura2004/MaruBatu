using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class PlayerImageAnimator : MonoBehaviour
{
    [SerializeField] private float moveDuration = 1f; // �⊮�ɂ����鎞��
    [SerializeField] private float colorDuration = 1f;

    [Header("X���ݒ�")]
    [SerializeField] private float xMovementDistance = 5f; // X���W�̈ړ�����
    [SerializeField] private Ease xEaseType = Ease.Linear; // X���W�̃C�[�W���O�̎��

    [Header("Z���ݒ�")]
    [SerializeField] private float zMovementDistance = 5f; // Z���W�̈ړ�����
    [SerializeField] private Ease zEaseType = Ease.Linear; // Z���W�̃C�[�W���O�̎��

    [Header("�X�v���C�g�ݒ�")]
    [SerializeField] private List<SpriteRenderer> spriteRenderers; // ������SpriteRenderer��ێ����郊�X�g

    private void Start()
    {
        // �����F�����ɐݒ�
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.black;
            }
        }
    }

    // �A�j���[�V�������J�n
    public void StartMovement()
    {
        MoveXPositive(); // X���W�̐������Ɉړ�
        MoveZPositive(); // Z���W�̐������Ɉړ�
        ChangeSpritesColorToWhite(); // �X�v���C�g�̐F�𔒂ɕύX
        Invoke(nameof(StopMovement), moveDuration);
    }

    // �A�j���[�V�������~
    public void StopMovement()
    {
        // Tween���~���鏈����ǉ����邱�Ƃ��ł��܂�
        DOTween.Kill(transform); // ���̃I�u�W�F�N�g�Ɋ֘A����STween���~
    }

    // X���W�̐������Ɉړ�
    private void MoveXPositive()
    {
        float targetX = transform.localPosition.x + xMovementDistance; // ���݂�X���W�Ɉړ������𑫂�

        transform.DOLocalMoveX(targetX, moveDuration)
            .SetEase(xEaseType)
            .OnComplete(null);
    }

    // X���W�̕������Ɉړ�
    private void MoveXNegative()
    {
        float targetX = transform.localPosition.x - xMovementDistance; // ���݂�X���W����ړ�����������

        transform.DOLocalMoveX(targetX, moveDuration)
            .SetEase(xEaseType)
            .OnComplete(null);
    }

    // Z���W�̐������Ɉړ�
    private void MoveZPositive()
    {
        float targetZ = transform.localPosition.z + zMovementDistance; // ���݂�Z���W�Ɉړ������𑫂�

        transform.DOLocalMoveZ(targetZ, moveDuration / 5.0f)
            .SetEase(zEaseType)
            .OnComplete(MoveZNegative); // �������Ɉړ�
    }

    // Z���W�̕������Ɉړ�
    private void MoveZNegative()
    {
        float targetZ = transform.localPosition.z - zMovementDistance; // ���݂�Z���W����ړ�����������

        transform.DOLocalMoveZ(targetZ, moveDuration / 5.0f)
            .SetEase(zEaseType)
            .OnComplete(MoveZPositive); // �������Ɉړ�
    }

    // �X�v���C�g�̐F�𔒂ɕω�������
    private void ChangeSpritesColorToWhite()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.DOColor(Color.white, colorDuration)
                    .SetEase(Ease.InExpo); // �w�肵�����ԂŔ��ɕ⊮
            }
        }
    }
}

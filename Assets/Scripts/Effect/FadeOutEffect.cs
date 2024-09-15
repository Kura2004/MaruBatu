using UnityEngine;
using DG.Tweening;

public class FadeOutEffect : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 2f; // �A���t�@�l��0�ɂȂ�܂ł̎��ԁi�b�j

    [SerializeField]
    private float targetAlpha = 1f; // �t�F�[�h�A�E�g�J�n���̃A���t�@�l

    private SpriteRenderer spriteRenderer; // SpriteRenderer�̎Q��

    private void Awake()
    {
        // SpriteRenderer�R���|�[�l���g���擾
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
    /// �t�F�[�h�A�E�g���J�n���郁�\�b�h
    /// </summary>
    public void StartFadeOut()
    {
        if (spriteRenderer != null)
        {
            // �O���[�̐F�ɑ΂��ăA���t�@�l��⊮�I��0�Ɍ���������
            Color targetColor = new Color(0.5f, 0.5f, 0.5f, targetAlpha);

            // DoTween���g�p���ăA���t�@�l��0�܂Ō���������
            spriteRenderer.DOColor(targetColor, fadeDuration)
                          .SetEase(Ease.Linear);
        }
    }
}


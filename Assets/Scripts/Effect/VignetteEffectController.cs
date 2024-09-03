using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;  // Post-processing�̖��O���

public class VignetteEffectController : SingletonMonoBehaviour<VignetteEffectController>
{
    [Header("Vignette Effect Settings")]
    [SerializeField] private PostProcessVolume postProcessVolume; // �C���X�y�N�^�[�Őݒ肷��PostProcessVolume
    [SerializeField] private float maxIntensity = 1f; // �C���X�y�N�^�[�ŕҏW�\�ȍő�Intensity
    [SerializeField] private float increaseDuration = 2f; // maxIntensity�Ɍ������A�j���[�V�����̎��ԁi�b�j
    [SerializeField] private float decreaseDuration = 2f; // ���ɖ߂�A�j���[�V�����̎��ԁi�b�j
    [SerializeField] private float waitDuration = 1f; // �A�j���[�V�����Ԃ̑ҋ@���ԁi�b�j
    [SerializeField] private AnimationType increaseType = AnimationType.Exponential; // �����̃A�j���[�V�����^�C�v
    [SerializeField] private AnimationType decreaseType = AnimationType.Logarithmic; // �����̃A�j���[�V�����^�C�v

    private Vignette vignette;
    private bool isEffectActive = false;
    private Coroutine animationCoroutine;

    public enum AnimationType
    {
        Linear,
        Exponential,
        Logarithmic
    }

    // ������
    private void Start()
    {
        if (postProcessVolume != null)
        {
            // Vignette�̐ݒ���擾
            if (!postProcessVolume.profile.TryGetSettings(out vignette))
            {
                Debug.LogError("PostProcessVolume��Vignette�ݒ肪������܂���B�������v���t�@�C�����ݒ肳��Ă��邩�m�F���Ă��������B");
            }
        }
        else
        {
            Debug.LogError("PostProcessVolume���ݒ肳��Ă��܂���B�C���X�y�N�^�[�Őݒ肵�Ă��������B");
        }
        StopVignetteEffect();  // ������ԂŃG�t�F�N�g���~
    }

    // �G�t�F�N�g��On/Off��؂�ւ��郁�\�b�h
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

    // �G�t�F�N�g���J�n����
    public void StartVignetteEffect()
    {
        if (vignette != null && animationCoroutine == null)
        {
            isEffectActive = true;
            animationCoroutine = StartCoroutine(AnimateVignetteIntensity());
        }
    }

    // �G�t�F�N�g���~����
    public void StopVignetteEffect()
    {
        if (vignette != null)
        {
            isEffectActive = false;
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);  // �R���[�`�����~
                animationCoroutine = null;  // �R���[�`���ϐ������Z�b�g
            }
            vignette.intensity.value = 0;  // Intensity�����Z�b�g
            Debug.Log("�G�t�F�N�g���~���܂���");
        }
    }

    // Vignette��Intensity���A�j���[�V�����ŉi���I�ɑ���������
    private IEnumerator AnimateVignetteIntensity()
    {
        while (true)
        {
            // Intensity��maxIntensity�Ɍ������đ���������A�j���[�V����
            yield return StartCoroutine(AnimateToValue(maxIntensity, increaseDuration, increaseType));

            // �A�j���[�V�����̊Ԃɑҋ@
            yield return new WaitForSeconds(waitDuration);

            // Intensity��0�Ɍ������Č���������A�j���[�V����
            yield return StartCoroutine(AnimateToValue(0, decreaseDuration, decreaseType));

        }
    }

    // Intensity��ڕW�l�܂ŃA�j���[�V����������
    private IEnumerator AnimateToValue(float targetValue, float duration, AnimationType animationType)
    {
        float startValue = vignette.intensity.value;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // �A�j���[�V�����^�C�v�ɉ��������
            switch (animationType)
            {
                case AnimationType.Linear:
                    vignette.intensity.value = Mathf.Lerp(startValue, targetValue, t);
                    break;
                case AnimationType.Exponential:
                    vignette.intensity.value = Mathf.Lerp(startValue, targetValue, Mathf.Pow(t, 2));  // t^2�ŉ���
                    break;
                case AnimationType.Logarithmic:
                    vignette.intensity.value = Mathf.Lerp(startValue, targetValue, Mathf.Log10(t + 1) / Mathf.Log10(2)); // �ΐ��Ō���
                    break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �Ō�ɖڕW�l���Z�b�g���ďI��
        vignette.intensity.value = targetValue;
    }
}

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;  // Post-processing�̖��O���
using System.Collections;

public class ChromaticAberrationEffectController : SingletonMonoBehaviour<ChromaticAberrationEffectController>
{
    [Header("Chromatic Aberration Effect Settings")]
    [SerializeField] private PostProcessVolume postProcessVolume; // �C���X�y�N�^�[�Őݒ肷��PostProcessVolume
    [SerializeField] private float maxIntensity = 1f; // �C���X�y�N�^�[�ŕҏW�\�ȍő�Intensity
    [SerializeField] private float increaseDuration = 2f; // maxIntensity�Ɍ������A�j���[�V�����̎��ԁi�b�j
    [SerializeField] private float decreaseDuration = 2f; // ���ɖ߂�A�j���[�V�����̎��ԁi�b�j
    [SerializeField] private float waitBetweenAnimations = 1f; // �A�j���[�V�����̊Ԃ̑ҋ@���ԁi�b�j
    [SerializeField] private AnimationType increaseType = AnimationType.Exponential; // �����̃A�j���[�V�����^�C�v
    [SerializeField] private AnimationType decreaseType = AnimationType.Logarithmic; // �����̃A�j���[�V�����^�C�v

    private ChromaticAberration chromaticAberration;
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
            // ChromaticAberration�̐ݒ���擾
            if (!postProcessVolume.profile.TryGetSettings(out chromaticAberration))
            {
                Debug.LogError("PostProcessVolume��Chromatic Aberration�ݒ肪������܂���B�������v���t�@�C�����ݒ肳��Ă��邩�m�F���Ă��������B");
            }
        }
        else
        {
            Debug.LogError("PostProcessVolume���ݒ肳��Ă��܂���B�C���X�y�N�^�[�Őݒ肵�Ă��������B");
        }
        StopChromaticAberrationEffect();  // ������ԂŃG�t�F�N�g���~
    }

    // �G�t�F�N�g��On/Off��؂�ւ��郁�\�b�h
    public void ToggleChromaticAberrationEffect()
    {
        if (isEffectActive)
        {
            StopChromaticAberrationEffect();
        }
        else
        {
            StartChromaticAberrationEffect();
        }
    }

    // �G�t�F�N�g���J�n����
    public void StartChromaticAberrationEffect()
    {
        if (chromaticAberration != null && animationCoroutine == null)
        {
            isEffectActive = true;
            animationCoroutine = StartCoroutine(AnimateChromaticAberrationIntensity());
        }
    }

    // �G�t�F�N�g���~����
    public void StopChromaticAberrationEffect()
    {
        if (chromaticAberration != null)
        {
            isEffectActive = false;
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);  // �R���[�`�����~
                animationCoroutine = null;  // �R���[�`���ϐ������Z�b�g
            }
            chromaticAberration.intensity.value = 0;  // Intensity�����Z�b�g
        }
    }

    // Chromatic Aberration��Intensity���A�j���[�V�����ŉi���I�ɑ���������
    private IEnumerator AnimateChromaticAberrationIntensity()
    {
        while (true)
        {
            // Intensity��maxIntensity�Ɍ������đ���������A�j���[�V����
            yield return StartCoroutine(AnimateToValue(maxIntensity, increaseDuration, increaseType));

            // �A�j���[�V�����̊Ԃ̑ҋ@����
            yield return new WaitForSeconds(waitBetweenAnimations);

            // Intensity��0�Ɍ������Č���������A�j���[�V����
            yield return StartCoroutine(AnimateToValue(0, decreaseDuration, decreaseType));

        }
    }

    // Intensity��ڕW�l�܂ŃA�j���[�V����������
    private IEnumerator AnimateToValue(float targetValue, float duration, AnimationType animationType)
    {
        float startValue = chromaticAberration.intensity.value;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // �A�j���[�V�����^�C�v�ɉ��������
            switch (animationType)
            {
                case AnimationType.Linear:
                    chromaticAberration.intensity.value = Mathf.Lerp(startValue, targetValue, t);
                    break;
                case AnimationType.Exponential:
                    chromaticAberration.intensity.value = Mathf.Lerp(startValue, targetValue, Mathf.Pow(t, 2));  // t^2�ŉ���
                    break;
                case AnimationType.Logarithmic:
                    chromaticAberration.intensity.value = Mathf.Lerp(startValue, targetValue, Mathf.Log10(t + 1) / Mathf.Log10(2)); // �ΐ��Ō���
                    break;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �Ō�ɖڕW�l���Z�b�g���ďI��
        chromaticAberration.intensity.value = targetValue;
    }
}

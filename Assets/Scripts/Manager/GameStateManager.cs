using UnityEngine;
using System.Collections;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    // ��]��Ԃ������v���p�e�B
    public bool IsRotating { get; private set; } = false;

    // �ՖʃZ�b�g�����t���O�i�O������̃A�N�Z�X�͉\�����A�ݒ�͓����̃��\�b�h�ōs���j
    public bool IsBoardSetupComplete { get; private set; } = false;

    // ��]���J�n���郁�\�b�h
    public void StartRotation()
    {
        Debug.Log("��]���n�߂܂�");
        IsRotating = true;
    }

    // ��]���I�����郁�\�b�h
    public void EndRotation()
    {
        IsRotating = false;
    }

    // �ՖʃZ�b�g�����̃t���O���X�V���郁�\�b�h
    public void SetBoardSetupComplete(bool isComplete)
    {
        IsBoardSetupComplete = isComplete;
        //ScenesAudio.UnPauseBgm();
        TimeLimitController.Instance.StartTimer();
        Debug.Log("Board setup complete status: " + IsBoardSetupComplete);
    }

    // �ՖʃZ�b�g���J�n���郁�\�b�h�i�����Ƃ��ăZ�b�g�A�b�v�����܂ł̕b�����󂯎��j
    public void StartBoardSetup(float setupDuration)
    {
        StartCoroutine(BoardSetupCoroutine(setupDuration));
    }

    // �R���[�`���Ŕ񓯊��ɔՖʃZ�b�g�A�b�v�������s��
    private IEnumerator BoardSetupCoroutine(float setupDuration)
    {
        // �ՖʃZ�b�g�̏����������ɋL�q
        Debug.Log("Starting board setup...");

        // �w�肳�ꂽ�b�������ҋ@
        yield return new WaitForSeconds(setupDuration);

        // �Z�b�g�A�b�v������Ƀt���O���X�V
        SetBoardSetupComplete(true);
    }

    // �ՖʃZ�b�g�����t���O�����Z�b�g���郁�\�b�h
    public void ResetBoardSetup()
    {
        IsBoardSetupComplete = false;
        Debug.Log("Board setup has been reset.");
    }
}

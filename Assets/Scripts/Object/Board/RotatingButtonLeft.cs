using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingButtonLeft : MonoBehaviour
{
    [SerializeField]
    private RotatingMassObjectManager rotatingManager; // ��]�������Ǘ�����}�l�[�W���[

    [SerializeField]
    private ObjectColorChanger colorChanger; // �F�̕ύX���Ǘ�����R���|�[�l���g

    bool fadeIn = false;

    private bool IsInteractionBlocked()
    {
        var turnManager = GameTurnManager.Instance;
        return CanvasBounce.isBlocked ||
               turnManager.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) ||
               turnManager.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece) ||
               TimeControllerToggle.isTimeStopped || !GameStateManager.Instance.IsBoardSetupComplete;
    }

    private void Start()
    {
        if (colorChanger == null)
        {
            Debug.LogError("ObjectColorChanger �R���|�[�l���g���ݒ肳��Ă��܂���");
        }
    }

    private void Update()
    {
        var turnMana = GameTurnManager.Instance;
        if ((turnMana.IsCurrentTurn(GameTurnManager.TurnState.PlayerRotateGroup) ||
            turnMana.IsCurrentTurn(GameTurnManager.TurnState.OpponentRotateGroup))
            && !fadeIn)
        {
            fadeInEffect();
        }

        if ((turnMana.IsCurrentTurn(GameTurnManager.TurnState.PlayerPlacePiece) ||
            turnMana.IsCurrentTurn(GameTurnManager.TurnState.OpponentPlacePiece))
            && fadeIn)
        {
            fadeOutEffect();
        }
    }

    private void OnMouseDown()
    {
        if (IsInteractionBlocked() || !rotatingManager.AnyMassClicked())
        {
            //ScenesAudio.BlockSe();
            return;
        }

        HandleClickInteraction();
    }

    private void HandleClickInteraction()
    {
        TimeLimitController.Instance.StopTimer();
        rotatingManager.StartRotationLeft(); // ����]���J�n
    }
    void fadeInEffect()
    {
        GetComponent<FadeInEffect>().StartFadeIn();
        fadeIn = true;
    }

    void fadeOutEffect()
    {
        GetComponent<FadeOutEffect>().StartFadeOut();
        fadeIn = false;
    }
}




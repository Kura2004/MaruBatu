using DG.Tweening;
using UnityEngine;

public class RotatingButtonRight : MonoBehaviour
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
            return;
        }

        HandleClickInteraction();
    }

    private void HandleClickInteraction()
    {
        TimeLimitController.Instance.StopTimer();
        rotatingManager.StartRotationRight(); // �E��]���J�n
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






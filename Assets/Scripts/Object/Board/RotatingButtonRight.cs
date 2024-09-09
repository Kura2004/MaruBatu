using DG.Tweening;
using UnityEngine;

public class RotatingButtonRight : MonoBehaviour
{
    [SerializeField]
    private RotatingMassObjectManager rotatingManager; // 回転処理を管理するマネージャー

    [SerializeField]
    private ObjectColorChanger colorChanger; // 色の変更を管理するコンポーネント


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
            Debug.LogError("ObjectColorChanger コンポーネントが設定されていません");
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
        rotatingManager.StartRotationRight(); // 右回転を開始
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






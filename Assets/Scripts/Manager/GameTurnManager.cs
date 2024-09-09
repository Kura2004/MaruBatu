using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���̃^�[���Ǘ����s���V���O���g���ȃN���X
/// </summary>
public class GameTurnManager : SingletonMonoBehaviour<GameTurnManager>
{
    public enum TurnState
    {
        PlayerPlacePiece,
        PlayerRotateGroup,
        OpponentPlacePiece,
        OpponentRotateGroup
    }

    public TurnState CurrentTurnState { get; private set; }
    public bool IsTurnChanging { get; private set; }
    private int turnChangeCounter = 0;
    public bool IsGameStarted;

    public int TotalTurnCount { get; private set; }

    protected override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Initialize();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        ProcessTurnChange();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    private void Initialize()
    {
        CurrentTurnState = TurnState.PlayerPlacePiece;
        IsTurnChanging = false;
        IsGameStarted = false;
        TotalTurnCount = 0;
        turnChangeCounter = 0;
    }

    public void AdvanceTurn()
    {
        TotalTurnCount++;
        switch (CurrentTurnState)
        {
            case TurnState.PlayerPlacePiece:
                CurrentTurnState = TurnState.PlayerRotateGroup;
                break;
            case TurnState.PlayerRotateGroup:
                CurrentTurnState = TurnState.OpponentPlacePiece;
                break;
            case TurnState.OpponentPlacePiece:
                CurrentTurnState = TurnState.OpponentRotateGroup;
                break;
            case TurnState.OpponentRotateGroup:
                CurrentTurnState = TurnState.PlayerPlacePiece;
                break;
        }
        Debug.Log("�^�[�����i�s���܂���: " + CurrentTurnState);
    }

    public bool IsCurrentTurn(TurnState turnState)
    {
        return CurrentTurnState == turnState;
    }

    public void SetTurnState(TurnState newTurnState)
    {
        CurrentTurnState = newTurnState;
        Debug.Log("�^�[����Ԃ��ݒ肳��܂���: " + CurrentTurnState);
    }

    public bool IsGameEnd()
    {
        return TotalTurnCount >= 16 * 2;
    }

    // TurnChange��ݒ肷�郁�\�b�h
    public void SetTurnChange(bool value)
    {
        IsTurnChanging = value;
        Debug.Log("TurnChange���ݒ肳��܂���: " + IsTurnChanging);
    }

    // LateUpdate���̏��������\�b�h��
    private void ProcessTurnChange()
    {
        if (IsTurnChanging)
        {
            turnChangeCounter++;
            if (turnChangeCounter >= 1)
            {
                turnChangeCounter = 0;
                IsTurnChanging = false;

                if (IsGameEnd())
                {
                    TimeLimitController.Instance.currentTime = 0;
                }
            }
        }
    }
}


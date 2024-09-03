using UnityEngine;
using System.Collections;

public class GameStateManager : SingletonMonoBehaviour<GameStateManager>
{
    // 回転状態を示すプロパティ
    public bool IsRotating { get; private set; } = false;

    // 盤面セット完了フラグ（外部からのアクセスは可能だが、設定は内部のメソッドで行う）
    public bool IsBoardSetupComplete { get; private set; } = false;

    // 回転を開始するメソッド
    public void StartRotation()
    {
        Debug.Log("回転を始めます");
        IsRotating = true;
    }

    // 回転を終了するメソッド
    public void EndRotation()
    {
        IsRotating = false;
    }

    // 盤面セット完了のフラグを更新するメソッド
    public void SetBoardSetupComplete(bool isComplete)
    {
        IsBoardSetupComplete = isComplete;
        //ScenesAudio.UnPauseBgm();
        TimeLimitController.Instance.StartTimer();
        Debug.Log("Board setup complete status: " + IsBoardSetupComplete);
    }

    // 盤面セットを開始するメソッド（引数としてセットアップ完了までの秒数を受け取る）
    public void StartBoardSetup(float setupDuration)
    {
        StartCoroutine(BoardSetupCoroutine(setupDuration));
    }

    // コルーチンで非同期に盤面セットアップ処理を行う
    private IEnumerator BoardSetupCoroutine(float setupDuration)
    {
        // 盤面セットの処理をここに記述
        Debug.Log("Starting board setup...");

        // 指定された秒数だけ待機
        yield return new WaitForSeconds(setupDuration);

        // セットアップ完了後にフラグを更新
        SetBoardSetupComplete(true);
    }

    // 盤面セット完了フラグをリセットするメソッド
    public void ResetBoardSetup()
    {
        IsBoardSetupComplete = false;
        Debug.Log("Board setup has been reset.");
    }
}

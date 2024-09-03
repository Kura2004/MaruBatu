using UnityEngine;
using System.Collections;
using DG.Tweening; // DOTweenのネームスペースをインポート

public class MassColorChecker : MonoBehaviour
{
    [SerializeField]
    private string massTag = "Mass"; // タグ名

    [SerializeField]
    private Material targetMaterial; // 色を変更するマテリアル

    [SerializeField]
    private Color targetColor = Color.red; // 変更後の色

    private float resetDelay = 0.03f; // ResetBoardSetupを呼び出すまでの遅延時間

    private GameObject[] mass = new GameObject[4]; // オブジェクトを保存する配列
    private int massIndex = 0; // 配列のインデックス
    private bool loadGameOver = false;
    private bool shouldResetBoardSetup = false; // ResetBoardSetupを実行するかを示すフラグ

    private void OnTriggerStay(Collider other)
    {
        // TagがMassのオブジェクトに当たっている場合
        if (other.CompareTag(massTag))
        {
            // オブジェクトを登録
            mass[massIndex] = other.gameObject;
            massIndex = (massIndex + 1) % mass.Length;
        }
    }

    private void Update()
    {
        var gameState = GameStateManager.Instance;
        if (!gameState.IsRotating && gameState.IsBoardSetupComplete && !loadGameOver)
        {
            if (HasFourOrMorePlayerObjects() || HasFourOrMoreOpponentObjects())
            {
                StartCoroutine(HandleGameOverCoroutine());
            }
        }
    }

    private void LateUpdate()
    {
        // フラグが立っていればボードリセットの処理を実行
        if (shouldResetBoardSetup)
        {
            ExecuteResetBoardSetup();
            shouldResetBoardSetup = false; // フラグをリセット
        }
    }

    private bool HasFourOrMorePlayerObjects()
    {
        return HasFourOrMoreObjectsOfColor(GlobalColorManager.Instance.playerColor);
    }

    private bool HasFourOrMoreOpponentObjects()
    {
        return HasFourOrMoreObjectsOfColor(GlobalColorManager.Instance.opponentColor);
    }

    private bool HasFourOrMoreObjectsOfColor(Color colorToCheck)
    {
        int count = 0;

        // 配列内のオブジェクトをチェック
        foreach (var obj in mass)
        {
            if (obj != null)
            {
                Renderer renderer = obj.GetComponent<Renderer>();

                if (renderer != null && renderer.material.color == colorToCheck
                    && obj.GetComponent<ObjectColorChangerWithoutReset>().isClicked)
                {
                    count++;
                }
            }
        }

        return count >= 4; // 4つ以上のオブジェクトが指定した色であればtrueを返す
    }

    private IEnumerator HandleGameOverCoroutine()
    {
        ScenesAudio.WinSe();
        TimeLimitController.Instance.StopTimer();
        ToggleMassState();

        // massのマテリアルカラーを指定した色に即時変更する
        foreach (var obj in mass)
        {
            if (obj != null)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null && targetMaterial != null)
                {
                    // カスタムシェーダーで色変更
                    targetMaterial.SetColor("_Color", targetColor); // シェーダーのプロパティ名に合わせて変更
                    renderer.material = targetMaterial;
                }
            }
        }

        // DOTweenを使って指定した遅延時間後にフラグを立てる
        DOTween.Sequence().AppendInterval(resetDelay).AppendCallback(() => {
            shouldResetBoardSetup = true; // 遅延後にフラグをセット
        });

        loadGameOver = true;
        ScenesLoader.Instance.LoadGameOver(3.0f);
        yield return null; // Coroutineを終了するために待機（必要に応じて他の処理を追加）
    }

    // ボードリセットの処理をメソッド化
    private void ExecuteResetBoardSetup()
    {
        GameStateManager.Instance.ResetBoardSetup();
        TimeLimitController.Instance.ResetEffect();
    }

    // 登録したマスの状態を切り替えるメソッド
    public void ToggleMassState()
    {
        foreach (var obj in mass)
        {
            if (obj != null)
            {
                var massMove = obj.GetComponent<ControlledRotationBySpeedToggle>();
                if (massMove != null)
                {
                    massMove.SetFastRotation(true);
                }
            }
        }
    }
}

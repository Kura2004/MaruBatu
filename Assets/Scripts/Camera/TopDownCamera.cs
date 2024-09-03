using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    [SerializeField] Transform target; // ターゲットのゲームオブジェクト
    [SerializeField] float height = 10f; // カメラの高さ

    void Start()
    {
        if (target != null)
        {
            // ターゲットの真上にカメラを配置
            Vector3 newPosition = target.position;
            newPosition.y += height;
            transform.position = newPosition;

            // カメラの向きをターゲットに向ける
            transform.LookAt(target);
        }

        //配置が終わったら削除
        Destroy(this);
    }
}


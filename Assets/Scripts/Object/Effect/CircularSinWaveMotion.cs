using UnityEngine;
using System.Collections.Generic;

public class CircularSinWaveMotion : MonoBehaviour
{
    [SerializeField] private List<GameObject> targetPrefabList; // 生成するターゲットのプレハブリスト
    [SerializeField] private Transform centerPoint; // 中心点
    [SerializeField] private float baseRadius = 5f; // 基本の円の半径
    [SerializeField] private float amplitude = 1f; // 上下運動の振幅
    [SerializeField] private float baseFrequency = 1f; // 基本の上下運動の周波数
    [SerializeField] private float radiusFrequency = 1f; // 半径の変化の周波数
    [SerializeField] private float rotationSpeed = 1f; // 円運動の速度

    private Transform[] targetArray;
    private Vector3[] initialPositions;

    void Start()
    {
        int numberOfTargets = targetPrefabList.Count;
        targetArray = new Transform[numberOfTargets];
        initialPositions = new Vector3[numberOfTargets];

        GenerateTargets(numberOfTargets);
        InitializeTargetPositions(numberOfTargets);
    }

    void Update()
    {
        AnimateTargets();
    }

    // ターゲットを生成
    private void GenerateTargets(int numberOfTargets)
    {
        for (int i = 0; i < numberOfTargets; i++)
        {
            GameObject newTarget = Instantiate(targetPrefabList[i]);
            targetArray[i] = newTarget.transform;
        }
    }

    // ターゲットを円周上に均等に配置
    private void InitializeTargetPositions(int numberOfTargets)
    {
        for (int i = 0; i < numberOfTargets; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfTargets;
            Vector3 newPosition = centerPoint.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * baseRadius;
            initialPositions[i] = newPosition;
            targetArray[i].position = newPosition;
        }
    }

    // ターゲットの上下運動と円運動を更新
    private void AnimateTargets()
    {
        for (int i = 0; i < targetArray.Length; i++)
        {
            Transform target = targetArray[i];
            if (target != null)
            {
                float time = Time.time;
                float frequency = baseFrequency * (1 + i * 0.1f); // インデックスによって周波数を調整

                // 上下運動
                float verticalOffset = Mathf.Sin(time * frequency) * amplitude * 0.3f;

                // 半径の変化
                float dynamicRadius = baseRadius + Mathf.Sin(time * radiusFrequency) * amplitude;

                // 円運動
                float angle = i * Mathf.PI * 2 / targetArray.Length + time * rotationSpeed;
                Vector3 offset = new Vector3(Mathf.Cos(angle), verticalOffset, Mathf.Sin(angle)) * dynamicRadius;
                target.position = centerPoint.position + offset;
            }
        }
    }
}






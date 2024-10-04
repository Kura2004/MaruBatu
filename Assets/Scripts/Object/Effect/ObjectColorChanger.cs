using UnityEngine;
using DG.Tweening;

public class ObjectColorChanger : MonoBehaviour
{
    [SerializeField]
    private Color originalColor = Color.white; // 元の色をインスペクターで設定できるように

    [SerializeField]
    private Color hoverAndClickColor = Color.red; // マウスが当たったときとクリック時の色

    [SerializeField]
    private float colorChangeDuration = 1f; // 色の補完にかかる時間

    private Renderer objectRenderer; // オブジェクトのRenderer
    private Tween colorTween; // 色の補完用のTween
    public bool isClicked = false; // クリック状態を保持するフラグ

    private bool isChanging = false;
    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material.color = originalColor; // オブジェクトの元の色を設定
        }
    }

    protected virtual void OnMouseEnter()
    {
        if (objectRenderer != null)
        {
            ColorChange();
        }
    }

    protected virtual void OnMouseExit()
    {
        if (objectRenderer != null)
        {
            // マウスがオブジェクトから出たときに色を元に戻す
            colorTween = objectRenderer.material.DOColor(originalColor, colorChangeDuration);
        }
    }

    protected virtual void OnMouseOver()
    {
        if (ShouldChangeColorOnMouseOver())
        {
            ColorChange();
        }
    }

    public void handleToClick()
    {
        if (objectRenderer != null)
        {
            isClicked = true; // クリック状態を記録
            objectRenderer.material.color = hoverAndClickColor;
        }
    }

    private void ColorChange()
    {
        isChanging = true;
        colorTween = objectRenderer.material.DOColor(hoverAndClickColor, colorChangeDuration)
            .OnComplete(() =>
            {
                isChanging = false;
            });
    }

    public void ChangeHoverColor(Color newColor)
    {
        hoverAndClickColor = newColor;
    }

    private bool ShouldChangeColorOnMouseOver()
    {
        return !GameStateManager.Instance.IsRotating && !isClicked
            && !isChanging;
    }
}

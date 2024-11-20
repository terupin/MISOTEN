using System.Collections;
using UnityEngine;

public class DenchikuController : MonoBehaviour
{
    private Vector3 originalPosition; // 元の位置
    private bool isReturning = false;

    private Renderer objectRenderer; // 視覚的変化のため
    private Vector3 originalScale;   // 元のスケール

    [SerializeField, Header("Denchikuの移動先")]
    private Vector3 targetPosition;  // 各Denchiku用の移動先座標

    private void Start()
    {
        // 初期位置を記録
        originalPosition = transform.localPosition;
        objectRenderer = GetComponent<Renderer>();
        originalScale = transform.localScale;
    }

    public void DetachAndMove()
    {
        if (isReturning) return; // 元に戻る途中は再実行しない

        // 親子関係を解除して移動を開始
        transform.parent = null;

        // 視覚効果を開始
        StartVisualEffect();

        // 移動処理
        StartCoroutine(MoveToTarget(targetPosition));
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        float duration = 1.5f; // 移動にかかる時間
        Vector3 startPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            // スケール変化の例 (小さくして戻す動き)
            float scaleMultiplier = 1.0f + Mathf.PingPong(elapsedTime * 2, 0.5f);
            transform.localScale = originalScale * scaleMultiplier;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        // 視覚効果を終了
        EndVisualEffect();

        // 10秒後に元の位置に戻る
        yield return new WaitForSeconds(10f);
        ReturnToOriginalPosition();
    }

    private void ReturnToOriginalPosition()
    {
        StartCoroutine(MoveToOriginal());
    }

    private IEnumerator MoveToOriginal()
    {
        isReturning = true;

        float elapsedTime = 0f;
        float duration = 1.5f; // 元の位置に戻る時間
        Vector3 startPosition = transform.position;

        StartVisualEffect(); // 戻るときもエフェクトを表示

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsedTime / duration);

            // スケール変化の例
            float scaleMultiplier = 1.0f + Mathf.PingPong(elapsedTime * 2, 0.5f);
            transform.localScale = originalScale * scaleMultiplier;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        transform.parent = GameObject.Find("Matsunaga").transform; // 元の親に戻す
        isReturning = false;

        EndVisualEffect();
    }

    /// <summary>
    /// 視覚的効果を開始
    /// </summary>
    private void StartVisualEffect()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.red; // 色を赤に変更
        }
    }

    /// <summary>
    /// 視覚的効果を終了
    /// </summary>
    private void EndVisualEffect()
    {
        if (objectRenderer != null)
        {
            objectRenderer.material.color = Color.white; // 元の色に戻す
        }

        transform.localScale = originalScale; // スケールを元に戻す
    }

    // 外部から移動先を設定
    public void SetTargetPosition(Vector3 newPosition)
    {
        targetPosition = newPosition;
    }
}

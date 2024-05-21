using UnityEngine;

public class TimerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private FryingManager fryingManager;
    private Vector3 initialPosition; // 初始位置

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        fryingManager = FindObjectOfType<FryingManager>();
        initialPosition = transform.localPosition; // 记录初始位置
    }

    public void UpdateTimer()
    {
        float progress = Mathf.Clamp01(fryingManager.fryingTime / fryingManager.fryingDuration);
        
        // 更新颜色
        spriteRenderer.color = Color.Lerp(Color.white, Color.red, progress);

        // 更新遮罩效果，从底部向上显示红色
        transform.localScale = new Vector3(0.3f, progress, 1);
        transform.localPosition = new Vector3(initialPosition.x, initialPosition.y - (1 - progress) / 2, initialPosition.z);
    }
}
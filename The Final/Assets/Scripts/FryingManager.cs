using UnityEngine;

public class FryingManager : MonoBehaviour
{
    public Collider2D dishDonutDoughAreaCollider;
    public Collider2D fryingPanCollider;
    public Collider2D dishFriedDonutCollider;

    public Collider2D[] smallFryingPanColliders; // 四个 drainCap 的 Collider
    public GameObject[] drainCaps; // 四个 drainCap 对象

    public float fryingTime = 0f; // 记录煎炸时间
    public float fryingDuration = 10f; // 煎炸持续时间

    private TimerController fryingTimerController; // 煎炸计时器控制器

    private GameObject selectedObject;
    private Vector2 originalPosition;
    private bool isInPan = false; // 表示甜甜圈面团是否在煎锅中
    private bool isReady = false;
    private GameObject currentDrainCap; // 当前放置 DonutDough 的 drainCap

    private void Start()
    {
        fryingTimerController = FindObjectOfType<TimerController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider != null && hitCollider.CompareTag("DonutDough"))
            {
                selectedObject = hitCollider.gameObject;
                originalPosition = selectedObject.transform.position;
               // isInPan = selectedObject.transform.parent == fryingPanCollider.transform; // 检查甜甜圈是否在煎锅中
            }
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectedObject.transform.position = mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            if (fryingPanCollider.OverlapPoint(selectedObject.transform.position))
            {
                for (int i = 0; i < smallFryingPanColliders.Length; i++)
                {
                    if (smallFryingPanColliders[i].OverlapPoint(selectedObject.transform.position))
                    {
                        selectedObject.transform.position = drainCaps[i].transform.position;
                        //selectedObject.transform.SetParent(drainCaps[i].transform);
                        isInPan = true;
                        fryingTime = 0f; // 重置煎炸时间
                        currentDrainCap = drainCaps[i]; // 记录当前的 drainCap
                        break;
                    }
                }
            }
            else if (dishFriedDonutCollider.OverlapPoint(selectedObject.transform.position))
            {
                if (isInPan)
                {
                    selectedObject.transform.position = dishFriedDonutCollider.transform.position;
                    selectedObject.transform.SetParent(dishFriedDonutCollider.transform);
                }
                else
                {
                    selectedObject.transform.position = originalPosition;
                }
            }
            else
            {
                selectedObject.transform.position = originalPosition;
            }

            selectedObject = null;
        }

        if (isInPan)
        {
            fryingTime += Time.deltaTime;
            fryingTimerController.UpdateTimer(); // 更新计时器
            if (fryingTime >= fryingDuration)
            {
                isReady = true;
            }
        }

        if (isReady && currentDrainCap != null)
        {
            Renderer renderer = currentDrainCap.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = true;
            }
            isReady = false; // 重置 isReady 状态
        }
    }
}

using UnityEngine;

public class RollingPinController : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 initialPosition; // 存储擀面杖的初始位置
    private DonutManager donutManager; // 对 DonutManager 类的引用

    void Start()
    {
        initialPosition = transform.position; // 在游戏开始时记录擀面杖的初始位置
        donutManager = FindObjectOfType<DonutManager>(); // 获取 DonutManager 类的引用
    }

    private void OnMouseDown() // 鼠标按下
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag() // 鼠标持续按下
    {
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }

    private void OnMouseUp() // 鼠标松开
    {
        transform.position = initialPosition; // 将擀面杖的位置重置为初始位置
    }

    private void OnTriggerExit2D(Collider2D other) // 当 RollingPin 的 collider 和其他 collider 相撞后离开
    {
        if (other.CompareTag("SmallDough")) 
        {
            Debug.Log("Small dough is rolled flat!");
            Vector3 doughPosition = other.transform.position;
            Destroy(other.gameObject); 
            Instantiate(donutManager.flatDoughPrefab, doughPosition, Quaternion.identity); 
        }
    }
}
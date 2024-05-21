using UnityEngine;

public class BigMoldController : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 initialPosition; 
    private DonutManager donutManager; // 对 DonutManager 类的引用
    private bool isMolding = false; // 是否正在模具
    void Start()
    {
        initialPosition = transform.position; // 在游戏开始时初始位置
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
        isMolding = true; // 设置为正在模具
    }

    private void OnMouseUp() // 鼠标松开
    {
        transform.position = initialPosition; // 位置重置为初始位置
        /*isMolding = false;*/
    }

    private void OnTriggerExit2D(Collider2D other) //  collider 和其他 collider 相撞后离开
    {
        /*Debug.Log("Rolling pin exited collider!");*/
        if (other.CompareTag("FlatDough") && isMolding) 
        {
            Vector3 doughPosition = other.transform.position;
            Destroy(other.gameObject); 
            Instantiate(donutManager.BigSizeDonutDoughPrefab, doughPosition, Quaternion.identity); 
        }
    }
}

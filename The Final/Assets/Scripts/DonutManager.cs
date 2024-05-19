using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FixedPointArea
{
    public Collider2D placementArea; // 固定区域的 collider
    public Transform fixedPoint; // 固定点
}

public enum DoughState
{
    SmallDough,
    FlatDough
}

public class Dough
{
    public GameObject gameObject;
    public DoughState state;
}

public class DonutManager : MonoBehaviour
{
    public GameObject bigDoughPrefab; // 大面团 prefab
    public GameObject smallDoughPrefab; // 小面团 prefab
    public GameObject flatDoughPrefab; // 饼状面团 prefab
    public Texture2D defaultCursorTexture; // 默认光标纹理
    public List<FixedPointArea> fixedPointAreas; // 固定点及其固定区域的列表

    private Dough currentDough; // 当前的面团

    void Update()
    {
        HandleDoughClickAndPlacement();
    }

    private void HandleDoughClickAndPlacement()
    {
        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0) && currentDough == null)
        {
            // 通过射线检测获取点击位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            // 如果点击到了大面团
            if (hit.collider != null && hit.collider.gameObject == bigDoughPrefab)
            {
                // 实例化小面团
                currentDough = new Dough();
                currentDough.gameObject = Instantiate(smallDoughPrefab, hit.point, Quaternion.identity);
                currentDough.state = DoughState.SmallDough;
                // 将鼠标变成小面团
                Cursor.SetCursor(smallDoughPrefab.GetComponent<SpriteRenderer>().sprite.texture, Vector2.zero, CursorMode.Auto);
            }
        }

        // 移动小面团到固定区域
        if (currentDough != null && Input.GetMouseButton(0))
        {
            // 将小面团移动到鼠标位置
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            currentDough.gameObject.transform.position = mousePosition;
        }

        // 放置小面团到固定区域
        if (currentDough != null && Input.GetMouseButtonUp(0))
        {
            bool placed = false;

            // 检测是否放置到了任何固定区域
            foreach (var fixedPointArea in fixedPointAreas)
            {
                if (fixedPointArea.placementArea.OverlapPoint(currentDough.gameObject.transform.position))
                {
                    // 放置小面团到固定点
                    currentDough.gameObject.transform.position = fixedPointArea.fixedPoint.position;
                    // 重置鼠标样式
                    Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
                    placed = true;
                    break;
                }
            }

            if (!placed)
            {
                // 如果没有放置到任何固定区域，则销毁小面团
                Destroy(currentDough.gameObject);
                // 重置鼠标样式
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
            }

            currentDough = null;
        }
    }

    public void FlattenDough(GameObject dough)
    {
        // 实例化饼状面团，替换小面团
        Vector3 position = dough.transform.position;
        Quaternion rotation = dough.transform.rotation;
        Destroy(dough);
        GameObject flatDough = Instantiate(flatDoughPrefab, position, rotation);
        flatDough.GetComponent<Dough>().state = DoughState.FlatDough;
    }
}

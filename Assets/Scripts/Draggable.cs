using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 m_mousePos;
    private Vector2 m_diferent;

    private Vector3 m_originalPos;
    void GetMousePos()
    {
        m_mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        GetMousePos();
        m_diferent = (Vector2)(m_mousePos - transform.position);
        if (transform.parent != null && transform.parent.CompareTag("Slot"))
        {
            Node slot = transform.parent.GetComponent<Node>();
            slot.isOccupied = false;
            
            transform.SetParent(null); 
        }
    }
    private void OnMouseDrag()
    {
        GetMousePos();
        m_mousePos.z = 10f;
        transform.position = (Vector2)m_mousePos - m_diferent;
    }

    private void OnMouseUp()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Slot"))
            {
                Node node = hit.GetComponent<Node>();

                if (!node.isOccupied) // Nếu slot chưa có item
                {
                    transform.SetParent(hit.transform, false);
                    transform.localPosition = new Vector3(0,0,-2);
                    node.isOccupied = true;
                    return;
                }
            }
        }
        transform.position = m_originalPos;
    }
}

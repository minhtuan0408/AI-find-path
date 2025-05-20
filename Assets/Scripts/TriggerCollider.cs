using UnityEngine;

public class TriggerCollider : MonoBehaviour
{
    private Collider2D coll;

    private void Start()
    {
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Node"))
        {
            print("Hello");
        }
    }
}

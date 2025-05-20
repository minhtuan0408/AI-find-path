using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public GameObject Player;

    public GameObject Target;


    //public List<bool> isChecked;
    // HashSet


    Stack<GameObject> RoadStack = new Stack<GameObject>();
    Queue<GameObject> RoadQueue = new Queue<GameObject>();

    HashSet<GameObject> visited = new HashSet<GameObject>();

    public void Awake()
    {
        //print(Player.transform.localPosition);
    }

    public void Reset()
    {
        RoadStack.Clear();
        visited.Clear();
    }

    public void PlayerActive()
    {
        StartCoroutine(PlayerMove(RoadStack, 2f));
    }

    IEnumerator PlayerMove(Stack<GameObject> roads, float speed)
    {
        List<GameObject> path = new List<GameObject>(roads);
        path.Reverse();
        Node node = Player.GetComponentInParent<Node>();
        node.isOccupied = false;

        foreach (GameObject road in path)
        {
            Player.transform.SetParent(road.transform);
            Vector3 startPos = Player.transform.position;
            Vector3 endPos = road.transform.position;

            float length = Vector3.Distance(startPos, endPos);
            float startTime = Time.time;

            while (Vector3.Distance(Player.transform.position, endPos) > 0.01f)
            {
                float timeMove = (Time.time - startTime) * speed;
                float fracJourney = timeMove / length;
                Player.transform.position = Vector3.Lerp(Player.transform.position, endPos, fracJourney);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
        }
        yield return null;
    }
    public void DPS_Path()
    {
        bool found = DFS(Player, RoadStack, visited);

        if (!found)
        {
            Debug.Log("Không tìm thấy target");
        }
    }
    public bool DFS(GameObject player, Stack<GameObject> Roads, HashSet<GameObject> Visited)
    {
        // Lấy node chứa player
        Node node = player.GetComponentInParent<Node>();
        if (node == null || Visited.Contains(node.gameObject)) return false;

        // Đánh dấu đã thăm node và thêm vào đường đi
        Visited.Add(node.gameObject);
        Roads.Push(node.gameObject);


        GameObject isTarget; 

        if (node.gameObject.transform.childCount > 0)
        {
            isTarget = node.gameObject.transform.GetChild(0).gameObject;

            if (isTarget.gameObject.CompareTag("Obstacle"))
            {
                Roads.Pop();
                return false;
            }

            if (isTarget.gameObject.CompareTag("Target"))
            {
                Debug.Log("Tìm thấy target");

                foreach (var road in Roads)
                {
                    Debug.Log("Step " + road.name);
                }
                return true;
            }
        }

        List<Node> neighbors = new List<Node>();
        if (node.Right != null) neighbors.Add(node.Right);
        if (node.Left != null) neighbors.Add(node.Left);
        if (node.Up != null) neighbors.Add(node.Up);
        if (node.Down != null) neighbors.Add(node.Down);


        foreach (var neighbor in neighbors)
        {
            if (neighbor != null && !Visited.Contains(neighbor.gameObject))
            {
                if (DFS(neighbor.gameObject, Roads, Visited))
                    return true;
            }
        }


        print("Xây lại" + node.gameObject.name);
        Roads.Pop();
        return false;
    }

    public bool BFS(GameObject player, Queue<GameObject> Roads, HashSet<GameObject> Visited)
    {
        Node node = player.GetComponentInParent<Node>();

        Roads.Enqueue(node.gameObject);
        visited.Add(node.gameObject);

        while (Roads.Count > 0)
        {
            GameObject current = Roads.Dequeue();
            if (current.gameObject.transform.childCount > 0)
            {
                GameObject TagCheck = current.transform.GetChild(0).gameObject;
                if (TagCheck.gameObject.CompareTag("Target"))
                {
                    return true;
                }
                if (TagCheck.gameObject.CompareTag("Obstacle"))
                {
                    continue;
                }
            }
                
            node = player.GetComponentInParent<Node>();
            List<Node> neighbors = new List<Node>();
            if (node.Right != null) neighbors.Add(node.Right);
            if (node.Left != null) neighbors.Add(node.Left);
            if (node.Up != null) neighbors.Add(node.Up);
            if (node.Down != null) neighbors.Add(node.Down);


            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor.gameObject))
                {
                    visited.Add(neighbor.gameObject);
                    Roads.Enqueue(neighbor.gameObject);
                }
            }
        }
        return false;
    }

}

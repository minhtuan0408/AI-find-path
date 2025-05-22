using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;


public class GameManager : MonoBehaviour
{
    public GameObject Player;

    public GameObject Target;


    //public List<bool> isChecked;
    // HashSet


    Stack<GameObject> RoadStack = new Stack<GameObject>();
    Queue<GameObject> RoadQueue = new Queue<GameObject>();
    PriorityQueue<GameObject> RoadPriorityQueue = new PriorityQueue<GameObject>();
    Dictionary<GameObject, GameObject> cameFrom = new Dictionary<GameObject, GameObject>();
    HashSet<GameObject> visited = new HashSet<GameObject>();

    Dictionary<GameObject, int> costRoad = new Dictionary<GameObject, int>();
    List<GameObject> path = new List<GameObject>();

    public int costTotal;


    public int index = 0;

    public TextMeshProUGUI Nofication;
    
    public void Awake()
    {
        //print(Player.transform.localPosition);
    }

    public void Reset()
    {
        RoadStack.Clear();
        RoadQueue.Clear();
        cameFrom.Clear();
        visited.Clear();
        costRoad.Clear();
        path.Clear();
        costTotal = 0;
    }

    public void HandleInputData(int val)
    {
        if (val == 0)   index = 0;
        else if (val == 1) index = 1;
        else if (val == 2) index = 2;
   

    }

    public void Solve()
    {
        int a = index;
        switch (a)
        {
            case 0:
                DPS_Path();
                break;
            case 1:
                BFS_Path();
                break;
            case 2:
                UCS_Path();
                break;
        }
    }
    public void PlayerActive()
    {
        StartCoroutine(PlayerMove(RoadStack, 2f));
    }

    IEnumerator PlayerMove(Stack<GameObject> roads, float speed)
    {
        path = new List<GameObject>(roads);
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
            Nofication.text = "Không tìm thấy target - reset mảng để thử lại";
            Debug.Log("Không tìm thấy target");
        }
        else
        {
            Nofication.text = "";
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

            if (isTarget.gameObject.CompareTag("Target"))
            {
                Debug.Log("Tìm thấy target");

                foreach (var road in Roads)
                {
                    Debug.Log("Step " + road.name);
                }
                return true;
            }
            else if (isTarget.gameObject.CompareTag("Obstacle"))
            {
                Roads.Pop(); // bỏ
                return false;
            }
            else if (isTarget.gameObject.CompareTag("Riven"))
            {
                Roads.Pop(); 
                return false;
            }
            else if (isTarget.gameObject.CompareTag("Grass"))
            {
                Roads.Pop();
                return false;
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

    public bool BFS(GameObject player, Queue<GameObject> Roads, HashSet<GameObject> Visited, Dictionary<GameObject, GameObject> RealRoadToTarget)
    {
        Node node = player.GetComponentInParent<Node>();
        if (node == null)
        {
            Debug.LogWarning("Node hợp lệ.");
            return false;
        }
        Roads.Enqueue(node.gameObject);
        visited.Add(node.gameObject);
        RealRoadToTarget[node.gameObject] = null;

        while (Roads.Count > 0)
        {
            
            GameObject current = Roads.Dequeue();
            //Debug.Log("Duyệt" + current.name);
            if (current.gameObject.transform.childCount > 0)
            {
                GameObject TagCheck = current.transform.GetChild(0).gameObject;
                if (TagCheck.gameObject.CompareTag("Target"))
                {
                    return true;
                }
                else if (TagCheck.gameObject.CompareTag("Obstacle"))
                {
                    continue;
                }
                else if (TagCheck.gameObject.CompareTag("Riven"))
                {
                    continue;
                }
                else if (TagCheck.gameObject.CompareTag("Grass"))
                {
                    continue;
                }
            }
                
            node = current.GetComponentInParent<Node>();
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
                    RealRoadToTarget[neighbor.gameObject] = current;
                }
            }
        }
        return false;
    }

    public void BFS_Path()
    {
        bool found = BFS(Player, RoadQueue, visited, cameFrom);
        if (!found)
        {
            Nofication.text = "Không tìm thấy target - reset mảng để thử lại";
        }
        else 
        {
            Nofication.text = "";
        }

        Stack<GameObject> SampleRoad = new Stack<GameObject>();
        GameObject target = Target;
        GameObject current = target.transform.parent.gameObject;
        while (current != null)
        {
            SampleRoad.Push(current);
            current = cameFrom[current];
        }
        RoadStack = ReverseStack(SampleRoad);
        foreach (var road in RoadStack)
        {
            Debug.Log("Step " + road.name);
        }

   
    }

    Stack<GameObject> ReverseStack(Stack<GameObject> original)
    {
        var list = new List<GameObject>(original);
        return new Stack<GameObject>(list);
    }


    // Riven = 5 cost
    // Grass = 3 Cost
    public bool UCS(GameObject player, PriorityQueue<GameObject> Roads, HashSet<GameObject> visited, Dictionary<GameObject, GameObject> RealRoadToTarget, Dictionary<GameObject, int> costRoad)
    {
        Node node = player.GetComponentInParent<Node>();
        if (node == null)
        {
            Debug.LogWarning("Node không hợp lệ.");
            return false;
        }

        Roads.Enqueue(node.gameObject, 0);
        costRoad[node.gameObject] = 0;
        RealRoadToTarget[node.gameObject] = null;

        while (Roads.Count > 0)
        {
            GameObject current = Roads.Dequeue();

            if (current.transform.childCount > 0)
            {
                GameObject TagCheck = current.transform.GetChild(0).gameObject;
                if (TagCheck.CompareTag("Target"))
                {
                    return true;
                }
                else if (TagCheck.CompareTag("Obstacle"))
                {
                    continue;
                }
            }

            Node currentNode = current.GetComponentInParent<Node>();
            List<Node> neighbors = new List<Node>();
            if (currentNode.Right != null) neighbors.Add(currentNode.Right);
            if (currentNode.Left != null) neighbors.Add(currentNode.Left);
            if (currentNode.Up != null) neighbors.Add(currentNode.Up);
            if (currentNode.Down != null) neighbors.Add(currentNode.Down);

            foreach (var neighbor in neighbors)
            {
                GameObject neighborObj = neighbor.gameObject;

                if (visited.Contains(neighborObj)) continue;

                int stepCost = 0; 
                if (neighborObj.transform.childCount > 0)
                {
                    GameObject tagObj = neighborObj.transform.GetChild(0).gameObject;
                    if (tagObj.CompareTag("Obstacle")) continue;
                    if (tagObj.CompareTag("Riven")) stepCost = 5;
                    else if (tagObj.CompareTag("Grass")) stepCost = 3;
                }

                int newCost = costRoad[current] + stepCost;

                if (!costRoad.ContainsKey(neighborObj) || newCost < costRoad[neighborObj])
                {
                    costRoad[neighborObj] = newCost;
                    RealRoadToTarget[neighborObj] = current;
                    Roads.Enqueue(neighborObj, newCost);
                }
        }
            visited.Add(current);
        }
        return false;
    }

    public void UCS_Path() 
    {
        bool found = UCS(Player, RoadPriorityQueue, visited, cameFrom, costRoad);


        Stack<GameObject> SampleRoad = new Stack<GameObject>();
        GameObject target = Target;
        GameObject current = target.transform.parent.gameObject;
        while (current != null)
        {
            SampleRoad.Push(current);
            current = cameFrom[current];
        }
        RoadStack = ReverseStack(SampleRoad);
        foreach (var road in RoadStack)
        {
            Debug.Log("Step " + road.name);
        }

        GameObject start = Player.transform.parent.gameObject;
        GameObject end = Target.transform.parent.gameObject;
        if (costRoad.ContainsKey(end))
        {
            costTotal = costRoad[end];  
        }

        if (!found)
        {
            Debug.Log("Không tìm thấy target");
            Nofication.text = "Không tìm thấy target";
        }
        else
        {
            Nofication.text = "Tổng tiền : " + costTotal.ToString();
        }

        foreach (var road in RoadStack)
        {
            Debug.Log("Step " + road.name);
        }

    }
}

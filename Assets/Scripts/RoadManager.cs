using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using NUnit.Framework.Interfaces;

public class RoadManager : MonoBehaviour
{
    public static RoadManager Instance;
    public GameObject NodePrefab;
    public int rows;
    public int columns;
    public float spacing;

    private List<Node> nodes;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        if (NodePrefab == null)
        {
            Debug.LogError("Prefab is not assigned!");
            return;
        }

        nodes = new List<Node>();
        // Tính toán offset để canh giữa lưới
        float offsetX = (columns - 1) * spacing / 2f;
        float offsetY = (rows - 1) * spacing / 2f;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // x theo cột (trục hoành), y theo hàng (trục tung) — từ trái sang phải, từ dưới lên trên
                float x = col * spacing - offsetX;
                float y = row * spacing - offsetY;

                Vector3 spawnPos = new Vector3(x, y, 0);
                GameObject newnode = Instantiate(NodePrefab, spawnPos, Quaternion.identity, transform);
                newnode.name = $"Node {col} : {row}";

                Node nodeComponent = newnode.GetComponent<Node>();
                nodes.Add(nodeComponent);
            }
        }

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                int index = y * columns + x;
                Node currentNode = nodes[index];
                currentNode.SetNeighbors(nodes, x, y, columns, rows);
            }
        }
    }
}

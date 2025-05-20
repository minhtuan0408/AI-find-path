using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class Node : MonoBehaviour
{
    public Node Right;
    public Node Left;
    public Node Up;
    public Node Down;


    public bool isOccupied;

    public void SetNeighbors(List<Node> nodeList, int x, int y, int width, int height)
    {
        int index = y * width + x;


        if (x > 0)
        {
            Left = nodeList[index - 1];
        }
        if (x < width - 1)
        {
            Right = nodeList[index + 1];
        }
        if (y > 0)
        {
            Down = nodeList[index - width];
        }
        if (y < height - 1)
        {
            Up = nodeList[index + width];
        }
    }

}

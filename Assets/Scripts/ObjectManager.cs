using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    public GameObject ObstaclePrefab;
    public GameObject GrassPrefab;
    public GameObject RivenPrefab;

    public List<GameObject> ObstacleList;
    public List<GameObject> GrassList;
    public List<GameObject> RivenList;

    #region Text
    public TextMeshProUGUI ObstacleText;
    public TextMeshProUGUI GrassText;
    public TextMeshProUGUI RivenText;
    #endregion

    public Vector3 spawnPosition = new Vector3(0, 0, -2);
    public void AddObstaclePrefab()
    {
        
        GameObject obj = Instantiate(ObstaclePrefab, spawnPosition, Quaternion.identity);
        ObstacleList.Add(obj);
        ObstacleText.text = ObstacleList.Count.ToString();
    }

    public void RemoveObstaclePrefab()
    {
        if (ObstacleList.Count > 0)
        {
            GameObject lastObstacle = ObstacleList[ObstacleList.Count - 1];
            
            if (lastObstacle.transform.parent != null)
            {
                Debug.Log("Reset");
                Node resetNode = lastObstacle.transform.parent.GetComponent<Node>();
                resetNode.isOccupied = false;
            }

            Destroy(lastObstacle);
            ObstacleList.RemoveAt(ObstacleList.Count - 1);
            ObstacleText.text = ObstacleList.Count.ToString();
        }
    }

    public void AddRivenPrefab()
    {
        GameObject obj = Instantiate(RivenPrefab, spawnPosition, Quaternion.identity);
        RivenList.Add(obj);
        RivenText.text = RivenList.Count.ToString();
    }

    public void RemoveRivenPrefab()
    {
        if (RivenList.Count > 0)
        {
            GameObject lastObstacle = RivenList[RivenList.Count - 1];
            if (lastObstacle.transform.parent != null)
            {
                Debug.Log("Reset");
                Node resetNode = lastObstacle.transform.parent.GetComponent<Node>();
                resetNode.isOccupied = false;
            }

            Destroy(lastObstacle);
            RivenList.RemoveAt(RivenList.Count - 1);
            RivenText.text = RivenList.Count.ToString();
        }
    }

    public void AddGrassPrefab()
    {
        GameObject obj = Instantiate(GrassPrefab, spawnPosition, Quaternion.identity);
        GrassList.Add(obj);
        GrassText.text = GrassList.Count.ToString();
    }

    public void RemoveGrassPrefab()
    {
        if (GrassList.Count > 0)
        {
            GameObject lastObstacle = GrassList[GrassList.Count - 1];
            if (lastObstacle.transform.parent != null)
            {
                Debug.Log("Reset");
                Node resetNode = lastObstacle.transform.parent.GetComponent<Node>();
                resetNode.isOccupied = false;
            }

            Destroy(lastObstacle);
            GrassList.RemoveAt(GrassList.Count - 1);
            GrassText.text = GrassList.Count.ToString();
        }
    }
}

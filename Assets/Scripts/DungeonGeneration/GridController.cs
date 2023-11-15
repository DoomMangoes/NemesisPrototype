using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public Room room;

    [System.Serializable]
    public struct Grid
    {
        public float xSize, zSize;

        public float verticalOffset, horizontalOffset;
    }

    public Grid grid;

    public GameObject gridTile;

    public List<Vector3> availablePoints = new List<Vector3>();

    void Awake()
    {
        room = GetComponentInParent<Room>();
        grid.xSize = 25;
        grid.zSize = 25;
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        grid.verticalOffset += room.transform.localPosition.y;
        grid.horizontalOffset += room.transform.localPosition.x;

        for (int z = 0; z < grid.zSize; z++)
        {
            for (int x = 0; x < grid.xSize; x++)
            {
                GameObject go = Instantiate(gridTile, transform);
                go.GetComponent<Transform>().position = new Vector3(x - (grid.xSize - grid.horizontalOffset), 0, z - (grid.zSize - grid.verticalOffset));
                go.name = "X: " + x + ", Z:" + z;
                availablePoints.Add(go.transform.position);
                go.SetActive(false);
            }
        }
        GetComponentInParent<ObjectRoomSpawner>().IntializeObjectSpawning();
    }
}

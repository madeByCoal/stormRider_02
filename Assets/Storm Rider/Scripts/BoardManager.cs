using UnityEngine;
using System;
using System.Collections.Generic;       //Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.


public class BoardManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        public int minimum;             //Minimum value for our Count class.
        public int maximum;             //Maximum value for our Count class.

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int startCoordX;
    public int startCoordY;
    public int endCoordX;
    public int endCoordY;
    //private int PreLoadLenth;
    //private int PreLoadWidth;
    public GameObject[] floorTiles;                                 
    public GameObject[] outerWallTiles;                           
    private Transform boardHolder;
    private Dictionary<Vector3, GameObject> mapHolder;
    private Direction playerMoveDir;

    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        mapHolder = new Dictionary<Vector3, GameObject>();
        boardHolder = new GameObject("Board").transform;
    }

    void CalMapCoord()           //依摄像机可视范围计算map坐标范围
    {
        Vector3 vector_ScreenToWorldPoint_Max = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
        Vector3 vector_ScreenToWorldPoint_Min = Camera.main.ScreenToWorldPoint(Vector3.zero);
        int maxCoordX = Mathf.CeilToInt((vector_ScreenToWorldPoint_Max - Camera.main.transform.position).x);
        int maxCoordY = Mathf.CeilToInt((vector_ScreenToWorldPoint_Max - Camera.main.transform.position).y) * 2;            //行数为2倍 因为y轴移动坐标为一半


        startCoordX = Mathf.CeilToInt(vector_ScreenToWorldPoint_Min.x);
        startCoordY = Mathf.CeilToInt(vector_ScreenToWorldPoint_Min.y)*2;                  //行数为2倍 因为y轴移动坐标为一半
        endCoordX = Mathf.CeilToInt(vector_ScreenToWorldPoint_Max.x);
        endCoordY = Mathf.CeilToInt(vector_ScreenToWorldPoint_Max.y)*2;

        switch (playerMoveDir)
        {
            case Direction.NorthEast:
                endCoordX += Mathf.RoundToInt(maxCoordX / 2);
                endCoordY += Mathf.RoundToInt(maxCoordY / 2);
                break;
            case Direction.NorthWest:
                startCoordX -= Mathf.RoundToInt(maxCoordX / 2);
                endCoordY += Mathf.RoundToInt(maxCoordY / 2);
                break;
            case Direction.SouthEast:
                startCoordY -= Mathf.RoundToInt(maxCoordY / 2);
                endCoordX += Mathf.RoundToInt(maxCoordX / 2);
                break;
            case Direction.SouthWest:
                startCoordX -= Mathf.RoundToInt(maxCoordX / 2);
                startCoordY -= Mathf.RoundToInt(maxCoordY / 2);
                break;
            default:
                break;
        }
    }


    void MapGenerator(int xStart,int yStart, int xEnd,int yEnd)
    {
        for (int x = xStart; x <= xEnd; x++)
        {
            for (int y = yStart; y <= yEnd; y++)
            {
                GameObject toInstantiate = null;
                    
                Vector3 InstantiatePositon;
                //if (Mathf.Abs(x) == columns || Mathf.Abs(y) == rows)
                //{
                //    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                //}
                //else
                //{
                //    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                //}

                toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                if ((x % 2 == 0 && y % 2 == 0)||(x % 2 != 0 && y % 2 != 0))     //同为奇数或偶数
                {
                    InstantiatePositon = new Vector3(x, y/2f, 0f);
                    if (!mapHolder.ContainsKey(InstantiatePositon))
                    {
                        UpdateMapHolder(InstantiatePositon, toInstantiate);
                    }
                }
            }
        }
    }

    void UpdateMapHolder(Vector3 position,GameObject sourceTile)
    {
        GameObject instanceTile = null;
        instanceTile = Instantiate(sourceTile, position, Quaternion.identity) as GameObject;
        instanceTile.name = string.Concat(sourceTile.name,"_", position.x.ToString(),"_", position.y.ToString());
        instanceTile.transform.SetParent(boardHolder);
        mapHolder.Add(position, instanceTile);

    }


    //Vector3 RandomPosition()
    //{
    //    int randomIndex = Random.Range(0, gridPositions.Count);
    //    Vector3 randomPosition = gridPositions[randomIndex];
    //    gridPositions.RemoveAt(randomIndex);
    //    return randomPosition;
    //}

    //void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    //{
    //    int objectCount = Random.Range(minimum, maximum + 1);
    //    for (int i = 0; i < objectCount; i++)
    //    {
    //        Vector3 randomPosition = RandomPosition();
    //        GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
    //        Instantiate(tileChoice, randomPosition, Quaternion.identity);
    //    }
    //}

    public void UpdateMap(int level)
    {
        //get player's move dircetion
        //set,get lenth(distence) and width for map generation
        //check if any tile exisitence in new map coordinate 
        //gen map where is empty

           
        CalMapCoord();
            
        MapGenerator(startCoordX, startCoordY, endCoordX, endCoordY);
    }
    public void getPlayerDir(Direction dir)
    {
        playerMoveDir = dir;
    }

    public GameObject getTargetFloor(GameObject curFloor)
    {
        Vector3 targetFloorPosition = curFloor.transform.position;
        GameObject targetFloor = null;

        switch (playerMoveDir)
        {
            case Direction.North:
                targetFloorPosition = new Vector3(curFloor.transform.position.x, curFloor.transform.position.y + 1, curFloor.transform.position.z);
                break;
            case Direction.NorthEast:
                targetFloorPosition = new Vector3(curFloor.transform.position.x + 1, curFloor.transform.position.y + 0.5f, curFloor.transform.position.z);
                break;
            case Direction.East:
                targetFloorPosition = new Vector3(curFloor.transform.position.x + 2, curFloor.transform.position.y, curFloor.transform.position.z);
                break;
            case Direction.SouthEast:
                targetFloorPosition = new Vector3(curFloor.transform.position.x + 1, curFloor.transform.position.y - 0.5f, curFloor.transform.position.z);
                break;
            case Direction.South:
                targetFloorPosition = new Vector3(curFloor.transform.position.x, curFloor.transform.position.y - 1, curFloor.transform.position.z);
                break;
            case Direction.SouthWest:
                targetFloorPosition = new Vector3(curFloor.transform.position.x - 1, curFloor.transform.position.y - 0.5f, curFloor.transform.position.z);
                break;
            case Direction.West:
                targetFloorPosition = new Vector3(curFloor.transform.position.x - 2, curFloor.transform.position.y, curFloor.transform.position.z);
                break;
            case Direction.NorthWest:
                targetFloorPosition = new Vector3(curFloor.transform.position.x - 1, curFloor.transform.position.y + 0.5f, curFloor.transform.position.z);
                break;
            default:
                break;
        }

        foreach (Vector3 floorPosition in mapHolder.Keys)
        {
            if (floorPosition == targetFloorPosition && targetFloorPosition != curFloor.transform.position)
            {
                targetFloor = mapHolder[floorPosition];
            }
        }
        return targetFloor;
    }
}

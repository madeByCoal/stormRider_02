using UnityEngine;
using System;
using System.Collections.Generic;       //Allows us to use Lists.
using Random = UnityEngine.Random;      //Tells Random to use the Unity Engine random number generator.

namespace Completed

{

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

        private int startCoordX;
        private int startCoordY;
        private int endCoordX;
        private int endCoordY;
        //private int PreLoadLenth;
        //private int PreLoadWidth;
        public GameObject[] floorTiles;                                 
        public GameObject[] outerWallTiles;                           
        private Transform boardHolder;
        private Dictionary<Vector3, GameObject> mapHolder;

        void Awake()
        {
            mapHolder = new Dictionary<Vector3, GameObject>();
        }

        void CalMapRange()           //依摄像机可视范围计算map坐标范围
        {
            Vector3 vector_ScreenToWorld = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
            float clampCoordX_max = (vector_ScreenToWorld - Camera.main.transform.position).x;
            float clampCoordY_max = (vector_ScreenToWorld - Camera.main.transform.position).y;
            endCoordX = Mathf.FloorToInt(clampCoordX_max);
            endCoordY = Mathf.FloorToInt(clampCoordY_max) *2;            //行数为2倍 因为y轴移动坐标为一半
            startCoordX = -endCoordX;
            startCoordY = -endCoordY;
        }



        

        void MapGenerator(int xStart,int yStart, int xEnd,int yEnd)
        {
            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    GameObject toInstantiate = null;
                    GameObject instance = null;
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
                        instance = Instantiate(toInstantiate, InstantiatePositon, Quaternion.identity) as GameObject;
                        instance.name = string.Concat(toInstantiate.name, x.ToString(), y.ToString());
                        instance.transform.SetParent(boardHolder);
                        UpdateMapHolder(instance.transform.position, instance);
                        //mapHolder.Add(instance.transform.position, instance);
                    }
                }
            }
        }
        void UpdateMapHolder(Vector3 position,GameObject Tile)
        {
            if (!mapHolder.ContainsKey(position))
            {
                mapHolder.Add(position, Tile);
            }
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


        public void InitialiseMap(int level)
        {
            CalMapRange();
            boardHolder = new GameObject("Board").transform;
            MapGenerator(startCoordX,startCoordY,endCoordX,endCoordY);
        }

        public void UpdateMap(int level)
        {
            //get player's move dircetion
            //set,get lenth(distence) and width for map generation
            //check if any tile exisitence in new map coordinate 
            //gen map where is empty

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            //Vector3 moveDirection = player.GetComponent<SR_Character_Controller_4>().moveDirection;
            CalMapRange();
        }
    }
}
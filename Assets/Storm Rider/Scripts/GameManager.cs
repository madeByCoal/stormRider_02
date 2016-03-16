using UnityEngine;
using System.Collections;

namespace Completed
{
    using System.Collections.Generic;       //Allows us to use Lists. 

    public class GameManager : MonoBehaviour
    {

        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
        private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
        private int level = 1;                                  //Current level number, expressed in game as "Day 1".

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
            boardScript = GetComponent<BoardManager>();
            InitGame();
        }

        void InitGame()
        {
            boardScript.InitialiseMap(level);
        }


        void Update()
        {

        }
    }
}
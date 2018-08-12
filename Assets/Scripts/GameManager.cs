using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists.
using UnityEngine.UI;                   //Allows us to use UI.
using UnityEngine.SceneManagement;

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
        private int level = 1;                                  //Current level number, expressed in game as "Day 1".
        private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.
        private Text timerText;
        private CountDown myTimer;

        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)
                //if not, set instance to this
                instance = this;
            //If instance already exists and it's not this:
            else if (instance != this)
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Get a component reference to the attached BoardManager script
            //boardScript = GetComponent<BoardManager>();

            //Call the InitGame function to initialize the first level
            InitGame();
        }

        //This is called each time a scene is loaded.
        void OnLevelWasLoaded(int index)
        {
            //Add one to our level number.
            level++;
            //Call InitGame to initialize our level.
            InitGame();
        }

        //Initializes the game for each level.
        void InitGame()
        {
            //While doingSetup is true the player can't move, prevent player from moving while title card is up.
            doingSetup = true;
            myTimer = GameObject.Find("TimerText").GetComponent<CountDown>();
            myTimer.StartTimer();

            //timerText.text = "Timer :" + level;
            //Call the SetupScene function of the BoardManager script, pass it current level number.
            //boardScript.SetupScene(level);
        }

        //Update is called every frame.
        void Update()
        {
            //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
        }

        public void Finished(string message) {
            myTimer.StopTimer();
        }

        public void GameOver(string message) {
            Invoke("ReloadLevel", 3f);
        }

        private void ReloadLevel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
    }
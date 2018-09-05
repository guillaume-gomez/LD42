using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists.
using UnityEngine.UI;                   //Allows us to use UI.
using UnityEngine.SceneManagement;

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
        private int level = 1;                                  //Current level number, expressed in game as "Day 1".
        public float levelStartDelay = 3.0f;
        public bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.
        public bool hasInvertedInput = false;
        public bool isTransiting = false;
        public AudioClip winSound;
        public AudioClip loseSound;
        private Text timerText;
        private GameObject beforeStartCanvas;
        private GameObject invertedInputCanvas;
        private CountDown myTimer;
        private LayerTypeEnum currentLayerType;
        private GameObject playerRef;
        private int nbRetry = 0;
        private GamePersistingData gamePersistingDataScript;


        private const int nbLevels = 4;
        private const float timerStep = 5f;

        public int Level {
            get
            {
                return instance.level;
            }
            set
            {
                instance.level = value;
            }
        }

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

            gamePersistingDataScript = GetComponent<GamePersistingData>();
            // UNCOMMENT THOSE TWO LINES TO TEST YOUR SCENE AS STANDALONE
            //playerRef = GameObject.FindGameObjectsWithTag("Player")[0];
            //InitGame();
        }

        void OnEnable()
        {
            //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnDisable()
        {
            //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        //This is called each time a scene is loaded.
        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)//OnLevelWasLoaded(int index)
        {
            // hardcoded index about credit and main menu
            if(scene.buildIndex != 0 && scene.buildIndex != (nbLevels + 3)) {
                playerRef = GameObject.FindGameObjectsWithTag("Player")[0];
                invertedInputCanvas = GameObject.Find("InputGlitchInfo");
                //Call InitGame to initialize our level.
                InitGame();
            } else {
                //That means we go back in the main menu
                SoundManager.instance.StopMusic();
            }
        }

        //Initializes the game for each level.
        void InitGame()
        {
            //While doingSetup is true the player can't move, prevent player from moving while title card is up.
            doingSetup = true;
            myTimer = GameObject.Find("TimerText").GetComponent<CountDown>();

            beforeStartCanvas = GameObject.Find("BeforeStartCanvas");
            invertedInputCanvas = GameObject.Find("InputGlitchInfo");

            float startDelay = (nbRetry >= 1) ? 0.01f : levelStartDelay;

            Invoke("HideBeforeStartCanvas", startDelay);
            Invoke("HideInputGlitchInfo", 0.001f);

            playerRef.SetActive(true);

        }

        private void HideBeforeStartCanvas()
        {
            beforeStartCanvas.SetActive(false);
            doingSetup = false;

            StartGame();
        }

        private void HideInputGlitchInfo() {
            invertedInputCanvas.SetActive(false);
        }

        void StartGame() {
            //add specific stuff just at the beginning of the level
            myTimer.StartTimer();
        }

        //Update is called every frame.
        void Update()
        {
        }

        private void ReloadLevel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
            SoundManager.instance.PlayMusic();
        }

        private void LoadNextLevel() {
            isTransiting = false;
            SoundManager.instance.PlayMusic();
            if(level + 1 > nbLevels) {
                // Go back end credits
                SceneManager.LoadScene(nbLevels + 3);
            } else {
                gamePersistingDataScript.Save();
                //Add one to our level number.
                level++;
                //hardcoded 2 represent the nb scenes before the first level
                SceneManager.LoadScene(2 + level);
            }
        }


        public void Finished(string message) {
            if(!doingSetup) {
                doingSetup = true;
                nbRetry = 0;
                SoundManager.instance.StopMusic();
                SoundManager.instance.PlaySingle(winSound);
                myTimer.StopTimer();
                isTransiting = true;
                Invoke("LoadNextLevel", 3f);
            }
        }

        public void GameOver(string message) {
            if(!doingSetup) {
                doingSetup = true;
                SoundManager.instance.StopMusic();
                SoundManager.instance.PlaySingle(loseSound);
                //invertedInputCanvas.SetActive(true);
                myTimer.StopTimer();
                playerRef.SetActive(false);
                doingSetup = true;
                nbRetry = nbRetry + 1;
                Invoke("ReloadLevel", 3f);
            }
        }

        public void AddTime() {
            myTimer.AddTime(timerStep);
        }

        public void ReduceTime() {
            myTimer.AddTime(-timerStep);
        }

        public void InvertedInput(float timer) {
            hasInvertedInput = true;
            invertedInputCanvas.SetActive(true);
            Invoke("BackToNormalInput", timer);
            Invoke("DisableInvertedInputCanvas", 4.8f);
        }

        public void SetPlayerLayer(LayerTypeEnum layerType, uint layerIndex) {
            if(currentLayerType != layerType)
            {
                SoundManager.instance.PlayAndSwitchMusic(layerType);
            }
            currentLayerType = layerType;
        }

        private void BackToNormalInput() {
            hasInvertedInput = false;
        }

        private void DisableInvertedInputCanvas() {
            invertedInputCanvas.SetActive(false);
        }
    }
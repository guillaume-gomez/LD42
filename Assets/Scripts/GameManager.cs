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
        public AudioClip winSound;
        public AudioClip loseSound;
        private Text timerText;
        private GameObject beforeStartCanvas;
        private GameObject invertedInputCanvas;
        private CountDown myTimer;
        private LayerTypeEnum currentLayerType;
        private GameObject playerRef;

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
            playerRef = GameObject.FindGameObjectsWithTag("Player")[0];
            //InitGame();
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

            beforeStartCanvas = GameObject.Find("BeforeStartCanvas");

            invertedInputCanvas = GameObject.Find("InputGlitchInfo");
            invertedInputCanvas.SetActive(false);
            Invoke("HideBeforeStartCanvas", levelStartDelay);

            playerRef.SetActive(true);

        }

        private void HideBeforeStartCanvas()
        {
            beforeStartCanvas.SetActive(false);
            doingSetup = false;

            StartGame();
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

        public void Finished(string message) {
            SoundManager.instance.PlaySingle(winSound);
            myTimer.StopTimer();
        }

        public void GameOver(string message) {
            doingSetup = true;
            SoundManager.instance.StopMusic();
            SoundManager.instance.PlaySingle(loseSound);
            playerRef.SetActive(false);
            Invoke("ReloadLevel", 3f);
        }

        public void AddTime(float value) {
            myTimer.AddTime(value);
        }

        public void InvertedInput(float timer) {
            hasInvertedInput = true;
            invertedInputCanvas.SetActive(true);
            Invoke("BackToNormalInput", timer);
            Invoke("DisableInvertedInputCanvas", 2.0f);
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
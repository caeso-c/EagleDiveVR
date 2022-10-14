using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameLogic : MonoBehaviour {

    public GameObject mainGameCamera;

    public bool hasVR_headset;
    private GameObject controller_L;
    private GameObject controller_R;

    public float thrust; //eagle moving speed
    public Transform SphereTransform; //Player object in hierarchy
    //note Game Mode 5 is now win condition,Mode 6 is lose condition, beat eagles (5) and bag knockdown (3) disabled.
    public int gameMode=0; //5 - beat eagles, 4-fishing, 3-knock down bags

    public GameObject lookTargetObj; // target so that Checkpoint script can orient UI elements towards viewer

    public GameObject BonusHolder; //bonus gameobject
    public TextMesh Points_Text; //for printing points
    public TextMesh Bonus_Text; //for printing bonus text
    public GameObject Eagle; //using for eagle animations
    public Animation EagleAnim; //current eagle animation
    private float TimeWings = 0; //play animated wings every 5 seconds

    public bool EagleFlight = false; //cap
    private float TimeFlupps = 0; //switching eagle animations
    public bool PlayFlupps = false; //play eagle fly animation

    public Text Global_Messages;
    public TextMesh LimitsAlert; //Out of map message
    public bool GL_Mes_View = false; //cap
    private bool Start_Mes_View = false; //play fadeout anim after last waypoint in every game modes
    private bool End_Mes_View = false; //play fadeout anim after last waypoint in every game modes
    private float GL_Mes_time = 0; //smooth fade out screen. After delay time for showing next message 

    public AudioManager gameAudioMgr;

    private float EagleSoundTime; //for random time sound
    public Transform[] SpawnZones; //for telepotation after quest ended
    private bool moving = true;
    public GameObject[] EagleBot; //Eagle bot
    public bool ShowEnding=false;
    public Image LogoImage;
    public GameObject[] Lines;
    public Transform ParentObject; //PlayerObject
    public GameObject PanelFading; //using for mobile VR smoothly fade in or out screen

    ////////////////////////////////////////////////////////////Life and level control

    public bool impacted = false;
    public GameObject Scenemanager; //see documentation

    ////////////////////////////////////////////////////////////PAUSE AND MAIN MENU

    private bool paused=false;
    public RectTransform PausePanel;
    public RectTransform WinPanel;
    public Image Cursor;
    private bool loadingState = false; //if user choosed option restart or go to main menu and eagle did not respond to a collision

    //////////////////////////////////////////////////////////Control type

    private bool clicked = false; //if mouse button down
    private float SpInc = 18; //increase speed by every intensity clicks, default value gets from thrust value
    private bool cooldown = false;
    private bool RandomizeFlups = true; //if user not click then activate time of autoflaps
    public bool gotItem = false; //if player has got item then eagle not react on user clicks for speed increases while player doesn't use item

    ////////////////////////////////////////////////////////////////For this 3 game modes

    public int BagsLeft = 6; //count of bags by default use in inspector window
    public int[] BotsCounts; //contains eaglebot points (fishes) the value gets from EagleZone script
    public int FishCount; //contains player eagle points when player gets the fish
    private bool Notify = false; //cap for black screen fade animation
    private float NotifyTime = 0; //time when black screen animation will not active
    public GameObject FishZone; //Fish zone spawner
    public GameObject BotsPointsPanel;//Mainleft side points panel
    public GameObject[] Counts, TargetBotsFishModes; //TextMesh for left side points of player eagle | TargetBotsFishModes - animated targets for 2 eagle BotHolders
    public GameObject[] TapMessages; //Help tap message
    public int EaglesBeated = 2; //how much eaglebots have to beat
    private GameObject[] fishes; //for 5th gamemode we remove unused every fishes functions


    void Awake () {
        
        Debug.Log("Awake called!");
        EagleAnim = Eagle.GetComponent<Animation>();
        SpInc = thrust;
        Scenemanager = GameObject.Find("SCENEMANAGER");
        if (Scenemanager == null) return;
        gameMode = Scenemanager.GetComponent<SceneGM>().GameMode;
        gameAudioMgr = GameObject.Find("/AudioManager").GetComponent<AudioManager>();
        if (mainGameCamera == null)
            {}
        PausePanel.transform.Find("Panel").gameObject.SetActive(false);
        //this will play game loop and Ambience
        gameAudioMgr.StartGameAmbience();


        //lock the cursor?
        CursorLockMode currentCursor;
        currentCursor = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = false;
        Screen.lockCursor=true;


    }

    void Start()
    {
        if(hasVR_headset == false)
        {
            Debug.Log("This should print!");
            Debug.Log(mainGameCamera.name);
            mainGameCamera.transform.parent.Find("LeftHand Controller").gameObject.SetActive(false);
            mainGameCamera.transform.parent.Find("RightHand Controller").gameObject.SetActive(false);

        }
        else
        {
            Debug.Log("This should not print!");
        }

        Global_Messages.gameObject.SetActive(true);
    }


    void Update()
    {
    	///Not currently being used - only Mode 4 working (3 is actually broken currently )

        if (gameMode == 3) //using for this asset as starting game mode
        {

            Global_Messages.text = "Knock down bags";

            if (!Start_Mes_View)
            {
#if UNITY_ANDROID
                PanelFading.SetActive(true); //playing fading out animation of panel    //See documentation
                PanelFading.GetComponent<Animator>().Play("PanelFalling", -1, 0f);      //See documentation
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                
#endif
                
                Global_Messages.gameObject.SetActive(false);
                if (Scenemanager != null)
                    Scenemanager.GetComponent<SceneGM>().GameMode = gameMode;
                Lines[0].GetComponent<TrailRenderer>().Clear();
                Lines[1].GetComponent<TrailRenderer>().Clear();
                Lines[0].SetActive(false);
                Lines[1].SetActive(false);
                StartCoroutine(SpawnOnPosition(2.0F, 0));
                Start_Mes_View = true;

                Debug.Log("When does this print?");
            }

            if (GL_Mes_View)
                GL_Mes_time += Time.deltaTime;

            if (GL_Mes_time >= 3.5f && GL_Mes_View)
            {
                GL_Mes_time = 0;
                GL_Mes_View = false;
            }

            if (!GL_Mes_View && !End_Mes_View)
            {

                Global_Messages.gameObject.SetActive(true);
                //Global_Messages.GetComponent<Animator>().Play("FadeIn_Text", -1, 0f);
                NotifyTime = 0;
                Notify = true;
                End_Mes_View = true;

            }

            //**********************************************************************

            if (NotifyTime < 5) //waiting 5 seconds
            {
                NotifyTime += Time.deltaTime;
            }
            else

                if (NotifyTime >= 5 && Notify)
            {
#if UNITY_ANDROID
                PanelFading.SetActive(false); //See documentation
#endif
                //Global_Messages.gameObject.SetActive(false);

                Notify = false;

            }

            if (BagsLeft <= 0)
            {
                Start_Mes_View = false;
                GL_Mes_View = true;
                End_Mes_View = false;
                NotifyTime = 0;
                //gameMode = 4;
            }

            Points_Text.text = "Bags left: " + BagsLeft.ToString();

        }

//////////////////////////////////////////////////////
///// This is currently the only game mode used
///////////////////////////////////////////////////////

        if (gameMode == 4) //using for level 2 Fishing Game
        {

            Global_Messages.text = "Catch 10 fish before the others!";

            if (!Start_Mes_View)
            {
#if UNITY_ANDROID
                PanelFading.SetActive(true); //playing fading out animation of panel    //See documentation
                PanelFading.GetComponent<Animator>().Play("PanelFalling", -1, 0f);      //See documentation
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                //GetComponent<Animation>().Play(); //See documentation
#endif
                //Global_Messages.gameObject.SetActive(false);
                if (Scenemanager != null)
                    Scenemanager.GetComponent<SceneGM>().GameMode = gameMode;
                Lines[0].GetComponent<TrailRenderer>().Clear();
                Lines[1].GetComponent<TrailRenderer>().Clear();
                Lines[0].SetActive(false);
                Lines[1].SetActive(false);
                StartCoroutine(SpawnOnPosition(0.1F, 0));
                Start_Mes_View = true;
            }

            if (GL_Mes_View)
                GL_Mes_time += Time.deltaTime;

            if (GL_Mes_time >= 3.5f && GL_Mes_View)
            {
                GL_Mes_time = 0;
                GL_Mes_View = false;
            }

            if (!GL_Mes_View && !End_Mes_View)
            {

                //start music
                if (gameAudioMgr != null)
                {
                    gameAudioMgr.StartGameMusic();
                    if (gameAudioMgr.menuLoop2D!= null)
                        gameAudioMgr.menuLoop2D.Stop();
                }

                Global_Messages.gameObject.SetActive(true);
                //Debug.Log("Playing Fade In text Animation!");
                //Global_Messages.GetComponent<Animator>().Play("FadeIn_Text", -1, 0f);
                NotifyTime = 0;
                Notify = true;
                End_Mes_View = true;

            }

            //**********************************************************************

            if (NotifyTime < 5) //waiting 5 seconds
            {
                NotifyTime += Time.deltaTime;
            }
            else

                if (NotifyTime >= 5 && Notify) 
            {
                EagleBot[0].SetActive(true); //eaglebots active
                EagleBot[1].SetActive(true);
                TargetBotsFishModes[0].SetActive(true); //active they targets with animations
                TargetBotsFishModes[1].SetActive(true);
                BonusHolder.SetActive(true); //eagleplayer interface
                BotsPointsPanel.SetActive(true);
                Counts[0].SetActive(true);
                Counts[1].SetActive(true);

                FishZone.SetActive(true);
#if UNITY_ANDROID
                PanelFading.SetActive(false); //See documentation
#endif
                //Global_Messages.gameObject.SetActive(false);

                Notify = false;

            }

            if (FishCount >= 10)
            {
                Start_Mes_View = false;
                GL_Mes_View = true;
                End_Mes_View = false;
                BonusHolder.SetActive(false);
                BotsPointsPanel.SetActive(false);
                Counts[0].SetActive(false);
                Counts[1].SetActive(false);
                FishZone.GetComponent<FishLogic>().enabled = false;
                NotifyTime = 0;
                FishCount = 0;
                //game Mode 5 == You Win!
                gameMode = 5;

            }

            Points_Text.text = "Fish: " + FishCount.ToString();
            Counts[0].GetComponent<TextMesh>().text = BotsCounts[0].ToString();
            Counts[1].GetComponent<TextMesh>().text = BotsCounts[1].ToString();

            //if the eagles beat you, you lose (Game Mode = 6)
            if (BotsCounts[0] >= 10 || BotsCounts[1] >=10)
            {
            	gameMode = 6;
            }

        }

        //if you win
        if (gameMode == 5)
        {

            thrust = 0;

            Eagle.SetActive(false);
            SphereTransform.GetComponent<SmoothLookAT>().enabled = false;

            //we have to use two different methods to deactivate certain UI elements because of platform differences
            //we also don't need to reset rotation if we're not in VR
            if (SphereTransform.name == "Player")
            {
                SphereTransform.Find("CanvasMOBILE").gameObject.SetActive(false);
            }
            else
            {
                SphereTransform.Find("Player").gameObject.SetActive(false);
                SphereTransform.rotation = Quaternion.identity;
            }

            if (!WinPanel.gameObject.activeInHierarchy)
            {
                WinPanel.gameObject.SetActive(true);
                WinPanel.transform.GetChild(0).GetComponent<Text>().text = "Congratulations, You Win!!";
                gameAudioMgr.WinGame();
                if(gameAudioMgr.eagleWingSource != null)
                    gameAudioMgr.eagleWingSource.Stop();
                if (gameAudioMgr.gameLoop2D != null)
                    gameAudioMgr.gameLoop2D.Stop();
                //PauseGame(true);
            }

        }

        if (gameMode == 6)
		{	
			
			thrust = 0;
            Eagle.SetActive(false);
            SphereTransform.GetComponent<SmoothLookAT>().enabled = false;
            //we have to use two different methods to deactivate certain UI elements because of platform differences
            //we also don't need to reset rotation if we're not in VR
            if (SphereTransform.name == "Player")
            {
                SphereTransform.Find("CanvasMOBILE").gameObject.SetActive(false);
            }
            else
            {
                SphereTransform.Find("Player").gameObject.SetActive(false);
				SphereTransform.rotation = Quaternion.identity;
            }


            if (!WinPanel.gameObject.activeInHierarchy)
            {
            	WinPanel.gameObject.SetActive(true);
            	WinPanel.transform.GetChild(0).GetComponent<Text>().text = "Sorry, You Lose! Try Again?";
				gameAudioMgr.LoseGame();
				if(gameAudioMgr.eagleWingSource != null)
					gameAudioMgr.eagleWingSource.Stop();
				if(gameAudioMgr.gameLoop2D != null)
					gameAudioMgr.gameLoop2D.Stop();
                //PauseGame(true);
            }

        }

//        if (gameMode == 5) //using for level 2
//        {
//
//            Global_Messages.text = "Beat 2 eagles";
//
//            if (!Start_Mes_View)
//            {
//#if UNITY_ANDROID
//                PanelFading.SetActive(true); //playing fading out animation of panel    //See documentation
//                PanelFading.GetComponent<Animator>().Play("PanelFalling", -1, 0f);      //See documentation
//#endif
//
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//                //GetComponent<Animation>().Play(); //See documentation
//#endif
//                //MusicHolder.GetComponent<Animation>().Play("FadeOutMusic"); //see documentation
//
//                fishes = GameObject.FindGameObjectsWithTag("Fish"); //delete non used fish functions
//                foreach (GameObject fish in fishes)
//                {
//                    Destroy(fish.GetComponent<Collider>()); //remove collider
//                    fish.GetComponent<FishLogic>().Canv.SetActive(false); //hide circle points
//                }
//
//                Global_Messages.gameObject.SetActive(false);
//                if (Scenemanager != null)
//                    Scenemanager.GetComponent<SceneGM>().GameMode = gameMode;
//                Lines[0].GetComponent<TrailRenderer>().Clear();
//                Lines[1].GetComponent<TrailRenderer>().Clear();
//                Lines[0].SetActive(false);
//                Lines[1].SetActive(false);
//                StartCoroutine(SpawnOnPosition(2.0F, 0));
//                Start_Mes_View = true;
//            }
//
//            if (GL_Mes_View)
//                GL_Mes_time += Time.deltaTime;
//
//            if (GL_Mes_time >= 3.5f && GL_Mes_View)
//            {
//                GL_Mes_time = 0;
//                GL_Mes_View = false;
//            }
//
//            if (!GL_Mes_View && !End_Mes_View)
//            {
//                //MusicHolder.GetComponent<AudioSource>().Stop();
//                //MusicHolder.GetComponent<AudioSource>().clip = BK_Music[0];
//                //MusicHolder.GetComponent<AudioSource>().Play();
//                //MusicHolder.GetComponent<Animation>().Play("FadeInMusic"); //see documentation
//                //GetComponent<AudioSource>().PlayOneShot(Notification, 1.5f);
//                Global_Messages.gameObject.SetActive(true);
//                Global_Messages.GetComponent<Animator>().Play("FadeIn_Text", -1, 0f);
//                NotifyTime = 0;
//                Notify = true;
//                End_Mes_View = true;
//
//            }
//
//            //**********************************************************************
//
//            if (NotifyTime < 5) //waiting 5 seconds
//            {
//                NotifyTime += Time.deltaTime;
//            }
//            else
//
//                if (NotifyTime >= 5 && Notify)
//            {
//                EagleBot[0].SetActive(true);
//                EagleBot[1].SetActive(true);
//                EagleBot[0].GetComponent<EagleBotLogic>().changeMaterial = true; //change eaglebot Trail materials
//                EagleBot[1].GetComponent<EagleBotLogic>().changeMaterial = true; //change eaglebot Trail materials
//                TargetBotsFishModes[0].SetActive(true);
//                TargetBotsFishModes[1].SetActive(true);
//
//#if UNITY_ANDROID
//                PanelFading.SetActive(false); //See documentation
//#endif
//                Global_Messages.gameObject.SetActive(false);
//
//                Notify = false;
//
//            }
//
//            if (EaglesBeated <= 0)
//            {
//                ShowEnding = true;
//                gameMode = 6; //cap for next upgrades
//            }
//
//            Points_Text.text = "Eagles left: " + EaglesBeated.ToString();
//            Counts[0].GetComponent<TextMesh>().text = BotsCounts[0].ToString();
//            Counts[1].GetComponent<TextMesh>().text = BotsCounts[1].ToString();
//
//        }


        EagleSoundTime += Time.deltaTime;
        if (EagleSoundTime >= 12 && gameMode < 5)            //randomize eagle sounds
        {
            gameAudioMgr.RndEagleSound(Eagle.transform.position);
            EagleSoundTime = 0;
        }

            if (EagleFlight)
            {
                TimeWings += Time.deltaTime;
                if (TimeWings >= 5)
                {
                    TimeWings = 0;
                    EagleFlight = false;
                }
            }

            if (ShowEnding)
            {
                if (!GetComponent<Animation>().isPlaying)
                {
                    Lines[0].SetActive(false);
                    Lines[1].SetActive(false);
                    Points_Text.gameObject.SetActive(false);
                    Bonus_Text.gameObject.SetActive(false);
                    BonusHolder.gameObject.SetActive(false);
                    //GetComponent<Animation>().Play("Ending"); //See documentation
                    MainMenu(); //back to main menu
                    LogoImage.gameObject.SetActive(true);
                    ShowEnding = false;
                }
            }

            if (impacted)
        {
            impacted = false;
        }

        if (clicked)
        {
            if (!cooldown)
            {
                thrust = SpInc * 1.5f; //convert to eagle speed
                TimeFlupps = 10;
            }
            if (cooldown)
            {
                thrust -= (SpInc / 2) * Time.deltaTime;
                if (thrust <= 18)
                {
                    thrust = 10;
                    SpInc = thrust;
                    RandomizeFlups = true;
                    TimeFlupps = 0;
                    clicked = false;
                }
            }

        }

        if (!gotItem) //if player doesn't hold item eagle will accelerate by pressing mouse button or tap
        {

        }
    }


    void PauseGame(bool pauseState)
    {
		if (pauseState) { Cursor.gameObject.SetActive(true); Time.timeScale = 0; }
        if (!pauseState) { Cursor.gameObject.SetActive(false); Time.timeScale = 1; }
    }


    void FixedUpdate()
    {

            if (moving)
            {

                if(SphereTransform != null && Camera.main !=null)
                    SphereTransform.transform.position += Camera.main.transform.forward * thrust * Time.deltaTime;
                    //Sets location of eagleWingSource game object to the eagle's position
                    if (gameAudioMgr.eagleWingSource != null)
                    	gameAudioMgr.eagleWingSource.transform.gameObject.transform.position = Eagle.transform.position;
                    else{}
        	}

                
                if (RandomizeFlups)
        TimeFlupps += Time.deltaTime;

        if (TimeFlupps >= 10)
        {
            EagleAnim["Armature|Fly_main"].speed = 2;
            EagleAnim.CrossFade("Armature|Fly_main");
            EagleFlight = true;
            if (gameAudioMgr.eagleWingSource != null)
            {
                gameAudioMgr.eagleWingSource.loop = true;
                if (!gameAudioMgr.eagleWingSource.isPlaying && gameMode < 5)
                     gameAudioMgr.eagleWingSource.Play();

            }
            TimeFlupps = 0;
        }
        else
        if (!EagleFlight)
        {
            EagleAnim["Armature|Free_fly"].speed = 2;
            EagleAnim.CrossFade("Armature|Free_fly");
            if (gameAudioMgr.eagleWingSource != null)
                gameAudioMgr.eagleWingSource.Stop();
        }
    }

   public IEnumerator SpawnOnPosition(float waitTime, int idxNum)
    {
        yield return new WaitForSeconds(waitTime);
        ParentObject.position = SpawnZones[idxNum].position;

        moving = false;
        UnityEngine.XR.InputTracking.Recenter();
        yield return new WaitForSeconds(.1f);
        moving = true;
        Lines[0].SetActive(true);
        Lines[1].SetActive(true);

    }

    public void Restart() //Restart the level
    {
#if UNITY_ANDROID
        PanelFading.gameObject.SetActive(true); //play fade out animation of panel  //See documentation
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //GetComponent<Animation>().Play(); //See documentation
#endif
		PauseGame(false);
        int currLV = SceneManager.GetActiveScene().buildIndex; //get current level number
        StartCoroutine(Option(1, currLV));


    }

    public void GameActions(string action)
    { 
        switch(action)
        {
            case"TestWin":
            gameMode = 5;
            break;
            case"TestLose":
            gameMode = 6;
            break;
            case"Pause":
                //Debug.Log("Pausing Game!");
                paused = !paused;
                PauseGame(paused);
                //PausePanel.gameObject.SetActive(paused);
                PausePanel.transform.Find("Panel").gameObject.SetActive(paused);
                WinPanel.transform.GetChild(0).GetComponent<Text>().text = "Game Paused";
                if (gameAudioMgr != null && gameAudioMgr.eagleWingSource != null)
                    gameAudioMgr.eagleWingSource.mute = paused;
            break;
            case"PlayFishing":
            Restart();
            break;
            case"ResetToMenu":
            break;
        }
    }

    public void ChangeThrust()
    {
        thrust += 5;
        if(thrust > 15)
            thrust = 0;
    }


    public void MainMenu() //Back to main menu
    {
#if UNITY_ANDROID
        PanelFading.gameObject.SetActive(true); //play fade out animation of panel  //See documentation
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //GetComponent<Animation>().Play(); //See documentation
#endif
       

        gameAudioMgr.fadeGameMusic.TransitionTo(1.0f);
        gameMode = 0;
        Scenemanager.GetComponent<SceneGM>().GameMode = gameMode;
        StartCoroutine(Option(1, 0));
    }

    public IEnumerator Option(float waitTime, int LV_idx) //restart or quit to main menu option where waitTime - waiting while panel smoothly fade and LV_idx - level index
    {
        Time.timeScale = 1;
        loadingState = true;
        yield return new WaitForSeconds(waitTime);
        if(LV_idx == 0)
            gameAudioMgr.RestartMenu();

        SceneManager.LoadScene(LV_idx);
    }

    public IEnumerator ResetByFall(float waitTime, int idxNum)
    {
        Lines[0].GetComponent<TrailRenderer>().Clear();
        Lines[1].GetComponent<TrailRenderer>().Clear();
        Lines[0].SetActive(false);
        Lines[1].SetActive(false);

#if UNITY_ANDROID
        PanelFading.gameObject.SetActive(true); //play fade out animation of panel  //See documentation
        PanelFading.GetComponent<Animator>().Play("PanelFalling", -1, 0f);//See documentation
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //GetComponent<Animation>().Play(); //See documentation
#endif

        ParentObject.position = SpawnZones[idxNum].position;

        moving = false;
        UnityEngine.XR.InputTracking.Recenter();
        yield return new WaitForSeconds(.3f);
        if (!loadingState)
        {
            int currLV = SceneManager.GetActiveScene().buildIndex; //get current level number
            SceneManager.LoadScene(currLV);
        }
    }
}

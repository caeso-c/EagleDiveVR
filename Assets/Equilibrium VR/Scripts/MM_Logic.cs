using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MM_Logic : MonoBehaviour {

    private bool clicked = false;                               //if player select the level then can't click on it again
    public GameObject CamHolder, FadePanel, Scenemanager;       //CamHolder - plays moving animation when player choosed the level,  FadePanel - smoothly fade out panel at start
    public AudioManager menuAudio;                                                            
     

    void Awake()
    {
        //if we have not already loaded the AudioManager object and script, load the Audio Manager scene
        if(!GameObject.Find("AudioManager"))
		    SceneManager.LoadScene(2,LoadSceneMode.Additive);

    }
    // Use this for initialization
    void Start ()
    {

        menuAudio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Scenemanager = GameObject.Find("SCENEMANAGER"); //Find SceneManager at start of the game and never delete this gameobject throughout a gaming session        //Scenemanager.GetComponent<AudioSource>().Stop(); //stop wind background sound
		if (menuAudio.gameLoop2D != null && menuAudio.gameLoop2D.isPlaying)
        {
            menuAudio.gameLoop2D.Stop();
            menuAudio.RestartMenu();
        }

    }

    public void GameModeSection(int GM) //where GM - Gamemode number
    {
        int levelnum = 0;

        if (GM == 3 || GM == 4 || GM == 5)
            levelnum = 1;

        LevelSection(levelnum, GM); //first build level index and game mode number

    }

	void Update()
    {
    	// if(Input.GetKeyUp("p"))    	
    	// 
    }

    public void PlayFishing()
    {
        {
    		if (Scenemanager.GetComponent<SceneGM>().GameMode == 0)
    			GameModeSection(4);
		 	// else if (Scenemanager.GetComponent<SceneGM>().GameMode == 4)
		 	// 	GameModeSection(0);
    	 }
    }
	

    void LevelSection(int levelnum,int gamemode) //levelNum set from GameModeSection
    {
        if (!clicked)
        {
            StartCoroutine(LevelLoad(2, levelnum));
            menuAudio.UI_ButtonClick();

            if (levelnum == 1)
            {
                Scenemanager.GetComponent<SceneGM>().GameMode = gamemode; //reseting GameMode to default
                //CamHolder.GetComponent<Animation>().Play("MM_Way02"); 
            }
            //Fade out music loop
            menuAudio.FadeMenuAudio();

           
            //FadePanel.GetComponent<Animator>().Play("MM_FadePanelIn", -1, 0);
            //Scenemanager.GetComponent<SceneGM>().GameMode = 0; //0 for loading default game mode
            clicked = true;
        }
    }

    public IEnumerator LevelLoad(float WaitTime, int LV_idx)
    {
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene(LV_idx);
    }
}

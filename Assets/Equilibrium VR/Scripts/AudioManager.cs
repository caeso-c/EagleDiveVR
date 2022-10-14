using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	#region SFX_AudioClips
	[Header("SFX AudioClips")]
    public AudioClip[] RndEagleSounds;
    public AudioClip eagleCry;
    public AudioClip eagleCrash;
    public AudioClip gotFish;
    public AudioClip waterSplash;
    public AudioClip UI_buttonClick;
    public AudioClip gameStart;
    #endregion
    //stingers - need music stingers here
    public AudioClip WinSound;
    public AudioClip LoseSound;

    [Header("Mixer Snapshots")]
    #region MixerSnapshots
    //Mixer Snapshots ctrl menu music fadeout and game ambience fadein;
    public AudioMixerSnapshot startMenuMusic;
    public AudioMixerSnapshot fadeMenuMusic;
    public AudioMixerSnapshot fadeAmbiences;
    public AudioMixerSnapshot fadeGameMusic;
    #endregion

	[Header("Audio Sources")]
    #region AudioSources
    public AudioSource eagleSource;
    public AudioSource UIsource;
    public AudioSource stingerSource;
    public AudioSource splashSource;

    //these automatically play in game from Audio Sources with clips

    //Ambiences in game
    public AudioSource windLoop;
    public AudioSource ambience2D;
    public AudioSource waterfallSrc3D;

    //Eagle Wing Flap
    public AudioSource eagleWingSource;

    //Music Loops
    public AudioSource menuLoop2D;
    public AudioSource gameLoop2D;

    #endregion
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(menuLoop2D != null)
            menuLoop2D.Play();
    }

    public void RestartMenu()
    {
        if(startMenuMusic != null)
            startMenuMusic.TransitionTo(1.5f);
        if(menuLoop2D != null)
            StartCoroutine(WaitTriggerSource(menuLoop2D, 1.2f,"Play"));
        if(gameLoop2D != null)
            StartCoroutine(WaitTriggerSource(gameLoop2D, 1.6f, "Stop"));

    }

    public void UI_ButtonClick()
    {
    	if (UIsource != null && UI_buttonClick !=null)
        	UIsource.PlayOneShot(UI_buttonClick);
    }

    public void RndEagleSound(Vector3 location)
    {
        if (RndEagleSounds.Length > 0 && !eagleSource.isPlaying) 
        {
            eagleSource.transform.position = location;
            float randomPitch = Random.Range(.8f, 1.3f);
            eagleSource.pitch = randomPitch;
            eagleSource.PlayOneShot (RndEagleSounds[Random.Range(0, RndEagleSounds.Length)]);
        }
    }

    public void GotFish()
	{	if (UIsource != null && gotFish !=null)
        	UIsource.PlayOneShot(gotFish);
    }

    public void WaterSplash(Vector3 location)
    {
		if (splashSource != null && waterSplash !=null)
        {
            splashSource.transform.position = location;
            splashSource.PlayOneShot(waterSplash);
        }	        
    }

    public void HitBoundary()
    {
		if (eagleSource != null && waterSplash !=null)
        	eagleSource.PlayOneShot(eagleCrash);
        if (stingerSource != null && LoseSound != null)
        {
            stingerSource.PlayOneShot(LoseSound);
        }
        if (gameLoop2D.isPlaying)
            gameLoop2D.Stop();
    }

    public void FadeMenuAudio()
    {
        if (fadeMenuMusic != null)
            fadeMenuMusic.TransitionTo(2.0f);
    }

    public void WinGame()
    {
		
		if (stingerSource != null && WinSound != null)
			stingerSource.PlayOneShot(WinSound);
        if (fadeGameMusic != null)
            fadeGameMusic.TransitionTo(1.0f);
    }

    public void LoseGame()
    {
		
		if (stingerSource != null && LoseSound != null)
			stingerSource.PlayOneShot(LoseSound);
        if (fadeGameMusic != null)
            fadeGameMusic.TransitionTo(.25f);
    }

    public void StartFlap()
    {
		if (eagleWingSource != null)
        	eagleWingSource.Play();
    }


    public void StartGameAmbience()
    {   //this is to address the weird glitch coming from the waterfall sound on level start
        //basically it just mutes the sound off for 1 sec and turns it back on so it can properly fade in.

        AudioSource glitchFix = transform.Find("Ambiences/3D_WaterfallAmbience").GetComponent<AudioSource>();
        if (glitchFix != null)
             glitchFix.mute = true;

        StartCoroutine(WaitTriggerSource(glitchFix, 1.0f, "MuteOff"));
		if (waterfallSrc3D !=null)
        	waterfallSrc3D.Play();
		if (ambience2D !=null)
        	ambience2D.Play();
        if(windLoop != null)
        	windLoop.Play();
		if(fadeAmbiences !=null)
			fadeAmbiences.TransitionTo(2.0f);

    }

    public void StartGameMusic()
    {
        if (menuLoop2D != null && menuLoop2D.isPlaying)
            menuLoop2D.Stop();
        if (gameLoop2D != null)
         gameLoop2D.Play();
    }

    public void AudioAction(AudioSource source, string action)
    {
        switch (action)
        {
            case "Stop":
                source.Stop();
                break;
            case "Play":
                source.Play();
                break;
            case "MuteOn":
                source.mute = true;
                break;
            case "MuteOff":
                source.mute = false;
                break;
        }
    }

    public IEnumerator WaitTriggerSource(AudioSource source, float time, string action)
    {
        yield return new WaitForSeconds(time);
        if (source != null)
            AudioAction(source, action);
    }
}

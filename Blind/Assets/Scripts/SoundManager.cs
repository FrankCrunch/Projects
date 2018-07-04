using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {
	
	[SerializeField]
	AudioSource[] efxSource;
	[SerializeField]
	AudioClip tableSound, spiderSound;
    
	static SoundManager instance = null; 
	
	float lowPitchRange = .95f, highPitchRange = 1.05f;  

    void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
    }

    //RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
    void RandomizeSfx (float volume, params AudioClip[] clips)
	{
		//Generate a random number between 0 and the length of our array of clips passed in.
		int randomIndex = Random.Range(0, clips.Length);

		//Choose a random pitch to play back our clip at between our high and low pitch ranges.
		float randomPitch = Random.Range(lowPitchRange, highPitchRange);

		for (int i = 0; i < efxSource.Length; i++)
			if (!efxSource [i].isPlaying) {
				//Set the pitch of the audio source to the randomly chosen pitch.
				efxSource [i].pitch = randomPitch;
				efxSource [i].volume = volume;
				//Set the clip to the clip at our randomly chosen index.
				efxSource [i].clip = clips [randomIndex];
				//Play the clip.
				efxSource [i].Play ();
				break;
			}
	}

	public static void PlayTableSound(){
		instance.efxSource[0].clip = instance.tableSound;
		instance.efxSource[0].Play();
	}
	
	public static void PlaySpiderSound(){
		instance.efxSource[1].clip = instance.spiderSound;
		instance.efxSource [1].volume = 1f;
		instance.efxSource[1].Play();
	}
	
	public static void ChangeVolume(float volume){
		instance.efxSource[0].volume = volume;
	}

}

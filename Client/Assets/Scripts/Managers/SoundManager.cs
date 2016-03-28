using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : SingletonComponent<SoundManager> {
    private AudioSource audio;
    private Hashtable sounds = new Hashtable();


	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Add(string key,AudioClip value){
        if(sounds.ContainsKey(key) || value == null)
            return;
        sounds.Add(key,value);
    }
    private AudioClip Get(string key){
        if(!sounds.ContainsKey(key))
            return null;
        return sounds[key] as AudioClip;
    }
    public AudioClip LoadAudioClip(string path){
        AudioClip ac = Get(path);
        if(ac == null){
            ac = (AudioClip)Resources.Load(path,typeof(AudioClip));
            Add(path,ac);
        }
        return ac;
    }
    public bool CanPlayBackSound(){
        string key = "Test.mp3";
        int i = PlayerPrefs.GetInt(key,1);
        return i == 1;
    }
    public void PlayerBackground(string name,bool canPlay){
        if(audio.clip != null){
            if(name.IndexOf(audio.clip.name) > -1){
                if(!canPlay){
                    audio.Stop();
                    audio.clip = null;
                    //清理内存

                }
                return;
            }

        }
        if(canPlay){
            audio.loop = true;
            audio.clip = LoadAudioClip(name);
            audio.Play();
        }else{
            audio.Stop();
            audio.clip = null;
            //清理内存
        }

    }
    public bool CanPlaySoundEffect(){
        string key = "effect.mp3";
        int i = PlayerPrefs.GetInt(key,1);
        return i == 1;
    }
    public void Play(AudioClip clip,Vector3 position){
        if(!CanPlaySoundEffect()) return;
        AudioSource.PlayClipAtPoint(clip,position);
    }


}

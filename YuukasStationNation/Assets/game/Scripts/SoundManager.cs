using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource soundPrefab;
    [SerializeField] private AudioSource musicPrefab;
    [SerializeField] private List<AudioSource> musicObjects;
    [SerializeField] private List<AudioClip> musicClips;
    [SerializeField] private List<AudioClip> DEBUG_audioClips;

    [SerializeField] private AnimationCurve musicCurve;

    private TimerScript musicChangeTimer;
    private int currentSelectedSong = -1;
    [SerializeField] private float musicTransitionTime;

    [Header("DEBUG")]
    [SerializeField] private int debug_music;

    private void Awake()
    {
        instance = this;
        musicChangeTimer = new TimerScript(musicTransitionTime);
    }
    private void Start()
    {
        //ChangeAllVolumes(SettingsManager.Cache);
        //SettingsManager.instance.ApplySettingsDelegate += ChangeAllVolumes;
        musicObjects = InstantiateMusic();
    }
    private void Update()
    {
        musicChangeTimer.Update();
        MusicChangeHandler();
    }

    #region PLAY SOUND
    public void PlaySound(int clipID, float volume, Vector3 position)
    {
        AudioSource audiosouce = Instantiate(soundPrefab, position, Quaternion.identity, transform);
        audiosouce.clip = DEBUG_audioClips[clipID];
        audiosouce.volume = volume;
        audiosouce.Play();
        Destroy(audiosouce.gameObject, DEBUG_audioClips[clipID].length);
        Debug.LogWarning("SOUNDMANAGER: using DEBUG_audioClip");
    }
    public void PlaySound(int clipID, float volume, float pitch)
    {
        AudioSource audiosouce = Instantiate(soundPrefab, transform.position, Quaternion.identity, transform);
        audiosouce.clip = DEBUG_audioClips[clipID];
        audiosouce.volume = volume;
        audiosouce.pitch = pitch;
        audiosouce.Play();
        Destroy(audiosouce.gameObject, DEBUG_audioClips[clipID].length);
        Debug.LogWarning("SOUNDMANAGER: using DEBUG_audioClip");

    }
    public GameObject PlaySound(AudioClip clip, float volume, Vector3 position)
    {
        AudioSource audiosouce = Instantiate(soundPrefab, position, Quaternion.identity, transform);
        audiosouce.clip = clip;
        audiosouce.volume = volume;
        audiosouce.Play();
        Destroy(audiosouce.gameObject, clip.length);
        return audiosouce.gameObject;

    }
    public GameObject PlaySound(AudioClip clip, float volume, float pitch)
    {
        AudioSource audiosouce = Instantiate(soundPrefab, transform.position, Quaternion.identity, transform);
        audiosouce.clip = clip;
        audiosouce.volume = volume;
        audiosouce.pitch = pitch;
        audiosouce.Play();
        Destroy(audiosouce.gameObject, clip.length);
        return audiosouce.gameObject;

    }
    public void PlaySoundRandom(List<AudioClip> clips, float volume, Vector3 position)
    {
        AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Count)];
        AudioSource audiosouce = Instantiate(soundPrefab, position, Quaternion.identity, transform);
        audiosouce.clip = clip;
        audiosouce.volume = volume;
        audiosouce.Play();
        Destroy(audiosouce.gameObject, clip.length);
    }
    #endregion

    public void PlayMusic(int musicID = 0, bool instant = false)
    {

        if (currentSelectedSong != musicID)
        {

            if (instant)
            {
                foreach (AudioSource musicObject in musicObjects)
                {
                    if (musicObject != musicObjects[currentSelectedSong])
                    {
                        musicObject.volume = 0;
                    }
                    else
                    {
                        musicObject.volume = 1;
                    }
                }

            }
            else
            {
                musicChangeTimer.Start(musicTransitionTime);
            }
            currentSelectedSong = musicID;
        }
    }

    //private void MusicChangeHandler()
    //{
    //    if ((currentSelectedSong < 0 || currentSelectedSong >= musicObjects.Count) && !musicChangeTimer.CompletionStatus)
    //    {
    //        float newVolume = musicCurve.Evaluate(musicChangeTimer.Progress());
    //        foreach (AudioSource audioObject in musicObjects)
    //        {
    //            if (audioObject.volume > newVolume)
    //            {
    //                audioObject.volume = 1 - newVolume;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (!musicChangeTimer.CompletionStatus)
    //        {

    //            float newVolume = musicCurve.Evaluate(musicChangeTimer.Progress());

    //            foreach (AudioSource audioObject in musicObjects)
    //            {
    //                if (audioObject != musicObjects[currentSelectedSong] && audioObject.volume > (1 - newVolume))
    //                {
    //                    audioObject.volume = (1 - newVolume);

    //                }
    //                else
    //                {
    //                    audioObject.volume = newVolume;
    //                }
    //            }
    //        }
    //        else if (musicChangeTimer.CompletionStatus && IfBetweenlistParameters(currentSelectedSong, musicObjects.Count) && musicObjects[currentSelectedSong].volume != 1)
    //        {
    //            foreach (AudioSource audioObject in musicObjects)
    //            {
    //                if (audioObject != musicObjects[currentSelectedSong])
    //                {
    //                    audioObject.volume = 0;
    //                }
    //                else
    //                {
    //                    audioObject.volume = 1;
    //                }
    //            }
    //        }
    //    }

    //}
    private void MusicChangeHandler()
    {
        if (currentSelectedSong < 0 || currentSelectedSong >= musicObjects.Count || musicChangeTimer.CompletionStatus)
        {
            return;
        }

        float newVolume = musicCurve.Evaluate(musicChangeTimer.Progress());

        for (int i = 0; i < musicObjects.Count; i++)
        {
            if (i == currentSelectedSong)
            {
                musicObjects[i].volume = newVolume;
            }
            else if (musicObjects[i].volume > (1 - newVolume))
            {
                musicObjects[i].volume = 1 - newVolume;
            }
        }

        if (musicChangeTimer.CompletionStatus)
        {
            for (int i = 0; i < musicObjects.Count; i++)
            {
                musicObjects[i].volume = (i == currentSelectedSong) ? 1 : 0;
            }
        }
    }
    private bool IfBetweenlistParameters(float toCheck, float max)
    {
        if (toCheck >= max || toCheck < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private List<AudioSource> InstantiateMusic()
    {
        List<AudioSource> newlist = new List<AudioSource>();
        foreach (AudioClip musicClip in musicClips)
        {
            AudioSource newSource = Instantiate(musicPrefab, transform.position, Quaternion.identity, transform);
            newSource.clip = musicClip;
            newSource.volume = 0;
            newSource.Play();
            newlist.Add(newSource);
        }
        return newlist;
    }

    #region SET VOLUME
    public void ChangeMasterVolume(float newVolume)
    {
        mixer.SetFloat("Master", Mathf.Log10(newVolume) * 20f);
    }
    public void ChangeMusicVolume(float newVolume)
    {
        mixer.SetFloat("Music", Mathf.Log10(newVolume) * 20f);
    }
    public void ChangeGeneralVolume(float newVolume)
    {
        mixer.SetFloat("General", Mathf.Log10(newVolume) * 20f);
    }
    /// <summary>
    /// the music volume range is from 0.0001 to 1
    /// </summary>
    /// <param name="data"></param>
    /// 
    //private void ChangeAllVolumes()
    //{
    //    SettingsData data = SettingsManager.Cache;
    //    ChangeMasterVolume(data.sound.masterVolume);
    //    ChangeMusicVolume(data.sound.musicVolume);
    //    ChangeGeneralVolume(data.sound.generalVolume);
    //}
    //private void ChangeAllVolumes(SettingsData data)
    //{
    //    ChangeMasterVolume(data.sound.masterVolume);
    //    ChangeMusicVolume(data.sound.musicVolume);
    //    ChangeGeneralVolume(data.sound.generalVolume);
    //}
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(SoundManager))]
public class EDITOR_SoundManager : Editor
{
    SerializedProperty debug_music;
    private void OnEnable()
    {
        debug_music = serializedObject.FindProperty("debug_music");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        SoundManager pointer = (SoundManager)target;

        if (GUILayout.Button("PlayMusicSmooth"))
        {
            pointer.PlayMusic(debug_music.intValue);
        }
        if (GUILayout.Button("PlayMusicInstant"))
        {
            pointer.PlayMusic(debug_music.intValue, true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

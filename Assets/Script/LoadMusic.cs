using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadMusic : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    public GameObject musicButton;
    public string soundPath;


    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        //local path
        //soundPath = Path.Combine(Application.persistentDataPath, "test.wav");
        //net path
        
        soundPath = "http://18.191.23.16/musicServer/files/"+musicButton.GetComponent<Button>().GetComponentInChildren<Text>().text;
    }
    void Update(){
        soundPath = "http://18.191.23.16/musicServer/files/"+musicButton.GetComponent<Button>().GetComponentInChildren<Text>().text;
    }

    public void playMusic()
    {   
        Debug.Log(soundPath);
        StartCoroutine(LoadAudio());
        
    }

    private IEnumerator LoadAudio()
    {
        UnityWebRequest www=GetAudioFromFile(soundPath);
         
        //WWW request = GetAudioFromFile(soundPath);

        yield return www;
        
        audioClip = DownloadHandlerAudioClip.GetContent(www);
        audioClip.name = "test.wav";

        PlayAudioFile();
    }

    private void PlayAudioFile()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        audioSource.loop = false;
    }

    private UnityWebRequest GetAudioFromFile(string audiopath)
    {
        UnityWebRequest www=UnityWebRequestMultimedia.GetAudioClip(audiopath,AudioType.WAV);
        www.SendWebRequest();
        while(!www.isDone){}
        //WWW request = new WWW(audiopath);
        //return request;
        return www;
    }

}

/* 
 * ------------------------------------------------------------------------------------------
 *                                        _____ _                     _                _    _ 
 *     /\                                / ____| |                   | |              | |  (_)
 *    /  \   _ __ ___   ___  _   _ ___  | |    | |__   ___   ___ ___ | | _____   _____| | ___ 
 *   / /\ \ | '_ ` _ \ / _ \| | | / __| | |    | '_ \ / _ \ / __/ _ \| |/ _ \ \ / / __| |/ / |
 *  / ____ \| | | | | | (_) | |_| \__ \ | |____| | | | (_) | (_| (_) | | (_) \ V /\__ \   <| |
 * /_/    \_\_| |_| |_|\___/ \__,_|___/  \_____|_| |_|\___/ \___\___/|_|\___/ \_/ |___/_|\_\_|           
 *                                                                                                                                                                          
 * <AmousQiu@dal.ca> wrote this file. As long as you retain this notice you
 * can do whatever you want with this stuff. If we meet some day, and you think this stuff is
 * worth it, you can buy me a beer in return (Personal prefer Garrison Raspberry).
 *                                                                        @Copyright Ziyu Qiu
 * ------------------------------------------------------------------------------------------
 */

/*FILE INTRODUTION PART 
  * ------------------------------------------------------------------------------------------
  *FileName: LoadMusic.cs
  *Function: -show all the photos existing in the folder.
  *!!!!!!Needs rewrite.
*/
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
        return www;
    }

}

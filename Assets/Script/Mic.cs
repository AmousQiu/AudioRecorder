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
  *FileName: Mic.cs
  *Function: -the function to be called
*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mic : MonoBehaviour
{
    AudioClip myAudioClip;
    public AudioSource test;
    public InputField nameInput;
    public InputField timeInput;
    public GameObject recordButton;
    public Text showText;
    public string musicName;

    void Start() {
        test=GetComponent<AudioSource>();
        timeInput.contentType=InputField.ContentType.IntegerNumber;
     }

    void Update(){
        if(nameInput.text!=null&&timeInput.text!=null){
            recordButton.SetActive(true);
        }
    }
  
   //start recording for specific time
    public void Record(){
        myAudioClip = Microphone.Start(null, false,int.Parse(timeInput.text) , 44100);
    }

    //save to local
    public void Save(){
        SavWav.Save("test",myAudioClip);
    }

    //upload to server
    public void upload(){
        if(SavWav.uploadToServer(nameInput.text,myAudioClip)=="success"){
           showText.text="Successfully upload "+nameInput.text+".wav";
        }else{
            showText.text="Please rename your file";
        }
        //SceneManager.LoadScene("SampleScene");       
    }
    

   //test demo 
    public void playMusic(){
        LoadMusic();
    }

    IEnumerator LoadMusic()
    {
        //string path = Application.persistentDataPath + "/MusicData" + "/Always.mp3";
        WWW www = new WWW("http://18.191.23.16/musicServer/files/test.wav");
        //WWW www = new WWW(path);
        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        AudioClip ac = www.GetAudioClip(true, true, AudioType.WAV);
        test.clip = ac;
        test.Play();
    }



}


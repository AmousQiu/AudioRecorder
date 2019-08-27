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
  
    public void Record(){
        myAudioClip = Microphone.Start(null, false,int.Parse(timeInput.text) , 44100);
    }

    public void End(){
        Microphone.End(null);
    }
    public void Save(){
        SavWav.Save("test",myAudioClip);
    }

    public void upload(){
        if(SavWav.uploadToServer(nameInput.text,myAudioClip)=="success"){
           showText.text="Successfully upload "+nameInput.text+".wav";
        }else{
            showText.text="Please rename your file";
        }
        //SceneManager.LoadScene("SampleScene");       
    }
    

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


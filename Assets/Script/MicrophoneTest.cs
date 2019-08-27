using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneTest : MonoBehaviour
{
    public AudioSource aud;

    bool isHaveMicroPhone;
    string device;
    public Text text;

    //Debug Text
    public Text clipLength;//记录音频文件的长度
    public Text devicePosition;//设备音频的位置
    public Text audioTime;//记录音频的时间
    public Text audioSampleTime;//

    
    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        string[] devices = Microphone.devices;

        if (devices.Length > 0)
        {
            isHaveMicroPhone = true;
            device = devices[0];
            text.text = devices[0];
        }
        else
        {
            isHaveMicroPhone = false;
            text.text = "没有获取到麦克风";
        }
    }

    //开始录音按钮
    public void OnclickButton()
    {
        if (!isHaveMicroPhone) return;

        aud.clip = Microphone.Start(device, true, 10, 10000);
        //aud.Play();
        //aud.timeSamples = Microphone.GetPosition(device);
        //aud.timeSamples = 0;
        Debug.Log("开始录音");
    }

    //开始播放按钮
    public void OnPlay()
    {
        aud.Play();
        aud.timeSamples = Microphone.GetPosition(device);//这里设置了之后就会近乎实时同步

        int min;
        int max;
        Microphone.GetDeviceCaps(device, out min,out max);
        //aud.timeSamples = 0;
        Debug.Log("开始播放"+min+" "+max);
    }




    private void Update()
    {
        clipLength.text = "     clipLength:" + aud.clip.length;
        devicePosition.text = " devicePosition:" + Microphone.GetPosition(device);
        audioTime.text = "      audioTime:" + aud.time;
        audioSampleTime.text = "audioSampleTime:" + aud.timeSamples;

        Debug.Log("     clipLength:" + aud.clip.length);
        Debug.Log(" devicePosition:" + Microphone.GetPosition(device));
        Debug.Log("      audioTime:" + aud.time);
        Debug.Log("audioSampleTime:" + aud.timeSamples);

        aud.timeSamples = Microphone.GetPosition(device);
    }
}

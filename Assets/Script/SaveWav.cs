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
  *FileName: SavWav.cs
  *Function: -
*/
using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
public static class SavWav
{
    private const uint HeaderSize = 44;
    private const float RescaleFactor = 32767; //to convert float to Int16

//Save the wav file to local
    public static void Save(string filename, AudioClip clip, bool trim = false)
    {
        if (!filename.ToLower().EndsWith(".wav"))
        {
            filename += ".wav";
        }
        var filepath = Path.Combine(Application.persistentDataPath, filename);

        Debug.Log(filepath);
        // Make sure directory exists if user is saving to sub dir.
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        using (var fileStream = new FileStream(filepath, FileMode.Create))
        using (var writer = new BinaryWriter(fileStream))
        {
            var wav = GetWav(clip, out var length, trim);
            writer.Write(wav, 0, (int)length);
        }

    }


    public static string uploadToServer(string filename, AudioClip clip, bool trim = false)
    {
        //Convert clip to wav file
        var wav = GetWav(clip, out var length, trim);
         
        //return a message from InsertIntoDB to see if duplicate or not.
        string result=insertIntoDB(filename, wav);
        return result;
    }


    //upload to UnityUpload.php 
    public static void upload(byte[] musicbytes, string filename)
    {
        string url = "http://18.191.23.16/musicServer/UnityUpload.php";
        WWWForm form = new WWWForm();
        //"Name" and "post" are specific term accroding to UnityUpload.php file
        form.AddField("Name", filename);
        form.AddBinaryData("post", musicbytes);
        WWW www = new WWW(url, form);
    }

    //Insert into database through InsertData.php
    public static string insertIntoDB(string filename, byte[] musicbytes)
    {
        string result="";
        WWWForm form = new WWWForm();
        form.AddField("fileNamePost", filename);
        string insertURL = "http://18.191.23.16/musicServer/InsertData.php";
        WWW www = new WWW(insertURL, form);
        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            while (!www.isDone)
            {
            }
            Debug.Log("insertNew wav file: " + www.text);
            if (www.text != "exist")
            {
                result= "success";
                upload(musicbytes, filename);
            }
            else
            {
                result="fail";
            }
        }
        return result;
    }






// Converting part.
    public static byte[] GetWav(AudioClip clip, out uint length, bool trim = false)
    {
        var data = ConvertAndWrite(clip, out length, out var samples, trim);

        WriteHeader(data, clip, length, samples);

        return data;
    }

    private static byte[] ConvertAndWrite(AudioClip clip, out uint length, out uint samplesAfterTrimming, bool trim)
    {
        var samples = new float[clip.samples];

        clip.GetData(samples, 0);

        var sampleCount = samples.Length;

        var start = 0;
        var end = sampleCount - 1;

        if (trim)
        {
            for (var i = 0; i < sampleCount; i++)
            {
                if ((short)(samples[i] * RescaleFactor) == 0)
                    continue;

                start = i;
                break;
            }

            for (var i = sampleCount - 1; i >= 0; i--)
            {
                if ((short)(samples[i] * RescaleFactor) == 0)
                    continue;

                end = i;
                break;
            }
        }

        var buffer = new byte[(sampleCount * 2) + HeaderSize];

        var p = HeaderSize;
        for (var i = start; i <= end; i++)
        {
            var value = (short)(samples[i] * RescaleFactor);
            buffer[p++] = (byte)(value >> 0);
            buffer[p++] = (byte)(value >> 8);
        }

        length = p;
        samplesAfterTrimming = (uint)(end - start + 1);
        return buffer;
    }

    private static void AddDataToBuffer(byte[] buffer, ref uint offset, byte[] addBytes)
    {
        foreach (var b in addBytes)
        {
            buffer[offset++] = b;
        }
    }

    private static void WriteHeader(byte[] stream, AudioClip clip, uint length, uint samples)
    {
        var hz = (uint)clip.frequency;
        var channels = (ushort)clip.channels;

        var offset = 0u;

        var riff = Encoding.UTF8.GetBytes("RIFF");
        AddDataToBuffer(stream, ref offset, riff);

        var chunkSize = BitConverter.GetBytes(length - 8);
        AddDataToBuffer(stream, ref offset, chunkSize);

        var wave = Encoding.UTF8.GetBytes("WAVE");
        AddDataToBuffer(stream, ref offset, wave);

        var fmt = Encoding.UTF8.GetBytes("fmt ");
        AddDataToBuffer(stream, ref offset, fmt);

        var subChunk1 = BitConverter.GetBytes(16u);
        AddDataToBuffer(stream, ref offset, subChunk1);

        const ushort two = 2;
        const ushort one = 1;

        var audioFormat = BitConverter.GetBytes(one);
        AddDataToBuffer(stream, ref offset, audioFormat);

        var numChannels = BitConverter.GetBytes(channels);
        AddDataToBuffer(stream, ref offset, numChannels);

        var sampleRate = BitConverter.GetBytes(hz);
        AddDataToBuffer(stream, ref offset, sampleRate);

        var byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
        AddDataToBuffer(stream, ref offset, byteRate);

        var blockAlign = (ushort)(channels * 2);
        AddDataToBuffer(stream, ref offset, BitConverter.GetBytes(blockAlign));

        ushort bps = 16;
        var bitsPerSample = BitConverter.GetBytes(bps);
        AddDataToBuffer(stream, ref offset, bitsPerSample);

        var dataString = Encoding.UTF8.GetBytes("data");
        AddDataToBuffer(stream, ref offset, dataString);

        var subChunk2 = BitConverter.GetBytes(samples * 2);
        AddDataToBuffer(stream, ref offset, subChunk2);
    }

    //load from url test
}

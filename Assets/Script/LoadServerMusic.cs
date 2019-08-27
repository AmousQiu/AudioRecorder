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
  *FileName: LoadServerMusic.cs
  *Function: -Get all the filename from the server
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadServerMusic : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] items;

    public GameObject musicButton;

    IEnumerator Start()
    {
        WWWForm musicForm = new WWWForm();
        WWW wwwMusic = new WWW("http://18.191.23.16/musicServer/itemsData.php");
        yield return wwwMusic;
        string allString = (wwwMusic.text);
        //seperate each tuples
        items = allString.Split(';');
        string filename;
        for (int i = 0; i < items.Length - 1; i++)
        {
            filename = GetDataValue(items[i], "FileName:");
            Debug.Log(filename);
        }
        Populate();
    }

    public void callRefresh(){
        SceneManager.LoadScene("SampleScene");
        //StartCoroutine(refresh());
    }
    public IEnumerator refresh()
    {
        WWWForm musicForm = new WWWForm();
        WWW wwwMusic = new WWW("http://18.191.23.16/musicServer/itemsData.php");
        yield return wwwMusic;
        string allString = (wwwMusic.text);
        //seperate each tuples
        items = allString.Split(';');
        string filename;
        for (int i = 0; i < items.Length - 1; i++)
        {
            filename = GetDataValue(items[i], "FileName:");
            Debug.Log(filename);
        }
        Populate();
    }

    void Populate()
    {
        GameObject newobj;
        for (int i = 0; i < items.Length - 1; i++)
        {
            newobj = (GameObject)Instantiate(musicButton, transform);
            newobj.GetComponent<Button>().GetComponentInChildren<Text>().text = GetDataValue(items[i], "FileName:") + ".wav";
        }
    }

    string GetDataValue(string data, string index)
    {
        Debug.Log("data: " + data + "index" + index);
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));
        }
        return value;
    }
}

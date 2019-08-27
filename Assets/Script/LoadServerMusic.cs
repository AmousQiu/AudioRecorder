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

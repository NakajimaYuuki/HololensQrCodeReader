using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Serializable]
public class Userinfo
{
    public string name;
    public string message;
}

[System.Serializable]
public class Scouter
{
    public int id;
    public string name;
    public int point;
    public bool is_presented;
}


public class WebApi : MonoBehaviour
{
    string url;
    // Use this for initialization
    void Start()
    {

    }

    public static Scouter GetApi(string url)
    {
        // 例外が発生したらとりあえず上で何とかするのでここではキャッチしない
        UnityWebRequest request = UnityWebRequest.Get(url);
        // リクエスト送信
        AsyncOperation checkAsync = request.Send();
        var scouter = new Scouter();

        while (!checkAsync.isDone);
        Debug.Log(request.downloadHandler.text);
        // GazeGestureManager gazeGestureManeger = FindObjectOfType<GazeGestureManager>(); 
        scouter = JsonUtility.FromJson<Scouter>(request.downloadHandler.text);
        
        return scouter;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

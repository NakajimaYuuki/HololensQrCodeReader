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
public class Ranking
{
    public int rank;
}



public class WebApi : MonoBehaviour
{
    string url;
    // Use this for initialization
    void Start()
    {

    }

    public static Ranking GetApi(string url)
    {
        // 例外が発生したらとりあえず上で何とかするのでここではキャッチしない
        UnityWebRequest request = UnityWebRequest.Get(url);
        // リクエスト送信
        AsyncOperation checkAsync = request.Send();
        var rank = new Ranking();

        while (!checkAsync.isDone);
        // GazeGestureManager gazeGestureManeger = FindObjectOfType<GazeGestureManager>(); 
        rank = JsonUtility.FromJson<Ranking>(request.downloadHandler.text);
        
        return rank;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

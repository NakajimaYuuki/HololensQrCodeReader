using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


[System.Serializable]
public class Userinfo
{
    public string name;
    public string message;
}

public class WebApi : MonoBehaviour
{

    string url;

    // Use this for initialization
    void Start()
    {

    }

    public static Userinfo GetApi(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        // リクエスト送信
        request.Send();

        // 通信エラーチェック
        Userinfo userinfo = new Userinfo();
        if (!request.isError)
        {
            Debug.Log(request.responseCode);
            if (request.responseCode == 200)
            {
                // テストデータのときはjsonを直でセット
                if (url != "https://www.google.co.jp/")
                {
                    userinfo = JsonUtility.FromJson<Userinfo>(request.downloadHandler.text);
                }
                else {
                    userinfo.name = "ギークラボ長野";
                    userinfo.message = "頑張ります！！";

                }
            }
        }
        return userinfo;
    }
    // Update is called once per frame
    void Update()
    {

    }
}

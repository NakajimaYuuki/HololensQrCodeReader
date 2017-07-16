using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR.WSA.Input;

public class GazeGestureManager : MonoBehaviour {

    public static GazeGestureManager Instance { get; private set; }
    public GameObject FocusedObject { get; private set; }
    public GameObject TextViewPrefab;
    public AudioClip captureAudioClip;
    public AudioClip failedAudioClip;

    GestureRecognizer gestureRecognizer;
    PhotoInput photoInput;
    QrDecoder qrDecoder;
    AudioSource captureAudioSource;
    AudioSource failedAudioSource;
    public bool isTest;


    void Awake () {
        isTest = true;
        Instance = this;
        photoInput = GetComponent<PhotoInput>();
        gestureRecognizer = new GestureRecognizer();
        gestureRecognizer.TappedEvent += GestureRecognizer_TappedEvent;
        gestureRecognizer.StartCapturingGestures();
        qrDecoder = gameObject.AddComponent<QrDecoder>();
	}

    void Start() {
        captureAudioSource = gameObject.AddComponent<AudioSource>();
        captureAudioSource.clip = captureAudioClip;
        captureAudioSource.playOnAwake = false;
        failedAudioSource = gameObject.AddComponent<AudioSource>();
        failedAudioSource.clip = failedAudioClip;
        failedAudioSource.playOnAwake = false;
    }

    private void Update() {

    }


    //　今はジェスチャーから呼ばれるけどいずれは音声認識も加えたい
    void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay) {

        // 既存のテキストを消すには？

        if (!isTest)
        {
            //　写真を撮影した上でメソッドを呼ぶ
            photoInput.CapturePhotoAsync(onPhotoCaptured);
        }
        else
        {
            //写真撮影を行わずにurlの取得だけ行う、ダミーデータをセットする
            onPhotoCaptured(null, 0, 0);

        }
    }
    // 表示している文字を消すためのクリア機能も欲しい


    // 写真撮影時のイベント
    void onPhotoCaptured(List<byte> image, int width, int height) {

        ///　テストパターン
        ///　①QRコードを読んでレスポンスを受け取ってそれを使う(実戦)
        ///　②QRコードを読まずにとりあえず表示だけしてみたいとき(表示やapiの確認、unityeditor)
        ///　③QRコードを読むけど、レスポンスは既定値(疎通的な意味で)
        ///　④

        string url = "https://www.google.co.jp/";

        Userinfo response = new Userinfo();
        response.name = "ギークラボ長野";
        response.message = "頑張ります！！";


        if (!isTest)
        {
            url = qrDecoder.Decode(image.ToArray(), width, height);
        }

        if (Utility.IsUrl(url)) {
            // ここで　apiを投げる
            response = WebApi.GetApi(url);
        }

        // このレスポンスを表示する
        // 名前と一言！
        Vector3 headPosition = Camera.main.transform.position;
        Vector3 gazeDirection = Camera.main.transform.forward;
        // 名前の表示
        showText(headPosition, gazeDirection, response.name);

        headPosition.y = headPosition.y - 0.1f;
        //　一言の表示
        showText(headPosition, gazeDirection, response.message);
        //　成功音声
        captureAudioSource.Play();

        // failedAudioSource.Play();
    }


    void showText(Vector3 headPosition, Vector3 gazeDirection, string text) {

        Debug.Log(Camera.main.transform.rotation);
        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo)) {
            var obj = Instantiate(TextViewPrefab, hitInfo.point, Camera.main.transform.rotation);
            var textMesh = obj.GetComponent<TextMesh>();
            textMesh.text = text;
        }
    }
}

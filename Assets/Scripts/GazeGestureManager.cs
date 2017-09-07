using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.VR.WSA.Input;
using System.IO;



public class GazeGestureManager : MonoBehaviour {

    public static GazeGestureManager Instance { get; private set; }
    public GameObject FocusedObject { get; private set; }
    public AudioClip captureAudioClip;
    public AudioClip failedAudioClip;

    GestureRecognizer gestureRecognizer;
    PhotoInput photoInput;
    QrDecoder qrDecoder;
    AudioSource captureAudioSource;
    AudioSource failedAudioSource;
    private bool isTest;
    public static int status;


    Scouter response = new Scouter();
    // ゲームオブジェクト
    public GameObject plate;
    public GameObject wordCursor;
    GameObject mainCamera;

    public int power;
    public int id;
    public GameObject textView;
    public TextMesh textMesh;
    public AudioSource countSe;

    public GameObject name;
    public GameObject glnagano;


    // ユニティちゃん
    public GameObject unityChan;
    private Animator unityChananime;
    private AudioSource unityVoice; // いっくよー
    private AudioSource unityGanabare;　//　ステージ中
    private AudioSource unityHighscore;　//　100点以上
    private AudioSource unityLowScore;　//　100点より下
    public int Count;

    // 外部からの書き込み用.
    public bool IsTest
    {
        get { return isTest; }
        set { isTest = value; }
    }



    void Awake () {
        /*ここで色々制御！！*/
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


        // 台座
        plate.SetActive(false);
        // カメラ
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        // textview
        textView.SetActive(false);
        countSe = textView.GetComponent<AudioSource>();


        // ユニティちゃん
        unityChananime = unityChan.GetComponent<Animator>();
        AudioSource[] audioSources = unityChan.GetComponents<AudioSource>();
        unityVoice = audioSources[3];
        unityGanabare = audioSources[2];
        unityHighscore = audioSources[1];
        unityLowScore = audioSources[0];

        unityChan.SetActive(false);
        SpatialMapping.Instance.DrawVisualMeshes = true;
        Count = 0;
        status = 0;
        power = 0;
        name.SetActive(false);
        
    }

    private void Update() {
        // フラグをみてオンの時は穴の表示と文字の表示を行う
        // status 0:最初
        //　　　　10:撮影待機
        //　　　　11:台座の出現
        //　　　　12:ユニティちゃんが現れる
        //        13:数値が上がるとともに棒も長くなっていく
        //　　　　14.エンド処理、数値に応じてunityちゃんがポーズをとる（低いと転ぶ、高いとｲｴｲｯって感じのポーズ）
        //　　　　0に戻る
        if (status == 10)
        {
            if(plate.activeSelf == false)
            {
                plate.transform.localScale = new Vector3(0, 0, 0.2f);
                plate.transform.position = new Vector3(wordCursor.transform.position.x, wordCursor.transform.position.y+0.4f, wordCursor.transform.position.z);
                plate.transform.localEulerAngles = new Vector3(0, mainCamera.transform.localEulerAngles.y + 180, 0);
                
                if (!unityChan.GetComponent<Rigidbody>())
                {
                    var rigidbody1 = plate.gameObject.AddComponent<Rigidbody>();
                    rigidbody1.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    rigidbody1.mass = 2.0f;
                    rigidbody1.useGravity = true;
                    rigidbody1.constraints = RigidbodyConstraints.FreezeRotation;

                }
                
                plate.SetActive(true);   
            }

            plate.transform.localScale += new Vector3(0.001f, 0.001f, 0);
            if (plate.transform.localScale.x >= 0.05f)
            {
                status++;
            }
        }
        else if (status == 11)
        {
            
            // ユニティちゃんが穴から現れる
            if (unityChan.activeSelf == false)
            {       
                unityChan.transform.position = new Vector3(plate.transform.position.x, plate.transform.position.y+0.3f, plate.transform.position.z-0.01f);
                unityChan.transform.localEulerAngles = new Vector3(0, mainCamera.transform.localEulerAngles.y+180, 0);
                if (!unityChan.GetComponent<Rigidbody>())
                {
                    var rigidbody = unityChan.gameObject.AddComponent<Rigidbody>();
                    rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    rigidbody.mass = 3.0f;
                    rigidbody.useGravity = true;
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation |
                                            RigidbodyConstraints.FreezePositionX|
                                            RigidbodyConstraints.FreezePositionZ;
                }
                unityChan.SetActive(true);
                // いっくよー
                unityVoice.Play();
            }
            else
            {
                unityChananime.Play("TopToGround");
                Count++;
                if (Count > 100)
                {
                    status++;
                    Count = 0;
                    Destroy(plate.GetComponent<Rigidbody>());
                }
            }
        }
        else if (status == 12)
        {
            //　カウントアップする
            if (!textView.activeSelf)
            {
                textView.transform.localPosition = new Vector3( unityChan.transform.position.x, unityChan.transform.position.y+0.1f, unityChan.transform.position.z);
                textView.transform.localEulerAngles = new Vector3(mainCamera.transform.localEulerAngles.x, mainCamera.transform.localEulerAngles.y, mainCamera.transform.localEulerAngles.z);
                textMesh = textView.GetComponent<TextMesh>();
                textMesh.text = "0";
                textView.SetActive(true);
            }
            else
            {
                // カウントアップしつつ上昇
                if (response.point >= power)
                {
                    if (!countSe.isPlaying)
                    {
                        countSe.Play();
                    }
                    //台座がY軸方向に伸びていくので合わせて台座ちゃんも伸びていく
                    textView.transform.localPosition = new Vector3(textView.transform.position.x, textView.transform.position.y + 0.002f, textView.transform.position.z);
                    textMesh.text = (int.Parse(textMesh.text) + 1).ToString();
                    plate.transform.localPosition = new Vector3(plate.transform.localPosition.x, plate.transform.localPosition.y + 0.002f, plate.transform.localPosition.z);
                    textView.transform.localEulerAngles = new Vector3(mainCamera.transform.localEulerAngles.x, mainCamera.transform.localEulerAngles.y, mainCamera.transform.localEulerAngles.z);
                    power = int.Parse(textMesh.text) + 1;
                    // ランダムでセリフも
                    if (power % 100 == 0)
                    {
                        unityChananime.Play("TopOfJump");
                    }
                    if (power % 200 == 0)
                    {
                        unityGanabare.Play();
                    }
                }
                else
                {
                    countSe.Stop();
                    status++;
                }
            }
        }
        else if (status == 13)
        {
            // ユニティちゃんの演出処理、+ライセンス表示+日本システム技研をよろしく！
            if (Count == 0)
            {
                Count++;
                if (power < 100)
                {
                    unityLowScore.Play();
                    unityChananime.Play("KneelDown");
                }
                else
                {
                    unityHighscore.Play();
                    unityChananime.Play("Salute");
                }
            }
            Count++;
            if (Count >= 250)
            {
                status++;
                Count = 0;
            }
        }
        if (status == 14)
        {
            // 名前と順位の表示
            if (!name.activeSelf)
            {
                name.transform.localPosition = new Vector3(textView.transform.position.x, textView.transform.position.y + 0.06f, textView.transform.position.z);
                name.transform.localEulerAngles = new Vector3(mainCamera.transform.localEulerAngles.x, mainCamera.transform.localEulerAngles.y, mainCamera.transform.localEulerAngles.z);
                var meshName = name.GetComponent<TextMesh>();
                meshName.characterSize = 0.08f;
                meshName.text = response.name+"さんは現在\n"+response.rank+ "位です！";
                name.SetActive(true);
            }
            Count++;
            if (Count >= 600)
            {
                status++;
                Count = 0;
            }
        }
        if (status == 15)
        {
            // 上を見てください
            status++;

        }
        if (status == 16)
        {
            // 初期化
            status = 0;
            power = 0;
            Count = 0;
            //　いろんなオブジェクトを戻す
            plate.SetActive(false);
            unityChan.SetActive(false);
            textView.SetActive(false);
            name.SetActive(false);
            Destroy(unityChan.GetComponent<Rigidbody>());
            Destroy(plate.GetComponent<Rigidbody>());
        }
    }


    //　今はジェスチャーから呼ばれるけどいずれは音声認識も加えたい
    void GestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay) {

        if (status != 0)
        {
            return;
        }
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

    // 写真撮影時のイベント
    void onPhotoCaptured(List<byte> image, int width, int height)
    {

        ///　テストパターン
        ///　①QRコードを読んでレスポンスを受け取ってそれを使う(実戦)
        ///　②QRコードを読まずにとりあえず表示だけしてみたいとき(表示やapiの確認、unityeditor)
        try
        {
            if (!isTest)
            {
                string url = qrDecoder.Decode(image.ToArray(), width, height);
                response = WebApi.GetApi(url);
            }
            else
            {
                string url = "https://esap.herokuapp.com/scouterapi/ranking/"+ id.ToString()+ "/";
                response = WebApi.GetApi(url);
                // Debug.Log(response);
                /*
                response.id = 1;
                response.point = 150;
                response.name = "ギークラボ長野";
                response.is_presented = false;
                */
            }
            if (response != null)
            {
                status = 10;
            }
            //　成功音声
            // captureAudioSource.Play();
        }
        catch
        {
            failedAudioSource.Play();
        }
    }


}
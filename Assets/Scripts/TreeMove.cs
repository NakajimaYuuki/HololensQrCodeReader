using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMove : MonoBehaviour {

    GameObject mainCamera;
    public GameObject wordCursor;
    // Use this for initialization
    void Start () {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

    }
	
	// Update is called once per frame
	void Update () {
        float z = 1.5f;
        if (wordCursor.transform.position.z >= 1.5f)
        {
            z = wordCursor.transform.position.z;
        }
        this.transform.localPosition = new Vector3(wordCursor.transform.position.x, wordCursor.transform.position.y + 0.1f, z);
        // plate.transform.localEulerAngles = new Vector3(0, mainCamera.transform.localEulerAngles.y + 180, 0);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCursor : MonoBehaviour {

    public GameObject worldcursor; 
    MeshRenderer meshRenderer;

    void Start() {
        meshRenderer = worldcursor.gameObject.GetComponentInChildren<MeshRenderer>();
    }

    void Update() {
        // Do a raycast into the world that will only hit the Spatial Mapping mesh.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;

        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
            30.0f, SpatialMapping.PhysicsRaycastMask))
        {
            // Move this object's parent object to
            // where the raycast hit the Spatial Mapping mesh.
            worldcursor.transform.position = hitInfo.point;

            // Rotate this object's parent object to face the user.
            /*
            Quaternion toQuat = Camera.main.transform.localRotation;
            toQuat.x = 0;
            toQuat.z = 0;
            worldcursor.transform.rotation = toQuat;*/
            worldcursor.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

        }
    }
}

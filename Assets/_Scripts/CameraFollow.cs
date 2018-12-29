using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {

    private Camera cam;
    public GameObject target;
    public float zOffset = 10;
    public float zoomLevel = 5;
    public Vector2 offsets = Vector2.zero;
	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        FollowTarget();
	}

    void FollowTarget() {
        if (target != null) {
            cam.orthographicSize = zoomLevel;
            transform.position = new Vector3(
                target.transform.position.x + offsets.x,
                target.transform.position.y + offsets.y,
                -zOffset
            );
        }
    }

    void OnValidate() {
        cam = GetComponent<Camera>();
        gameObject.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            -zOffset
        );
        cam.orthographicSize = zoomLevel;
    }
}

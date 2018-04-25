using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

    private Camera cam;
    public float BASE_FOLLOW_SMOOTHNESS = 1.0f;
    void Start () {
        cam = Camera.main;
	}
	
	void FixedUpdate () {
        cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position, BASE_FOLLOW_SMOOTHNESS) + Vector3.back;
    }
}

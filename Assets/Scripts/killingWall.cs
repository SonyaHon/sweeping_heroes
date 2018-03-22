using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killingWall : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("WOW, YURE DEAD "+other);
		Destroy (other.gameObject);
	}
		
}

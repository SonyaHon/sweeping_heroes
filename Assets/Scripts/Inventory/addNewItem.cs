using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addNewItem : MonoBehaviour {
	public int ID;
	GameObject inventory;
	GameObject player;
	bool isTrigger;
	public int scale;

	// Use this for initialization
	void Start () {
		scale = 5;
		inventory = GameObject.Find ("Inventory");
		player = GameObject.FindWithTag ("Player");

		Debug.Log(this.transform.GetComponent<Collider2D>().isTrigger.ToString());
		Debug.Log ("Я родился!");
		isTrigger = false;
		this.GetComponentInParent<SpriteRenderer>().sprite = inventory.GetComponent<ItemDatabase> ().FetchItemByID (ID).Sprite;
		this.GetComponentInParent<RectTransform> ().localScale =new Vector3(scale,scale,0);
	}
		
	//void OnTriggerExit(Collider2D suchPlayer){
	//	Debug.Log ("тебе повезло");
	//}

	void OnTriggerStay2D(Collider2D other){
		isTrigger = true;
	}

	void OnTriggerExit2D(Collider2D other){
		isTrigger = false;
	}

	void Update(){
		if (isTrigger) {
			if (Input.GetKeyDown (KeyCode.Z)) {
				inventory.GetComponent<Inventory> ().AddItem (ID);
				Destroy (this.transform.parent.gameObject);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		isTrigger = true;
	}
}

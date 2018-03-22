using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour {
	private Item item;
	private string data;
	private GameObject tooltip;

	public void Activate(Item item){
		this.item = item;
		ConstructDataString ();
		tooltip.SetActive (true);
	}

	public void Deactivate(){
		tooltip.SetActive (false);
	}

	public void ConstructDataString(){
		data = item.title;
		tooltip.transform.GetChild (0).GetComponent<Text> ().text = data;
	}
	// Use this for initialization
	void Start () {
		tooltip = GameObject.Find ("Tooltip");
		Debug.Log ("We are here. wow");
		tooltip.SetActive (false);
	}

	void Update(){
		if (tooltip.activeSelf) {
			tooltip.transform.position = Input.mousePosition;
		}
	}
	
	// Update is called once per frame

}

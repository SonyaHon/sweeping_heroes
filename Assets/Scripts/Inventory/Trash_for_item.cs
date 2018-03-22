using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trash_for_item : MonoBehaviour, IDropHandler {
	public GameObject Item_on_the_ground;
	GameObject buffer_obj;

	// Use this for initialization
	void Start () {
		buffer_obj = GameObject.Find ("Object buffer");
	}

	public void OnDrop(PointerEventData eventData){
		Debug.Log ("OnDrop in trash");
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> ();
		droppedItem.isAlive = false; //DIE, MOTHERFUCER, DIE
		GameObject item = Instantiate(Item_on_the_ground ) ;
		item.transform.position = GameObject.FindWithTag ("Player").transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

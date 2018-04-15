using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler  {
	public Item item;
	public int amount;
	public int slot;
	public int ID;

	public bool isAlive;

	private Transform originalParent;
	private Inventory inv;
	private Tooltip tooltip;
	private Vector2 offset;
	private GameObject buffer_obj;

	void Start() {
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		tooltip = inv.GetComponent<Tooltip> ();
		buffer_obj = GameObject.Find ("Object buffer");
		isAlive = true;
	}

	public void OnBeginDrag(PointerEventData eventData) {
		Debug.Log ("УУУУ, СУКА. ЩА ВЫЛЕЧУ");
		if (item != null) {
			offset = eventData.position - new Vector2( this.transform.position.x,this.transform.position.y  );
			originalParent = this.transform.parent;
			this.transform.SetParent (this.buffer_obj.transform);
			this.transform.position = eventData.position - offset; 
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}

	public void OnDrag(PointerEventData eventData) {
		if (item != null) {
			this.transform.position = eventData.position - offset;            
		}
	}

	public void OnEndDrag(PointerEventData eventData) {
		Debug.Log ("Ябал это говно");
		if (isAlive) {
			this.transform.SetParent (inv.slots[slot].transform);
			this.transform.position = inv.slots[slot].transform.position;
			GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			inv.items[slot] = new Item();
			GetComponent<CanvasGroup> ().blocksRaycasts = true;
			Destroy (this.gameObject);
		}
	}

	public void OnPointerEnter(PointerEventData eventData){
		tooltip.Activate (item);
	}

	public void OnPointerExit(PointerEventData eventData){
		tooltip.Deactivate ();
	}


}

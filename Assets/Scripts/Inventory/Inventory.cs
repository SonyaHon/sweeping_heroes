using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	ItemDatabase database;
	GameObject slotPanel;
	GameObject currentSlotPanel;

	private bool isShowing = true;

	Tooltip tooltip;

	GameObject inventoryPanel;
	GameObject currentInventoryPanel;
	GameObject buffer_obj;

	public GameObject inventorySlot;
	public GameObject inventoryItem;

	int slotAmount, currentSlotAmount;
	public List<Item> items = new List<Item> ();
	public List<GameObject> slots = new List<GameObject> ();

	void Start(){
		database = GetComponent<ItemDatabase> ();

		slotAmount = 9;
		currentSlotAmount = 3;

		inventoryPanel = GameObject.Find ("Inventory Panel");

		currentSlotPanel = GameObject.Find ("Current Slot Panel").gameObject; //над потом более аккуратный поиск замутить
		for (int i = 0; i < currentSlotAmount; i++) {
			items.Add (new Item ());
			slots.Add (Instantiate (inventorySlot));
			slots [i].GetComponent<Slot>().slotID = i;
			slots [i].transform.SetParent (currentSlotPanel.transform);
		}

		slotPanel = inventoryPanel.transform.Find ("Slot Panel").gameObject;
		for (int i = currentSlotAmount; i < (currentSlotAmount+slotAmount) ; i++) {
			items.Add (new Item ());
			slots.Add (Instantiate (inventorySlot));
			slots [i].GetComponent<Slot>().slotID = i;
			slots [i].transform.SetParent (slotPanel.transform);
		}
			

		buffer_obj = GameObject.Find ("Object buffer");
		tooltip = this.GetComponent<Tooltip> ();

		/*AddItem (0);
		AddItem (1);
		AddItem (0);
		AddItem (0);
		AddItem (1);
		AddItem (1);*/
	}

	public void AddItem(int id){
		Item itemToAdd = database.FetchItemByID (id);
		if (itemToAdd.Stackable && CheckIfItemIsInInventory (itemToAdd)) {
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == id) {
					ItemData data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();

					data.amount ++;

					data.transform.GetChild (0).GetComponent<Text> ().text = data.amount.ToString ();
					break;
				}
			}
		} else {
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == -1) {
					Debug.Log ("Добавляем");
					items [i] = itemToAdd;
					GameObject itemObj = Instantiate (inventoryItem);
					itemObj.GetComponent<ItemData> ().item = itemToAdd;
					itemObj.GetComponent<ItemData> ().slot = i;
					itemObj.GetComponent<ItemData> ().ID = itemToAdd.ID;
					itemObj.transform.SetParent (slots [i].transform);
					itemObj.transform.localPosition = Vector3.zero;
					itemObj.GetComponent<Image> ().sprite = itemToAdd.Sprite;

					itemObj.name = itemToAdd.title;
					if (itemToAdd.Stackable) {
						ItemData data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();
						data.amount = 1;
					}


					break;
				}
			}
		}
	}

	public void RemoveOneItem(int id){
		Item itemToRemove = database.FetchItemByID (id);
		ItemData data;
		if (itemToRemove.Stackable && CheckIfItemIsInInventory (itemToRemove) ) {
			
			for (int i = 0; i < slots.Count; i++) {
				
				if ( !slots [i].GetComponent<Slot>().isEpty() ) {
					if (slots [i].transform.childCount > 0) {
						data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();
					}else {
						data = buffer_obj.transform.GetChild (0).GetComponent<ItemData> ();
					}


					if (data.item.ID == id) {
						if (data.amount > 2) {
							data.amount--;
							data.transform.GetChild (0).GetComponent<Text> ().text = data.amount.ToString ();
							break;
						}
						if (data.amount == 2) {
							data.amount--;
							data.transform.GetChild (0).GetComponent<Text> ().text = "";
							break;
						}
						if (data.amount <= 1) {
							data.amount--;
							RemoveFullStockOfItems (id);
							break;
						}
						break;
					}
				}
			}
		} else {
			if (!CheckIfItemIsInInventory (itemToRemove)) {
				Debug.Log ("Такого предмета в инвентаре нет");
			} else
				if ( !itemToRemove.Stackable ){
					Debug.Log ("Ошибка типа. Инвентарь на стакается, хотя вызвана функция для удаления одного объекта из стакающегося инвентаря. \n Поэтому объект просто удаляется");
					RemoveFullStockOfItems (id);
				}
		}
	}

	public void RemoveFullStockOfItems(int id){ // Удаление первого попавшегося с данным id
		Item itemToRemove = database.FetchItemByID (id);
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == id) {
				if (slots [i].transform.childCount == 1) {
					Destroy (slots [i].transform.GetChild (0).gameObject);
				} else {
					Destroy ( buffer_obj.transform.GetChild(0).gameObject );
				}
				
				items[i] = new Item();
				break;
			}
		}
	}

	bool CheckIfItemIsInInventory(Item item){
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == item.ID)
				return true;
		}
		return false;
	}

	public float GetMassImpact(){
		float impact = 0;
		for (int i = 0; i < currentSlotAmount; i++) {
			if (slots [i].transform.childCount > 0) {
				impact += items[ i ].massImpact;
			}
		}
		return impact;
	}

	public float GetVelocityImpact(){
		float impact = 0;
		for (int i = 0; i < currentSlotAmount; i++) {
			if (slots [i].transform.childCount > 0) {
				impact += items[ i ].velocityImpact;
			}
		}
		return impact;
	}

	public float GetAccelerationImpact(){
		float impact = 0;
		for (int i = 0; i < currentSlotAmount; i++) {
			if (slots [i].transform.childCount > 0) {
				impact += items[ i ].accelerationImpact;
			}
		}
		return impact;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.I)) {
			isShowing = !isShowing;
			inventoryPanel.SetActive (isShowing);
			tooltip.Deactivate ();
			//tooltip.SetActive (isShowing);
			Debug.Log ("Key down");
		}

		if (Input.GetKeyDown (KeyCode.L)) {
			RemoveOneItem (0);
		}
		if (Input.GetKeyDown (KeyCode.K)) {
			RemoveOneItem (1);
		}
	}
}
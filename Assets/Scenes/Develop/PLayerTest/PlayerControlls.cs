using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour {


	GameObject inv_GameObject;
	Inventory inv_class;

	public float INITIAL_MASS = 1f;
	public float ACCELERATION_FROM_INPUT = 0.4f;
	public float VECLOCITY_MINIMUM = 0.01f;
	public float VELOCITY_MAXIMUM = 100f;
	public float ROTATION_SPEED = 0.2f;


	private float current_mass;
	private Vector2 current_velocity;
	private Vector2 current_acceleration;

	// Use this for initialization
	void Start () {
		inv_GameObject = GameObject.FindWithTag ("Inventory");
		inv_class = inv_GameObject.GetComponent<Inventory>();
			
		current_velocity = Vector2.zero;
		current_acceleration = Vector2.zero;
		current_mass = INITIAL_MASS;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos = Input.mousePosition;
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);
		Vector2 dir = new Vector2 (mousePos.x - transform.position.x, mousePos.y - transform.position.y);
		transform.up = Vector2.Lerp(transform.up, -dir, ROTATION_SPEED);
	}


    void FixedUpdate()
    {
        float modified_mass = this.ModifyMassFromInventory();
        float coef = (1 / (modified_mass / 5));
        if (coef > 1) coef = 1;
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            current_acceleration.x += ACCELERATION_FROM_INPUT * Input.GetAxisRaw("Horizontal") * coef ;
        }
        if(Input.GetAxisRaw("Vertical") != 0)
        {
            current_acceleration.y += ACCELERATION_FROM_INPUT * Input.GetAxisRaw("Vertical") * coef;
        }
        current_acceleration = this.ModifyAccelerationFromInventory();
        current_velocity = current_acceleration;
        current_velocity = this.ModifyVelocityFromInventory();
        transform.position += new Vector3(current_velocity.x, current_velocity.y);
        current_acceleration = Vector2.Lerp(current_acceleration, Vector2.zero, 1 / modified_mass);
        if (Mathf.Abs(current_velocity.x) <= VECLOCITY_MINIMUM)
            current_velocity.x = 0;
        if (Mathf.Abs(current_velocity.y) <= VECLOCITY_MINIMUM)
            current_velocity.y = 0;
        if (Mathf.Abs(current_acceleration.x) <= VECLOCITY_MINIMUM)
            current_acceleration.x = 0;
        if (Mathf.Abs(current_acceleration.y) <= VECLOCITY_MINIMUM)
            current_acceleration.y = 0;
    }

    Vector2 ModifyFromMass(Vector2 initial) {
		Vector2 val = initial * this.current_mass;
		return val;
	}

	float ModifyMassFromInventory() {
		return (current_mass * inv_class.GetMassImpact() );
	}

	Vector2 ModifyAccelerationFromInventory() {
		return ( current_acceleration * inv_class.GetVelocityImpact () );
	}

	Vector2  ModifyVelocityFromInventory() {
		return ( current_velocity * inv_class.GetAccelerationImpact () );
	}

	
}

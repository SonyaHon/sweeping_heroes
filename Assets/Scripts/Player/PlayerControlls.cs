using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlls : MonoBehaviour {


	GameObject inv_GameObject;
	Inventory inv_class;

    public GameObject bulletPrefab;

	public float INITIAL_MASS = 1f;
	public float ACCELERATION_FROM_INPUT = 0.4f;
	public float VECLOCITY_MINIMUM = 0.01f;
	public float ROTATION_SPEED = 0.2f;

	private float current_mass;
	private Vector2 current_velocity;
	private Vector2 current_acceleration;

    public float DECAY_COEF = 0.2f;
    public float MASS_DECREAS_PERCENT_OVER_TIME = 0.1f;

    public float FIRE_COOLDOWN_MS = 1000;
    private float cur_timer = 0;
    private bool fired = false;

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

        if(fired)
        {
            cur_timer += Time.deltaTime;
            if(cur_timer >= FIRE_COOLDOWN_MS)
            {
                cur_timer = 0;
                fired = false;
            }
        }



        if (Input.GetAxisRaw("Fire1") > 0 && fired == false)
        {
            this.Fire();
            fired = true;
            
        }

	}

    private void Fire()
    {
        GameObject bl = Instantiate(bulletPrefab);
        bl.transform.position = transform.position;
        bl.transform.up = transform.up;
        bl.transform.Rotate(0, 0, 90);
    }

    private void FixedUpdate()
    {
        float modified_mass = this.ModifyMassFromInventory();
        if(modified_mass == 1.0f)
        {
            // we have instant stop

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                current_acceleration.x = ACCELERATION_FROM_INPUT * Input.GetAxisRaw("Horizontal");
            }
            else
            {
                current_acceleration.x = 0;
            }
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                current_acceleration.y = ACCELERATION_FROM_INPUT * Input.GetAxisRaw("Vertical");
            }
            else
            {
                current_acceleration.y = 0;
            }

            transform.position += new Vector3(current_acceleration.x, current_acceleration.y);
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                current_acceleration.x = ACCELERATION_FROM_INPUT * Input.GetAxisRaw("Horizontal");
            }
            else
            {
                current_acceleration.x = 0;
            }
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                current_acceleration.y = ACCELERATION_FROM_INPUT * Input.GetAxisRaw("Vertical");
            }
            else
            {
                current_acceleration.y = 0;
            }

            current_acceleration = this.ModifyAccelerationFromInventory();

            if(current_acceleration.x != 0)
            {
                current_velocity = Vector2.Lerp(current_velocity, new Vector2(current_acceleration.x, current_velocity.y), DECAY_COEF);
            }
            else
            {
                current_velocity = Vector2.Lerp(current_velocity, new Vector2(0, current_velocity.y), DECAY_COEF / modified_mass);
            }

            if (current_acceleration.y != 0)
            {
                current_velocity = Vector2.Lerp(current_velocity, new Vector2(current_velocity.x, current_acceleration.y), DECAY_COEF);
            }
            else
            {
                current_velocity = Vector2.Lerp(current_velocity, new Vector2(current_velocity.x, 0), DECAY_COEF / modified_mass);
            }


            if (Mathf.Abs(current_velocity.x) <= VECLOCITY_MINIMUM)
                current_velocity.x = 0;

            if (Mathf.Abs(current_velocity.y) <= VECLOCITY_MINIMUM)
                current_velocity.y = 0;

            if (current_velocity != Vector2.zero) this.DecreaseMass();

            transform.position += new Vector3(current_velocity.x, current_velocity.y);
        }
    }

    private void  AddToMass(float value)
    {
        this.current_mass += value;
    }

    private void DecreaseMass()
    {
        this.current_mass -= this.current_mass * MASS_DECREAS_PERCENT_OVER_TIME;
        if (this.current_mass < 1) this.current_mass = 1;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBulletControls : MonoBehaviour {

    public float SPEED;
    public float LIFETIME = 20;

    private float cur_timer = 0;

	void FixedUpdate () {
        transform.position += transform.right * -SPEED;	
	}

    private void Update()
    {
        cur_timer += Time.deltaTime;
        if(cur_timer >= LIFETIME)
        {
            Destroy(this.gameObject);
        }
    }
}

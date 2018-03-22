using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFriction : MonoBehaviour
{
    public float BaseFriction = 0.8f;

    private Rigidbody2D rigidBody;
    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rigidBody.velocity = rigidBody.velocity * BaseFriction;
    }
    // Update is called once per frame
    void Update()
    {

    }
}

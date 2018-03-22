using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerShooterAI : MonoBehaviour
{

    public string PlayerGameObjectName = "Player";

    public float MinDistance = 3f;
    public float TurningSpeed = 100f;
    public float MovementSpeed = 100f;
    public float VelocityMovementTreshold = 40f;
    public float SteeringForce = 0.5f;

    private GameObject player;

    private IBulletSpawner bulletSpawner;

    private Quaternion desiredRotation;
    private Vector2 desiredPosition;

    private bool moveTowardsPlayer = true;

    private Rigidbody2D rigidBody;


    void Start()
    {
        player = GameObject.Find(PlayerGameObjectName);
        rigidBody = GetComponent<Rigidbody2D>();
        bulletSpawner = GetComponentInChildren<IBulletSpawner>();
    }

    void FixedUpdate()
    {
        Vector2 diff = player.transform.position - gameObject.transform.position;
        float distance = diff.magnitude;
        Debug.Log(distance);
        if (distance > MinDistance)
        {
            desiredPosition = player.transform.position;
            moveTowardsPlayer = true;
        }
        else
        {
            desiredPosition = transform.position;
            moveTowardsPlayer = false;
        }
        if (rigidBody.velocity.magnitude > VelocityMovementTreshold || !moveTowardsPlayer)
            rigidBody.velocity *= SteeringForce;

        if (moveTowardsPlayer)
            rigidBody.AddForce((-(Vector2)transform.position + desiredPosition).normalized * MovementSpeed * Time.deltaTime, ForceMode2D.Force);
        else
        {
            if (bulletSpawner.CanSpawn())
            {
                bulletSpawner.SpawnBullet(diff.normalized);
            }
        }


        desiredRotation = Quaternion.Euler(0f, 0f, (180 / Mathf.PI) * Mathf.Atan2(diff.normalized.y, diff.normalized.x));
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, Time.deltaTime * TurningSpeed);
        //transform.position = Vector3.MoveTowards(transform.position, desiredPosition, MovementSpeed * Time.deltaTime);
    }
}

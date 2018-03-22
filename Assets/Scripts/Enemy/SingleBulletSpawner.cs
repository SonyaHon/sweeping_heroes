using System;
using UnityEngine;

public class SingleBulletSpawner : MonoBehaviour, IBulletSpawner
{
    public GameObject BulletPrefab;

    public float ShotInterval = 0.5f;
    private float elapsedTimeSinceShot = 0f;
    private bool canSpawn = true;

    public bool CanSpawn()
    {
        return canSpawn;
    }

    public void SpawnBullet(Vector2 trajectory)
    {
        if (!CanSpawn())
            return;

        canSpawn = false;
        elapsedTimeSinceShot = 0;

        GameObject bulletObj = Instantiate(BulletPrefab, transform.position, transform.rotation).gameObject;
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        Rigidbody2D rigidbody2D = bulletObj.GetComponent<Rigidbody2D>();

        rigidbody2D.velocity = bullet.DeafaultVelocity * trajectory;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTimeSinceShot >= ShotInterval)
        {
            elapsedTimeSinceShot = ShotInterval;
            canSpawn = true;
        }
        else
            elapsedTimeSinceShot += Time.deltaTime;
    }
}

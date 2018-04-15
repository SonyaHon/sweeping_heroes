using UnityEngine;

public interface IBulletSpawner
{
    void SpawnBullet(Vector2 trajectory);
    bool CanSpawn();
}
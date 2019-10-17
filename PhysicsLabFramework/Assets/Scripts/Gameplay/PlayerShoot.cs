using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletSpawnpoint;

    private const float BULLET_LIFETIME = 2.5f;

    private List<GameObject> bulletList;

    private void ShootBullet()
    {
        var newBullet = Instantiate(bulletPrefab, bulletSpawnpoint.transform.position, GetComponentInParent<Transform>().rotation);

        bulletList.Add(newBullet);
        Destroy(newBullet, BULLET_LIFETIME);
    }

    private void MoveBullets()
    {
        foreach (var bullet in bulletList)
        {
            var p2d = bullet.GetComponent<Particle2DComponent>();

            p2d.AddForce(ForceGenerator.GenerateForce_Gravity(p2d.mass, -bullet.transform.up));
        }
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
            // ShootBullet();
        // MoveBullets();
    }

    private void Awake()
    {
        bulletList = new List<GameObject>();
    }
}

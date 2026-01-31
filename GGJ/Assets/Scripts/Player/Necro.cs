using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necro : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float projectileRange = 4f;
    [SerializeField] private float projectileImpactRadius = 2f;
    [SerializeField] private GameObject _projectilePrefab;

    private float projectileIntialSpeed, projectileTime;
    

    public void FireNecroProjectile(Vector3 direction)
    {
        //projectileDirection = Quaternion.AngleAxis(45f, transform.right) * direction;
    }
}

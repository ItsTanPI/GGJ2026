using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Interaction;
using Player;
using UnityEngine;
using Utilities;

public class Necro : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float projectileRange = 4f;
    [SerializeField] private float projectileImpactRadius = 2f;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private LayerMask _skeletonMask;
    [SerializeField] private float necroCooldown = 1f;

    [Header("Puppet Settings")]
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    
    public bool IsPuppeteering => victim != null;
    
    private float projectileIntialSpeed, projectileTime;
    private Vector3 projectileDirection;
    private Vector3 startPosition, forward, up;

    private Transform currentProjectile = null;

    private bool isCoolingDown = false;
    private Skeleton.Skeleton victim = null;

    private MaskManager _maskManager;
    
    private void Start()
    {
        _maskManager = GetComponent<MaskManager>();
    }

    public void FireNecroProjectile()
    {
        if(isCoolingDown || victim) return;
        
        projectileDirection = Quaternion.AngleAxis(45f, transform.right) * transform.forward;
        projectileIntialSpeed = Mathf.Sqrt(projectileRange * Physics.gravity.magnitude);
        projectileTime = 1.414f * projectileIntialSpeed / Physics.gravity.magnitude;
        
        currentProjectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity).transform;
        currentProjectile.GetComponent<NecroProjectile>().OnHit += ProjectileHitSomething;
        StopAllCoroutines();
        StartCoroutine(nameof(DriveProjectile));
        StartCoroutine(Cooldown());
    }

    private void ProjectileHitSomething(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, projectileImpactRadius, _skeletonMask);
            
        for (int i = 0; i < hits.Length; i++)
        {
            Skeleton.Skeleton skeleton = hits[i].GetComponent<Skeleton.Skeleton>();
            if (skeleton)
            {
                victim = skeleton;
                _targetGroup.AddMember(victim.transform, 1f, 2f);
                victim.Resurrect();
            }
        }
    }
    
    //Player shoots projectile
    //Victim found
    //Camera target group adds skeleton puppet
    //Skeleton updates input on its own
    //RT triggers puppet collapse and mask throw charging
    //Repeat
    
    IEnumerator DriveProjectile()
    {
        float timer = 0f;
        startPosition = transform.position;
        forward = transform.forward;
        up = transform.up;

        while (timer < projectileTime * 2f)
        {
            float x = 0.707f * projectileIntialSpeed * timer;
            float y = 0.707f * projectileIntialSpeed * timer - Physics.gravity.magnitude * 0.5f * timer * timer;
            
            Vector3 projectilePos = startPosition + forward * x + up * y;
            currentProjectile.transform.position = projectilePos;
            
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        if (!victim) return;
        victim.Move(_maskManager.MoveInput, _maskManager.LookInput);
    }

    public void KillSkeleton()
    {
        if(!victim) return;
        _targetGroup.RemoveMember(victim.transform);
        victim.Die();
        victim = null;
    }
    
    IEnumerator Cooldown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(necroCooldown);
        isCoolingDown = false;
    }
}
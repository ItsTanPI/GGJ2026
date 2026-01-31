using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Throw : MonoBehaviour
{
    [SerializeField] Transform Hand;
    [SerializeField] Transform HandSlot;

    [SerializeField] Animator _animatorController;
    [SerializeField] float MaxForce;
    [SerializeField] float MinForce;
    [SerializeField] float MaxTime;
    [SerializeField] float CoolDownTime;
    [SerializeField] bool canThrow;
    [SerializeField] bool isHolding;
    public Rigidbody Item;
    bool Disable;

    [SerializeField] float Radius;
    [SerializeField] LayerMask LayertoPickUp;

    public AnimationSync _animationSync;

    private void Start()
    {
        canThrow = true;
        _animationSync = GetComponentInChildren<AnimationSync>();
        _animationSync.ThrowObject.AddListener(() => ThrowItemInHand());

    }

    bool isCharging = false;
    float holdTime = 0;
    private void Update()
    {
        if (Item)
        {
            isHolding = true;


            if (isCharging)
            {
                holdTime += Time.deltaTime;
                holdTime = Mathf.Min(holdTime, MaxTime);

                float chargeNormalized = holdTime / MaxTime;

                _animatorController.Play("Charge", 1, chargeNormalized);

            }

            Item.Sleep();
            Item.position = HandSlot.position;
        }
        else
        {
            isHolding = false;
        }


        _animatorController.SetBool("Holding", isHolding);
    }




    public void StartCharging()
    {
        if (Disable) return;
        if (!canThrow) return;
        if (Item == null) return;

        holdTime = 0f;
        isCharging = true;
    }

    Vector2 lookDirection;
    public void ReleaseThrow(Vector2 direction)
    {
        if (!isCharging) return;
        isCharging = false;

        lookDirection = new Vector2(transform.forward.x, transform.forward.z);
        _animatorController.SetTrigger("Throw");
    }


    public void ThrowItemInHand()
    {


        if (Disable) return;
        if (!canThrow) return;
        if (Item == null) return;

        float chargePercent = Mathf.Clamp01(holdTime / MaxTime);
        chargePercent = Mathf.SmoothStep(0f, 1f, chargePercent);

        float finalForce = Mathf.Lerp(MinForce, MaxForce, chargePercent);

        Item.position = HandSlot.position;
        Item.velocity = Vector3.zero;
        Item.angularVelocity = Vector3.zero;

        Vector3 throwDir = new Vector3(lookDirection.x, 1f, lookDirection.y).normalized;
        Item.AddForce(throwDir * finalForce, ForceMode.Impulse);

        Item = null;

        _animatorController.SetLayerWeight(1, 1);

        StartCoroutine(CoolDown());
    }

    public void GetFirstObjectOnLayer()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, Radius, LayertoPickUp);

        if (hits.Length == 0) return;

        PickUp PUS = hits[0].GetComponentInParent<PickUp>();
        if (PUS == null) return;

        if (PUS.CurrentParrent != null && PUS.CurrentParrent != this)
        {
            PUS.CurrentParrent.Item = null;
        }

        PUS.CurrentParrent = this;

        Item = hits[0].attachedRigidbody;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green; 
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

    IEnumerator CoolDown()
    {
        canThrow = false;
        yield return new WaitForSeconds(CoolDownTime);
        canThrow = true;
    }

    private void OnDisable()
    {
        Disable = true;
    }

    private void OnEnable()
    {
        Disable = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] Animator _animatorController;
    [SerializeField] Trigger _Trigger;
    [SerializeField] int Damage;
    [SerializeField] float CoolDownTime;
    [SerializeField] bool canAttack;
    bool Disable;
    public AnimationSync _animationSync;


    private void Start()
    {
        canAttack = true;
        _animationSync = GetComponentInChildren<AnimationSync>();
        _animationSync.Damage.AddListener(() => DealDamage());
    }

    public void LightAttack()
    {
        if (Disable) return;
        if (!canAttack) return;
        _animatorController.SetTrigger("MeleeAttack");
    }

    public void DealDamage()
    {
        List<Health> healths = _Trigger.GetAllObjectsInRange<Health>();

        foreach (Health health in healths)
        {

            health.TakeDamage(Damage);
        }

        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(CoolDownTime);
        canAttack = true;
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

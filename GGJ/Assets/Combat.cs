using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] Animator _animatorController; 

    public void Attack()
    {
        _animatorController.SetLayerWeight(1, 1);
        _animatorController.SetTrigger("MeleeAttack");
        Debug.Log("ds");
    }

}

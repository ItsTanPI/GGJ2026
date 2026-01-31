using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationSync : MonoBehaviour
{
    public UnityEvent Damage;
    public UnityEvent ThrowObject;

    public void Attack()
    {
        Damage.Invoke();
    }

    public void Throw()
    {
        ThrowObject.Invoke();
    }
}

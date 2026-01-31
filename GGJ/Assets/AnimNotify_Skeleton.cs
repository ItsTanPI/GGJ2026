using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimNotify_Skeleton : StateMachineBehaviour
{
     public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Skeleton.Skeleton>().AnimNotify_SetCanPuppeteer(true);        
    }
}

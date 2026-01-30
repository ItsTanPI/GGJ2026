using System;
using UnityEngine;

namespace Masks
{
    public class MeleeMask : Mask
    {
        [SerializeField] private float meleeDelay = 0.1f;
        
        private float meleeTimer = 0f;
        
        public override void Activate()
        {
            base.Activate();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (meleeTimer >= meleeDelay)
                {
                    meleeTimer = 0f;
                    ExecuteMeleeAttack();
                }
                
                meleeTimer += Time.deltaTime;
            }
        }

        private void ExecuteMeleeAttack()
        {
            //TODO: Stuff on player character anim and stuff
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }
    }
}
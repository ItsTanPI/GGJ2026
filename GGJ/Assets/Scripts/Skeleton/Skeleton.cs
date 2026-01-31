using System;
using UnityEngine;

namespace Skeleton
{
    public class Skeleton : MonoBehaviour
    {
        private SkeletonMovement _movement;
        private Animator _animator;
        private bool _canPuppeteer = false;

        private void Start()
        {
            _movement = GetComponent<SkeletonMovement>();
            _animator = GetComponent<Animator>();
        }

        public void Resurrect()
        {
            _animator.SetTrigger("Resurrect");
        }

        public void Die()
        {
            _animator.SetTrigger("Death");
        }
        
        public void AnimNotify_SetCanPuppeteer(bool canPuppeteer)
        {
            _canPuppeteer = canPuppeteer;
        }
        
        public void Move(Vector2 moveInput, Vector2 lookInput)
        {
            if (!_canPuppeteer) return;
            _movement.Move(moveInput, lookInput);
        }
    }
}
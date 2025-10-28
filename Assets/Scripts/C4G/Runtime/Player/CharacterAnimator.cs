using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c4g
{
    public class CharacterAnimator : MonoBehaviour
    {
        public Animator Animator
        {
            get
            {
                if(_animator == null)
                {
                    _animator = GetComponentInChildren<Animator>();
                }

                return _animator;
            }
        }

        private Navigator _navigator;
        private Animator _animator;

        private void Start()
        {
            _navigator = GetComponent<Navigator>();
        }

        private void Update()
        {
            var animator = Animator;

            if (animator != null)
            {
                animator.SetFloat("Speed", _navigator.Speed);
            }
        }
    }
}
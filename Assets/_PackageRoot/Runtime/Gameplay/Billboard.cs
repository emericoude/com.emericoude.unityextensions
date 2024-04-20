using System;
using UnityEngine;

namespace Emericoude.Gameplay.Common
{
    /// <summary>
    /// A simple billboard implementation that should cover most billboard types.
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        public enum BillboardMethod
        {
            LookAtTarget,
            CopyTargetRotation
        }
        
        [Tooltip("The method to billboard this object.\n\n" +
                 "LookAtTarget: Looks directly the target, generally the camera.\n\n" +
                 "CopyTargetRotation: Copies the target's rotation (reversed). Generally more niche.")]
        public BillboardMethod rotationMethod = BillboardMethod.LookAtTarget;

        [Tooltip("How the rotation is applied to each axes. Set an axis to 0 if you don't want the billboard to have influence over that axis.")]
        public Vector3 rotationAxes = Vector3.one;

        [Tooltip("Set this to true to flip the billboard. Useful for certain things like world-space UI that generally needs to face away from the camera.")]
        public bool flipped = false;

        [Tooltip("If true, assigns the 'main' camera on start as the target. This only works if target is unassigned.")]
        [SerializeField] private bool defaultTargetIsMainCamera = true;
        
        [Tooltip("The billboard target.")]
        [SerializeField] private Transform target;

        private void Start()
        {
            if (defaultTargetIsMainCamera && target == null)
            {
                var mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    SetTarget(mainCamera.transform);
                }
            }
            
            enabled = target != null;
        }

        private void LateUpdate()
        {
            transform.rotation = this.GetBillboardRotation();
        }

        private Quaternion GetBillboardRotation()
        {
            Quaternion targetRotation = this.rotationMethod switch
            {
                BillboardMethod.LookAtTarget => Quaternion.LookRotation(target.position - transform.position).normalized,
                BillboardMethod.CopyTargetRotation => Quaternion.LookRotation(-target.transform.forward),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (flipped)
            {
                targetRotation = Quaternion.Euler(-targetRotation.eulerAngles.x, targetRotation.eulerAngles.y + 180, -targetRotation.eulerAngles.z);
            }

            Vector3 currentRotationEuler = transform.rotation.eulerAngles;
            Vector3 targetRotationEuler = targetRotation.eulerAngles;

            return Quaternion.Euler(
                Mathf.LerpUnclamped(currentRotationEuler.x, targetRotationEuler.x, rotationAxes.x),
                Mathf.LerpUnclamped(currentRotationEuler.y, targetRotationEuler.y, rotationAxes.y),
                Mathf.LerpUnclamped(currentRotationEuler.z, targetRotationEuler.z, rotationAxes.z)
            );
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            enabled = target != null;
        }
    }
}

using System;
using UnityEngine;

namespace Emericoude.Gameplay
{
    /// <summary> A simple billboard implementation that should cover most billboard types. </summary>
    public class Billboard : MonoBehaviour
    {
        public enum BillboardMethod
        {
            LookAtTarget,
            CopyTargetRotation
        }
        
        private static Camera staticallyCachedMainCamera;
        
        [Header("Settings: Target")]
        [Tooltip("If true, assigns the 'main' camera on start as the target. This only works if target is unassigned.")]
        [SerializeField] private bool defaultTargetIsMainCamera = true;
        
        [Tooltip("The billboard target.")]
        [SerializeField] private Transform target;
        
        [Header("Settings: Rotation")]
        [Tooltip("The method to billboard this object.\n\n" +
                 "LookAtTarget: Looks directly the target, generally the camera.\n\n" +
                 "CopyTargetRotation: Copies the target's rotation (reversed). Generally more niche.")]
        public BillboardMethod rotationMethod = BillboardMethod.LookAtTarget;

        [Tooltip("How the rotation is applied to each axes. Set an axis to 0 if you don't want the billboard to have influence over that axis.")]
        public Vector3 rotationAxes = Vector3.one;

        [Tooltip("Set this to true to flip the billboard. Useful for certain things like world-space UI that generally needs to face away from the camera.")]
        public bool flipped = false;

        [Header("Settings: Position")] 
        [Tooltip("A world-space offset from its parent. Useful if you want something to always stay on top of an object, such as a healthbar.")]
        public Vector3 worldSpaceOffset = Vector3.zero;
        
        private Vector3 cachedLocalPosition;
        
        protected virtual void Start()
        {
            cachedLocalPosition = transform.localPosition;
            
            if (defaultTargetIsMainCamera && target == null)
            {
                if (staticallyCachedMainCamera == null)
                {
                    staticallyCachedMainCamera = Camera.main;
                }

                SetTarget(staticallyCachedMainCamera?.transform);
            }

            enabled = target != null;
        }

        private void LateUpdate()
        {
            transform.position = GetBillboardPosition();
            transform.rotation = GetBillboardRotation();
        }

        private Vector3 GetBillboardPosition()
        {
            return transform.parent ? 
                transform.parent.position + cachedLocalPosition + worldSpaceOffset : 
                cachedLocalPosition + worldSpaceOffset;
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

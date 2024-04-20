using System.Collections;
using System.Collections.Generic;

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
        public BillboardMethod Method = BillboardMethod.LookAtTarget;

        [Tooltip("How the rotation is applied to each axes. Set an axis to 0 if you don't want the billboard to have influence over that axis.")]
        public Vector3 LookAxes = Vector3.one;

        [Tooltip("Set this to true to flip the billboard. Useful for certain things like world-space UI that generally needs to face away from the camera.")]
        public bool Flipped = false;

        private Transform target;

        private void Start()
        {
            target = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if (target == null) return;
            transform.rotation = this.GetBillboardRotation();
        }

        protected virtual Quaternion GetBillboardRotation()
        {
            Quaternion rotation = this.Method switch
            {
                BillboardMethod.LookAtTarget => Quaternion.LookRotation(target.position - transform.position).normalized,
                BillboardMethod.CopyTargetRotation => Quaternion.LookRotation(-target.transform.forward),
                _ => Quaternion.LookRotation(target.position - transform.position).normalized
            };

            if (Flipped)
            {
                rotation = Quaternion.Euler(-rotation.eulerAngles.x, rotation.eulerAngles.y + 180, -rotation.eulerAngles.z);
            }

            Vector3 currentRotationEuler = transform.rotation.eulerAngles;
            Vector3 targetRotationEuler = rotation.eulerAngles;

            return Quaternion.Euler(
                Mathf.LerpUnclamped(currentRotationEuler.x, targetRotationEuler.x, LookAxes.x),
                Mathf.LerpUnclamped(currentRotationEuler.y, targetRotationEuler.y, LookAxes.y),
                Mathf.LerpUnclamped(currentRotationEuler.z, targetRotationEuler.z, LookAxes.z)
            );
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}

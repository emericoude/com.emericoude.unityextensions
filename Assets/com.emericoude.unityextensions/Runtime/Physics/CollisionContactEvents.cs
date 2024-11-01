using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Emericoude
{
    /// <summary>
    /// A component that keeps track of active collisions, and throws an event if a new contact (with the same collider) has appeared. <para/>
    /// 
    /// As opposed to Unity's OnCollisionEnter, this will trigger if two objects are already in contact. <para/>
    /// 
    /// Use case scenario: You want to play a sound effect every time an object physically bumps with another, but notice that OnCollisionEnter doesn't trigger if the objects were already in contact.
    /// For instance, if a plank of wood is at 45 degree from the ground, and the bottom-most edge is in contact with the ground. Once the plank falls on the floor, OnCollisionEnter would not trigger, whereas the events here will.
    /// Note that in this example, you can use the collision parameter of the events to change volume, create cutoffs, etc. You may also want to limit how many sounds per millisecond interval can play. <para/>
    ///
    /// This can come with a significant performance cost depending on your needs, as it runs collision checks for every OnCollisionStay call, so use this wisely. Consider using DOTS if you need a more in-depth and custom approach.
    /// </summary>
    /// <remarks> This script only works if Physics.ReuseCollisionCallbacks is false (and will force it off)! </remarks>
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionContactEvents : MonoBehaviour
    {
        [Tooltip("Triggered when a new contact occurs (or more precisely when the contact amount with a gameObject is increased). As opposed to OnCollisionEnter, this will trigger if the two objects are already in contact. Triggers only once per frame, per gameObject in contact.")]
        public UnityEvent<Collision> onContactEnter = new UnityEvent<Collision>();

        [Tooltip("Triggered when a contact is lost (or more precisely when the contact amount with a gameObject is decreased). As opposed to OnCollisionExit, this will trigger if the two objects are still in contact. Triggers only once per frame, per gameObject in contact.")]
        public UnityEvent<Collision> onContactExit = new UnityEvent<Collision>();
        
        [DrawInDebugInfoBox]
        private readonly Dictionary<GameObject, Collision> activeCollisions = new Dictionary<GameObject, Collision>();

        private void Awake()
        {
            UnityEngine.Physics.reuseCollisionCallbacks = false;
        }

        private void OnCollisionEnter(Collision collision) {
            if (!this.activeCollisions.TryAdd(collision.gameObject, collision)) return;
            this.onContactEnter?.Invoke(collision);
        }

        private void OnCollisionStay(Collision currentCollision) {
            if (!this.activeCollisions.TryGetValue(currentCollision.gameObject, out var previousFrameCollision)) return;
        
            if (previousFrameCollision.contacts.Length < currentCollision.contacts.Length) {
                this.onContactEnter?.Invoke(currentCollision);
            }
            else if (previousFrameCollision.contacts.Length > currentCollision.contacts.Length) {
                this.onContactExit?.Invoke(currentCollision);
            }

            this.activeCollisions[currentCollision.gameObject] = currentCollision;
        }

        private void OnCollisionExit(Collision collision) {
            if (!this.activeCollisions.Remove(collision.gameObject)) return;
            this.onContactExit?.Invoke(collision);
        }
    }
}
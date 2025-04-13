using System;
using Emericoude.Helpers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

using static Emericoude.Navigation3D;

namespace Emericoude
{
    /// <summary>
    /// Sort of the equivalent to Unity's <see cref="Selectable"/>, but for 3D objects. It uses the same underlying systems (EventSystem & uGUI).
    /// </summary>
    public abstract class Navigable3D : MonoBehaviour, 
        IMoveHandler,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler
    {
        [Tooltip("Whether this element is interactable.")]
        [SerializeField] private bool interactable = true;
        public bool Interactable
        {
            get => this.interactable && this.isActiveAndEnabled;
            set
            {
                if (this.interactable == value) return;
                if (!value)
                {
                    this.Deselect();
                    this.ExitHover();
                }
                
                this.interactable = value; //run this after because of the interactable checks inside Deselect()
            }
        }
        
        public Navigation3D navigation = Navigation3D.DefaultNavigation;
        private new Collider collider;
        
        [SerializeField] private new Camera camera;
        public Camera Camera
        {
            get => this.camera;
            set
            {
                this.camera = value;
                #if UNITY_EDITOR || UNITY_DEVELOPMENT //mute warnings in non-development builds
                if (this.camera == null)
                {
                    Debug.LogWarning("Assigning a null camera, consider disabling interaction.", this);
                }
                else
                {
                    if (!this.camera.TryGetComponent(out PhysicsRaycaster physicsRaycaster))
                    {
                        Debug.LogWarning("Camera does not have a Physics Raycaster. Navigation 3D will not work with pointers.");
                    }
                }
                #endif
            }
        }

        public UnityEvent onSelect = new();
        public UnityEvent onDeselect  = new();
        public UnityEvent onHoverEnter = new();
        public UnityEvent onHoverExit = new();

        public bool IsHovered { get; private set; } = false;
        public bool IsSelected { get; private set; } = false;
        
        private RaycastHit[] navigationHits;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            if (this.collider == null && !this.TryGetComponent(out this.collider))
            {
                this.collider = (Collider)Undo.AddComponent(this.gameObject, typeof(BoxCollider));
                this.collider.isTrigger = true;
            }
        }
#endif

        protected virtual void Awake()
        {
            if (this.collider == null && !this.TryGetComponent(out this.collider))
            {
                Debug.LogWarning($"Missing collider on {this.gameObject.name}. {this.GetType()} component will be disabled.", this);
                this.interactable = false;
                this.enabled = false;
            }
        }

        protected virtual void Start()
        {
            this.navigationHits = new RaycastHit[this.navigation.SphereCastMaximumHits];
            if (this.camera == null) this.Camera = Camera.main;
        }

        protected virtual void OnDisable()
        {
            this.Deselect();
            this.ExitHover();
        }

        public void OnMove(AxisEventData eventData)
        {
            if (this.TryGetSelectableForNavigation(eventData, out GameObject selectable))
            {
                eventData.selectedObject = selectable;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this.EnterHover();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this.ExitHover();
        }

        public void OnSelect(BaseEventData eventData)
        {
            this.Select();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            this.Deselect();
        }
        
        public virtual void Select(BaseEventData eventData = null)
        {
            if (this.IsSelected) return;
            if (!this.Interactable) return;
            if (EventSystem.current == null) return;
            
            if (!EventSystem.current.alreadySelecting && EventSystem.current.currentSelectedGameObject != this.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(this.gameObject, eventData);
            }
            
            this.IsSelected = true;
            this.onSelect.Invoke();
        }

        public virtual void Deselect(BaseEventData eventData = null)
        {
            if (!this.IsSelected) return;
            if (!this.interactable) return; //using the field (and not the property) to avoid disable state conflicts
            if (EventSystem.current == null) return;

            if (EventSystem.current.currentSelectedGameObject == this.gameObject && !EventSystem.current.alreadySelecting)
            {
                EventSystem.current.SetSelectedGameObject(null, eventData);
            }
            
            this.IsSelected = false;
            this.onDeselect.Invoke();
        }

        protected virtual void EnterHover()
        {
            if (this.IsHovered) return;
            if (!this.Interactable) return;
            
            this.IsHovered = true;
            this.onHoverEnter.Invoke();
        }

        protected virtual void ExitHover()
        {
            if (!this.IsHovered) return;
            if (!this.interactable) return; //using the field (and not the property) to avoid disable state conflicts
            
            this.IsHovered = false;
            this.onHoverExit.Invoke();
        }
        
        #region Navigation Utilities

        private bool TryGetSelectableForNavigation(AxisEventData axisEventData, out GameObject selectable)
        {
            selectable = null;
            
            if (axisEventData.moveDir == MoveDirection.None) return false;
            if (this.navigation.NavigationMode == Mode.None) return false;
            if (this.navigation.NavigationMode == Mode.Horizontal && axisEventData.moveDir is MoveDirection.Up or MoveDirection.Down) return false;
            if (this.navigation.NavigationMode == Mode.Vertical && axisEventData.moveDir is MoveDirection.Left or MoveDirection.Right) return false;

            selectable = this.navigation.NavigationMode switch {
                Mode.Horizontal => this.GetSelectableInDirection(axisEventData, true),
                Mode.Vertical => this.GetSelectableInDirection(axisEventData, true),
                Mode.Automatic => this.GetSelectableInDirection(axisEventData, false),
                Mode.Explicit => this.navigation.GetExplicitNavigation(axisEventData),
                _ => throw new ArgumentOutOfRangeException()
            };

            return selectable != null;
        }

        private GameObject GetSelectableInDirection(AxisEventData axisEventData, bool clampToOrthogonals = false)
        {
            Vector3 inputDirection = clampToOrthogonals ? this.GetMoveDirectionVector(axisEventData.moveDir) : axisEventData.moveVector;
            Vector3 inputDirectionTransformed = this.GetNavigationDirectionTransformed(inputDirection);
            Debug.DrawRay(this.collider.bounds.center, inputDirectionTransformed * 2.0f, Color.red, 1f, true);

            if (this.navigation.AutomateSphereCastCalculationFromColliderBounds)
            {
                this.navigation.SphereCastRadius = this.collider.bounds.extents.AverageComponents();
                this.navigation.SphereCastMaximumDistance = this.collider.bounds.size.LargestComponent() * 2.0f; //the x2 is super arbitrary
            }
            
            int hitCount = UnityEngine.Physics.SphereCastNonAlloc(
                this.collider.bounds.center,
                this.navigation.SphereCastRadius,
                inputDirectionTransformed,
                this.navigationHits,
                this.navigation.SphereCastMaximumDistance,
                this.navigation.SphereCastLayer,
                QueryTriggerInteraction.Collide
            );
            
            float nearestDistance = float.PositiveInfinity;
            GameObject nearestSelectable = null;
            for (int i = hitCount - 1; i >= 0; i--)
            {
                RaycastHit hit = this.navigationHits[i];
                if (hit.collider == this.collider) continue;

                float distance = Vector3.Distance(this.collider.bounds.center, hit.collider.bounds.center);
                if (distance > nearestDistance) continue;

                if (!hit.collider.TryGetComponent(out Navigable3D selectable3D)) continue;
                if (!selectable3D.interactable) continue;
                nearestSelectable = selectable3D.gameObject;
                nearestDistance = distance;
            }

            return nearestSelectable;
        }

        private Vector3 GetNavigationDirectionTransformed(Vector3 rawMoveDirection)
        {
            if (rawMoveDirection == Vector3.zero) return Vector3.zero;
            return this.navigation.NavigationAxisMode switch
            {
                AxisMode.WorldSpace => rawMoveDirection,
                AxisMode.LocalSpace => this.transform.InverseTransformDirection(rawMoveDirection),
                AxisMode.CameraSpace => this.camera.transform.InverseTransformDirection(rawMoveDirection),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Vector3 GetMoveDirectionVector(MoveDirection moveDirection)
        {
            return moveDirection switch
            {
                MoveDirection.Left => Vector3.left,
                MoveDirection.Up => Vector3.up,
                MoveDirection.Right => Vector3.right,
                MoveDirection.Down => Vector3.down,
                MoveDirection.None => Vector3.zero,
                _ => throw new ArgumentOutOfRangeException(nameof(moveDirection), moveDirection, null)
            };
        }
        
        #endregion
    }
}

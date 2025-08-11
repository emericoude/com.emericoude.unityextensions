using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Emericoude
{
    public class Button3D : Navigable3D,
        IPointerClickHandler, ISubmitHandler
    {
        [Tooltip("Event triggered when the button is clicked (or submitted).")]
        public UnityEvent onClicked = new();
        
        public void OnPointerClick(PointerEventData eventData)
        {
            this.Click();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            this.Click();
        }

        public void Click()
        {
            if (!this.Interactable) return;
            this.onClicked?.Invoke();
        }
    }
}
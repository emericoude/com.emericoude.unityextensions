using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Emericoude
{
    public class Button3D : Navigatable3D,
        IPointerClickHandler, ISubmitHandler
    {
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
using UnityEngine.UIElements;

namespace Emericoude.Helpers
{
    public static class VisualElementHelpers
    {
        public static VisualElement AddEmptyVisualElement(this VisualElement visualElement)
        {
            var ve = new VisualElement();
            visualElement.Add(ve);
            return ve;
        }
    }
}
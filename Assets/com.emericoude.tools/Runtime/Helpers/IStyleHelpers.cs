using UnityEngine.UIElements;

namespace Emericoude.Helpers
{
    public static class StyleHelpers
    {
        public static void SetDisplay(this IStyle style, bool value)
        {
            style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        public static void SetPadding(this IStyle style, int padding)
        {
            style.paddingBottom = padding;
            style.paddingLeft = padding;
            style.paddingRight = padding;
            style.paddingTop = padding;
        }

        public static void SetBorderRadius(this IStyle style, int radius)
        {
            style.borderBottomLeftRadius = radius;
            style.borderBottomRightRadius = radius;
            style.borderTopLeftRadius = radius;
            style.borderTopRightRadius = radius;
        }

        public static void SetMargin(this IStyle style, int margin)
        {
            style.marginBottom = margin;
            style.marginLeft = margin;
            style.marginRight = margin;
            style.marginTop = margin;
        }
    }
}
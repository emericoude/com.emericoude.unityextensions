using System;
using Sirenix.OdinInspector;

namespace Emericoude.Attributes
{
    /// <summary> Removes label and foldout from non-MonoBehaviour. Does nothing if you do not have Odin Inspector. </summary>
    /// <remarks> Combines the following attributes from Odin: [HideLabel], [InLineProperty]. </remarks>
#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [HideLabel, InlineProperty]
#endif
    public class DrawFieldOnlyAttribute : Attribute { }
}
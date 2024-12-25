using System;
using Sirenix.OdinInspector;

namespace Emericoude.Attributes
{
    /// <summary> Displays the field or property inside an "Info" box. Does nothing if you do not have Odin Inspector. </summary>
    /// <remarks> This combines the following attributes from Odin: [ShowInInspector], [ReadOnly], [VerticalGroup] and [FoldoutGroup]. </remarks>
#if ODIN_INSPECTOR
    [IncludeMyAttributes]
    [ShowInInspector, ReadOnly, VerticalGroup("Info Parent", PaddingTop = 8f, PaddingBottom = 8f, Order = 999), FoldoutGroup("Info Parent/Debug Info")]
#endif
    public class DrawInDebugInfoBoxAttribute : Attribute { }
}
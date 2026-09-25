using System;
using JetBrains.Annotations;

namespace OpenTabletDriver.Plugin.Attributes
{
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access, ImplicitUseTargetFlags.Itself)]
    public class OnPropertiesLoadedAttribute : Attribute;
}

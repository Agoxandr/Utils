using System;

namespace Agoxandr.Utils
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class HideScriptField : Attribute { } 
}
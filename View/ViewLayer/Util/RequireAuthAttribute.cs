using System;

namespace ViewLayer.Util
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RequireAuthAttribute : Attribute
    {
        public string Roles { get; private set; }

        public RequireAuthAttribute(string roles)
        {
            Roles = roles;
        }
    }
}

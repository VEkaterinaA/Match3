using System;
using UnityEngine;

namespace Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ReadOnlyFieldAttribute : PropertyAttribute
    {
    }
}
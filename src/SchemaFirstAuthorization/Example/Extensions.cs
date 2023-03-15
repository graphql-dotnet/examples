using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Example
{
    public static class Extensions
    {
        public static List<Type> WithAttribute<T>(this Assembly assembly)
            where T : Attribute
        {
            return assembly.GetTypes().Where(x => x.GetCustomAttributes<T>().Length > 0).ToList();
        }

        public static T[] GetCustomAttributes<T>(this Type type)
            where T : Attribute
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>().ToArray();
        }
    }
}
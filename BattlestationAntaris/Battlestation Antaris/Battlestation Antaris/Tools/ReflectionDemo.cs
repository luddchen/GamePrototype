using System;
using System.Reflection;

namespace Battlestation_Antaris.Tools
{
    public class ReflectionDemo
    {

        public static void PrintAttribute(Object obj, String attributeName)
        {
            FieldInfo info = obj.GetType().GetField(attributeName);
            Console.Out.WriteLine(info.GetValue(obj));
        }

        public static Object CallMethod(Object obj, String methodName, Object[] parameters)
        {
            MethodInfo info = obj.GetType().GetMethod(methodName);
            return info.Invoke(obj, parameters);
        }

    }
}

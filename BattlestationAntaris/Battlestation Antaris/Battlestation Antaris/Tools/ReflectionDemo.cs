using System;
using System.Reflection;

namespace Battlestation_Antaris.Tools
{
    public class ReflectionDemo
    {

        public abstract class ReflectiveCall<T, V>
        {
            public T obj;
            public String methodName;
            public Object[] parameters;
            public abstract String resultFormat(V obj);
        }

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


        public static String CallMethod<T, V>(ReflectiveCall<T, V> refCall)
        {
            V result = (V)CallMethod(refCall.obj, refCall.methodName, refCall.parameters);
            return refCall.resultFormat(result);
        }

    }
}

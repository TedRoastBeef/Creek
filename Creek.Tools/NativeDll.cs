using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;

namespace Creek.Tools
{
    internal class DynamicDllImportMetaObject : DynamicMetaObject
    {
        public DynamicDllImportMetaObject(Expression expression, object value)
            : base(expression, BindingRestrictions.Empty, value)
        {
        }

        public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
        {
            Type returnType = GetMethodReturnType(binder);
            var types = new Type[args.Length];
            var arguments = new Expression[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                Type type = args[i].LimitType;
                Expression expression = args[i].Expression;
                dynamic typedParameterExpression = expression;
                if (typedParameterExpression.IsByRef)
                {
                    types[i] = type.MakeByRefType();
                }
                else
                {
                    types[i] = type;
                }
                arguments[i] = expression;
            }
            MethodInfo method = (base.Value as DynamicDllImport).GetInvokeMethod(binder.Name, returnType, types);
            Expression callingExpression;
            if (method.ReturnType == typeof (void))
            {
                callingExpression = Expression.Block(Expression.Call(method, arguments),
                                                     Expression.Default(typeof (object)));
            }
            else
            {
                callingExpression = Expression.Convert(Expression.Call(method, arguments), typeof (object));
            }
            BindingRestrictions bindingRestrictions = BindingRestrictions.GetTypeRestriction(Expression,
                                                                                             typeof (DynamicDllImport));
            return new DynamicMetaObject(callingExpression, bindingRestrictions);
        }

        private Type GetMethodReturnType(InvokeMemberBinder binder)
        {
            var types =
                binder.GetType().GetField("m_typeArguments", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(
                    binder) as IList<Type>;
            if ((types != null) && (types.Count > 0))
            {
                return types[0];
            }
            return null;
        }
    }

    public class DynamicDllImport : DynamicObject
    {
        public CallingConvention CallingConvention = CallingConvention.Cdecl;
        public CharSet CharSet = CharSet.Auto;

        private AssemblyBuilder assemblyBuilder;

        private string assemblyName;
        internal string dllName;
        private int methodIndex;
        private ModuleBuilder moduleBuilder;

        public DynamicDllImport(string dllName)
        {
            this.dllName = dllName;
        }

        public DynamicDllImport(string dllName, CharSet charSet = CharSet.Auto,
                                CallingConvention callingConvention = CallingConvention.Cdecl)
        {
            this.dllName = dllName;
            CharSet = charSet;
            CallingConvention = callingConvention;
        }

        public string DllName
        {
            get { return dllName; }
        }

        internal string AssemblyName
        {
            get
            {
                if (assemblyName == null)
                {
                    assemblyName = GetAssemblyName();
                }
                return assemblyName;
            }
            set { assemblyName = value; }
        }

        private string GetAssemblyName()
        {
            return (new FileInfo(dllName)).Name;
        }

        private string GetDefineTypeName(string methodName)
        {
            return string.Format("{0}_{1}", methodName, Interlocked.Increment(ref methodIndex));
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new DynamicDllImportMetaObject(parameter, this);
        }

        public MethodInfo GetInvokeMethod(string methodName, Type returnType, Type[] types)
        {
            string entryName = methodName;
            if (assemblyBuilder == null)
            {
                var assemblyName = new AssemblyName(AssemblyName);
                assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
                moduleBuilder = assemblyBuilder.DefineDynamicModule(AssemblyName);
            }

            TypeBuilder defineType = moduleBuilder.DefineType(GetDefineTypeName(methodName));
            MethodBuilder methodBuilder = defineType.DefinePInvokeMethod(methodName, dllName, entryName,
                                                                         MethodAttributes.Public |
                                                                         MethodAttributes.Static |
                                                                         MethodAttributes.PinvokeImpl,
                                                                         CallingConventions.Standard,
                                                                         returnType, types,
                                                                         CallingConvention, CharSet);
            if ((returnType != null) && (returnType != typeof (void)))
            {
                methodBuilder.SetImplementationFlags(MethodImplAttributes.PreserveSig |
                                                     methodBuilder.GetMethodImplementationFlags());
            }
            Type type = defineType.CreateType();

            MethodInfo method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
            return method;
        }
    }
}
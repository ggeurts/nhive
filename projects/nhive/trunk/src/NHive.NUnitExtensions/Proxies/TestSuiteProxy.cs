namespace NHive.NUnitExtensions.Proxies
{
    using System;
    using System.Reflection;
    using NUnit.Core;
    using System.Diagnostics;
    using System.Reflection.Emit;

    internal class TestSuiteProxy
    {
        private static readonly ConstructorInfo _ctorByName;
        private static readonly Proc<object, Test> _addObject;
        private static readonly Proc<object, Type> _addType;

        private Test _testSuite;

        static TestSuiteProxy()
        {
            try
            {
                _ctorByName = NUnitType.Core.TestSuite.GetConstructor(new Type[] { typeof(string) });
                _addObject = CreateProc<Test>(NUnitType.Core.TestSuite, "Add");
                _addType = CreateProc<Type>(NUnitType.Core.TestSuite, "Add");
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
                throw;
            }
        }

        public TestSuiteProxy(string name)
        {
            _testSuite = (Test) _ctorByName.Invoke(new object[] { name });
        }

        public void Add(Test fixture)
        {
            _addObject(_testSuite, fixture);
        }

        public void Add(Type fixtureType)
        {
            _addType(_testSuite, fixtureType);
        }

        public Test InnerTestSuite
        {
            get { return _testSuite; }
        }

        private static Proc<object, T> CreateProc<T>(Type type, string methodName)
        {
            DynamicMethod dynamicMethod = CreateDynamicProc<T>(type, methodName);
            return (Proc<object, T>) dynamicMethod.CreateDelegate(typeof(Proc<object, T>));
        }

        private static DynamicMethod CreateDynamicProc<T>(Type type, string methodName)
        {
            MethodInfo targetMethod = type.GetMethod(methodName, new Type[] { typeof(T) });
            if (targetMethod == null)
            {
                throw new ArgumentException(
                    string.Format("Cannot find method '{0}({1})' on type '{2}'.",
                        methodName, typeof(T).Name, type.FullName));
            }

            DynamicMethod dynamicMethod = new DynamicMethod(
                methodName, typeof(void), new Type[] { typeof(object), typeof(T) }, type.Module);

            ILGenerator il = dynamicMethod.GetILGenerator();
            
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, type);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(targetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, targetMethod);
            il.Emit(OpCodes.Ret);

            return dynamicMethod;
        }
    }
}


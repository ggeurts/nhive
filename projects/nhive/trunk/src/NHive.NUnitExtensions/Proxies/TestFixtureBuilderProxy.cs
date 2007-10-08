namespace NHive.NUnitExtensions.Proxies
{
    using System;
    using System.Reflection;

    internal static class TestFixtureBuilderProxy
    {
        public static readonly Func<bool, Type> CanBuildFrom = 
            CreateDelegate<bool, Type>("CanBuildFrom");
        public static readonly Func<object, Type> BuildFromType =
            CreateDelegate<object, Type>("BuildFrom");
        public static readonly Func<object, object> BuildFromFixture =
            CreateDelegate<object, object>("BuildFrom");

        private static Func<R, X> CreateDelegate<R, X>(string methodName)
        {
            MethodInfo methodInfo = NUnitType.Core.TestFixtureBuilder.GetMethod(
                methodName, new Type[] { typeof(X) });
            return (Func<R, X>) Delegate.CreateDelegate(
                typeof(Func<R, X>), methodInfo);
        }
    }
}

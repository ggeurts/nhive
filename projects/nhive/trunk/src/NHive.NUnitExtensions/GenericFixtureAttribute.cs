namespace NHive.NUnitExtensions
{
    using System;

    /// <summary>
    /// Specifies type arguments for a generic test fixture. A generic test fixture
    /// is a generic type definitions that is decorated with one or more 
    /// <see cref="GenericFixtureAttribute" /> attributes. For each 
    /// <see cref="GenericFixtureAttribute" /> one closed generic
    /// test fixture type will be generated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class GenericFixtureAttribute : Attribute
    {
        private Type[] _genericTypeArgs;

        public GenericFixtureAttribute(params Type[] genericTypeArgs)
        {
            _genericTypeArgs = genericTypeArgs ?? Type.EmptyTypes;
        }

        public Type[] GenericTypeArguments
        {
            get { return _genericTypeArgs; }
        }
    }
}

namespace NHive.NUnitExtensions.Proxies
{
    using System;
    using System.Collections;
    using System.Reflection;
    using NUnit.Core.Extensibility;

    internal class SuiteBuildersProxy
    {
        private IExtensionPoint _extensionPoint;
        private IList _items;

        public SuiteBuildersProxy(IExtensionHost host)
        {
            _extensionPoint = host.GetExtensionPoint("SuiteBuilders");
            _items = GetSuiteBuildersList(_extensionPoint);
        }

        private IList GetSuiteBuildersList(IExtensionPoint extensionPoint)
        {
            Type type = extensionPoint.GetType();
            if (type.IsInterface)
            {
                type = type.UnderlyingSystemType;
            }

            FieldInfo[] fields = type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                if (typeof(IList).IsAssignableFrom(field.FieldType))
                {
                    return (IList) field.GetValue(_extensionPoint);
                }
            }

            return null;
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public ISuiteBuilder this[int index]
        {
            get { return (ISuiteBuilder) _items[index]; }
        }

        public void Install(object extension)
        {
            Install(extension, true);
        }

        public void Install(object extension, bool append)
        {
            _extensionPoint.Install(extension);
            if (!append && _items != null)
            {
                _items.Remove(extension);
                _items.Insert(0, extension);
            }
        }
    }
}

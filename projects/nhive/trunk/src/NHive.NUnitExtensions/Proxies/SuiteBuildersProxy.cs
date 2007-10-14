namespace NHive.NUnitExtensions.Proxies
{
    using System;
    using System.Collections;
    using System.Reflection;
    using NUnit.Core.Extensibility;

    /// <summary>
    /// Proxy for <see cref="NUnit.Core.Extensibility.SuiteBuilderCollection"/>. This proxy
    /// is only needed when certain suite builder extensions must be inserted before
    /// other suite builder extensions. This class should become obsolete as soon as
    /// NUnit provides an official mechanism to manipulate the priority of extensions.
    /// </summary>
    internal class SuiteBuildersProxy
    {
        // Reference to NUnit SuiteBuilders extension point.
        private IExtensionPoint _extensionPoint;
        // Reference to NUnit list of suite builder extensions.
        private IList _items;

        /// <summary>
        /// Constructs new <see cref="SuiteBuildersProxy"/> instance for
        /// a specific NUnit extension host.
        /// </summary>
        /// <param name="host">An NUnit extension host.</param>
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

        /// <summary>
        /// Returns total number of registered suite builders.
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Returns the suite builder extension at a specified index.
        /// </summary>
        /// <param name="index">The index of the suite builder to be returned.</param>
        /// <returns>The suite builder.</returns>
        public ISuiteBuilder this[int index]
        {
            get { return (ISuiteBuilder) _items[index]; }
        }

        /// <summary>
        /// Installs a new suite builder extension. The suite builder extension 
        /// is installed with a lower priority than all existing suite builder extensions.
        /// </summary>
        /// <param name="extension">The suite builder extension to be installed.</param>
        public void Install(ISuiteBuilder extension)
        {
            Install(extension, true);
        }

        /// <summary>
        /// Installs a new suite builder extension.
        /// </summary>
        /// <param name="extension">The suite builder extension to be installed.</param>
        /// <param name="append">Indicates whether the suite builder extension is
        /// to be installed with a lower priority than all existing suite builder extensions
        /// (a <c>true</c> value) or with a higher priority than all existing suite
        ///  builder extensions (a <c>false</c> value).
        /// </param>
        public void Install(ISuiteBuilder extension, bool append)
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

namespace NHive.NUnitExtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Core.Extensibility;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;

    [TestFixture]
    public class AddInTests
    {
        private IAddin _addin;
        private MockExtensionHost _host;

        [SetUp]
        public void SetUp()
        {
            _addin = new AddIn();
            _host = new MockExtensionHost();
            _addin.Install(_host);
        }

        [Test]
        public void InstallsAtLeastOneExtension()
        {
            Assert.That(_host.ExtensionCount, Is.GreaterThanOrEqualTo(1));
        }

        [Test]
        public void InstallsGenericFixtureBuilderExtension()
        {
            GenericFixtureBuilder extension;
            Assert.That(_host.TryFindExtension<GenericFixtureBuilder>(out extension));
        }

        /// <summary>
        /// NUnit extension point name constants
        /// </summary>
        public static class ExtensionPointName
        {
            public static readonly string SuiteBuilders = "SuiteBuilders";
        }

        /// <summary>
        /// Mock implementation of the NUnit <see cref="IExtensionHost"/> interface.
        /// </summary>
        private class MockExtensionHost : IExtensionHost
        {
            private Dictionary<string, MockExtensionPoint> _extensionPoints
                = new Dictionary<string, MockExtensionPoint>();

            public MockExtensionHost()
            {
                AddNewExtensionPoint(ExtensionPointName.SuiteBuilders);
            }

            #region Public query operations

            public int ExtensionCount
            {
                get
                {
                    int count = 0;
                    foreach (MockExtensionPoint extensionPoint in _extensionPoints.Values)
                    {
                        count += extensionPoint.ExtensionCount;
                    }
                    return count;
                }
            }

            public bool TryFindExtension<T>(out T extension)
            {
                foreach (MockExtensionPoint extensionPoint in _extensionPoints.Values)
                {
                    if (extensionPoint.TryFindExtension(out extension))
                    {
                        return true;
                    }
                }

                extension = default(T);
                return false;
            }

            #endregion

            #region IExtensionHost Members

            public IExtensionPoint[] ExtensionPoints
            {
                get 
                {
                    MockExtensionPoint[] result = new MockExtensionPoint[_extensionPoints.Count];
                    _extensionPoints.Values.CopyTo(result, 0);
                    return (IExtensionPoint[]) result;
                }
            }

            public ExtensionType ExtensionTypes
            {
                get { return ExtensionType.Core; }
            }

            public IFrameworkRegistry FrameworkRegistry
            {
                get { throw new NotImplementedException(); }
            }

            public IExtensionPoint GetExtensionPoint(string name)
            {
                return _extensionPoints[name];
            }

            #endregion

            #region Private methods

            private void AddNewExtensionPoint(string name)
            {
                MockExtensionPoint extensionPoint = new MockExtensionPoint(this, name);
                _extensionPoints.Add(name, extensionPoint);
            }

            #endregion
        }

        /// <summary>
        /// Mock implementation of the NUnit <see cref="IExtensionPoint"/> interface.
        /// </summary>
        private class MockExtensionPoint : IExtensionPoint
        {
            private string _name;
            private IExtensionHost _host;
            private List<object> _extensions = new List<object>();

            public MockExtensionPoint(IExtensionHost host, string name)
            {
                _host = host;
                _name = name;
            }

            #region Public operations

            public int ExtensionCount
            {
                get { return _extensions.Count; }
            }

            public bool TryFindExtension<T>(out T result)
            {
                foreach (object extension in _extensions)
                {
                    if (extension is T)
                    {
                        result = (T) extension;
                        return true;
                    }
                }

                result = default(T);
                return false;
            }

            #endregion

            #region IExtensionPoint Members

            public IExtensionHost Host
            {
                get { return _host; }
            }

            public string Name
            {
                get { return _name; }
            }

            public void Install(object extension)
            {
                _extensions.Add(extension);
            }

            public void Remove(object extension)
            {
                _extensions.Remove(extension);
            }

            #endregion
        }
    }
}

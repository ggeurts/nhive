namespace NHive.NUnitExtensions
{
    using System;
    using NUnit.Core.Extensibility;
    using NHive.NUnitExtensions.Proxies;
    
    /// <summary>
    /// NUnit addin that supports running generic test fixtures.
    /// </summary>
    [NUnitAddin(
        Type=ExtensionType.Core,
        Name="Generic Tests Addin", 
        Description="Generates and runs test fixtures from generic type definitions.")]
	public class AddIn : IAddin
	{
		#region IAddin Members

        public bool Install(IExtensionHost host)
		{
            SuiteBuildersProxy suiteBuilders = new SuiteBuildersProxy(host);
            suiteBuilders.Install(new GenericFixtureBuilder(), false);
			return true;
		}

        #endregion
	}
}

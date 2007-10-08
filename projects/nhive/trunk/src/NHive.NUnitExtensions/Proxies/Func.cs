namespace NHive.NUnitExtensions.Proxies
{
    internal delegate R Func<R>();
    internal delegate R Func<R, X>(X x);
    internal delegate R Func<R, X, Y>(X x, Y y);
}

namespace NHive.NUnitExtensions.Proxies
{
    public delegate R Func<R>();
    public delegate R Func<R, X>(X x);
    public delegate R Func<R, X, Y>(X x, Y y);
}

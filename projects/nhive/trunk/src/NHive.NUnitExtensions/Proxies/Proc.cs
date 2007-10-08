namespace NHive.NUnitExtensions.Proxies
{
    internal delegate void Proc();
    internal delegate void Proc<X>(X x);
    internal delegate void Proc<X, Y>(X x, Y y);
}

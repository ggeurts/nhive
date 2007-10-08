namespace NHive.NUnitExtensions.Proxies
{
    public delegate void Proc();
    public delegate void Proc<X>(X x);
    public delegate void Proc<X, Y>(X x, Y y);
}

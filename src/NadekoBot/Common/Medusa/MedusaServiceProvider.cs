using System.Runtime.CompilerServices;

namespace NadekoBot.Common.Medusa;

public class MedusaServiceProvider : IServiceProvider
{
    private readonly IServiceProvider _nadekoServices;
    private readonly WeakReference<IServiceProvider> _medusaServices;

    public MedusaServiceProvider(IServiceProvider nadekoServices, WeakReference<IServiceProvider> medusaServices)
    {
        _nadekoServices = nadekoServices;
        _medusaServices = medusaServices;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public object? GetService(Type serviceType)
    {
        if (!serviceType.Assembly.IsCollectible)
            return _nadekoServices.GetService(serviceType);

        return _medusaServices.TryGetTarget(out var target)
            ? target.GetService(serviceType)
            : null;
    }
}
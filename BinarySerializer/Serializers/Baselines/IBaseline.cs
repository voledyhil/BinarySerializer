using System;

namespace BinarySerializer.Serializers.Baselines
{
    public interface IBaseline : IDisposable
    {
        IBaseline GetCopyOrNull();
    }
}
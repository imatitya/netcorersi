using System.Diagnostics;
using NETCoreRemoteServices.Tests.Contracts;

namespace Contract
{
    public interface IMyCustomService
    {
        void DoSomething();

        int Sum(int x, int y);

        long Sum(long x, long y);

        string GetMachineName();

        string GetEnvironmentVariables();

        User GetAuthor();

        bool NonPrimitive(ProcessStartInfo info);

        string EmbeddedJObject();

        void ThrowAnException();
    }
}
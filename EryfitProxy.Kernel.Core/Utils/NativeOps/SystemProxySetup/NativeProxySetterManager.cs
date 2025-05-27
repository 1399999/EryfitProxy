

using System;
using System.Runtime.InteropServices;
using EryfitProxy.Kernel.Core.Proxy;
using EryfitProxy.Kernel.Utils.NativeOps.SystemProxySetup.Linux;
using EryfitProxy.Kernel.Utils.NativeOps.SystemProxySetup.macOs;
using EryfitProxy.Kernel.Utils.NativeOps.SystemProxySetup.Win;

namespace EryfitProxy.Kernel.Utils.NativeOps.SystemProxySetup
{
    public class NativeProxySetterManager : ISystemProxySetterManager
    {
        public ISystemProxySetter Get()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new WindowsSystemProxySetter();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return new LinuxProxySetter();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return new MacOsProxySetter();

            // TODO : return a "do-nothing" implementation instead of throwing
            throw new NotSupportedException("This platform is not supported");
        }
    }
}

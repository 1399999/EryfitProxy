

using System.Linq;
using EryfitProxy.Kernel.Core.Breakpoints;
using EryfitProxy.Kernel.Rules.Actions;

namespace EryfitProxy.Kernel
{
    internal class ProxyExecutionContext
    {
        public ProxyExecutionContext(EryfitSetting startupSetting)
        {
            BreakPointManager = new BreakPointManager(startupSetting
                                                      .AlterationRules.Where(r => r.Action is BreakPointAction)
                                                      .Select(a => a.Filter));
        }
        
        public BreakPointManager BreakPointManager { get; }
    }
}

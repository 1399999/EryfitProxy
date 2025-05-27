

using EryfitProxy.Kernel.Writers;

namespace EryfitProxy.Kernel.Rules
{
    public class StartupContext
    {
        public StartupContext(EryfitSetting setting, VariableContext variableContext, RealtimeArchiveWriter archiveWriter)
        {
            Setting = setting;
            VariableContext = variableContext;
            ArchiveWriter = archiveWriter;
        }

        public EryfitSetting Setting { get;  }

        public VariableContext VariableContext { get; }

        public RealtimeArchiveWriter ArchiveWriter { get; }
    }
}

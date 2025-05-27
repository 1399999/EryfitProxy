

using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Clients.Mock;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;
using YamlDotNet.Serialization;

namespace EryfitProxy.Kernel.Rules.Actions.HighLevelActions
{
    [ActionMetadata("Reply with eryfit welcome page")]
    public class MountWelcomePageAction : Action
    {
        public override FilterScope ActionScope => InternalScope;

        [JsonIgnore]
        [YamlIgnore]
        internal FilterScope InternalScope { get; set; } = FilterScope.DnsSolveDone;

        public override string DefaultDescription => "Reply with welcome page";

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            if (context.PreMadeResponse == null) {
                var bodyContent = Clients.Mock.BodyContent.CreateFromString(FileStore.welcome);

                context.PreMadeResponse = new MockedResponseContent(200,
                    bodyContent) {
                    Headers = {
                        new MockedResponseHeader("Content-Type", "text/html")
                    }
                };
            }

            return default;
        }
    }
}

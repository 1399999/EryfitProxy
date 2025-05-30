

using System.Collections.Generic;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients.Mock;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions.HighLevelActions
{
    /// <summary>
    ///     Mock completely a response.
    /// </summary>
    [ActionMetadata("Reply with a pre-made response from a raw text or file")]
    public class MockedResponseAction : Action
    {
        public MockedResponseAction(MockedResponseContent?  response)
        {
            Response = response ?? new MockedResponseContent(200, Clients.Mock.BodyContent.CreateFromString(""));
        }

        /// <summary>
        ///     The response
        /// </summary>
        [ActionDistinctive(Expand = true)]
        public MockedResponseContent Response { get; set; } 

        public override FilterScope ActionScope => FilterScope.RequestHeaderReceivedFromClient;

        public override string DefaultDescription => "Mock response";

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            context.PreMadeResponse = Response;

            return default;
        }

        public override IEnumerable<ActionExample> GetExamples()
        {
            {
                var bodyContent = Clients.Mock.BodyContent.CreateFromString("{ \"result\": true }");
                bodyContent.Type = BodyType.Json;

                yield return new ActionExample("Mock a response with a raw text",
                    new MockedResponseAction(new MockedResponseContent(200, bodyContent)
                    {
                        Headers = {
                            new ("DNT", "1"),
                            new ("X-Custom-Header", "Custom-HeaderValue"),
                        },
                    }));
            }

            {
                var bodyContent = Clients.Mock.BodyContent.CreateFromFile("/path/to/my/response.data"); 
                bodyContent.Type = BodyType.Binary;

                yield return new ActionExample("Mock a response with a file.",
                    new MockedResponseAction(new MockedResponseContent(404, bodyContent)
                    {
                        Headers = {
                            new ("Server", "Eryfit"),
                            new ("X-Custom-Header-2", "Custom-HeaderValue-2"),
                        },
                    }));
            }
        }

        public static MockedResponseAction BuildDefaultInstance()
        {
            var bodyContent = Clients.Mock.BodyContent.CreateFromString("{ \"result\": true }");

            bodyContent.Type = BodyType.Json;

            return new MockedResponseAction(new MockedResponseContent(200, bodyContent)
            {
                Headers = {
                    new ("DNT", "1"),
                    new ("X-Custom-Header", "Custom-HeaderValue")
                },
            });
        }
    }
}

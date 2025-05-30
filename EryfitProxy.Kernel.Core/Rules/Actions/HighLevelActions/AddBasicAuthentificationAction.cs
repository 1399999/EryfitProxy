

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Clients.Headers;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions.HighLevelActions
{
    [ActionMetadata("Add a basic authentication (RFC 7617) to incoming exchanges with an username and a password")]
    public class AddBasicAuthenticationAction : Action
    {
        public AddBasicAuthenticationAction(string username, string password)
        {
            Username = username;
            Password = password;
        }

        [ActionDistinctive]
        public string Username { get; set; }

        [ActionDistinctive]
        public string Password { get; set; }

        public override FilterScope ActionScope => FilterScope.RequestHeaderReceivedFromClient;

        public override string DefaultDescription { get; } = "Basic authentication";

        public override ValueTask InternalAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            var userName = Username.EvaluateVariable(context);
            var password = Password.EvaluateVariable(context);

            var auth = $"{userName}:{password}";
            var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));

            context.RequestHeaderAlterations.Add(new HeaderAlterationAdd("Authorization",
                $"Basic {base64}"));

            return default;
        }

        public override IEnumerable<ActionExample> GetExamples()
        {
            yield return new ActionExample("Add a basic authentication with username `lilou` and password `multipass`",
                new AddBasicAuthenticationAction("lilou", "multipass"));
        }
    }
}



using System.Collections.Generic;
using System.IO;
using System.Threading;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Misc.Streams;

namespace EryfitProxy.Kernel.Writers
{
    public class EventOnlyArchiveWriter : RealtimeArchiveWriter
    {
        public override void UpdateTags(IEnumerable<Tag> tags)
        {
        }

        protected override bool ExchangeUpdateRequired(Exchange exchange)
        {
            return false;
        }

        public override bool Update(ExchangeInfo exchangeInfo, CancellationToken cancellationToken)
        {
            return true;
        }

        public override void Update(ConnectionInfo connectionInfo, CancellationToken cancellationToken)
        {
        }

        protected override bool ConnectionUpdateRequired(Connection connection)
        {
            return false; 
        }

        protected override void InternalUpdate(DownstreamErrorInfo connectionInfo, CancellationToken cancellationToken)
        {

        }

        public override Stream CreateRequestBodyStream(int exchangeId)
        {
            return Stream.Null;
        }

        public override Stream CreateResponseBodyStream(int exchangeId)
        {
            return Stream.Null;
        }

        public override Stream CreateWebSocketRequestContent(int exchangeId, int messageId)
        {
            return Stream.Null;
        }

        public override Stream CreateWebSocketResponseContent(int exchangeId, int messageId)
        {
            return Stream.Null;
        }

        public override void ClearErrors()
        {

        }
        
    }
}

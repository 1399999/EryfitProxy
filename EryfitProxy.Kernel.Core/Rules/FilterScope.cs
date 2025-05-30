

using System.ComponentModel;

namespace EryfitProxy.Kernel.Rules
{
    /// <summary>
    ///     The filter scope defines the minimal timing where a filter can be run.
    ///     These specific timing are :
    ///     - OnAuthorityReceived : This scope denotes the moment eryfit is aware the destination authority.
    ///  In a regular proxy connection, tt will occur the moment where
    ///     eryfit parsed
    ///     CONNECT request.
    ///     - RequestHeaderReceivedFromClient : The moment when the full request header is parsed
    ///     - RequestBodyReceivedFromClient :  This timing is trigger only after the request body is sent to the upstream due
    ///     to streaming
    ///     - ResponseHeaderReceivedFromRemote : The moment where eryfit got the response header from the server
    ///     - ResponseBodyReceivedFromRemote :  (Aka complete) This timing is trigger only after the response body is sent to
    ///     the downstream due to streaming.
    ///     - OutOfScope : Indicates a filter that is not usable for live alteration. This kind of filter applies only for view
    ///     filters in Eryfit Desktop
    /// </summary>
    public enum FilterScope
    {
        [Description("This scope denotes the moment eryfit is aware the destination authority." +
                     " In a regular proxy connection, it will occur the moment where eryfit parsed the CONNECT request.")]
        OnAuthorityReceived,

        [Description("This scope occurs the moment eryfit parsed the request header receiveid from client")]
        RequestHeaderReceivedFromClient,

        [Description("This scope occurs the moment eryfit ends solving the DNS of the remote host")]
        DnsSolveDone,

        [Description("This scope occurs the moment eryfit received fully the request body from the client. In a full" +
                     "streaming mode which is the default mode, this event occurs when the full body is already fully sent to the remote server. ")]
        RequestBodyReceivedFromClient,

        [Description("This scope occurs the moment eryfit has done parsing the response header.")]
        ResponseHeaderReceivedFromRemote,

        [Description("This scope occurs the moment eryfit received the the response body from the server. " +
                     "In a full streaming mode (which is the default mode), this event occurs the the full body is already sent to the client.")]
        ResponseBodyReceivedFromRemote,

        [Description("Applied only on action. The action associated with this scope will copy his value from the triggering filter.")]
        CopySibling = 10000, 

        [Description("Means that the filter or action associated to this scope won't be trigger in the regular HTTP flow. This scope" +
                     " is applied only on view filter and internal actions.")]
        OutOfScope = 99999
    }
}

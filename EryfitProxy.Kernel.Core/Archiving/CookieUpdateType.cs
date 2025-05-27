namespace EryfitProxy.Kernel
{
    public enum CookieUpdateType
    {
        None, // no change
        AddedFromServer,
        AddedFromClient,
        UpdatedFromServer,
        UpdatedFromClient,
        RemovedByServer,
        RemovedByClient
    }
}
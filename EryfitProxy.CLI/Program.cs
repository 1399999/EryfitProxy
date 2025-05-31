namespace EryfitProxy.CLI;

internal class Program
{
    static async Task Main(string[] args)
    {
        Settings.Navigate();

        //Console.WriteLine(" ==================== Eryfit 0.0.0 ====================\n");

        //while (true) 
        //{
        //    Console.Write("> ");

        //    string[] inputVectors = Console.ReadLine().Trim().Split(' ');

        //    if (inputVectors[0] == "setting" || inputVectors[0] == "settings") 
        //    {
        //        Settings.Navigate(inputVectors);
        //    }
        //}

        //// Create a proxy instance
        //await using (var proxy = new Proxy(eryfitStartupSetting))
        //{
        //    var endpoints = proxy.Run();

        //    using var httpClient = new HttpClient(new HttpClientHandler()
        //    {
        //        // We instruct the HttpClient to use the proxy
        //        Proxy = new WebProxy($"http://127.0.0.1:{endpoints.First().Port}"),
        //        UseProxy = true
        //    });

        //    // Make a request to a remote website
        //    using var response = await httpClient.GetAsync("https://www.eryfit.io/hello");

        //    // Eryfit is in full streaming mode, this means that the actual body content 
        //    // is only captured when the client reads it. 

        //    await (await response.Content.ReadAsStreamAsync()).CopyToAsync(Stream.Null);
        //}

        //// Packing the output files must be after the proxy dispose because some files may 
        //// remain write-locked. 

        //// Pack the files into fxzy file. This is the recommended file format as it can holds raw capture datas. 
        //Packager.Export(tempDirectory, "mycapture.fxzy");

        //// Pack the files into a HAR file
        //Packager.ExportAsHttpArchive(tempDirectory, "mycapture.har");
    }
}

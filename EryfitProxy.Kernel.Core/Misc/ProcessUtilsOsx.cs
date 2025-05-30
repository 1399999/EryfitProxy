

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Misc.Streams;

namespace EryfitProxy.Kernel.Misc
{
    internal static class ProcessUtilsOsx
    {
        /// <summary>
        /// Try to register current process in a sudo session
        /// </summary>
        /// <param name="askPasswordPrompt"></param>
        /// <returns></returns>
        internal static async Task<bool> OsxTryAcquireElevation(string askPasswordPrompt)
        {
            // The following code solve issue where osascript is the only 
            // "dependencyless" way to run a graphical sudo command on osx
            // Unfortunately, osascript does not save the launching process root access
            // making eryfit re-ask the password every time ca configuration changed, which is annoying
            // for the final user. 
            // sudo does save the session, so we had to do the following trick. 

            // First we check if we can already sudo 

            var canElevated = await ProcessUtilX.CanElevated().ConfigureAwait(false);

            if (canElevated)
            {
                // There is a very tight window between the check process and the actual start 
                // where the root timestamp may expired, in this case stdin may be blocked forever. 
                // Make sure that subsequent sudo process is running with the -n option 

                return true;
            }

            // Now we need to ask the password via osascript 

            var numberTries = 3;  // We tries 3 times

            for (int i = 0; i < numberTries; i++)
            {
                var result =
                    await AskForElevation(askPasswordPrompt).ConfigureAwait(false);

                if (result == PasswordElevationRequestResult.OK)
                    return true;

                if (result == PasswordElevationRequestResult.Refused)
                    break;

                // Otherwise, we try again
            }

            return false;
        }

        private static async Task<PasswordElevationRequestResult> AskForElevation(string askPasswordPrompt)
        {
            var osascript = new ProcessStartInfo("osascript", $"-e \"Tell application \\\"System Events\\\" " +
                                                              $"to display dialog \\\"{askPasswordPrompt}\\\" " +
                                                              $"default answer \\\"\\\" with hidden answer\" -e \"text returned of result\"")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            var osascriptProcess = Process.Start(osascript)!;

            var buffer = await osascriptProcess.StandardOutput.BaseStream.ToArrayGreedyAsync().ConfigureAwait(false);

            await osascriptProcess.WaitForExitAsync().ConfigureAwait(false);

            if (osascriptProcess.ExitCode != 0)
            {
                return PasswordElevationRequestResult.Refused;
            }

            try
            {
                var checkStartInfo = new ProcessStartInfo("sudo", "-S -v")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardInput = true
                };

                var checkStartProcess = Process.Start(checkStartInfo)!;

                await checkStartProcess.StandardInput.BaseStream.WriteAsync(buffer).ConfigureAwait(false);
                await checkStartProcess.StandardInput.BaseStream.DisposeAsync().ConfigureAwait(false);

                await checkStartProcess.WaitForExitAsync().ConfigureAwait(false);

                return checkStartProcess.ExitCode == 0 ? PasswordElevationRequestResult.OK : PasswordElevationRequestResult.BadPassword;
            }
            finally
            {
                Array.Clear(buffer,0, buffer.Length); // Remove the password to minimize attack window 
            }
        }

        internal enum PasswordElevationRequestResult
        {
            Refused,
            BadPassword,
            OK
        }
    }
}

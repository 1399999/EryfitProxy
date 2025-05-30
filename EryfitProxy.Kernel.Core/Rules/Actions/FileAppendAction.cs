

using System.IO;
using System.Threading.Tasks;
using EryfitProxy.Kernel.Core;
using EryfitProxy.Kernel.Core.Breakpoints;

namespace EryfitProxy.Kernel.Rules.Actions
{
    [ActionMetadata("Write to a file. Captured variable are interpreted.")]
    public class FileAppendAction : MultipleScopeAction
    {
        /// <summary>
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="text"></param>
        public FileAppendAction(string filename, string? text)
        {
            Filename = filename;
            Text = text;
        }

        [ActionDistinctive(Description = "Filename")]
        public string Filename { get; }

        [ActionDistinctive(Description = "Text to write")]
        public string? Text { get; set; }

        [ActionDistinctive(Description = "Default encoding. UTF-8 if not any.")]
        public string? Encoding { get; set; }

        public override string DefaultDescription { get; } = "Write to file";

        public override ValueTask MultiScopeAlter(
            ExchangeContext context, Exchange? exchange, Connection? connection, FilterScope scope,
            BreakPointManager breakPointManager)
        {
            if (Text != null && !string.IsNullOrWhiteSpace(Filename)) {
                var fileName = Filename.EvaluateVariable(context)!;
                var finalText = Text.EvaluateVariable(context);

                var encoding = string.IsNullOrWhiteSpace(Encoding)
                    ? System.Text.Encoding.UTF8
                    : System.Text.Encoding.GetEncoding(Encoding);

                var directoryName = Path.GetDirectoryName(fileName);

                if (!string.IsNullOrWhiteSpace(directoryName))
                    Directory.CreateDirectory(directoryName);

                lock (this) {
                    File.AppendAllText(fileName, finalText, encoding);
                }
            }

            return default;
        }
    }
}

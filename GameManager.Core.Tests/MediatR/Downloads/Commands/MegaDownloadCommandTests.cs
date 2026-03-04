using System.IO;

using GameManager.Core.MediatR.Download.Commands;

namespace GameManager.Core.Tests.MediatR.Downloads.Commands
{
    public class MegaDownloadCommandTests
    {
        [IntegrationFact]
        public async Task DownloadWorks()
        {
            var dest = Path.Combine(Path.GetTempPath(), "MegaDownloadTest");
            var hut = new MegaDownloadCommandHandler();
            var res = await hut.Handle(new MegaDownloadCommand
            {
                DestinationFolder = dest,
                Url = "https://mega.nz/file/hA1EwR5R#FLPubpk9YzKLfwq-ZZjO1Ai4jtN3Mqj70fdP6LnYBqY",
                ProgressHandler = new Progress<double>(x => System.Diagnostics.Debug.WriteLine("{0}%", x))
            }, CancellationToken.None);

            File.Exists(res);
        }
    }
}

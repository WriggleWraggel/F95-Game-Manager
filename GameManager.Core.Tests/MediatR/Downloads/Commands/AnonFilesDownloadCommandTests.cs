using System.IO;

using GameManager.Core.MediatR.Download.Commands;

namespace GameManager.Core.Tests.MediatR.Downloads.Commands
{
    public class AnonFilesDownloadCommandTests
    {
        [IntegrationFact]
        public async Task DownloadWorks()
        {
            var dest = Path.Combine(Path.GetTempPath(), "AnonDownloadTest");
            var hut = new AnonFilesDownloadCommandHandler();
            var res = await hut.Handle(new AnonFilesDownloadCommand
            {
                DestinationFolder = dest,
                Url = "https://anonfiles.com/l819w6B2xd",
                ProgressHandler = new Progress<double>(x => System.Diagnostics.Debug.WriteLine("{0}%", x))
            }, CancellationToken.None);

            File.Exists(res);
        }
    }
}

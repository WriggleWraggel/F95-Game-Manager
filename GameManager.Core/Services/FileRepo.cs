using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameManager.Core.Data.Settings;

using MediatR;

namespace GameManager.Core.Services;

public interface IFileRepo
{
    List<string> GetArchiveFilePathsInFolder(string directory, bool recurseSubdirectories);
    List<string> GetCompressedFiles(string archivePath);
    List<string> GetGameExecutableFiles(string gamePath);
    void DeleteFile(string path);
    void MoveFile(string source, string dest, bool overrideIfExists = false);
    void MakeFolderSafely(string folderPath);
    List<string> GetFoldersInPath(string gamePath);
    void DeleteDirectory(string folder);
    void EmptyDirectory(string folder);
}

class FileRepo : IFileRepo
{
    public List<string> GetArchiveFilePathsInFolder(string directory, bool recurseSubdirectories)
    {
        var files = Directory.EnumerateFiles(
            directory,
            "*.*",
            new EnumerationOptions { RecurseSubdirectories = recurseSubdirectories }
        );

        var compressedFiles = files
            .Where(_ => SettingsConsts.CompressedFilesTypeExtensions
                .Any(x => _.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        return compressedFiles;
    }

    public List<string> GetCompressedFiles(string archivePath)
    {
        return Directory.EnumerateFiles(archivePath, "*.*")
            .Where(_ => SettingsConsts.CompressedFilesTypeExtensions
                .Any(x => _.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
            .Select(_ => _.Replace(archivePath, ""))
            .ToList();
    }

    public List<string> GetGameExecutableFiles(string gamePath)
    {
        return Directory.EnumerateFiles(gamePath, "*.*", SearchOption.AllDirectories)
            .Where(path =>
                SettingsConsts.GameExecutableFileExtensions
                    .Any(x => path.EndsWith(x, StringComparison.OrdinalIgnoreCase)) &&
               !SettingsConsts.GameExecuteablePathExlcusions
                    .Any(x => path.Contains(@$"\{x}\", StringComparison.OrdinalIgnoreCase))
            )
            .Select(_ => _.Replace(gamePath, ""))
            .ToList();
    }

    public void DeleteFile(string path)
    {
        File.Delete(path);
    }

    public void MoveFile(string source, string dest, bool overrideIfExists = false)
    {
        File.Move(source, dest, true);
    }

    public void MakeFolderSafely(string folderPath)
    {
        if ( !Directory.Exists(folderPath) )
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    public List<string> GetFoldersInPath(string gamePath)
    {
        return Directory.GetDirectories(gamePath).ToList();
    }

    public void DeleteDirectory(string folder) => Directory.Delete(folder, true);
    
    public void EmptyDirectory(string folder)
    {
        foreach (var file in Directory.EnumerateFiles(folder))
        {
            File.Delete(file);
        }

        foreach (var subDir in Directory.EnumerateDirectories(folder))
        {
            DeleteDirectory(subDir);
        }
    }
}
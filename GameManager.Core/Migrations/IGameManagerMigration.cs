using GameManager.Core.Data.Settings;
using GameManager.Core.Services;

namespace GameManager.Core.Migrations;

public interface IGameManagerMigration
{
    int MigrationOrder { get; }
    Task ApplyMigration(AppSettings settings);
}

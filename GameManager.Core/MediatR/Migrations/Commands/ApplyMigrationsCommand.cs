using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameManager.Core.Data.Settings;
using GameManager.Core.Migrations;

namespace GameManager.Core.MediatR.Migrations.Commands
{
    /// <summary>
    /// Returns the list of migrations that have been applied
    /// </summary>
    /// <param name="Migrations"></param>
    /// <param name="settings"></param>
    public record ApplyMigrationsCommand(List<string> Migrations, AppSettings settings) : IRequest<List<string>>;

    internal class ApplyMigrationsCommandHandler : IRequestHandler<ApplyMigrationsCommand, List<string>>
    {
        private readonly IEnumerable<IGameManagerMigration> _migrations;
        public ApplyMigrationsCommandHandler(IEnumerable<IGameManagerMigration> migrations)
        {
            _migrations = migrations;
        }

        public async Task<List<string>> Handle(ApplyMigrationsCommand request, CancellationToken cancellationToken)
        {
            //compare each migration to the list of migrations to apply
            var migrationsToApply = _migrations.Where(_ => request.Migrations.Contains(_.GetType().Name));

            //apply each migration and wait for them to complete
            var tasks = new List<Task>();
            foreach ( var migration in migrationsToApply )
                tasks.Add(migration.ApplyMigration(request.settings));

            await Task.WhenAll(tasks);

            return migrationsToApply.Select(_ => _.GetType().Name).ToList();
        }
    }
}

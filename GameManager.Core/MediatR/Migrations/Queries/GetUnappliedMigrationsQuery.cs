using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameManager.Core.Data.Settings;
using GameManager.Core.Migrations;

namespace GameManager.Core.MediatR.Migrations.Queries;

public record GetUnappliedMigrationsQuery(AppSettings Settings) : IRequest<List<string>>;

public class GetUnappliedMigrationsQueryHandler : IRequestHandler<GetUnappliedMigrationsQuery, List<string>>
{
    public async Task<List<string>> Handle(GetUnappliedMigrationsQuery request, CancellationToken cancellationToken)
    {
        var migrations = typeof(IGameManagerMigration).Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IGameManagerMigration)));

        return migrations
            .Where(m => !request.Settings.AppliedMigrations.Contains(m.Name))
            .Select(m => m.Name)
            .ToList();
    }
}

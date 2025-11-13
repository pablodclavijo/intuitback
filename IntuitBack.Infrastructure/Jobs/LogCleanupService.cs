using IntuitBack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntuitBack.Infrastructure.Jobs;

public class LogCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _intervalo = TimeSpan.FromHours(24);

    public LogCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var fechaLimite = DateTime.UtcNow.AddDays(-365);
                var logsAntiguos = await context.Logs
                    .Where(l => l.FechaCreacion < fechaLimite)
                    .ToListAsync(stoppingToken);

                if (logsAntiguos.Any())
                {
                    context.Logs.RemoveRange(logsAntiguos);
                    await context.SaveChangesAsync(stoppingToken);
                }
            }
            catch
            {
                
            }

            await Task.Delay(_intervalo, stoppingToken);
        }
    }
}

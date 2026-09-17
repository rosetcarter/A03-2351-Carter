using Fluxor;
using Day06Demo.Services;

namespace Day06Demo.Features.Dashboard.Store;

public class DashboardEffects(IDashboardService dashboardService)
{
    [EffectMethod]
    public async Task HandleLoadDashboardAction(
        LoadDashboardAction action, IDispatcher dispatcher)
    {
        var result = await dashboardService.GetDashboardDataAsync();
        if (result.IsSuccess)
        {
            var data = result.Value;
            dispatcher.Dispatch(new LoadDashboardSuccessAction(
                data.NotificationCount, data.ProductCount, data.UserName));
        }
        else
        {
            var errors = result.Errors.Select(e => e.ToString()).ToList();
            dispatcher.Dispatch(new LoadDashboardFailureAction(string.Join("; ", errors)));
        }
    }
}

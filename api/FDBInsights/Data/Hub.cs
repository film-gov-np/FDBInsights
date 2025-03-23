using Microsoft.AspNetCore.SignalR;

namespace FDBInsights.Data;

public class DashboardHub : Hub
{
    public async Task SendProducts()
    {
        //var products = productRepository.GetProducts();
        var products = "assa";
        await Clients.All.SendAsync("ReceivedProducts", products);
    }
}
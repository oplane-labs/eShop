using System.Net;

namespace eShop.WebApp.Services;

public class OrderingService(HttpClient httpClient)
{
    private readonly string remoteServiceBaseUrl = "/api/Orders/";

    public Task<OrderRecord[]> GetOrders()
    {
        return httpClient.GetFromJsonAsync<OrderRecord[]>(remoteServiceBaseUrl)!;
    }

    public Task CreateOrder(CreateOrderRequest request, Guid requestId)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, remoteServiceBaseUrl);
        requestMessage.Headers.Add("x-requestid", requestId.ToString());
        requestMessage.Content = JsonContent.Create(request);
        return httpClient.SendAsync(requestMessage);
    }

    public async Task<string> CreateShareLink(int orderId)
    {
        var response = await httpClient.PostAsync($"{remoteServiceBaseUrl}{orderId}/share", content: null);
        response.EnsureSuccessStatusCode();
        var shareLink = await response.Content.ReadFromJsonAsync<ShareLinkRecord>();
        return shareLink!.Code;
    }

    public async Task StopSharing(int orderId)
    {
        var response = await httpClient.DeleteAsync($"{remoteServiceBaseUrl}{orderId}/share");
        if (response.StatusCode != HttpStatusCode.NotFound)
        {
            response.EnsureSuccessStatusCode();
        }
    }

    public async Task<TrackedOrderRecord?> GetSharedOrder(string code)
    {
        var response = await httpClient.GetAsync($"{remoteServiceBaseUrl}track/{code}");
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TrackedOrderRecord>();
    }
}

public record OrderRecord(
    int OrderNumber,
    DateTime Date,
    string Status,
    decimal Total);

public record ShareLinkRecord(string Code);

public record TrackedOrderRecord(
    int OrderNumber,
    DateTime Date,
    string Status,
    string? Description,
    string Street,
    string City,
    string State,
    string Zipcode,
    string Country,
    List<TrackedOrderItemRecord> OrderItems,
    decimal Total);

public record TrackedOrderItemRecord(
    string ProductName,
    int Units,
    decimal UnitPrice,
    string? PictureUrl);

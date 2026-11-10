namespace eShop.Ordering.Infrastructure.Sharing;

public class OrderShareLink
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

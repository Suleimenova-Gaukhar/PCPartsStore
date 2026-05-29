namespace PCPartsStore.Models;

public class CartItem
{
    public int Id { get; set; }
    public Component Component { get; set; } = new();
    public int Quantity { get; set; } = 1;
    public decimal Subtotal => Component.Price * Quantity;
}
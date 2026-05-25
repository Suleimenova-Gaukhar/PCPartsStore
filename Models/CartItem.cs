namespace PCPartsStore.Models;

public class CartItem
{
    public Component Component { get; set; } = new();
    public int Quantity { get; set; } = 1;
    public decimal Subtotal 
    {
        get
        {
            return Component.Price * Quantity;
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Models
{
    public class Inventory
    {
        public int ProductID { get; set; }


    [Required]
        public string ProductName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }


}

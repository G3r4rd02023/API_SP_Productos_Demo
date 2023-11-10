namespace API_SP_Productos_Demo.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string CodigoBarra { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public decimal Precio { get; set; }
    }
}

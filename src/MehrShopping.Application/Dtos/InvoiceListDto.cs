namespace MehrShopping.Application.Dtos
{
    public class InvoiceListDto
    {
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerNationalCode { get; set; }
        public List<InvoiceItemListDto> InvoiceItemList { get; set; } = new List<InvoiceItemListDto>();

        public InvoiceListDto(
            string customerFirstName,
            string customerLastName,
            string customerNationalCode,
            List<InvoiceItemListDto> invoiceItemList)
        {
            CustomerFirstName = customerFirstName;
            CustomerLastName = customerLastName;
            CustomerNationalCode = customerNationalCode;
            InvoiceItemList = invoiceItemList;
        }

    }

    public class InvoiceItemListDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public InvoiceItemListDto(string productName, int quantity)
        {
            ProductName = productName;
            Quantity = quantity;
        }

    }
}

namespace FDBInsights.Models;

public class TicketTransaction
{
    public long TicketTransactionID { get; set; }
    public string TheaterName { get; set; }
    public string TheaterCode { get; set; }
    public string ScreenName { get; set; }
    public int? ScreenID { get; set; }
    public string ShowTypeName { get; set; }
    public int? ShowTypeID { get; set; }
    public string MovieName { get; set; }
    public string MovieCode { get; set; }
    public string FiscalYear { get; set; }
    public byte? FiscalYearID { get; set; }
    public DateTime? ShowDateTime { get; set; }
    public long? ShowID { get; set; }
    public DateTime? PrintDateTime { get; set; }
    public string TicketTypeName { get; set; }
    public int? TicketTypeID { get; set; }
    public string PaymentTypeName { get; set; }
    public byte? PaymentTypeID { get; set; }
    public string TicketCode { get; set; }
    public string SeatNo { get; set; }
    public string TicketStatusName { get; set; }
    public byte? TicketStatusValue { get; set; }
    public decimal? TicketPrice { get; set; }
    public string TicketsTax { get; set; }
    public string TicketsCharge { get; set; }
    public string DistributorCode { get; set; }
    public decimal? DistributorCommissionValue { get; set; }
    public string TicketCancelledReason { get; set; }
    public DateTime? TicketCancelledDateTime { get; set; }
    public DateTime? AddedDateTime { get; set; }
    public string Header { get; set; }
    public bool Extracted { get; set; } = false;
    public DateTime? UpdatedOn { get; set; }
    public string IPAddress { get; set; }
    public decimal? TicketNetPrice { get; set; }
}
namespace Infinicare_Ojash_Devkota.Models;

public class TransactionDetails {
    public Guid TransactionDetailsId { get; set; }
    public String SenderAccountNumber { get; set; } = String.Empty;
    public String ReceiverAccountNumber { get; set; } = String.Empty;
    public String SenderBankName { get; set; } = String.Empty;
    public String ReceiverBankName { get; set; } = String.Empty;
    public double TransferAmountMYR { get; set; }
    public double TransferAmountNPR { get; set; }
    public DateTime TransferDate { get; set; } = DateTime.Now;
    public double ExchangeRate { get; set; }
}
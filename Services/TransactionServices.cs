using Infinicare_Ojash_Devkota.Data;
using Infinicare_Ojash_Devkota.Models;

namespace Infinicare_Ojash_Devkota.Services;

public class TransactionServices {
    private readonly ApplicationDBContext _context;
    private readonly UserServices _userServices;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public TransactionServices(ApplicationDBContext context, UserServices userServices, IHttpContextAccessor httpContextAccessor) {
        _context = context;
        _userServices = userServices;
        _httpContextAccessor = httpContextAccessor;
    }

    public List<TransactionDetails> readTransactions() {
        var authUser = _userServices.RetrieveUserDetails(_httpContextAccessor.HttpContext.User.Identity.Name);
        return _context.TransactionDetails.Where(details => 
            details.SenderAccountNumber == authUser.UserDetails.AccountNumber
            ).ToList();
    }

    public List<TransactionDetails> readTransactions(DateTime startDate) { 
        var authUser = _userServices.RetrieveUserDetails(_httpContextAccessor.HttpContext.User.Identity.Name);
        return _context.TransactionDetails.Where(details => 
            details.SenderAccountNumber == authUser.UserDetails.AccountNumber &&
            details.TransferDate >= startDate
            ).ToList();
    }
    
    public List<TransactionDetails> readTransactions(DateTime startDate, DateTime endDate) {
        var authUser = _userServices.RetrieveUserDetails(_httpContextAccessor.HttpContext.User.Identity.Name);
        return _context.TransactionDetails.Where(details =>
            details.SenderAccountNumber == authUser.UserDetails.AccountNumber &&
            details.TransferDate >= startDate && details.TransferDate <= endDate
        ).ToList();

    }

    public bool registerTransaction(TransactionDetails transaction) {
        using var dbContextTransaction = _context.Database.BeginTransaction();
        try {
            _context.TransactionDetails.Add(transaction);
            _context.SaveChanges();
            dbContextTransaction.Commit();
            return true;
        }
        catch (Exception e) {
            Console.WriteLine(e.Message);
            dbContextTransaction.Rollback();
            return false;
        }
    }
}
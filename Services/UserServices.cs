using System.Security.Cryptography;
using System.Text;
using Infinicare_Ojash_Devkota.Data;
using Infinicare_Ojash_Devkota.Models;
using Infinicare_Ojash_Devkota.ViewModels;

namespace Infinicare_Ojash_Devkota.Services;

public class UserServices {
    private readonly ApplicationDBContext _context;

    public UserServices(ApplicationDBContext context) {
        _context = context;
    }

    public bool CheckIfUserExists(String username) {
        return _context.UserCredentials.SingleOrDefault(u => u.UserName == username) is not null;
    }
    public bool RegisterUser(UserInfoModel userInfo) {
        using var dbContextTransaction = _context.Database.BeginTransaction();
        string hashedPassword = HashPassword(userInfo.UserCredentials.UserPassword);
        try {
            var user = new UserCredentials() {
                UserId = new Guid(),
                UserName = userInfo.UserCredentials.UserName,
                UserPassword = hashedPassword,
                UserDetails = userInfo.UserDetails
            };

            userInfo.UserDetails = new UserDetails() {
                AccountNumber = userInfo.UserDetails.AccountNumber,
                FirstName = userInfo.UserDetails.FirstName,
                MiddleName = userInfo.UserDetails.MiddleName,
                LastName = userInfo.UserDetails.LastName,
                Country = userInfo.UserDetails.Country,
                UserId = user.UserId,
                UserCredentials = user,
            };
            _context.UserCredentials.Add(user);
            _context.UserDetails.Add(userInfo.UserDetails);
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
    
    public bool ValidateUserCredentials(UserCredentials userCredentials) {
        return true;
        var user = _context.UserCredentials.SingleOrDefault(user=> 
            user.UserName == userCredentials.UserName && userCredentials.UserPassword == HashPassword(userCredentials.UserPassword));
        return user is not null; 
    }

    public UserInfoModel RetrieveUserDetails(string username) {
        UserInfoModel user = _context.UserCredentials.Join(
            _context.UserDetails,
            credentials => credentials.UserId,
            details =>  details.UserId,
            (credentials, details) =>
                new UserInfoModel() {
                    UserCredentials = credentials,
                    UserDetails = details
                }
            ).Single(u => u.UserCredentials.UserName == username);
        return user;
    }

    private string HashPassword(string password) {
        using var hmac = new HMACSHA512();
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }
}
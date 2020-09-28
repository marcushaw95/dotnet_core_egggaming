using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CashEntertainment.Models.Models_User;
using CashEntertainment.DB;

namespace CashEntertainment.DataAccess
{
    public interface IRepo_User
    {
        Task<Tuple<int, long>> UserRegisterNewAccount(string LoginID, string FullName, string Password, string Email, string Phonenumber, string CountryCode, string DOB , int Gender, string Upline);
        int UserUpdateAccountDetails(long _MemberSrno, string FullName, string _Email, string _Phonenumber, string _DOB , int Gender);
        int UserUpdatePassword(string NewPassword, string OldPassword, long MemberSrno);
        Tuple<int, long, int> AuthrorizeAccount(string LoginID, string _Password, string LoginCountry, string UserIpAddress);
        Tuple<int, Models_User_Profile> GetUserProfile(long MemberSrno);

        Tuple<int, long> AuthorizeAdminAccount(string LoginID, string _Password);

        List<Models_User_Listing> RetrieveAllMemberListing();
        int InactiveMember(long MemberSrno, bool status);
        int ChangeMemberPassword(string AdminLoginID, long _MemberSrno, string NewPassword);

        MstAuthenticate Authenticate(string username, string password);

        List<Models_User_Login_Log_List> RetrieveUserLoginLogListByFilter(string loginid, string startdate, string enddate);
    }
}

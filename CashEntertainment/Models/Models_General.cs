using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashEntertainment.Models
{
     class Models_General
    {
        public const int ERR_INCORRECT_CREDENTIALS = 1000;
        public const int ERR_ACCOUNT_LOCKED = 1001;
        public const int ERR_PHONENUMBER_EXIST = 1003;
        public const int ERR_SERVER_BUSY_INTERNAL_ERROR = 1006;
        public const int ERR_USERNAME_EXIST = 1011;
        public const int ERR_EMAIL_EXIST = 1012;
        public const int ERR_EXISTING_DATA_DETECTED = 1013;
        public const int ERR_INCORRECT_PASSWORD = 1014;
        public const int ERR_ACCOUNT_NOT_CREATED = 1015;
        public const int ERR_BANK_INFO_NOT_MATCH = 1016;
        public const int ERR_WITHDRAWAL_NOT_FOUND = 1017;
        public const int ERR_BANNER_NOT_FOUND = 1018;
        public const int ERR_ANNOUNCEMENT_NOT_FOUND = 1019;
        public const int ERR_USER_NOT_FOUND = 1020;
        public const int ERR_USER_BANK_NOT_FOUND = 1021;
        public const int ERR_USER_WALLET_NOT_FOUND = 1022;
        public const int ERR_ADMIN_BANK_NOT_FOUND = 1023;
        public const int ERR_INSUFFICIENT_CASH_BALANCE = 1024;
        public const int ERR_AMOUNT_CANNOT_BE_ZERO_NULL = 1025;
        public const int ERR_WALLET_CREDIT_INSUFFICIENT = 1026;
        public const int ERR_GAME_TOKEN_NOT_FOUND = 1027;
        public const int ERR_CANNOT_LOGIN_GAME = 1028;
        public const int ERR_BALANCE_NOT_FOUND = 1029;
        public const int ERR_GAME_NOT_FOUND = 1030;
        public const int ERR_REJECT_CHANGE_MAINTENANCE = 1031;
        public const int ERR_UPLINE_REFERCODE_NOT_EXIST = 1032;
        public const int ERR_PAYMENT = 1033;
        public const int ERR_EXCHANGE_RATE_NOT_FOUND = 1034;
        public const int ERR_EXCHANGE_RATE_CURRENCY_EXIST = 1035;
        public const int ERR_SETTINGS_NOT_FOUND = 1036;
        public const int ERR_GET_TICKETS = 1037;
        public const int ERR_WALLET_TURNOVER_CLEAR = 1038;
        public const int ERR_CURRENCY_NOT_SAME = 1039;
        public const int ERR_TRANSFER_USER_NOT_FOUND = 1040;



        public const int SUCC_UPDATE_PASSWORD = 2001;
        public const int SUCC_CREATE_ACCOUNT = 2002;
        public const int SUCC_UPDATE_PROFILE = 2003;
        public const int SUCC_AUTHORIZE_GRANTED = 2004;
        public const int SUCC_CREATE_REQUEST_TOPUP = 2005;
        public const int SUCC_CREATE_REQUEST_WITHDRAWAL = 2006;
        public const int SUCC_ADMIN_APPROVE_WITHDRAWAL = 2007;
        public const int SUCC_ADMIN_REJECT_WITHDRAWAL = 2008;
        public const int SUCC_ADMIN_CREATE_ANNOUNCEMENT = 2009;
        public const int SUCC_ADMIN_UPDATE_ANNOUNCEMENT = 2010;
        public const int SUCC_ADMIN_CREATE_BANNER = 2011;
        public const int SUCC_ADMIN_UPDATE_BANNER = 2012;
        public const int SUCC_ADMIN_APPROVE_TOPUP = 2013;
        public const int SUCC_ADMIN_REJECT_TOPUP = 2014;
        public const int SUCC_MEMBER_ADD_BANK = 2015;
        public const int SUCC_MEMBER_UPDATE_BANK = 2016;
        public const int SUCC_ADMIN_ADD_BANK = 2017;
        public const int SUCC_ADMIN_UPDATE_BANK = 2018;
        public const int SUCC_TOPUP_GAME_CREDIT = 2019;
        public const int SUCC_WITHDRAWAL_GAME_CREDIT = 2020;
        public const int SUCC_INACTIVE_MEMBER = 2021;
        public const int SUCC_ACTIVE_MEMBER = 2022;
        public const int SUCC_CHANGE_MEMBER_PASSWORD = 2023;
        public const int SUCC_OPEN_GAME = 2024;
        public const int SUCC_ADMIN_CHANGE_MAINTENANCE = 2025;
        public const int SUCC_TOPUP = 2026;
        public const int SUCC_CREATE_PAYMENT_REQUEST = 2027;
        public const int SUCC_PAYMENT = 2028;
        public const int SUCC_ADMIN_CREATE_EXCHANGE_RATE = 2029;
        public const int SUCC_ADMIN_UPDATE_EXCHANGE_RATE = 2030;
        public const int SUCC_ADMIN_UPDATE_SETTINGS = 2031;
        public const int SUCC_CHANGE_UPLINE = 2032;
        public const int SUCC_GET_TICKETS = 2033;
        public const int SUCC_TRANSFER_CREDIT = 2034;
        public const int SUCC_CHANGE_MEMBER_TURNOVER = 2035;
        public const int SUCC_GET_ALL_GAMER_WALLET_BALANCE = 2036;
        public const int SUCC_RECOLLECT_ALL_GAMER_WALLET_BALANCE = 2037;
        public const int SUCC_ADMIN_DELETE_ANNOUNCEMENT = 2038;
        public const int SUCC_CREATE_ACCOUNT_WITHOUT_GAME_ACCOUNT = 2039;
        public const int SUCC_CHECK_USER_GAME_REGISTER = 2040;
        public const int SUCC_EDIT_CASH_WALLET = 2041;
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CashEntertainment.DB
{
    public partial class UAT_CasinoContext : DbContext
    {
        public UAT_CasinoContext()
        {
        }

        public UAT_CasinoContext(DbContextOptions<UAT_CasinoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LogAdminChangeMemberPassword> LogAdminChangeMemberPassword { get; set; }
        public virtual DbSet<LogApiCall> LogApiCall { get; set; }
        public virtual DbSet<LogApiResponse> LogApiResponse { get; set; }
        public virtual DbSet<LogErrorSp> LogErrorSp { get; set; }
        public virtual DbSet<LogErrorSystem> LogErrorSystem { get; set; }
        public virtual DbSet<LogLoginIp> LogLoginIp { get; set; }
        public virtual DbSet<LogRecreateUser> LogRecreateUser { get; set; }
        public virtual DbSet<LogRequestPaymentGateway> LogRequestPaymentGateway { get; set; }
        public virtual DbSet<LogResponsePaymentGateway> LogResponsePaymentGateway { get; set; }
        public virtual DbSet<LogTickets> LogTickets { get; set; }
        public virtual DbSet<LogTopup> LogTopup { get; set; }
        public virtual DbSet<LogTurnover> LogTurnover { get; set; }
        public virtual DbSet<LogUserGameCreditTransaction> LogUserGameCreditTransaction { get; set; }
        public virtual DbSet<LogUserTrackingWallet> LogUserTrackingWallet { get; set; }
        public virtual DbSet<LogWithdraw> LogWithdraw { get; set; }
        public virtual DbSet<MstAdminAccount> MstAdminAccount { get; set; }
        public virtual DbSet<MstAdminBank> MstAdminBank { get; set; }
        public virtual DbSet<MstAnnouncement> MstAnnouncement { get; set; }
        public virtual DbSet<MstAuthenticate> MstAuthenticate { get; set; }
        public virtual DbSet<MstBank> MstBank { get; set; }
        public virtual DbSet<MstBanner> MstBanner { get; set; }
        public virtual DbSet<MstCountry> MstCountry { get; set; }
        public virtual DbSet<MstExchangeRate> MstExchangeRate { get; set; }
        public virtual DbSet<MstGame> MstGame { get; set; }
        public virtual DbSet<MstPaylah88Bank> MstPaylah88Bank { get; set; }
        public virtual DbSet<MstSettings> MstSettings { get; set; }
        public virtual DbSet<MstSubGame> MstSubGame { get; set; }
        public virtual DbSet<MstTopUp> MstTopUp { get; set; }
        public virtual DbSet<MstUser> MstUser { get; set; }
        public virtual DbSet<MstUserAccount> MstUserAccount { get; set; }
        public virtual DbSet<MstUserBank> MstUserBank { get; set; }
        public virtual DbSet<MstUserGameAccount> MstUserGameAccount { get; set; }
        public virtual DbSet<MstUserGameWallet> MstUserGameWallet { get; set; }
        public virtual DbSet<MstUserWallet> MstUserWallet { get; set; }
        public virtual DbSet<MstWinlose> MstWinlose { get; set; }
        public virtual DbSet<MstWithdraw> MstWithdraw { get; set; }
        public virtual DbSet<TblErrLog> TblErrLog { get; set; }
        public virtual DbSet<TblErrStoredProcedures> TblErrStoredProcedures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogAdminChangeMemberPassword>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_Admin_Change_Member_Password");

                entity.Property(e => e.ActionBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CurrentPassword)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.PreviousPassword)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<LogApiCall>(entity =>
            {
                entity.HasKey(e => e.Srno)
                    .HasName("PK_API_LogApiCall");

                entity.ToTable("Log_API_Call");

                entity.Property(e => e.Action).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.SendUrl)
                    .HasColumnName("SendURL")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<LogApiResponse>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_API_Response");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<LogErrorSp>(entity =>
            {
                entity.ToTable("Log_Error_SP");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.File).HasMaxLength(200);
            });

            modelBuilder.Entity<LogErrorSystem>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_Error_System");

                entity.Property(e => e.Context)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(CONVERT([nvarchar],getdate(),(112)))");

                entity.Property(e => e.Details)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            modelBuilder.Entity<LogLoginIp>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_LoginIP");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(CONVERT([nvarchar],getdate(),(112)))");

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasColumnName("IP")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LogRecreateUser>(entity =>
            {
                entity.HasKey(e => e.MemberSrno);

                entity.ToTable("Log_Recreate_User");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(CONVERT([nvarchar],getdate(),(112)))");

                entity.Property(e => e.CurrentGameId)
                    .IsRequired()
                    .HasColumnName("Current GameId")
                    .HasMaxLength(200);

                entity.Property(e => e.CurrentGamePassword)
                    .IsRequired()
                    .HasColumnName("Current GamePassword")
                    .HasMaxLength(200);

                entity.Property(e => e.LoginId)
                    .IsRequired()
                    .HasColumnName("LoginID")
                    .HasMaxLength(50);

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.PreviousGameId)
                    .IsRequired()
                    .HasColumnName("Previous GameId")
                    .HasMaxLength(200);

                entity.Property(e => e.PreviousGamePassword)
                    .IsRequired()
                    .HasColumnName("Previous GamePassword")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<LogRequestPaymentGateway>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_Request_Payment_Gateway");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.BackUrl)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Bank)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.ClientIp)
                    .IsRequired()
                    .HasColumnName("ClientIP")
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Datetime)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DepositUrl)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FrontUrl)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Merchant)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Reference)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LogResponsePaymentGateway>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_Response_Payment_Gateway");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Datetime)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50);

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Merchant)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Note)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Reference)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LogTickets>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_Tickets");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(CONVERT([nvarchar],getdate(),(112)))");

                entity.Property(e => e.Currency).HasMaxLength(200);

                entity.Property(e => e.GameType).HasMaxLength(200);

                entity.Property(e => e.PlayerWinLoss).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Result).HasMaxLength(200);

                entity.Property(e => e.RoundId)
                    .HasColumnName("RoundID")
                    .HasMaxLength(200);

                entity.Property(e => e.Stake).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.StakeMoney).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.StatementDate).HasColumnType("datetime");

                entity.Property(e => e.TicketId)
                    .HasColumnName("TicketID")
                    .HasMaxLength(200);

                entity.Property(e => e.TicketSessionId)
                    .HasColumnName("TicketSessionID")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<LogTopup>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_Topup");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CurrentTotalAmount)
                    .HasColumnName("Current_Total_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.PreviousAmount)
                    .HasColumnName("Previous_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("Transaction_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.WalletFrom)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.WalletTo)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            modelBuilder.Entity<LogTurnover>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_Turnover");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.PreviousAmount)
                    .HasColumnName("Previous_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TotalAmount)
                    .HasColumnName("Total_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.UpdatedAmount)
                    .HasColumnName("Updated_Amount")
                    .HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<LogUserGameCreditTransaction>(entity =>
            {
                entity.HasKey(e => e.Srno)
                    .HasName("PK_Log_User_Transfer");

                entity.ToTable("Log_User_Game_Credit_Transaction");

                entity.Property(e => e.AfterAmount).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.BeforeAmount).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.GameApi)
                    .IsRequired()
                    .HasColumnName("GameAPI")
                    .HasMaxLength(50);

                entity.Property(e => e.Player)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TransactionType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TransferAmount).HasColumnType("decimal(18, 6)");

                entity.Property(e => e.TransferDate).HasColumnType("datetime");

                entity.Property(e => e.TrasactionId)
                    .IsRequired()
                    .HasColumnName("TrasactionID")
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<LogUserTrackingWallet>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_User_Tracking_Wallet");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CurrentTotalAmount)
                    .HasColumnName("Current_Total_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.PreviousAmount)
                    .HasColumnName("Previous_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("Transaction_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.WalletFrom)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.WalletTo)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            modelBuilder.Entity<LogWithdraw>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Log_Withdraw");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CurrentTotalAmount)
                    .HasColumnName("Current_Total_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.PreviousAmount)
                    .HasColumnName("Previous_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TransactionAmount)
                    .HasColumnName("Transaction_Amount")
                    .HasColumnType("decimal(18, 4)");

                entity.Property(e => e.WalletFrom)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.WalletTo)
                    .IsRequired()
                    .HasMaxLength(4000);
            });

            modelBuilder.Entity<MstAdminAccount>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Admin_Account");

                entity.Property(e => e.AdminType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Auth)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.LoginId)
                    .IsRequired()
                    .HasColumnName("LoginID")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MstAdminBank>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Admin_Bank");

                entity.Property(e => e.BankAccountHolder)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BankCardNo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ACTIVE')");
            });

            modelBuilder.Entity<MstAnnouncement>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Announcement");

                entity.Property(e => e.AnnouncementContentCn).HasColumnName("AnnouncementContentCN");

                entity.Property(e => e.AnnouncementContentEn).HasColumnName("AnnouncementContentEN");

                entity.Property(e => e.AnnouncementContentMs).HasColumnName("AnnouncementContentMS");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsPublish)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.TitleCn).HasColumnName("TitleCN");

                entity.Property(e => e.TitleEn).HasColumnName("TitleEN");

                entity.Property(e => e.TitleMs).HasColumnName("TitleMS");
            });

            modelBuilder.Entity<MstAuthenticate>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Authenticate");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.User)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MstBank>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Bank");

                entity.Property(e => e.BankCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BankName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'MY')");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Currency).HasMaxLength(200);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MstBanner>(entity =>
            {
                entity.HasKey(e => e.Srno)
                    .HasName("PK_dbo.Mst_Banner");

                entity.ToTable("Mst_Banner");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ImagePath).IsRequired();

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RedirectUrl).HasColumnName("RedirectURL");
            });

            modelBuilder.Entity<MstCountry>(entity =>
            {
                entity.HasKey(e => e.CountryCode);

                entity.ToTable("Mst_Country");

                entity.Property(e => e.CountryCode).HasMaxLength(50);

                entity.Property(e => e.Cnnames)
                    .HasColumnName("CNNames")
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CountryName).HasMaxLength(50);

                entity.Property(e => e.CountryPhone).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Srno).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MstExchangeRate>(entity =>
            {
                entity.HasKey(e => e.Srno)
                    .HasName("PK_dbo.Mst_Exchange_Rate");

                entity.ToTable("Mst_Exchange_Rate");

                entity.Property(e => e.Base).HasMaxLength(10);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Currency).HasMaxLength(50);

                entity.Property(e => e.LastUpdateBy).HasMaxLength(50);

                entity.Property(e => e.ModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 8)");
            });

            modelBuilder.Entity<MstGame>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Game");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GameCategory).HasMaxLength(50);

                entity.Property(e => e.IsSubgame).HasColumnName("isSubgame");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VendorCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VendorImageUrl)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.VendorName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<MstPaylah88Bank>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Paylah88Bank");

                entity.Property(e => e.BankCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.BankName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'MY')");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Currency).HasMaxLength(200);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MstSettings>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Settings");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.LastUpdateBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.SettingName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SettingValue)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MstSubGame>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Sub_Game");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GameCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.GameImageUrl)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.GameImageUrl2)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.GameName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.GameType).HasMaxLength(255);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VendorCode)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<MstTopUp>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_TopUp");

                entity.Property(e => e.ApproveBy).HasMaxLength(50);

                entity.Property(e => e.ApproveDate).HasColumnType("datetime");

                entity.Property(e => e.ApproveRemark)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BankAccountHolder).HasMaxLength(50);

                entity.Property(e => e.BankAccountNumber).HasMaxLength(50);

                entity.Property(e => e.BankCode).HasMaxLength(50);

                entity.Property(e => e.BankName).HasMaxLength(50);

                entity.Property(e => e.Currency).HasMaxLength(200);

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.RejectBy)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RejectDate).HasColumnType("datetime");

                entity.Property(e => e.RejectRemark)
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status).HasDefaultValueSql("((0))");

                entity.Property(e => e.TopupAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TopupImageProof)
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TransactionHash).HasMaxLength(400);

                entity.Property(e => e.TransactionReferenceNumber).HasMaxLength(50);

                entity.Property(e => e.WalletType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<MstUser>(entity =>
            {
                entity.HasKey(e => e.MemberSrno);

                entity.ToTable("Mst_User");

                entity.Property(e => e.MemberSrno).ValueGeneratedNever();

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('CHN')");

                entity.Property(e => e.DirectSponsor)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.DoB)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RefCode).HasMaxLength(255);

                entity.Property(e => e.RegisterDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Srno).ValueGeneratedOnAdd();

                entity.Property(e => e.Upline).HasMaxLength(255);

                entity.Property(e => e.UserLevel)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('MEMBER')");
            });

            modelBuilder.Entity<MstUserAccount>(entity =>
            {
                entity.HasKey(e => e.MemberSrno)
                    .HasName("PK_Mst_User_Security");

                entity.ToTable("Mst_User_Account");

                entity.Property(e => e.AccountType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'(\"MEMBER\")')");

                entity.Property(e => e.LoginId)
                    .IsRequired()
                    .HasColumnName("LoginID")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('ACTIVE')");

                entity.Property(e => e.VerifiedBy).HasMaxLength(50);

                entity.Property(e => e.VerifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MstUserBank>(entity =>
            {
                entity.HasKey(e => e.Srno)
                    .HasName("PK_Mst_UserBank");

                entity.ToTable("Mst_User_Bank");

                entity.Property(e => e.BankAccountHolder)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BankCardNo)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'ACTIVE')");
            });

            modelBuilder.Entity<MstUserGameAccount>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_User_Game_Account");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GameId)
                    .IsRequired()
                    .HasColumnName("GameID")
                    .HasMaxLength(50);

                entity.Property(e => e.GamePassword)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(N'abc123')");
            });

            modelBuilder.Entity<MstUserGameWallet>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_User_Game_Wallet");

                entity.Property(e => e.GameCredit).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.GameId)
                    .IsRequired()
                    .HasColumnName("GameID")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<MstUserWallet>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_User_Wallet");

                entity.Property(e => e.CashCredit).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.Commission).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PendingWinLossWithdrawAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.PendingWithdrawAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TurnoverAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.TwelveTurnoverAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.WinLossCommission).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<MstWinlose>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Winlose");

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Currency)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.GameType).HasMaxLength(400);

                entity.Property(e => e.StakeAmount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.WinloseAmount).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<MstWithdraw>(entity =>
            {
                entity.HasKey(e => e.Srno);

                entity.ToTable("Mst_Withdraw");

                entity.Property(e => e.ApproveBy).HasMaxLength(50);

                entity.Property(e => e.ApproveDate).HasColumnType("datetime");

                entity.Property(e => e.ApproveRemark).HasMaxLength(500);

                entity.Property(e => e.BankAccountHolder).HasMaxLength(200);

                entity.Property(e => e.BankCardNo).HasMaxLength(50);

                entity.Property(e => e.BankCode).HasMaxLength(50);

                entity.Property(e => e.BankName).HasMaxLength(200);

                entity.Property(e => e.Currency).HasMaxLength(200);

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.RejectBy).HasMaxLength(50);

                entity.Property(e => e.RejectDate).HasColumnType("datetime");

                entity.Property(e => e.RejectRemark).HasMaxLength(500);

                entity.Property(e => e.RequestDate).HasColumnType("datetime");

                entity.Property(e => e.ToAddress).HasMaxLength(200);

                entity.Property(e => e.WithdrawAmount).HasColumnType("decimal(18, 4)");
            });

            modelBuilder.Entity<TblErrLog>(entity =>
            {
                entity.HasKey(e => e.ErrIndex);

                entity.ToTable("Tbl_Err_Log");

                entity.Property(e => e.ErrIndex).HasColumnName("Err_Index");

                entity.Property(e => e.ErrDateTime)
                    .HasColumnName("Err_DateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.ErrForm)
                    .HasColumnName("Err_Form")
                    .HasMaxLength(255);

                entity.Property(e => e.ErrIp)
                    .HasColumnName("Err_IP")
                    .HasMaxLength(50);

                entity.Property(e => e.ErrLoginId)
                    .HasColumnName("Err_LoginID")
                    .HasMaxLength(50);

                entity.Property(e => e.ErrMessage)
                    .HasColumnName("Err_Message")
                    .HasColumnType("ntext");

                entity.Property(e => e.ErrStackTrace)
                    .HasColumnName("Err_Stack_Trace")
                    .HasColumnType("ntext");
            });

            modelBuilder.Entity<TblErrStoredProcedures>(entity =>
            {
                entity.HasKey(e => e.ErrIndex);

                entity.ToTable("Tbl_Err_StoredProcedures");

                entity.Property(e => e.ErrIndex).HasColumnName("Err_Index");

                entity.Property(e => e.ErrDateTime).HasColumnName("Err_DateTime");

                entity.Property(e => e.ErrFile)
                    .HasColumnName("Err_File")
                    .HasMaxLength(200);

                entity.Property(e => e.ErrMessage).HasColumnName("Err_Message");

                entity.Property(e => e.ErrSeverity).HasColumnName("Err_Severity");

                entity.Property(e => e.ErrState).HasColumnName("Err_State");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

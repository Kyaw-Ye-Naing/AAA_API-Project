using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AAA_API.Models;
using AAA_API.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AAA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBalanceController : ControllerBase
    {
        private readonly ILogger<UserBalanceController> _logger;
        private readonly Gambling_AppContext _context;
        private readonly IConfiguration _configuartion;
        public UserBalanceController(ILogger<UserBalanceController> logger, Gambling_AppContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuartion = configuration;
        }

        // CACULATION TOTAL AMOUNT : api/UserBalance/balance
        [HttpGet]
        [AllowAnonymous]
        [Route("balance")]
        public async Task<List<ViewUserBalance>> UserBalance()
        {
            var response = new List<ViewUserBalance>();
            var connString = _configuartion.GetConnectionString("DefaultConnection");
            SqlConnection sql = new SqlConnection(connString);
            try
            {

                if (sql.State == ConnectionState.Closed)
                {
                    await sql.OpenAsync();
                }
                SqlCommand cmd = new SqlCommand("SP_UserListWithBalance", sql)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@userId", 1);

                using (var reader = cmd.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        response.Add(MapToValue(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                string m = ex.Message;
            }
            finally
            {
                await sql.CloseAsync();
            }

            return response;
        }

        //Read data from user balance stored procedure
        private ViewUserBalance MapToValue(SqlDataReader reader)
        {
            return new ViewUserBalance()
            {
                UserId = Convert.ToDecimal(reader["userId"].ToString()),
                UserName = reader["userName"].ToString(),
                Inward = Convert.ToDecimal(reader["Inward"].ToString()),
                Outward = Convert.ToDecimal(reader["outward"].ToString()),
                Balance = Convert.ToDecimal(reader["balance"].ToString())
            };
        }

        // ADDING BALANCE : api/UserBalance/add/5
        [HttpPut("add/{id}")]
        [Authorize(Policy = "Person")]
        public IActionResult AddBalance(decimal id, [FromBody] Balance balance)
        {
            //Created user's balance
            var user_id = User.FindFirst("userId")?.Value;
            DateTime moment = DateTime.Now;
            TblUserPosting userPosting = new TblUserPosting();
            var role = _context.TblUser.Where(a => a.UserId == id).First().RoleId;
            var year = moment.Year;
            var month = moment.Month;
            var day = moment.Day;
            var hour = moment.Hour;
            var minute = moment.Minute;
            var second = moment.Second;
            var str = "UB" + id.ToString() + role.ToString() + year.ToString() + month.ToString() + day.ToString() +
                hour.ToString() + minute.ToString() + second.ToString();
            userPosting.UserId = id;
            userPosting.PostingNo = str;
            userPosting.TransactionTypeId = 1;
            userPosting.Inward = balance.Amount;
            userPosting.Outward = 0;
            userPosting.Active = true;
            userPosting.CreatedBy = Decimal.Parse(user_id);
            userPosting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(userPosting);
            _context.SaveChanges();

            //Created user' balance
            TblUserBalance userBalance = new TblUserBalance();
            userBalance.PostingNo = str;
            userBalance.TransactionTypeId = 1;
            userBalance.UserId = id;
            userBalance.Inward = Convert.ToInt32(balance.Amount);
            userBalance.Outward = 0;
            userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(userBalance);
            _context.SaveChanges();

            //Current logged user's balance
            DateTime parent_moment = DateTime.Now;
            TblUserPosting parent_userPosting = new TblUserPosting();
            var parent_role = _context.TblUser.Where(a => a.UserId == Decimal.Parse(user_id)).First().RoleId;
            var parent_year = parent_moment.Year;
            var parent_month = parent_moment.Month;
            var parent_day = parent_moment.Day;
            var parent_hour = parent_moment.Hour;
            var parent_minute = parent_moment.Minute;
            var parent_second = parent_moment.Second;
            var parent_str = "UB" + id.ToString() + parent_role.ToString() + parent_year.ToString() + parent_month.ToString() + parent_day.ToString() +
                parent_hour.ToString() + parent_minute.ToString() + parent_second.ToString();
            parent_userPosting.UserId = Decimal.Parse(user_id);
            parent_userPosting.PostingNo = parent_str;
            parent_userPosting.TransactionTypeId = 2;
            parent_userPosting.Inward = 0;
            parent_userPosting.Outward = balance.Amount;
            parent_userPosting.Active = true;
            parent_userPosting.CreatedBy = Decimal.Parse(user_id);
            parent_userPosting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(parent_userPosting);
            _context.SaveChanges();

            //Current logged user's balance
            TblUserBalance parent_userBalance = new TblUserBalance();
            parent_userBalance.PostingNo = parent_str;
            parent_userBalance.TransactionTypeId = 2;
            parent_userBalance.UserId = Decimal.Parse(user_id);
            parent_userBalance.Inward = 0;
            parent_userBalance.Outward = Convert.ToInt32(balance.Amount);
            parent_userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(parent_userBalance);
            _context.SaveChanges();
            return Ok(new { message = "Deposit Successfully" });
        }

        // REMOVING BALANCE :  api/UserBalance/remove/5
        [HttpPut("remove/{id}")]
        [Authorize(Policy = "Person")]
        public IActionResult RemoveBalance(decimal id, [FromBody] Balance balance)
        {
            //Created user's balance
            var user_id = User.FindFirst("userId")?.Value;
            DateTime moment = DateTime.Now;
            TblUserPosting userPosting = new TblUserPosting();
            var role = _context.TblUser.Where(a => a.UserId == id).First().RoleId;
            var year = moment.Year;
            var month = moment.Month;
            var day = moment.Day;
            var hour = moment.Hour;
            var minute = moment.Minute;
            var second = moment.Second;
            var str = "UB" + id.ToString() + role.ToString() + year.ToString() + month.ToString() + day.ToString() +
                hour.ToString() + minute.ToString() + second.ToString();
            userPosting.UserId = id;
            userPosting.PostingNo = str;
            userPosting.TransactionTypeId = 2;
            userPosting.Inward = 0;
            userPosting.Outward = balance.Amount;
            userPosting.Active = true;
            userPosting.CreatedBy = Decimal.Parse(user_id);
            userPosting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(userPosting);
            _context.SaveChanges();

            //Created user' balance
            TblUserBalance userBalance = new TblUserBalance();
            userBalance.PostingNo = str;
            userBalance.TransactionTypeId = 2;
            userBalance.UserId = id;
            userBalance.Inward = 0;
            userBalance.Outward = Convert.ToInt32(balance.Amount);
            userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(userBalance);
            _context.SaveChanges();

            //Current logged user's balance
            DateTime parent_moment = DateTime.Now;
            TblUserPosting parent_userPosting = new TblUserPosting();
            var parent_role = _context.TblUser.Where(a => a.UserId == Decimal.Parse(user_id)).First().RoleId;
            var parent_year = parent_moment.Year;
            var parent_month = parent_moment.Month;
            var parent_day = parent_moment.Day;
            var parent_hour = parent_moment.Hour;
            var parent_minute = parent_moment.Minute;
            var parent_second = parent_moment.Second;
            var parent_str = "UB" + id.ToString() + parent_role.ToString() + parent_year.ToString() + parent_month.ToString() + parent_day.ToString() +
                parent_hour.ToString() + parent_minute.ToString() + parent_second.ToString();
            parent_userPosting.UserId = Decimal.Parse(user_id);
            parent_userPosting.PostingNo = parent_str;
            parent_userPosting.TransactionTypeId = 1;
            parent_userPosting.Inward = balance.Amount;
            parent_userPosting.Outward = 0;
            parent_userPosting.Active = true;
            parent_userPosting.CreatedBy = Decimal.Parse(user_id);
            parent_userPosting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(parent_userPosting);
            _context.SaveChanges();

            //Current logged user's balance
            TblUserBalance parent_userBalance = new TblUserBalance();
            parent_userBalance.PostingNo = parent_str;
            parent_userBalance.TransactionTypeId = 1;
            parent_userBalance.UserId = Decimal.Parse(user_id);
            parent_userBalance.Inward = Convert.ToInt32(balance.Amount);
            parent_userBalance.Outward = 0;
            parent_userBalance.CreatedDate = DateTime.Now;
            _context.TblUserBalance.Add(parent_userBalance);
            _context.SaveChanges();
            return Ok(new { message = "Withdraw Successfully " });
        }

        // MANAGING CREDIT BALANCE :  api/UserBalance/credit/5
        [HttpPut("credit/{id}")]
        [Authorize(Policy = "Person")]
        public IActionResult CreditManage(decimal id, Credit creditAmount)
        {
            //Created user's credit
            var user_id = User.FindFirst("userId")?.Value;
            TblCredit credit = new TblCredit();
            DateTime moment = DateTime.Now;
            var role = _context.TblUser.Where(a => a.UserId == id).First().RoleId;
            var year = moment.Year;
            var month = moment.Month;
            var day = moment.Day;
            var hour = moment.Hour;
            var minute = moment.Minute;
            var second = moment.Second;
            var str = "CR" + id.ToString() + role.ToString() + year.ToString() + month.ToString() + day.ToString() +
                hour.ToString() + minute.ToString() + second.ToString();
            credit.PostingNo = str;
            credit.TransactionTypeId = 5;
            credit.UserId = id;
            credit.Amount = creditAmount.Amount;
            credit.Active = true;
            credit.CreatedBy = Decimal.Parse(user_id);
            credit.CreatedDate = DateTime.Now;
            _context.TblCredit.Add(credit);
            _context.SaveChanges();

            //Current logged user's credit
            TblCredit parent_credit = new TblCredit();
            DateTime parent_moment = DateTime.Now;
            var parent_role = _context.TblUser.Where(a => a.UserId == Decimal.Parse(user_id)).First().RoleId;
            var parent_year = moment.Year;
            var parent_month = moment.Month;
            var parent_day = moment.Day;
            var parent_hour = moment.Hour;
            var parent_minute = moment.Minute;
            var parent_second = moment.Second;
            var parent_str = "CR" + user_id.ToString() + parent_role.ToString() + parent_year.ToString() + parent_month.ToString() + parent_day.ToString() +
                parent_hour.ToString() + parent_minute.ToString() + parent_second.ToString();
            parent_credit.PostingNo = parent_str;
            parent_credit.TransactionTypeId = 6;
            parent_credit.UserId = Decimal.Parse(user_id);
            parent_credit.Amount = creditAmount.Amount;
            parent_credit.Active = true;
            parent_credit.CreatedBy = Decimal.Parse(user_id);
            parent_credit.CreatedDate = DateTime.Now;
            _context.TblCredit.Add(parent_credit);
            _context.SaveChanges();

            //Created user's credit
            TblUserPosting posting = new TblUserPosting();
            posting.PostingNo = str;
            posting.TransactionTypeId = 5;
            posting.UserId = id;
            posting.Inward = creditAmount.Amount;
            posting.Outward = 0;
            posting.Active = true;
            posting.CreatedBy = Decimal.Parse(user_id);
            posting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(posting);
            _context.SaveChanges();

            //Current logged user's credit
            TblUserPosting parent_posting = new TblUserPosting();
            parent_posting.PostingNo = parent_str;
            parent_posting.TransactionTypeId = 6;
            parent_posting.UserId = Decimal.Parse(user_id);
            parent_posting.Inward = 0;
            parent_posting.Outward = creditAmount.Amount;
            parent_posting.Active = true;
            parent_posting.CreatedBy = Decimal.Parse(user_id);
            parent_posting.CreatedDate = DateTime.Now;
            _context.TblUserPosting.Add(parent_posting);
            _context.SaveChanges();
            return Ok();
        }

        // MANAGING CREDIT BALANCE :  api/UserBalance/credit/5
        [HttpPut("minmax/{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult MinMaxCalculation(decimal id, MinMax minMax)
        {
            var result = _context.TblGamblingType.Find(Convert.ToInt32(id));
            if (result == null)
            {
                return NotFound();

            }

            //Update gamblingType table
            result.MaxBetAmount = minMax.MaxBetAmount;
            result.MinBetAmount = minMax.MinBetAmount;
            _context.SaveChanges();
            return Ok(new
            {
                message = "Update Successfully"
            });
        }
    }
}

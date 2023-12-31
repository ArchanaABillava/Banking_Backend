﻿using BankAuth.Context;
using BankAuth.Models;
using BankAuth.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace BankAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IEmailService _emailService;

        public class Payload
        {
            public int? Id { get; set; }
            public string? status { get; set; }
            public string? comment { get; set; }
        }

        private string GenerateRandomNumber()
        {
            Random random = new Random();
            long randomNumber = (long)(random.NextDouble() * 900000000000) + 100000000000;
            string loanAccNum = randomNumber.ToString();
            return loanAccNum;
        }

        public LoanController(AppDbContext appDbContext, IEmailService emailService)
        {
            _authContext = appDbContext;
            _emailService = emailService;

        }


        //API to get list of All loans
        [HttpGet("getAllLoans")]
        public async Task<IEnumerable<LoanDetails>> GetLoanDetails()
        {
            return await _authContext.LoanDetails
        .Where(ld => ld.LoanStatus == "Processing")
        .ToListAsync();
        }

        //API to update Loan details based on loan ID
        [HttpPut("updateLoan")]
        public async Task<IActionResult> UpdateLoan([FromBody] LoanDetails LoanObj, int loanid)
        {
            try
            {
                var existingLoanObj = await _authContext.LoanDetails.FindAsync(loanid);

                if (existingLoanObj == null)
                {
                    return NotFound();
                }

                float loanAmount;
                if (!float.TryParse(LoanObj.LoanAmount, out loanAmount))
                {
                    return BadRequest("Invalid loan amount.");
                }

                float annualIncome;
                if (!float.TryParse(LoanObj.AnnualIncome, out annualIncome))
                {
                    return BadRequest("Invalid annual income.");
                }

                int? tenure = LoanObj.Tenure;
                if (!tenure.HasValue)
                {
                    return BadRequest("Tenure is required.");
                }

                float monthlyIncome = annualIncome / 12;
                DateTime currentDate = DateTime.Now;
                DateTime futureDate = currentDate.AddYears((int)(tenure.Value / 12));
                string loanStartDate = currentDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-GB"));
                string loanEndDate = futureDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-GB"));

                double interest = (double)(LoanObj.Interest / 100);
                var total_loan_Amount = Math.Round(loanAmount + (loanAmount * interest * tenure.Value)).ToString();
                var loan_Emi = Math.Round(loanAmount * (interest / 12) * Math.Pow((1 + (interest / 12)), (tenure.Value * 12)) / (Math.Pow((1 + (interest / 12)), (tenure.Value * 12)) - 1)).ToString();

                existingLoanObj.LoanId = loanid;
                existingLoanObj.AccountNum = LoanObj.AccountNum;
                existingLoanObj.LoanType = LoanObj.LoanType;
                existingLoanObj.LoanAmount = LoanObj.LoanAmount;
                existingLoanObj.Interest = LoanObj.Interest;
                existingLoanObj.Tenure = tenure.Value;
                existingLoanObj.LoanEmi = loan_Emi.ToString();
                existingLoanObj.LoanTotalAmount = total_loan_Amount.ToString();
                existingLoanObj.MonthlyIncome = monthlyIncome.ToString();
                existingLoanObj.AnnualIncome = LoanObj.AnnualIncome;
                existingLoanObj.OtherEmi = LoanObj.OtherEmi;
                existingLoanObj.LoanStartDate = loanStartDate;
                existingLoanObj.LoanEndDate = loanEndDate;
                existingLoanObj.LoanPurpose = LoanObj.LoanPurpose;
                existingLoanObj.PropertyArea = LoanObj.PropertyArea;
                existingLoanObj.PropertyLoc = LoanObj.PropertyLoc;
                existingLoanObj.PropertyValue = LoanObj.PropertyValue;
                existingLoanObj.OngoingLoan = LoanObj.OngoingLoan;
                existingLoanObj.VehiclePrice = LoanObj.VehiclePrice;
                existingLoanObj.VehicleRCNumber = LoanObj.VehicleRCNumber;
                existingLoanObj.VehicleType = LoanObj.VehicleType;
                existingLoanObj.VendorAddress = LoanObj.VendorAddress;
                existingLoanObj.VendorName = LoanObj.VendorName;
                existingLoanObj.CourseDuration = LoanObj.CourseDuration;
                existingLoanObj.CourseName = LoanObj.CourseName;
                existingLoanObj.TotalFee = LoanObj.TotalFee;
                existingLoanObj.EducationType = LoanObj.EducationType;
                existingLoanObj.InstituteName = LoanObj.InstituteName;
                existingLoanObj.LoanStatus = "Processing";

                _authContext.Entry(existingLoanObj).State = EntityState.Modified;
                await _authContext.SaveChangesAsync();

                return Ok(new { Message = "Loan Updated successfully" });
            }
            catch (Exception ex)
            {
                // Handle exception or log the error
                return StatusCode(500, "An error occurred while updating the loan." + ex.Message);
            }
        }



        [HttpGet("getAllAckLoans")]

        public async Task<IEnumerable<LoanDetails>> GetLoanAckDetails()
        {
            return await _authContext.LoanDetails
        .Where(ld => ld.LoanStatus != "Processing")
        .ToListAsync();
        }


        [HttpGet("getInterest")]
        public async Task<float?> GetInterestByLoanType(string loanType)
        {
            var type = await _authContext.LoanInterest.FirstOrDefaultAsync(x => x.LoanType == loanType);
            return type.LoanInterest;

        }

        [HttpPost("applyLoan")]

        public async Task<IActionResult> ApplyLoan([FromBody] LoanDetails LoanObj)
        {
            var interest_value = await GetInterestByLoanType(LoanObj.LoanType);

            LoanObj.Interest = interest_value;



            float loanAmount = float.Parse(LoanObj.LoanAmount);
            var monthlyIncome = (float.Parse(LoanObj.AnnualIncome) / 12).ToString();
            var tenure = LoanObj.Tenure / 12;


            DateTime currentDate = DateTime.Now;
            DateTime futureDate = DateTime.Now;
            string loanStartDate = currentDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-GB"));
            futureDate = futureDate.AddYears((int)tenure);
            string loanEndDate = futureDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("en-GB"));



            var total_loan_Amount = (Math.Round((double)(loanAmount + (loanAmount * (LoanObj.Interest / 100) * tenure)))).ToString();
            var loan_Emi = (Math.Round((double)(loanAmount * (LoanObj.Interest / 1200) * Math.Pow((double)(1 + (LoanObj.Interest / 1200)), (double)(tenure * 12))) / (Math.Pow((double)(1 + (LoanObj.Interest / 1200)), (double)(tenure * 12)) - 1))).ToString();



            var updatedLoanObj = new LoanDetails
            {
                LoanId = LoanObj.LoanId,
                AccountNum = LoanObj.AccountNum,
                LoanType = LoanObj.LoanType,
                LoanAmount = LoanObj.LoanAmount,
                Interest = LoanObj.Interest,
                Tenure = LoanObj.Tenure,
                LoanEmi = loan_Emi,
                LoanTotalAmount = total_loan_Amount,
                MonthlyIncome = monthlyIncome,
                AnnualIncome = LoanObj.AnnualIncome,
                OtherEmi = LoanObj.OtherEmi,
                LoanStartDate = loanStartDate,
                LoanEndDate = loanEndDate,
                LoanPurpose = LoanObj.LoanPurpose,
                PropertyArea = LoanObj.PropertyArea,
                PropertyLoc = LoanObj.PropertyLoc,
                PropertyValue = LoanObj.PropertyValue,
                OngoingLoan = LoanObj.OngoingLoan,
                VehiclePrice = LoanObj.VehiclePrice,
                VehicleRCNumber = LoanObj.VehicleRCNumber,
                VehicleType = LoanObj.VehicleType,
                VendorAddress = LoanObj.VendorAddress,
                VendorName = LoanObj.VendorName,
                CourseDuration = LoanObj.CourseDuration,
                CourseName = LoanObj.CourseName,
                TotalFee = LoanObj.TotalFee,
                EducationType = LoanObj.EducationType,
                InstituteName = LoanObj.InstituteName,
                LoanStatus = "Processing"
            };

            _authContext.LoanDetails.Add(updatedLoanObj);

            await _authContext.SaveChangesAsync();


            return Ok(new { updatedLoanObj.LoanId });


        }
        [HttpGet("getLoanByAccountNum")]
        public async Task<IActionResult> getLoanByAccountNumber(string accountnum)
        {
            var loanDetails = await _authContext.LoanDetails.Where(x => x.AccountNum == accountnum).OrderByDescending(x => x.LoanId).FirstOrDefaultAsync();


            if (loanDetails == null)
            {
                return BadRequest(new
                {

                    Message = "No Loans taken by the user"
                });
            }
            return Ok(loanDetails);
        }


        [HttpGet("getLoanStatusByAccountNum")]
        public async Task<IActionResult> getLoanStatusByAccountNumber(string accountnum)
        {
            var loanDetails = await _authContext.LoanDetails.Where(x => x.AccountNum == accountnum).ToListAsync();


            if (loanDetails == null)
            {
                return BadRequest(new
                {

                    Message = "No Loans taken by the user"
                });
            }
            return Ok(loanDetails);
        }
        [HttpPut("status")]
        public async Task<IActionResult> UpdateLoanStatus([FromBody] Payload payload)
        {
            var loan = await _authContext.LoanDetails.FindAsync(payload.Id);
            if (loan == null)
            {
                return NotFound();
            }

            loan.LoanStatus = payload.status;
            loan.Comment = payload.comment;
            loan.Modified_At = DateTime.Now;
            loan.LoanAccNum = GenerateRandomNumber();


            _authContext.Entry(loan).State = EntityState.Modified;

            try
            {
                var htmlContent = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #f2f2f2;
            }}
            
            .container {{
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
                background-color: #ffffff;
                border-radius: 5px;
                box-shadow: 0px 2px 4px rgba(0, 0, 0, 0.1);
            }}
            
            .header {{
                text-align: center;
                margin-bottom: 20px;
            }}
            
            .content {{
                margin-bottom: 20px;
            }}
        </style>
    </head>
    <body>
        <div class=""container"">
            <h2 class=""header"">Updated Loan Application Status</h2>
            <div class=""content"">
                <p>Dear Sir/Madam,</p>
                <p>We are pleased to inform you that your loan application has been reviewed and verified by our managers.</p>
                <p>Loan Details:</p>
                <ul>
                    <li>Loan ID: {payload.Id}</li>
                    <li>Loan Status: {payload.status}</li>
                </ul>
                <p>Our team has provided the following comments regarding your application:</p>
                <blockquote>{payload.comment}</blockquote>
                <p>If you have any further questions or require additional information, please feel free to visit our branch or contact our customer support team.</p>
                <p>Thank you for choosing our services.</p>
                <p>Sincerely,</p>
                <p>Alpha Bank</p>
            </div>
        </div>
    </body>
    </html>";

                var emailMessage = new Message(
                    new string[] { "archanaabillava0154@gmail.com" },
                    "Updated Loan Application Status",
                    htmlContent);

                await _authContext.SaveChangesAsync();
                _emailService.SendEmail(emailMessage);

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }
        }
       

    }






}
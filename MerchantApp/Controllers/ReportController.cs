using AutoMapper;
using ClosedXML.Excel;
using Data;
using MerchantApp.Exceptions;
using MerchantApp.Requests;
using MerchantApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace MerchantApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        //private readonly AppDbcontext _db;
        //private readonly IMapper _mapper;
        //private readonly Models.UsersMerchant _currentUser;
        private readonly IReportService _reportService;


        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        //[Authorize(Roles ="Administrator")]
        //[HttpGet("items-selling-on-branch")]
        //public IActionResult GetItemsOnBranch([FromQuery] ReportRequest request)
        //{
        //    var branch = _db.Branches.Where(x => x.Id == request.BranchId && x.Active == true).FirstOrDefault();
        //    if (branch == null)
        //        return StatusCode(404, "Branch not found");

        //    var query = _db.ItemBranch.Where(x=>x.BranchId==request.BranchId).AsQueryable();
        //    query = query.Include(x => x.Item);
            
        //    var list = query.ToList();
        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("Items");
        //        //worksheet.Style.Font.Bold = true;
        //        worksheet.ColumnWidth = 25;
                
        //        //title style 
        //        worksheet.Rows(1,3).Style.Font.Bold = true;
        //        worksheet.Rows(1,3).Style.Font.FontColor = XLColor.AirForceBlue;
        //        worksheet.Row(1).Style.Font.FontSize = 20;
        //        worksheet.Rows(2,3).Style.Font.FontSize = 11;

        //        //worksheet.Range(1, 1, 1, 4).Merge();
                
        //        var currentRow = 1;
        //        //title
        //        worksheet.Cell(currentRow++, 1).Value = $"Items currently selling on branch: {branch.Name}";
        //        worksheet.Cell(currentRow++, 1).Value = $"User: {_currentUser.Name} {_currentUser.LastName}";
        //        worksheet.Cell(currentRow, 1).Value = $"Date: {DateTime.Now.ToShortDateString()}";

        //        //table header style
        //        currentRow = 7;
        //        worksheet.Row(currentRow).Style.Font.Bold = true;
        //        worksheet.Row(currentRow).Style.Font.FontColor = XLColor.AirForceBlue;
                
                
        //        //table header
        //        worksheet.Cell(currentRow, 1).Value = "Name";
        //        worksheet.Cell(currentRow, 2).Value = "Code";
        //        worksheet.Cell(currentRow, 3).Value = "Price ($)";

        //        //table body
        //        foreach (var item in list)
        //        {
        //            currentRow++;
        //            //wrap text 
        //            worksheet.Row(currentRow).Style.Alignment.WrapText = true;
        //            //insert in cells
        //            worksheet.Cell(currentRow, 1).Value = item.Item.Name;
        //            worksheet.Cell(currentRow, 2).Value = item.Item.Code;
        //            worksheet.Cell(currentRow, 3).Value = item.Item.Price;
        //        }


        //        using (var stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            var content = stream.ToArray();
        //            //return 
        //            return File(
        //                content,
        //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                "ItemsOnBranch.xlsx"
        //                );
        //        }
        //    }
        //}

        [Authorize(Roles ="Administrator")]
        [HttpGet("items-selling-on-branch")]
        public IActionResult GetItemsOnBranch([FromQuery] ReportRequest request)
        {
            try
            {
                return File(
                        _reportService.GetItemsOnBranch(request),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ItemsOnBranch.xlsx"
                        );
            }
            catch (CustomException e)
            {

                return StatusCode(404, e.Message);
            }
        }

        [Authorize(Roles ="Administrator")]
        [HttpGet("currently-active-merchants")]
        public IActionResult GetMerchants()
        {
            try
            {
                return File(
                        _reportService.GetMerchants(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Merchants.xlsx"
                        );
            }
            catch (CustomException e)
            {

                return StatusCode(404, e.Message);
            }
        }
        
        [Authorize(Roles = "Administrator")]
        [HttpGet("monthly-sale")]
        public IActionResult GetMonthlySale([FromQuery] ReportMonthlySaleRequest request)
        {
            try
            {
                return File(
                        _reportService.GetMonthlySale(request),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "MonthlySale.xlsx"
                        );
            }
            catch (CustomException e)
            {

                return StatusCode(404, e.Message);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("history-of-transactions")]
        public IActionResult GetHistoryOfTransactions([FromQuery] ReportHistoryTransactionsRequest request)
        {
            try
            {
                return File(
                        _reportService.GetHistoryOfTransactions(request),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "MonthlySale.xlsx"
                        );
            }
            catch (CustomException e)
            {

                return StatusCode(404, e.Message);
            }
        }
    }
}
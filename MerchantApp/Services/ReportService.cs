using AutoMapper;
using ClosedXML.Excel;
using Data;
using MerchantApp.Exceptions;
using MerchantApp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace MerchantApp.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbcontext _db;
        private readonly IMapper _mapper;
        private readonly Models.UsersMerchant _currentUser;

        public ReportService(AppDbcontext db, IMapper mapper, IHttpContextAccessor accessor)
        {
            _db = db;
            _mapper = mapper;
            var username = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username").Value;
            _currentUser = _mapper.Map<Models.UsersMerchant>(_db.UsersMerchants.Where(x => x.Username == username).Include(x => x.Role).Include(x => x.Branch).FirstOrDefault());
        }


        public byte[] GetItemsOnBranch(ReportRequest request)
        {
            var branch = _db.Branches.Where(x => x.Id == request.BranchId && x.Active == true).FirstOrDefault();
            if (branch == null)
                throw new CustomException("Branch not found");
            //nadji mi listu itema koje prodajem na branchu
            var query = _db.ItemBranch.Where(x => x.BranchId == request.BranchId).AsQueryable();
            query = query.Include(x => x.Item);

            var list = query.ToList();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Items");
                //worksheet.Style.Font.Bold = true;

                //STYLE
                worksheet.Column(1).Width = 25;
                worksheet.Column(2).Width = 10;
                worksheet.Column(3).Width = 10;
                worksheet.Column(4).Width = 10;

                #region TITLE
                //title style 
                worksheet.Rows(1, 4).Style.Font.Bold = true;
                worksheet.Rows(1, 4).Style.Font.FontColor = XLColor.AirForceBlue;
                worksheet.Row(1).Style.Font.FontSize = 11;
                worksheet.Row(3).Style.Font.FontSize = 20;
                worksheet.Row(4).Style.Font.FontSize = 11;

                //worksheet.Range(1, 1, 1, 4).Merge();

                var currentRow = 1;
                //title
                worksheet.Cell(currentRow, 1).Value = $"User: {_currentUser.Name} {_currentUser.LastName}";
                worksheet.Cell(currentRow, 6).Value = $"Date: {DateTime.Now.ToShortDateString()}";
                currentRow += 2;

                worksheet.Range(currentRow, 1, currentRow, 7).Merge();
                worksheet.Row(currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow++, 1).Value = $"Items currently selling on branch: {branch.Name}";
                #endregion

                #region TABLE
                //table header style
                currentRow = 8;
                worksheet.Row(currentRow).Style.Font.Bold = true;
                worksheet.Row(currentRow).Style.Font.FontColor = XLColor.AirForceBlue;


                //table header
                worksheet.Cell(currentRow, 1).Value = "Name";
                worksheet.Cell(currentRow, 2).Value = "Code";
                worksheet.Cell(currentRow, 3).Value = "Price ($)";
                worksheet.Cell(currentRow, 4).Value = "Quantity (IU)";

                //table body
                foreach (var itembBranch in list)
                {
                    //udji u ItemDetails, izvuci mi u listu sve detalje koji su vezani za trazeni item na branchu
                    var listItemDetail = _db.ItemDetails.Where(x => x.ItemBranchId == itembBranch.Id).ToList();
                    int quantity = 0;

                    foreach (var x in listItemDetail)
                    {
                        quantity += x.Quantity;
                    }
                    //napravi mi red
                    currentRow++;

                    //wrap text 
                    worksheet.Row(currentRow).Style.Alignment.WrapText = true;

                    //insert in cells
                    worksheet.Cell(currentRow, 1).Value = itembBranch.Item.Name;
                    worksheet.Cell(currentRow, 2).Value = itembBranch.Item.Code;
                    worksheet.Cell(currentRow, 3).Value = itembBranch.Item.Price;
                    worksheet.Cell(currentRow, 4).Value = quantity;
                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        public byte[] GetMerchants()
        {
            var merchants = _db.UsersMerchants.Where(x => x.Role.Name == "Merchant").ToList();
            var branches = _db.Branches.ToList();

            using (var workbook = new XLWorkbook())
            {

                foreach (var branch in branches)
                {
                    var worksheet = workbook.Worksheets.Add(branch.Name);

                    var merchantBranch = merchants.Where(x => x.BranchId == branch.Id).ToList();

                    //column width
                    //worksheet.ColumnWidth = 25;
                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 25;
                    worksheet.Column(4).Width = 15;
                    worksheet.Column(5).Width = 15;


                    #region TITLE
                    //title style 
                    worksheet.Rows(1, 4).Style.Font.Bold = true;
                    worksheet.Rows(1, 4).Style.Font.FontColor = XLColor.AirForceBlue;
                    worksheet.Row(1).Style.Font.FontSize = 11;
                    worksheet.Row(3).Style.Font.FontSize = 20;
                    worksheet.Row(4).Style.Font.FontSize = 11;

                    //worksheet.Range(1, 1, 1, 4).Merge();

                    var currentRow = 1;
                    //title
                    worksheet.Cell(currentRow, 1).Value = $"User: {_currentUser.Name} {_currentUser.LastName}";
                    worksheet.Cell(currentRow, 5).Value = $"Date: {DateTime.Now.ToShortDateString()}";
                    currentRow += 2;

                    worksheet.Range(currentRow, 1, currentRow, 5).Merge();
                    worksheet.Row(currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow++, 1).Value = $"Merchants working on branch: {branch.Name}";
                    #endregion

                    #region TABLE
                    //table header style
                    currentRow = 8;
                    worksheet.Row(currentRow).Style.Font.Bold = true;
                    worksheet.Row(currentRow).Style.Font.FontColor = XLColor.AirForceBlue;

                    //table header
                    worksheet.Cell(currentRow, 1).Value = "Name";
                    worksheet.Cell(currentRow, 2).Value = "Last name";
                    worksheet.Cell(currentRow, 3).Value = "Email";
                    worksheet.Cell(currentRow, 4).Value = "Phone number";
                    worksheet.Cell(currentRow, 5).Value = "Date rigistered";


                    //table body
                    foreach (var merchant in merchantBranch)
                    {

                        //new row
                        currentRow++;

                        //wrap text 
                        worksheet.Row(currentRow).Style.Alignment.WrapText = true;

                        //insert in cells
                        worksheet.Cell(currentRow, 1).Value = merchant.Name;
                        worksheet.Cell(currentRow, 2).Value = merchant.LastName;
                        worksheet.Cell(currentRow, 3).Value = merchant.Email;
                        worksheet.Cell(currentRow, 4).Value = merchant.PhoneNumber;
                        worksheet.Cell(currentRow, 5).Value = merchant.DateRegistered.ToShortDateString();
                    }
                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }


        }

        public byte[] GetMonthlySale(ReportMonthlySaleRequest request)
        {
            //if (request.Month < 1 || request.Month > 12)
            //    throw new CustomException("Invalid input for month.");
            var branch = _db.Branches.Where(x => x.Id == request.BranchId && x.Active == true).FirstOrDefault();
            if (branch == null)
                throw new CustomException("Branch not found");

            //find list of items on branch
            var query = _db.ItemBranch.Where(x => x.BranchId == request.BranchId).AsQueryable();
            query = query.Where(x => x.Active == true);
            query = query.Include(x => x.Item);

            var listItemBranch = query.ToList();

            //make list of orders from reqursted month
            //var listOrders = _db.Orders.Where(x => x.orderDate.Month == request.Month).ToList();
            var listOrders = _db.Orders.Where(x => x.orderDate >= request.DateFrom && x.orderDate <= request.DateTo).ToList();
            //take shopping cart IDs from list of orders
            var listShoppingCarts = listOrders.Select(x => x.ShoppingCartId).ToList();


            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Items");

                #region Style
                //STYLE
                worksheet.Column(1).Width = 25;
                worksheet.Column(2).Width = 10;
                worksheet.Column(3).Width = 10;
                worksheet.Column(4).Width = 10;


                //title style 
                worksheet.Rows(1, 4).Style.Font.Bold = true;
                worksheet.Rows(1, 4).Style.Font.FontColor = XLColor.AirForceBlue;
                worksheet.Row(1).Style.Font.FontSize = 11;
                worksheet.Row(3).Style.Font.FontSize = 20;
                worksheet.Row(4).Style.Font.FontSize = 11;
                #endregion

                var currentRow = 1;

                #region TITLE
                //get name of the month
                //var monthName = CultureInfo.GetCultureInfo("en-GB").DateTimeFormat.GetMonthName(request.Month);

                worksheet.Cell(currentRow, 1).Value = $"User: {_currentUser.Name} {_currentUser.LastName}";
                worksheet.Cell(currentRow, 6).Value = $"Date: {DateTime.Now.ToShortDateString()}";
                currentRow += 2;

                worksheet.Range(currentRow, 1, currentRow, 7).Merge();
                worksheet.Row(currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow++, 1).Value = $"Items sold on branch {branch.Name}";// during {monthName}

                worksheet.Cell(currentRow, 2).Value = $"From: {request.DateFrom.ToShortDateString()}";// during {monthName}
                worksheet.Cell(currentRow++, 4).Value = $"To: {request.DateTo.ToShortDateString()}";// during {monthName}
                #endregion

                #region TABLE 
                //table header style
                currentRow = 8;
                worksheet.Row(currentRow).Style.Font.Bold = true;
                worksheet.Row(currentRow).Style.Font.FontColor = XLColor.AirForceBlue;


                //table header
                worksheet.Cell(currentRow, 1).Value = "Name";
                worksheet.Cell(currentRow, 2).Value = "Code";
                worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, 3).Value = "Qty";
                worksheet.Cell(currentRow, 4).Value = "Profit ($)";



                //table body
                foreach (var itemBranch in listItemBranch)
                {
                    //take all details from ordered shopping carts
                    var scDetailsWithCurrentItem = _db.SCDetails.Where(x => x.ShoppingCart.Ordered == true).AsQueryable();
                    scDetailsWithCurrentItem.Include(x => x.ItemDetails);

                    //filter them by current item from foreach
                    scDetailsWithCurrentItem = scDetailsWithCurrentItem.Where(x => x.ItemDetails.ItemBranchId == itemBranch.Id);

                    //turn it into a list
                    var listScDetailsWithCurrentItem = scDetailsWithCurrentItem.ToList();

                    //calculate how many was sold and price
                    int quantity = 0;
                    float totalPrice = 0;
                    foreach (var SCD in listScDetailsWithCurrentItem)
                    {
                        if (listShoppingCarts.Contains(SCD.ShoppingCartId))
                        {
                            quantity += SCD.quantity;
                            totalPrice += SCD.TotalPrice;
                        }
                    }

                    //make a row in excel
                    currentRow++;

                    //wrap text 
                    worksheet.Row(currentRow).Style.Alignment.WrapText = true;

                    //insert values in cells
                    worksheet.Cell(currentRow, 1).Value = itemBranch.Item.Name;
                    worksheet.Cell(currentRow, 2).Value = itemBranch.Item.Code;
                    //worksheet.Cell(currentRow, 3).Value = itemBranch.Item.Price;
                    worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, 3).Value = quantity;
                    worksheet.Cell(currentRow, 4).Value = totalPrice;

                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }

            }
        }

        public byte[] GetHistoryOfTransactions(ReportHistoryTransactionsRequest request)
        {

            var queryTransaction = _db.Transactions.Include(x => x.Orders).AsQueryable();
            if (request.DateFrom != null)
                queryTransaction = queryTransaction.Where(x => x.Orders.orderDate >= request.DateFrom);
            if (request.DateTo != null)
                queryTransaction = queryTransaction.Where(x => x.Orders.orderDate <= request.DateTo);

            queryTransaction = queryTransaction.Include(x => x.Orders.Coupon);
                                               //.Include(x => x.PaymentMethod);
            queryTransaction = queryTransaction.OrderBy(x => x.Orders.orderDate);
            var listTransactions = queryTransaction.ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Transaction");


                #region Style
                //STYLE
                worksheet.Column(1).Width = 15;
                worksheet.Column(2).Width = 12;
                worksheet.Column(3).Width = 15;
                worksheet.Column(4).Width = 12;
                worksheet.Column(5).Width = 12;
                worksheet.Column(6).Width = 12;


                //title style 
                worksheet.Rows(1, 4).Style.Font.Bold = true;
                worksheet.Rows(1, 4).Style.Font.FontColor = XLColor.AirForceBlue;
                worksheet.Row(1).Style.Font.FontSize = 11;
                worksheet.Row(3).Style.Font.FontSize = 20;
                worksheet.Row(4).Style.Font.FontSize = 11;
                #endregion

                var currentRow = 1;

                #region TITLE

                worksheet.Cell(currentRow, 1).Value = $"User: {_currentUser.Name} {_currentUser.LastName}";
                worksheet.Cell(currentRow, 6).Value = $"Date: {DateTime.Now.ToShortDateString()}";
                currentRow += 2;

                worksheet.Range(currentRow, 1, currentRow, 7).Merge();
                worksheet.Row(currentRow).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow++, 1).Value = $"History of transactions";

                var from = "";
                if (request.DateFrom != null)
                    from = request.DateFrom?.ToShortDateString();
                var to = DateTime.Now.ToShortDateString();
                if (request.DateTo != null)
                    from = request.DateTo?.ToShortDateString();

                worksheet.Cell(currentRow, 2).Value = $"From: {from}";
                worksheet.Cell(currentRow++, 5).Value = $"To: {to}";
                #endregion

                #region TABLE 
                //table header style
                currentRow = 8;
                worksheet.Row(currentRow).Style.Font.Bold = true;
                worksheet.Row(currentRow).Style.Font.FontColor = XLColor.AirForceBlue;


                //table header
                #region tableheader style
                worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                #endregion

                worksheet.Cell(currentRow, 1).Value = "Order Date";
                worksheet.Cell(currentRow, 2).Value = "Order ID";
                //worksheet.Cell(currentRow, 3).Value = "Payment method";
                worksheet.Cell(currentRow, 3).Value = "Total price";
                worksheet.Cell(currentRow, 4).Value = "Discount (%)";
                worksheet.Cell(currentRow, 5).Value = "Final price ($)";


                //table body
                foreach (var transaction in listTransactions)
                {

                    //make a row in excel
                    currentRow++;

                    //wrap text 
                    worksheet.Row(currentRow).Style.Alignment.WrapText = true;

                    #region cell alignment
                    worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    #endregion

                    //insert values in cells
                    worksheet.Cell(currentRow, 1).Value = transaction.Orders.orderDate.ToShortDateString();
                    worksheet.Cell(currentRow, 2).Value = transaction.Orders.Id;
                    //worksheet.Cell(currentRow, 3).Value = transaction.PaymentMethod.PaymentMethodName;
                    worksheet.Cell(currentRow, 3).Value = transaction.Orders.totalPrice;
                    var coupon = transaction.Orders.Coupon;
                    if (coupon == null)
                        worksheet.Cell(currentRow, 4).Value = 0;
                    else
                        worksheet.Cell(currentRow, 4).Value = coupon.Discount;
                    worksheet.Cell(currentRow, 5).Value = transaction.Orders.finalPrice;

                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }


        }
    }
}
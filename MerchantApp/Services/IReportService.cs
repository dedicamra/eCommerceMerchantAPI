using ClosedXML.Excel;
using MerchantApp.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MerchantApp.Services
{
    public interface IReportService
    {
        byte[] GetItemsOnBranch(ReportRequest request);
        byte[] GetMerchants();
        byte[] GetMonthlySale(ReportMonthlySaleRequest request);
        byte[] GetHistoryOfTransactions(ReportHistoryTransactionsRequest request);
    }
}

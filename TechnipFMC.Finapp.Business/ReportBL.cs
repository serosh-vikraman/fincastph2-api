using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using TechnipFMC.Finapp.Business.Interfaces;
using TechnipFMC.Finapp.Data;
using TechnipFMC.Finapp.Models;

namespace TechnipFMC.Finapp.Business
{
    public class ReportBL : IReportBL
    {
        private readonly ReportRepository _reportRepository = new ReportRepository();

        private string numberformat = "#,##0.000";
        private string decimalformat = "#0.00";
        public byte[] GetProjectLifeCycleReportExcel(string projectIds, int scopeId)
        {
            var reportData = _reportRepository.GetProjectLifeCycleReportData(projectIds, scopeId);
            //if (reportData.Actuals.Count == 0)
            //{
            //    throw new Exception("No ScenarioData Found.");
            //}

            var years = reportData.Actuals.Select(a => a.Year).Distinct().OrderBy(y => y).ToList();
            var currentYear = DateTime.Now.Year;
            var noOfYears = years.Count;

            ExcelPackage excelPkg = new ExcelPackage();
            ExcelWorksheet workSheet = excelPkg.Workbook.Worksheets.Add("Profit & Loss Report");

            var rv_StartColumn = 3;
            var rv_EndColumn = rv_StartColumn + noOfYears;
            var gm_StartColumn = rv_EndColumn + 2;
            var gm_EndColumn = gm_StartColumn + noOfYears;

            workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(2).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(3).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(4).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(5).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(2).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(3).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(4).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(5).Style.Fill.BackgroundColor.SetColor(Color.White);

            workSheet.Cells[1, rv_StartColumn, 5, rv_EndColumn].Style.Border.Top.Style = ExcelBorderStyle.Thick;
            workSheet.Cells[1, rv_StartColumn, 5, rv_EndColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            workSheet.Cells[1, rv_StartColumn, 5, rv_EndColumn].Style.Border.Left.Style = ExcelBorderStyle.Thick;
            workSheet.Cells[1, rv_StartColumn, 5, rv_EndColumn].Style.Border.Right.Style = ExcelBorderStyle.Thick;

            workSheet.Cells[1, gm_StartColumn, 5, gm_EndColumn].Style.Border.Top.Style = ExcelBorderStyle.Thick;
            workSheet.Cells[1, gm_StartColumn, 5, gm_EndColumn].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            workSheet.Cells[1, gm_StartColumn, 5, gm_EndColumn].Style.Border.Left.Style = ExcelBorderStyle.Thick;
            workSheet.Cells[1, gm_StartColumn, 5, gm_EndColumn].Style.Border.Right.Style = ExcelBorderStyle.Thick;

            workSheet.Cells["A4:B5"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["A4:B5"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["A4:B5"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["A4:B5"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

            workSheet.Cells[2, rv_StartColumn, 5, rv_EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells[2, gm_StartColumn, 5, gm_EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Orange);

            workSheet.Cells["A4"].Value = "Project No";
            workSheet.Cells["A4:A5"].Merge = true;
            workSheet.Cells["A4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["A4"].Style.Fill.BackgroundColor.SetColor(Color.Orange);

            workSheet.Cells["B4"].Value = "Project No";
            workSheet.Cells["B4:B5"].Merge = true;
            workSheet.Cells["B4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["B4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["B4"].Style.Fill.BackgroundColor.SetColor(Color.Orange);

            workSheet.Cells[1, rv_StartColumn].Value = (scopeId == 2) ? "ORDER INTAKE - ACTUALS - REVENUE" : "P & L - ACTUALS - REVENUE";
            workSheet.Cells[1, rv_StartColumn, 1, rv_EndColumn].Merge = true;
            workSheet.Cells[1, rv_StartColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, rv_StartColumn].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            workSheet.Cells[2, rv_StartColumn, 3, (rv_EndColumn - 1)].Merge = true;

            workSheet.Cells[2, rv_EndColumn].Value = "Latest ForeCast for Available";
            workSheet.Cells[2, rv_EndColumn, 4, rv_EndColumn].Merge = true;
            workSheet.Cells[2, rv_EndColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[2, rv_EndColumn].Style.WrapText = true;
            workSheet.Column(rv_EndColumn).Width = 8.15;

            workSheet.Cells[1, gm_StartColumn].Value = (scopeId == 2) ? "ORDER INTAKE - ACTUALS - GROSS MARGIN" : "P & L - ACTUALS - GROSS MARGIN";
            workSheet.Cells[1, gm_StartColumn, 1, gm_EndColumn].Merge = true;
            workSheet.Cells[1, gm_StartColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells[1, gm_StartColumn].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            workSheet.Cells[2, gm_StartColumn, 3, (gm_EndColumn - 1)].Merge = true;

            workSheet.Cells[2, gm_EndColumn].Value = "Latest ForeCast for Available";
            workSheet.Cells[2, gm_EndColumn, 4, gm_EndColumn].Merge = true;
            workSheet.Cells[2, gm_EndColumn].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells[2, gm_EndColumn].Style.WrapText = true;
            workSheet.Column(gm_EndColumn).Width = 8.15;

            var pId = 0;
            var row = 6;
            foreach (var projectId in projectIds.Split(',').ToList())
            {
                var rv_col = rv_StartColumn;
                var gm_col = gm_StartColumn;
                if (pId == Convert.ToInt32(projectId))
                {
                    continue;
                }
                pId = Convert.ToInt32(projectId);

                var projectScenariosActuals = reportData.Actuals.Where(p => p.ProjectID == pId).ToList();
                var projectScenariosForeCasts = reportData.Actuals.Where(p => p.ProjectID == pId).ToList();
                var count = 1;
                foreach (var year in years)
                {
                    var rv_Data_AC = projectScenariosActuals.FirstOrDefault(p => p.Year == year && p.ScenarioDataTypeID == 1);
                    var gm_Data_AC = projectScenariosActuals.FirstOrDefault(p => p.Year == year && p.ScenarioDataTypeID == 2);

                    var rv_Data_FC = projectScenariosForeCasts.FirstOrDefault(p => p.Year == year && p.ScenarioDataTypeID == 1);
                    var gm_Data_FC = projectScenariosForeCasts.FirstOrDefault(p => p.Year == year && p.ScenarioDataTypeID == 2);

                    if (year == currentYear)
                    {
                        workSheet.Cells[4, rv_col].Value = rv_Data_AC.ScenarioName;
                        workSheet.Cells[5, rv_col].Value = $"Y{count} - {year}";

                        workSheet.Cells[4, gm_col].Value = gm_Data_AC.ScenarioName;
                        workSheet.Cells[5, gm_col].Value = $"Y{count} - {year}";

                        var month = DateTime.Now.Month;
                        if (month >= 1 && month <= 3)
                        {
                            workSheet.Cells[row, rv_col].Value = rv_Data_FC.Total;
                            workSheet.Cells[row, gm_col].Value = gm_Data_FC.Total;
                        }
                        if (month >= 3 && month <= 6)
                        {
                            workSheet.Cells[row, rv_col].Value = rv_Data_AC.Q1New + rv_Data_FC.Q2New + rv_Data_FC.Q3New + rv_Data_FC.Q4New;
                            workSheet.Cells[row, gm_col].Value = gm_Data_AC.Q1New + gm_Data_FC.Q2New + gm_Data_FC.Q3New + gm_Data_FC.Q4New;
                        }
                        if (month >= 6 && month <= 9)
                        {
                            workSheet.Cells[row, rv_col].Value = rv_Data_AC.Q1New + rv_Data_AC.Q2New + rv_Data_FC.Q3New + rv_Data_FC.Q4New;
                            workSheet.Cells[row, gm_col].Value = gm_Data_AC.Q1New + gm_Data_AC.Q2New + gm_Data_FC.Q3New + gm_Data_FC.Q4New;
                        }
                        if (month >= 9 && month <= 12)
                        {
                            workSheet.Cells[row, rv_col].Value = rv_Data_AC.Q1New + rv_Data_AC.Q2New + rv_Data_AC.Q3New + rv_Data_FC.Q4New;
                            workSheet.Cells[row, gm_col].Value = gm_Data_AC.Q1New + gm_Data_AC.Q2New + gm_Data_AC.Q3New + gm_Data_FC.Q4New;
                        }
                    }
                    else if (year > currentYear)
                    {
                        workSheet.Cells[5, rv_col].Value = $"{year}+";
                        workSheet.Cells[5, gm_col].Value = $"{year}+";

                        var rvfc = projectScenariosActuals.Where(p => p.Year > currentYear && p.ScenarioDataTypeID == 1).Select(p => p.Total).AsQueryable<decimal>().Sum();
                        var gmfc = projectScenariosActuals.Where(p => p.Year > currentYear && p.ScenarioDataTypeID == 2).Select(p => p.Total).AsQueryable<decimal>().Sum();

                        workSheet.Cells[row, rv_col].Value = rvfc;
                        workSheet.Cells[row, gm_col].Value = gmfc;
                    }
                    else
                    {
                        if (rv_Data_AC != null)
                        {
                            workSheet.Cells[4, rv_col].Value = rv_Data_AC.ScenarioName;
                            workSheet.Cells[5, rv_col].Value = $"Y{count} - {year}";
                            workSheet.Cells[row, rv_col].Value = rv_Data_AC.Total;
                        }
                        if (gm_Data_AC != null)
                        {
                            workSheet.Cells[4, gm_col].Value = gm_Data_AC.ScenarioName;
                            workSheet.Cells[5, gm_col].Value = $"Y{count} - {year}";
                            workSheet.Cells[row, gm_col].Value = gm_Data_AC.Total;
                        }
                    }
                    count++;
                    rv_col++; gm_col++;
                }
            }

            return excelPkg.GetAsByteArray();
        }

        public byte[] GetREPExtractReportDataExcel(int year, int reportTypeId, int scenarioId, string groupLevels)
        {
            var reportData = _reportRepository.GetREPExtractReportData(year, reportTypeId, scenarioId, groupLevels);
            if (reportData.REPExtractData.Count == 0)
            {
                throw new Exception("No ScenarioData Found.");
            }

            ExcelPackage excelPkg = new ExcelPackage();
            ExcelWorksheet workSheet = excelPkg.Workbook.Worksheets.Add("REP Extract");

            workSheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(3).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(4).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(5).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(6).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(3).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(4).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(5).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(6).Style.Fill.BackgroundColor.SetColor(Color.White);

            workSheet.Cells["A1:O7"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            workSheet.Cells["A1:O7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            workSheet.Cells["A1:O7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            workSheet.Cells["A1:O7"].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            workSheet.Cells["A5:D6"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["A5:D6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["A5:D6"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["A5:D6"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

            workSheet.Cells["G1 "].Value = (reportTypeId == 1) ? "REP REPORT FOR ACTUALS" : "REP REPORT FOR FORECAST";
            workSheet.Cells["G1:K1"].Merge = true;
            workSheet.Cells["G1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["G1"].Style.Fill.BackgroundColor.SetColor(Color.PeachPuff);

            workSheet.Cells["G2 "].Value = (reportTypeId == 1) ? $"YEAR - {year}" : "Current Year & All ForeCast";
            workSheet.Cells["G2:K2"].Merge = true;
            workSheet.Cells["G2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            workSheet.Cells["G3 "].Value = (reportTypeId == 1) ? $"SCENARIO - {reportData.Scenario.ScenarioName} (PROFIT $ LOSS ACTUALS)" : $"SCENARIO - {reportData.Scenario.ScenarioName} (Current Forecast Scenario)";
            workSheet.Cells["G3:K3"].Merge = true;
            workSheet.Cells["G3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["G3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            workSheet.Cells["A5"].Value = "Entity";
            workSheet.Cells["A5:A6"].Merge = true;
            workSheet.Column(1).Width = 12;
            workSheet.Cells["A5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["A5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            workSheet.Cells["B5"].Value = "Smart Link";
            workSheet.Cells["B5:B6"].Merge = true;
            workSheet.Column(2).Width = 9;
            workSheet.Cells["B5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["B5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["B5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            workSheet.Cells["C5"].Value = "BUCategory";
            workSheet.Cells["C5:C6"].Merge = true;
            workSheet.Column(3).Width = 11.20;
            workSheet.Cells["C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["C5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            workSheet.Cells["D5"].Value = "Segment Code";
            workSheet.Cells["D5:D6"].Merge = true;
            workSheet.Column(4).Width = 14.50;
            workSheet.Cells["D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["D5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            if (reportTypeId == 1)
            {
                //REPORT TYPE 1
                workSheet.Cells["E4:H6"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["E4:H6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["E4:H6"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["E4:H6"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

                workSheet.Cells["J4:M6"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["J4:M6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["J4:M6"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["J4:M6"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

                //REVENUE
                workSheet.Cells["E4"].Value = "REVENUE";
                workSheet.Cells["E4:H4"].Merge = true;
                workSheet.Cells["E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["E4"].Style.Fill.BackgroundColor.SetColor(Color.Green);

                workSheet.Cells["E5"].Value = "Q1";
                workSheet.Cells["E5:E6"].Merge = true;
                workSheet.Column(5).Width = 8.11;
                workSheet.Cells["E5"].Style.WrapText = true;
                workSheet.Cells["E5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["E5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["E5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["F5"].Value = "Q2 \n (Q1 + Q2)";
                workSheet.Cells["F5:F6"].Merge = true;
                workSheet.Cells["F5"].Style.WrapText = true;
                workSheet.Column(6).Width = 9.33;
                workSheet.Cells["F5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["F5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["F5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["G5"].Value = "Q3 \n (Q1 + Q2 + Q3)";
                workSheet.Cells["G5:G6"].Merge = true;
                workSheet.Column(7).Width = 15.56;
                workSheet.Cells["G5"].Style.WrapText = true;
                workSheet.Cells["G5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["G5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["G5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["H5"].Value = "Q4 \n (Q1 + Q2 + Q3 + Q4)";
                workSheet.Cells["H5:H6"].Merge = true;
                workSheet.Column(8).Width = 18;
                workSheet.Cells["H5"].Style.WrapText = true;
                workSheet.Cells["H5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["H5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["H5"].Style.Fill.BackgroundColor.SetColor(Color.BlueViolet);

                //GROSS MARIN
                workSheet.Cells["J4"].Value = "GROSS MARGIN";
                workSheet.Cells["J4:M4"].Merge = true;
                workSheet.Cells["J4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["J4"].Style.Fill.BackgroundColor.SetColor(Color.Green);

                workSheet.Cells["J5"].Value = "Q1";
                workSheet.Cells["J5:J6"].Merge = true;
                workSheet.Column(10).Width = 8.11;
                workSheet.Cells["J5"].Style.WrapText = true;
                workSheet.Cells["J5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["J5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["J5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["K5"].Value = "Q2 \n (Q1 + Q2)";
                workSheet.Cells["K5:K6"].Merge = true;
                workSheet.Cells["K5"].Style.WrapText = true;
                workSheet.Column(11).Width = 9.33;
                workSheet.Cells["K5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["K5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["K5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["L5"].Value = "Q3 \n (Q1 + Q2 + Q3)";
                workSheet.Cells["L5:L6"].Merge = true;
                workSheet.Column(12).Width = 15.56;
                workSheet.Cells["L5"].Style.WrapText = true;
                workSheet.Cells["L5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["L5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["L5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["M5"].Value = "Q4 \n (Q1 + Q2 + Q3 + Q4)";
                workSheet.Cells["M5:M6"].Merge = true;
                workSheet.Column(13).Width = 18;
                workSheet.Cells["M5"].Style.WrapText = true;
                workSheet.Cells["M5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["M5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["M5"].Style.Fill.BackgroundColor.SetColor(Color.BlueViolet);

                var row = 7;
                var segment = "";
                var entity = "";
                var buCategoryCode = "";
                var smartViewCode = "";

                foreach (var data in reportData.REPExtractData)
                {
                    if (data.BUCategoryCode == buCategoryCode && data.ProjectEntityCode == entity
                        && data.ProjectSegmentCode == segment && data.SmartViewCode == smartViewCode)
                    {
                        continue;
                    }

                    segment = data.ProjectSegmentCode;
                    entity = data.ProjectEntityCode;
                    buCategoryCode = data.BUCategoryCode;
                    smartViewCode = data.SmartViewCode;

                    var rv_Data = reportData.REPExtractData.FirstOrDefault(p => p.ProjectEntityCode == entity
                    && p.ProjectSegmentCode == segment && p.SmartViewCode == smartViewCode
                    && p.BUCategoryCode == buCategoryCode && p.ScenarioDataTypeID == 1);

                    var gm_Data = reportData.REPExtractData.FirstOrDefault(p => p.ProjectEntityCode == entity
                    && p.ProjectSegmentCode == segment && p.SmartViewCode == smartViewCode
                    && p.BUCategoryCode == buCategoryCode && p.ScenarioDataTypeID == 2);

                    workSheet.Cells[$"A{row}"].Value = data.ProjectEntityName;
                    workSheet.Cells[$"B{row}"].Value = data.SmartViewCode;
                    workSheet.Cells[$"C{row}"].Value = data.BUCategoryName;
                    workSheet.Cells[$"D{row}"].Value = data.ProjectSegmentName;

                    if (rv_Data != null)
                    {
                        workSheet.Cells[$"E{row}"].Value = rv_Data.Q1New;
                        workSheet.Cells[$"F{row}"].Value = rv_Data.Q1New + rv_Data.Q2New;
                        workSheet.Cells[$"G{row}"].Value = rv_Data.Q1New + rv_Data.Q2New + rv_Data.Q3New;
                        workSheet.Cells[$"H{row}"].Value = rv_Data.Q1New + rv_Data.Q2New + rv_Data.Q3New + rv_Data.Q4New;
                    }
                    if (gm_Data != null)
                    {
                        workSheet.Cells[$"J{row}"].Value = gm_Data.Q1New;
                        workSheet.Cells[$"K{row}"].Value = gm_Data.Q1New + gm_Data.Q2New;
                        workSheet.Cells[$"L{row}"].Value = gm_Data.Q1New + gm_Data.Q2New + gm_Data.Q3New;
                        workSheet.Cells[$"M{row}"].Value = gm_Data.Q1New + gm_Data.Q2New + gm_Data.Q3New + gm_Data.Q4New;
                    }
                    row++;
                }

                workSheet.Cells[$"A{row + 1}"].Value = "Grand Total :";

                workSheet.Cells[$"E{row + 1}"].Formula = $"=SUM(E7:E{row})";
                workSheet.Cells[$"F{row + 1}"].Formula = $"=SUM(F7:F{row})";
                workSheet.Cells[$"G{row + 1}"].Formula = $"=SUM(G7:G{row})";
                workSheet.Cells[$"H{row + 1}"].Formula = $"=SUM(H7:H{row})";

                workSheet.Cells[$"J{row + 1}"].Formula = $"=SUM(J7:J{row})";
                workSheet.Cells[$"K{row + 1}"].Formula = $"=SUM(K7:K{row})";
                workSheet.Cells[$"L{row + 1}"].Formula = $"=SUM(L7:L{row})";
                workSheet.Cells[$"M{row + 1}"].Formula = $"=SUM(M7:M{row})";
            }
            else
            {
                //REPORT TYPE 2
                workSheet.Cells["E4:H6"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["E4:H6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["E4:H6"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["E4:H6"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

                workSheet.Cells["J4:M6"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["J4:M6"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["J4:M6"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
                workSheet.Cells["J4:M6"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

                //REVENUE
                workSheet.Cells["E4"].Value = "REVENUE";
                workSheet.Cells["E4:H4"].Merge = true;
                workSheet.Cells["E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["E4"].Style.Fill.BackgroundColor.SetColor(Color.Green);

                workSheet.Cells["E5"].Value = "Q1";
                workSheet.Cells["E5:E6"].Merge = true;
                workSheet.Column(5).Width = 8.11;
                workSheet.Cells["E5"].Style.WrapText = true;
                workSheet.Cells["E5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["E5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["E5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["F5"].Value = "Q2 \n (Q1 + Q2)";
                workSheet.Cells["F5:F6"].Merge = true;
                workSheet.Cells["F5"].Style.WrapText = true;
                workSheet.Column(6).Width = 9.33;
                workSheet.Cells["F5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["F5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["F5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["G5"].Value = "Q3 \n (Q1 + Q2 + Q3)";
                workSheet.Cells["G5:G6"].Merge = true;
                workSheet.Column(7).Width = 15.56;
                workSheet.Cells["G5"].Style.WrapText = true;
                workSheet.Cells["G5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["G5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["G5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["H5"].Value = "Q4 \n (Q1 + Q2 + Q3 + Q4)";
                workSheet.Cells["H5:H6"].Merge = true;
                workSheet.Column(8).Width = 18;
                workSheet.Cells["H5"].Style.WrapText = true;
                workSheet.Cells["H5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["H5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["H5"].Style.Fill.BackgroundColor.SetColor(Color.BlueViolet);

                //GROSS MARIN
                workSheet.Cells["J4"].Value = "GROSS MARGIN";
                workSheet.Cells["J4:M4"].Merge = true;
                workSheet.Cells["J4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["J4"].Style.Fill.BackgroundColor.SetColor(Color.Green);

                workSheet.Cells["J5"].Value = "Q1";
                workSheet.Cells["J5:J6"].Merge = true;
                workSheet.Column(10).Width = 8.11;
                workSheet.Cells["J5"].Style.WrapText = true;
                workSheet.Cells["J5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["J5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["J5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["K5"].Value = "Q2 \n (Q1 + Q2)";
                workSheet.Cells["K5:K6"].Merge = true;
                workSheet.Cells["K5"].Style.WrapText = true;
                workSheet.Column(11).Width = 9.33;
                workSheet.Cells["K5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["K5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["K5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["L5"].Value = "Q3 \n (Q1 + Q2 + Q3)";
                workSheet.Cells["L5:L6"].Merge = true;
                workSheet.Column(12).Width = 15.56;
                workSheet.Cells["L5"].Style.WrapText = true;
                workSheet.Cells["L5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["L5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["L5"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                workSheet.Cells["M5"].Value = "Q4 \n (Q1 + Q2 + Q3 + Q4)";
                workSheet.Cells["M5:M6"].Merge = true;
                workSheet.Column(13).Width = 18;
                workSheet.Cells["M5"].Style.WrapText = true;
                workSheet.Cells["M5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                workSheet.Cells["M5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells["M5"].Style.Fill.BackgroundColor.SetColor(Color.BlueViolet);

                var row = 7;
                var segment = "";
                var entity = "";
                var buCategoryCode = "";
                var smartViewCode = "";

                foreach (var data in reportData.REPExtractData)
                {
                    if (data.BUCategoryCode == buCategoryCode && data.ProjectEntityCode == entity
                        && data.ProjectSegmentCode == segment && data.SmartViewCode == smartViewCode)
                    {
                        continue;
                    }

                    segment = data.ProjectSegmentCode;
                    entity = data.ProjectEntityCode;
                    buCategoryCode = data.BUCategoryCode;
                    smartViewCode = data.SmartViewCode;

                    var rv_Data = reportData.REPExtractData.FirstOrDefault(p => p.ProjectEntityCode == entity
                    && p.ProjectSegmentCode == segment && p.SmartViewCode == smartViewCode
                    && p.BUCategoryCode == buCategoryCode && p.ScenarioDataTypeID == 1);

                    var gm_Data = reportData.REPExtractData.FirstOrDefault(p => p.ProjectEntityCode == entity
                    && p.ProjectSegmentCode == segment && p.SmartViewCode == smartViewCode
                    && p.BUCategoryCode == buCategoryCode && p.ScenarioDataTypeID == 2);

                    workSheet.Cells[$"A{row}"].Value = data.ProjectEntityName;
                    workSheet.Cells[$"B{row}"].Value = data.SmartViewCode;
                    workSheet.Cells[$"C{row}"].Value = data.BUCategoryName;
                    workSheet.Cells[$"D{row}"].Value = data.ProjectSegmentName;

                    if (rv_Data != null)
                    {
                        workSheet.Cells[$"E{row}"].Value = rv_Data.Q1New;
                        workSheet.Cells[$"F{row}"].Value = rv_Data.Q1New + rv_Data.Q2New;
                        workSheet.Cells[$"G{row}"].Value = rv_Data.Q1New + rv_Data.Q2New + rv_Data.Q3New;
                        workSheet.Cells[$"H{row}"].Value = rv_Data.Q1New + rv_Data.Q2New + rv_Data.Q3New + rv_Data.Q4New;
                    }
                    if (gm_Data != null)
                    {
                        workSheet.Cells[$"J{row}"].Value = gm_Data.Q1New;
                        workSheet.Cells[$"K{row}"].Value = gm_Data.Q1New + gm_Data.Q2New;
                        workSheet.Cells[$"L{row}"].Value = gm_Data.Q1New + gm_Data.Q2New + gm_Data.Q3New;
                        workSheet.Cells[$"M{row}"].Value = gm_Data.Q1New + gm_Data.Q2New + gm_Data.Q3New + gm_Data.Q4New;
                    }
                    row++;
                }

                workSheet.Cells[$"A{row + 1}"].Value = "Grand Total :";

                workSheet.Cells[$"E{row + 1}"].Formula = $"=SUM(E7:E{row})";
                workSheet.Cells[$"F{row + 1}"].Formula = $"=SUM(F7:F{row})";
                workSheet.Cells[$"G{row + 1}"].Formula = $"=SUM(G7:G{row})";
                workSheet.Cells[$"H{row + 1}"].Formula = $"=SUM(H7:H{row})";

                workSheet.Cells[$"J{row + 1}"].Formula = $"=SUM(J7:J{row})";
                workSheet.Cells[$"K{row + 1}"].Formula = $"=SUM(K7:K{row})";
                workSheet.Cells[$"L{row + 1}"].Formula = $"=SUM(L7:L{row})";
                workSheet.Cells[$"M{row + 1}"].Formula = $"=SUM(M7:M{row})";
            }

            return excelPkg.GetAsByteArray();
        }

        public byte[] GetVarianceAnalysisExcel(VarianceAnalysisConfig config, VarianceAnalysisResponseModel data, VarianceAnalysisResponseModel dataGM)
        {
            // number formats
            string positiveFormat = "#,##0.00_)";
            string negativeFormat = "(#,##0.00)";
            string zeroFormat = "-_)";
            string numberFormat = positiveFormat + ";" + negativeFormat;
            string fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat;

            int projectCodeWidth = 30;
            int projectCodeElseWidth = 20;
            int dataWidth = 15;

            var quarters = config.Quarters.Split(',');
            var colcountQuarters = quarters.Length;
            var headercounter = colcountQuarters * 3 + 4;
            ExcelPackage excelPkg = new ExcelPackage();
            ExcelWorksheet worksheet = excelPkg.Workbook.Worksheets.Add("Variance Analysis Report");
            var header = "REVENUE - " + config.Year;

            var headerGM = "GROSS MARGIN - " + config.Year; 
            var colcount = 1;
            var rowcount = 1;

            #region Header Row

            #region blank row
            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            worksheet.Column(colcount).Width = projectCodeWidth;
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            worksheet.Column(colcount).Width = projectCodeElseWidth;
            colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            #endregion

            //colcount++;
            Color colFromHexHeaderGreen = System.Drawing.ColorTranslator.FromHtml("#75D562");
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.BackgroundColor.SetColor(colFromHexHeaderGreen);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = header;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            //GM
            Color colFromHexHeaderBlue = System.Drawing.ColorTranslator.FromHtml("#63C5E9 ");
            colcount = colcount + headercounter + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.BackgroundColor.SetColor(colFromHexHeaderBlue);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = headerGM;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            #endregion

            #region First Row
            rowcount++;
            colcount = 1;
            #region blank row
            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            worksheet.Column(colcount).Width = projectCodeWidth;
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            worksheet.Column(colcount).Width = projectCodeElseWidth;
            colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            #endregion

            Color colFromHexLghtRed = System.Drawing.ColorTranslator.FromHtml("#F9BDA9");

            //colcount++;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.BackgroundColor.SetColor(colFromHexLghtRed);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Previous Budget - S1";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Previous Forecast - S2";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Current Forecast - S3";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Variance";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            //GM
            colcount = colcount + 2;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.BackgroundColor.SetColor(colFromHexLghtRed);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Previous Budget - S1";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Previous Forecast - S2";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Current Forecast - S3";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Variance";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            #endregion

            #region Second Row

            rowcount++;
            colcount = 1;
            #region blank row
            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            worksheet.Column(colcount).Width = projectCodeWidth;
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            worksheet.Column(colcount).Width = projectCodeElseWidth;
            colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            #endregion


            //colcount++;
            Color colFromHexGreen = System.Drawing.ColorTranslator.FromHtml("#92E0B5");
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.BackgroundColor.SetColor(colFromHexGreen);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Compare Scenario 2 - C";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Compare Scenario 1 - B";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Base Scenario - A";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Variance";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            //GM
            colcount = colcount + 2;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.BackgroundColor.SetColor(colFromHexGreen);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Compare Scenario 2 - C";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Compare Scenario 1 - B";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Base Scenario - A";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Variance";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            #endregion

            #region Third Row
            rowcount++;
            colcount = 1;

            #region blank row
            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            worksheet.Column(colcount).Width = projectCodeWidth;
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            worksheet.Column(colcount).Width = projectCodeElseWidth;
            colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;


            #endregion

            //colcount++;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.BackgroundColor.SetColor(colFromHexLghtRed);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = data.GrandTotal[0].CS2;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = data.GrandTotal[0].CS1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = data.GrandTotal[0].BaseScenario;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount].Value = "A-C";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = dataWidth;
            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount++;
            worksheet.Cells[rowcount, colcount].Value = "A-B";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = dataWidth;
            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            //GM
            colcount++;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.Fill.BackgroundColor.SetColor(colFromHexLghtRed);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercounter)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = dataGM.GrandTotal[0].CS2;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = dataGM.GrandTotal[0].CS1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = dataGM.GrandTotal[0].BaseScenario;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + colcountQuarters)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount = colcount + colcountQuarters + 1;
            worksheet.Cells[rowcount, colcount].Value = "A-C";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = dataWidth;
            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);


            colcount++;
            worksheet.Cells[rowcount, colcount].Value = "A-B";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = dataWidth;
            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            #endregion

            #region Fourth Row
            rowcount++;
            colcount = 1;



            Color colFromHexYellow = System.Drawing.ColorTranslator.FromHtml("#FFF7A7");
            Color colFromHexGreenCum = System.Drawing.ColorTranslator.FromHtml("#C3E9D7");
            Color colFromHexGreenCumBase = System.Drawing.ColorTranslator.FromHtml("#4EDF9A");
            Color colFromHexBlue = System.Drawing.ColorTranslator.FromHtml("#ACE8F5");

            worksheet.Cells[rowcount, colcount].Value = "Project No";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = projectCodeWidth;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);

            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "Project Name";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = projectCodeElseWidth;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "Contract Status";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "Entity";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "Contract Nature/Cat";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "Segment";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            //colcount++;

            //worksheet.Cells[rowcount, colcount].Value = "Sponsored/Non Sponsored";
            //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            //worksheet.Column(colcount).Width = projectCodeElseWidth;
            //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            //colcount++;

            if (quarters.Contains("Q1"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q1 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q2"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q2 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q3"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q3 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q4"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q4 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }


            worksheet.Cells[rowcount, colcount].Value = "Total Cumulative";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = dataWidth;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            colcount++;

            if (quarters.Contains("Q1"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q1 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q2"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q2 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q3"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q3 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q4"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q4 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }


            worksheet.Cells[rowcount, colcount].Value = "Total Cumulative";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = dataWidth;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexGreenCum);
            colcount++;

            if (quarters.Contains("Q1"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q1 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q2"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q2 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q3"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q3 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q4"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q4 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }


            worksheet.Cells[rowcount, colcount].Value = "Total Cumulative";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexGreenCumBase);
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "Curr  Fore Vs Budget";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexBlue);
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "Curr  Fore Vs Forecast";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexBlue);

            //GM
            colcount++;
            if (quarters.Contains("Q1"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q1 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q2"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q2 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q3"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q3 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q4"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q4 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }


            worksheet.Cells[rowcount, colcount].Value = "Total Cumulative";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            colcount++;

            if (quarters.Contains("Q1"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q1 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q2"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q2 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q3"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q3 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q4"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q4 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }


            worksheet.Cells[rowcount, colcount].Value = "Total Cumulative";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexGreenCum);
            colcount++;

            if (quarters.Contains("Q1"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q1 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q2"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q2 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q3"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q3 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }
            if (quarters.Contains("Q4"))
            {
                worksheet.Cells[rowcount, colcount].Value = "Q4 " + config.Year;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;
            }


            worksheet.Cells[rowcount, colcount].Value = "Total Cumulative";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexGreenCumBase);
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "Curr  Fore Vs Budget";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexBlue);
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "Curr  Fore Vs Forecast";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexBlue);


            #endregion

            #region DataRow
            Color colFromHexgrandTotal = System.Drawing.ColorTranslator.FromHtml("#FBDACA");
            foreach (var item in data.GridResponse)
            {
                rowcount++;
                colcount = 1;

                if (item.RecordType == "GroupTotal")
                {

                    worksheet.Cells[rowcount, colcount].Value = item.ProjectNo;
                    worksheet.Column(colcount).Width = projectCodeWidth;
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells[rowcount, colcount, rowcount, (colcount + 6 + headercounter + 1 + headercounter + 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount, rowcount, (colcount + 6 + headercounter + 1 + headercounter + 1)].Style.Fill.BackgroundColor.SetColor(colFromHexGreenCum);
                    worksheet.Cells[rowcount, colcount, rowcount, (colcount + 6 + headercounter + 1 + headercounter + 1)].Style.Font.Bold = true;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Value = string.Empty;
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = projectCodeElseWidth;
                    colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;
                }
                else
                {
                    worksheet.Cells[rowcount, colcount].Value = item.ProjectNo;
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = projectCodeWidth;


                    colcount++;
                    if (item.ClubbingParameterName != null && item.ClubbingParameterName != "")
                        worksheet.Cells[rowcount, colcount].Value = item.ClubbingParameterName;
                    else
                        worksheet.Cells[rowcount, colcount].Value = item.ProjectName;
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = projectCodeElseWidth;
                    colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = item.ContractStatusCode;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = item.ProjectEntityCode;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = item.ContractTypeCode;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = item.ProjectSegmentCode;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;

                    //worksheet.Cells[rowcount, colcount].Value = item.ManagementCategoryCode;
                    //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //worksheet.Column(colcount).Width = projectCodeElseWidth;
                    //colcount++;
                }

                if (quarters.Contains("Q1"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS2Q1.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = item.CS2Q1.Value;
                        if (item.CS2Q1.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);

                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q2"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS2Q2.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.CS2Q2.Value);
                        if (item.CS2Q2.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);

                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q3"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS2Q3.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.CS2Q3.Value);
                        if (item.CS2Q3.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);

                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q4"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS2Q4.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.CS2Q4.Value);
                        if (item.CS2Q4.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);

                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.TotalCS2.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = (item.TotalCS2.Value);
                    if (item.TotalCS2.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);

                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                if (item.RecordType != "GroupTotal")
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);

                colcount++;

                if (quarters.Contains("Q1"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS1Q1.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.CS1Q1.Value);
                        if (item.CS1Q1.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);

                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q2"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS1Q2.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.CS1Q2.Value);
                        if (item.CS1Q2.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);

                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q3"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS1Q3.HasValue)
                    {

                        worksheet.Cells[rowcount, colcount].Value = (item.CS1Q3.Value);
                        if (item.CS1Q3.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q4"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS1Q4.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.CS1Q4.Value);
                        if (item.CS1Q4.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;

                }

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.TotalCS1.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = (item.TotalCS1.Value);
                    if (item.TotalCS1.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                if (item.RecordType != "GroupTotal")
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexBlue);
                colcount++;

                if (quarters.Contains("Q1"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.BaseQ1.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.BaseQ1.Value);
                        if (item.BaseQ1.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q2"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.BaseQ2.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.BaseQ2.Value);
                        if (item.BaseQ2.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q3"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.BaseQ3.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.BaseQ3.Value);
                        if (item.BaseQ3.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q4"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.BaseQ4.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (item.BaseQ4.Value);
                        if (item.BaseQ4.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;

                }

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.TotalBase.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = (item.TotalBase.Value);
                    if (item.TotalBase.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                if (item.RecordType != "GroupTotal")
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexGreenCumBase);
                colcount++;

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.Var1.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = item.Var1.Value;
                    if (item.Var1.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colcount++;

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.Var2.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = item.Var2.Value;
                    if (item.Var2.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                colcount++;

                //GM
                List<VarianceAnalysisResponse> itemGM;
                if (item.RecordType == "GroupTotal")
                {
                    itemGM = dataGM.GridResponse.Where(a => a.GroupingParametersCode == item.GroupingParametersCode && a.OriginalProjectNo == item.OriginalProjectNo).ToList();

                }
                else
                    itemGM = dataGM.GridResponse.Where(a => a.GroupingParametersCode == item.GroupingParametersCode && a.ProjectNo == item.OriginalProjectNo).ToList();
                if (itemGM.Count > 1)
                {
                    throw new Exception("More than one record found!");
                }
                var itemGMrecord = itemGM.FirstOrDefault();
                if (quarters.Contains("Q1"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.CS2Q1.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.CS2Q1.Value);
                        if (itemGMrecord.CS2Q1.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q2"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.CS2Q2.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.CS2Q2.Value);
                        if (itemGMrecord.CS2Q2.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q3"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.CS2Q3.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.CS2Q3.Value);
                        if (itemGMrecord.CS2Q3.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q4"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.CS2Q4.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.CS2Q4.Value);
                        if (itemGMrecord.CS2Q4.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (itemGMrecord.TotalCS2.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.TotalCS2.Value);
                    if (itemGMrecord.TotalCS2.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                if (item.RecordType != "GroupTotal")
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                colcount++;

                if (quarters.Contains("Q1"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.CS1Q1.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.CS1Q1.Value);
                        if (itemGMrecord.CS1Q1.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q2"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.CS1Q2.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.CS1Q2.Value);
                        if (itemGMrecord.CS1Q2.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q3"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.CS1Q3.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.CS1Q3.Value);
                        if (itemGMrecord.CS1Q3.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q4"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.CS1Q4.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.CS1Q4.Value);
                        if (itemGMrecord.CS1Q4.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;

                }

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (itemGMrecord.TotalCS1.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.TotalCS1.Value);
                    if (itemGMrecord.TotalCS1.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                if (item.RecordType != "GroupTotal")
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexBlue);
                colcount++;

                if (quarters.Contains("Q1"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.BaseQ1.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.BaseQ1.Value);
                        if (itemGMrecord.BaseQ1.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q2"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.BaseQ2.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.BaseQ2.Value);
                        if (itemGMrecord.BaseQ2.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q3"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.BaseQ3.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.BaseQ3.Value);
                        if (itemGMrecord.BaseQ3.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q4"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGMrecord.BaseQ4.HasValue)
                    {
                        worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.BaseQ4.Value);
                        if (itemGMrecord.BaseQ4.Value < 0)
                            worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    }
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;

                }

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (itemGMrecord.TotalBase.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.TotalBase.Value);
                    if (itemGMrecord.TotalBase.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                if (item.RecordType != "GroupTotal")
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexGreenCumBase);
                colcount++;

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (itemGMrecord.Var1.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.Var1.Value);
                    if (itemGMrecord.Var1.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colcount++;

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (itemGMrecord.Var2.HasValue)
                {
                    worksheet.Cells[rowcount, colcount].Value = (itemGMrecord.Var2.Value);
                    if (itemGMrecord.Var1.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                }
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colcount++;
            }
            #endregion

            #region Grand Total
            foreach (var item in data.GrandTotal)
            {
                rowcount++;
                colcount = 1;

                worksheet.Cells[rowcount, colcount].Value = "GRAND TOTAL";
                worksheet.Column(colcount).Width = projectCodeWidth;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 6)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 7)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 7)].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 7)].Style.Font.Bold = true;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = projectCodeElseWidth;
                colcount++;

                //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                //worksheet.Column(colcount).Width = projectCodeElseWidth;
                //colcount++;

                //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                //worksheet.Column(colcount).Width = projectCodeElseWidth;
                //colcount++;

                //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                //worksheet.Column(colcount).Width = projectCodeElseWidth;
                //colcount++;

                //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                //worksheet.Column(colcount).Width = projectCodeElseWidth;
                //colcount++;

                //worksheet.Cells[rowcount, colcount].Value = string.Empty;
                //worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                //worksheet.Column(colcount).Width = projectCodeElseWidth;
                //colcount++;

                if (quarters.Contains("Q1"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS2Q1.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.CS2Q1.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q2"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS2Q2.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.CS2Q2.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q3"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS2Q3.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.CS2Q3.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q4"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS2Q4.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.CS2Q4.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.TotalCS2.HasValue)
                    worksheet.Cells[rowcount, colcount].Value = (item.TotalCS2.Value);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colcount++;

                if (quarters.Contains("Q1"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS1Q1.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.CS1Q1.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q2"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS1Q2.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.CS1Q2.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q3"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS1Q3.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.CS1Q3.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q4"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.CS1Q4.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.CS1Q4.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;

                }

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.TotalCS1.HasValue)
                    worksheet.Cells[rowcount, colcount].Value = (item.TotalCS1.Value);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colcount++;

                if (quarters.Contains("Q1"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.BaseQ1.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.BaseQ1.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q2"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.BaseQ2.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.BaseQ2.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q3"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.BaseQ3.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.BaseQ3.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
                if (quarters.Contains("Q4"))
                {
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (item.BaseQ4.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (item.BaseQ4.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;

                }

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.TotalBase.HasValue)
                    worksheet.Cells[rowcount, colcount].Value = (item.TotalBase.Value);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colcount++;

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.Var1.HasValue)
                    worksheet.Cells[rowcount, colcount].Value = (item.Var1.Value);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colcount++;

                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                if (item.Var2.HasValue)
                    worksheet.Cells[rowcount, colcount].Value = (item.Var2.Value);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                //GM
                foreach (var itemGM in dataGM.GrandTotal)
                {
                    colcount++;

                    if (quarters.Contains("Q1"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.CS2Q1.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.CS2Q1.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }
                    if (quarters.Contains("Q2"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.CS2Q2.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.CS2Q2.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }
                    if (quarters.Contains("Q3"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.CS2Q3.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.CS2Q3.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }
                    if (quarters.Contains("Q4"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.CS2Q4.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.CS2Q4.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }

                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGM.TotalCS2.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (itemGM.TotalCS2.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;


                    if (quarters.Contains("Q1"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.CS1Q1.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.CS1Q1.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }
                    if (quarters.Contains("Q2"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.CS1Q2.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.CS1Q2.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }
                    if (quarters.Contains("Q3"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.CS1Q3.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.CS1Q3.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }
                    if (quarters.Contains("Q4"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.CS1Q4.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.CS1Q4.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;

                    }

                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGM.TotalCS1.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (itemGM.TotalCS1.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;

                    if (quarters.Contains("Q1"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.BaseQ1.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.BaseQ1.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }
                    if (quarters.Contains("Q2"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.BaseQ2.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.BaseQ2.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }
                    if (quarters.Contains("Q3"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.BaseQ3.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.BaseQ3.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;
                    }
                    if (quarters.Contains("Q4"))
                    {
                        worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                        if (itemGM.BaseQ4.HasValue)
                            worksheet.Cells[rowcount, colcount].Value = (itemGM.BaseQ4.Value);
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = dataWidth;
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        colcount++;

                    }

                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGM.TotalBase.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (itemGM.TotalBase.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;

                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGM.Var1.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (itemGM.Var1.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;

                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    if (itemGM.Var2.HasValue)
                        worksheet.Cells[rowcount, colcount].Value = (itemGM.Var2.Value);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexgrandTotal);
                    worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    colcount++;
                }
            }
            #endregion
            return excelPkg.GetAsByteArray();
        }

        public VarianceAnalysisResponseModel GetVarianceAnalysisReport(VarianceAnalysisConfig config)
        {
            VarianceAnalysisResponseModel responseGrid = new VarianceAnalysisResponseModel();

            List<VarianceAnalysisResponse> response = new List<VarianceAnalysisResponse>();
            var quarters = config.Quarters.Split(',').OrderBy(c => c).ToList();
            var returnList = new ReportRepository().GetVarianceAnalysisReport(config);
            var groupName = "00";

            Decimal? TotalBase = null;
            Decimal? TotalCS1 = null;
            Decimal? TotalCS2 = null;

            Decimal? GroupBaseQ1 = null, GroupBaseQ2 = null, GroupBaseQ3 = null, GroupBaseQ4 = null, GroupBaseTotal = null;
            Decimal? GroupCS1Q1 = null, GroupCS1Q2 = null, GroupCS1Q3 = null, GroupCS1Q4 = null, GroupCS1Total = null;
            Decimal? GroupCS2Q1 = null, GroupCS2Q2 = null, GroupCS2Q3 = null, GroupCS2Q4 = null, GroupCS2Total = null;
            Decimal? GrossBaseQ1 = null, GrossBaseQ2 = null, GrossBaseQ3 = null, GrossBaseQ4 = null, GrossBaseTotal = null;
            Decimal? GrossCS1Q1 = null, GrossCS1Q2 = null, GrossCS1Q3 = null, GrossCS1Q4 = null, GrossCS1Total = null;
            Decimal? GrossCS2Q1 = null, GrossCS2Q2 = null, GrossCS2Q3 = null, GrossCS2Q4 = null, GrossCS2Total = null;

            string baseScenario = "";
            string cs1 = "";
            string cs2 = "";
            int i = 0;
            var _ProjectNo = "";
            var _ProjectName = "";
            var _GroupingParametersCode = "";
            var _LastGroupingParametersCode = "";
            var _GroupingParametersName = "";
            foreach (var item in returnList)
            {
                if (i == 0)
                {
                    baseScenario = item.BaseScenario;
                    cs1 = item.CS1;
                    cs2 = item.CS2;
                }
                i = 2;
                TotalBase = null;
                TotalCS1 = null;
                TotalCS2 = null;

                var IsGroupChanged = false;
                _ProjectNo = item.ProjectNo;
                _ProjectName = item.ProjectName;
                _GroupingParametersCode = item.GroupingParametersCode;
                if (groupName != "00" && groupName != item.GroupingParametersCode)
                {
                    IsGroupChanged = true;
                    Decimal? var11 = null;
                    Decimal? var22 = null;
                    if (GroupBaseTotal.HasValue)
                    {
                        var11 = GroupBaseTotal.Value;
                        if (GroupCS2Total.HasValue)
                            var11 = GroupBaseTotal.Value - GroupCS2Total.Value;
                        var22 = GroupBaseTotal.Value;
                        if (GroupCS1Total.HasValue)
                            var22 = GroupBaseTotal.Value - GroupCS1Total.Value;
                    }
                    else
                    {
                        if (GroupCS2Total.HasValue)
                            var11 = -1 * GroupCS2Total.Value;
                        if (GroupCS1Total.HasValue)
                            var22 = -1 * GroupCS1Total.Value;
                    }
                    if (config.SubTotalRequired == "Y")
                    {
                        response.Add(
                            new VarianceAnalysisResponse()
                            {
                                RecordType = "GroupTotal",
                                ProjectNo = "SUBTOTAL " + _GroupingParametersName,
                                BaseQ1 = GroupBaseQ1,
                                BaseQ2 = GroupBaseQ2,
                                BaseQ3 = GroupBaseQ3,
                                BaseQ4 = GroupBaseQ4,
                                TotalBase = GroupBaseTotal,
                                CS1Q1 = GroupCS1Q1,
                                CS1Q2 = GroupCS1Q2,
                                CS1Q3 = GroupCS1Q3,
                                CS1Q4 = GroupCS1Q4,
                                TotalCS1 = GroupCS1Total,
                                CS2Q1 = GroupCS2Q1,
                                CS2Q2 = GroupCS2Q2,
                                CS2Q3 = GroupCS2Q3,
                                CS2Q4 = GroupCS2Q4,
                                TotalCS2 = GroupCS2Total,
                                Var1 = var11,
                                Var2 = var22,
                                OriginalProjectNo = _LastGroupingParametersCode,
                                GroupingParametersCode = _LastGroupingParametersCode,
                                OriginalProjectName = $"{item.ProjectNo} - {item.ProjectName}"

                            }
                            );
                    }

                }
                if (IsGroupChanged)
                {
                    GroupBaseQ1 = null; GroupBaseQ2 = null; GroupBaseQ3 = null; GroupBaseQ4 = null; GroupBaseTotal = null;
                    GroupCS1Q1 = null; GroupCS1Q2 = null; GroupCS1Q3 = null; GroupCS1Q4 = null; GroupCS1Total = null;
                    GroupCS2Q1 = null; GroupCS2Q2 = null; GroupCS2Q3 = null; GroupCS2Q4 = null; GroupCS2Total = null;
                }
                //if (item.GroupingParametersCode == "")
                //{
                //    GroupBaseQ1 = null; GroupBaseQ2 = null; GroupBaseQ3 = null; GroupBaseQ4 = null; GroupBaseTotal = null;
                //    GroupCS1Q1 = null; GroupCS1Q2 = null; GroupCS1Q3 = null; GroupCS1Q4 = null; GroupCS1Total = null;
                //    GroupCS2Q1 = null; GroupCS2Q2 = null; GroupCS2Q3 = null; GroupCS2Q4 = null; GroupCS2Total = null;
                //}

                VarianceAnalysisResponse respItem = new VarianceAnalysisResponse();
                if (quarters.Contains("Q1"))
                {
                    if (item.BaseQ1.HasValue)
                    {
                        if (TotalBase.HasValue)
                            TotalBase += item.BaseQ1.Value;
                        else
                            TotalBase = item.BaseQ1.Value;
                    }
                    //TotalBase += (item.BaseQ1.HasValue == true ? item.BaseQ1.Value : item.BaseQ1);
                    if (item.CS1Q1.HasValue)
                    {
                        if (TotalCS1.HasValue)
                            TotalCS1 += item.CS1Q1.Value;
                        else
                            TotalCS1 = item.CS1Q1.Value;
                    }
                    //TotalCS1 += (item.CS1Q1.HasValue == true ? item.CS1Q1.Value : item.CS1Q1);
                    if (item.CS2Q1.HasValue)
                    {
                        if (TotalCS2.HasValue)
                            TotalCS2 += item.CS2Q1.Value;
                        else
                            TotalCS2 = item.CS2Q1.Value;
                    }
                    //TotalCS2 += (item.CS2Q1.HasValue == true ? item.CS2Q1.Value : item.CS2Q1);
                    if (item.BaseQ1.HasValue)
                    {
                        if (GroupBaseQ1.HasValue)
                            GroupBaseQ1 += item.BaseQ1.Value;
                        else
                            GroupBaseQ1 = item.BaseQ1.Value;
                    }
                    //GroupBaseQ1 += (item.BaseQ1.HasValue == true ? item.BaseQ1.Value : item.BaseQ1);
                    if (item.CS1Q1.HasValue)
                    {
                        if (GroupCS1Q1.HasValue)
                            GroupCS1Q1 += item.CS1Q1.Value;
                        else
                            GroupCS1Q1 = item.CS1Q1.Value;
                    }
                    //GroupCS1Q1 += (item.CS1Q1.HasValue == true ? item.CS1Q1.Value : item.CS1Q1);
                    if (item.CS2Q1.HasValue)
                    {
                        if (GroupCS2Q1.HasValue)
                            GroupCS2Q1 += item.CS2Q1.Value;
                        else
                            GroupCS2Q1 = item.CS2Q1.Value;
                    }
                    //GroupCS2Q1 += (item.CS2Q1.HasValue == true ? item.CS2Q1.Value : item.CS2Q1);
                    if (item.BaseQ1.HasValue)
                    {
                        if (GroupBaseTotal.HasValue)
                            GroupBaseTotal += item.BaseQ1.Value;
                        else
                            GroupBaseTotal = item.BaseQ1.Value;
                    }
                    // GroupBaseTotal += (item.BaseQ1.HasValue == true ? item.BaseQ1.Value : item.BaseQ1);
                    if (item.CS1Q1.HasValue)
                    {
                        if (GroupCS1Total.HasValue)
                            GroupCS1Total += item.CS1Q1.Value;
                        else
                            GroupCS1Total = item.CS1Q1.Value;
                    }
                    //GroupCS1Total += (item.CS1Q1.HasValue == true ? item.CS1Q1.Value : item.CS1Q1);
                    if (item.CS2Q1.HasValue)
                    {
                        if (GroupCS2Total.HasValue)
                            GroupCS2Total += item.CS2Q1.Value;
                        else
                            GroupCS2Total = item.CS2Q1.Value;
                    }
                    //GroupCS2Total += (item.CS2Q1.HasValue == true ? item.CS2Q1.Value : item.CS2Q1);
                    if (item.BaseQ1.HasValue)
                    {
                        if (GrossBaseQ1.HasValue)
                            GrossBaseQ1 += item.BaseQ1.Value;
                        else
                            GrossBaseQ1 = item.BaseQ1.Value;
                    }
                    //GrossBaseQ1 += (item.BaseQ1.HasValue == true ? item.BaseQ1.Value : item.BaseQ1);
                    if (item.BaseQ1.HasValue)
                    {
                        if (GrossBaseTotal.HasValue)
                            GrossBaseTotal += item.BaseQ1.Value;
                        else
                            GrossBaseTotal = item.BaseQ1.Value;
                    }
                    //GrossBaseTotal += (item.BaseQ1.HasValue == true ? item.BaseQ1.Value : item.BaseQ1);
                    if (item.CS1Q1.HasValue)
                    {
                        if (GrossCS1Q1.HasValue)
                            GrossCS1Q1 += item.CS1Q1.Value;
                        else
                            GrossCS1Q1 = item.CS1Q1.Value;
                    }
                    //GrossCS1Q1 += (item.CS1Q1.HasValue == true ? item.CS1Q1.Value : item.CS1Q1);
                    if (item.CS1Q1.HasValue)
                    {
                        if (GrossCS1Total.HasValue)
                            GrossCS1Total += item.CS1Q1.Value;
                        else
                            GrossCS1Total = item.CS1Q1.Value;
                    }
                    //GrossCS1Total += (item.CS1Q1.HasValue == true ? item.CS1Q1.Value : item.CS1Q1);
                    if (item.CS2Q1.HasValue)
                    {
                        if (GrossCS2Q1.HasValue)
                            GrossCS2Q1 += item.CS2Q1.Value;
                        else
                            GrossCS2Q1 = item.CS2Q1.Value;
                    }
                    //GrossCS2Q1 += (item.CS2Q1.HasValue == true ? item.CS2Q1.Value : item.CS2Q1);
                    if (item.CS2Q1.HasValue)
                    {
                        if (GrossCS2Total.HasValue)
                            GrossCS2Total += item.CS2Q1.Value;
                        else
                            GrossCS2Total = item.CS2Q1.Value;
                    }
                    //GrossCS2Total += (item.CS2Q1.HasValue == true ? item.CS2Q1.Value : item.CS2Q1);
                }
                else
                {
                    item.BaseQ1 = null;
                    item.CS1Q1 = null;
                    item.CS2Q1 = null;
                }
                if (quarters.Contains("Q2"))
                {
                    if (item.BaseQ2.HasValue)
                    {
                        if (TotalBase.HasValue)
                            TotalBase += item.BaseQ2.Value;
                        else
                            TotalBase = item.BaseQ2.Value;
                    }
                    //TotalBase += (item.BaseQ2.HasValue == true ? item.BaseQ2.Value : item.BaseQ2);
                    if (item.CS1Q2.HasValue)
                    {
                        if (TotalCS1.HasValue)
                            TotalCS1 += item.CS1Q2.Value;
                        else
                            TotalCS1 = item.CS1Q2.Value;
                    }
                    //TotalCS1 += (item.CS1Q2.HasValue == true ? item.CS1Q2.Value : item.CS1Q2);
                    if (item.CS2Q2.HasValue)
                    {
                        if (TotalCS2.HasValue)
                            TotalCS2 += item.CS2Q2.Value;
                        else
                            TotalCS2 = item.CS2Q2.Value;
                    }
                    //TotalCS2 += (item.CS2Q2.HasValue == true ? item.CS2Q2.Value : item.CS2Q2);
                    if (item.BaseQ2.HasValue)
                    {
                        if (GroupBaseQ2.HasValue)
                            GroupBaseQ2 += item.BaseQ2.Value;
                        else
                            GroupBaseQ2 = item.BaseQ2.Value;
                    }
                    //GroupBaseQ2 += (item.BaseQ2.HasValue == true ? item.BaseQ2.Value : item.BaseQ2);
                    if (item.CS1Q2.HasValue)
                    {
                        if (GroupCS1Q2.HasValue)
                            GroupCS1Q2 += item.CS1Q2.Value;
                        else
                            GroupCS1Q2 = item.CS1Q2.Value;
                    }
                    //GroupCS1Q2 += (item.CS1Q2.HasValue == true ? item.CS1Q2.Value : item.CS1Q2);
                    if (item.CS2Q2.HasValue)
                    {
                        if (GroupCS2Q2.HasValue)
                            GroupCS2Q2 += item.CS2Q2.Value;
                        else
                            GroupCS2Q2 = item.CS2Q2.Value;
                    }
                    //GroupCS2Q2 += (item.CS2Q2.HasValue == true ? item.CS2Q2.Value : item.CS2Q2);
                    if (item.BaseQ2.HasValue)
                    {
                        if (GroupBaseTotal.HasValue)
                            GroupBaseTotal += item.BaseQ2.Value;
                        else
                            GroupBaseTotal = item.BaseQ2.Value;
                    }
                    //GroupBaseTotal += (item.BaseQ2.HasValue == true ? item.BaseQ2.Value : item.BaseQ2);
                    if (item.CS1Q2.HasValue)
                    {
                        if (GroupCS1Total.HasValue)
                            GroupCS1Total += item.CS1Q2.Value;
                        else
                            GroupCS1Total = item.CS1Q2.Value;
                    }
                    //GroupCS1Total += (item.CS1Q2.HasValue == true ? item.CS1Q2.Value : item.CS1Q2);
                    if (item.CS2Q2.HasValue)
                    {
                        if (GroupCS2Total.HasValue)
                            GroupCS2Total += item.CS2Q2.Value;
                        else
                            GroupCS2Total = item.CS2Q2.Value;
                    }
                    //GroupCS2Total += (item.CS2Q2.HasValue == true ? item.CS2Q2.Value : item.CS2Q2);
                    if (item.BaseQ2.HasValue)
                    {
                        if (GrossBaseQ2.HasValue)
                            GrossBaseQ2 += item.BaseQ2.Value;
                        else
                            GrossBaseQ2 = item.BaseQ2.Value;
                    }
                    //GrossBaseQ2 += (item.BaseQ2.HasValue == true ? item.BaseQ2.Value : item.BaseQ2);
                    if (item.BaseQ2.HasValue)
                    {
                        if (GrossBaseTotal.HasValue)
                            GrossBaseTotal += item.BaseQ2.Value;
                        else
                            GrossBaseTotal = item.BaseQ2.Value;
                    }
                    //GrossBaseTotal += (item.BaseQ2.HasValue == true ? item.BaseQ2.Value : item.BaseQ2);
                    if (item.CS1Q2.HasValue)
                    {
                        if (GrossCS1Q2.HasValue)
                            GrossCS1Q2 += item.CS1Q2.Value;
                        else
                            GrossCS1Q2 = item.CS1Q2.Value;
                    }
                    //GrossCS1Q2 += (item.CS1Q2.HasValue == true ? item.CS1Q2.Value : item.CS1Q2);
                    if (item.CS1Q2.HasValue)
                    {
                        if (GrossCS1Total.HasValue)
                            GrossCS1Total += item.CS1Q2.Value;
                        else
                            GrossCS1Total = item.CS1Q2.Value;
                    }
                    //GrossCS1Total += (item.CS1Q2.HasValue == true ? item.CS1Q2.Value : item.CS1Q2);
                    if (item.CS2Q2.HasValue)
                    {
                        if (GrossCS2Q2.HasValue)
                            GrossCS2Q2 += item.CS2Q2.Value;
                        else
                            GrossCS2Q2 = item.CS2Q2.Value;
                    }
                    //GrossCS2Q2 += (item.CS2Q2.HasValue == true ? item.CS2Q2.Value : item.CS2Q2);
                    if (item.CS2Q2.HasValue)
                    {
                        if (GrossCS2Total.HasValue)
                            GrossCS2Total += item.CS2Q2.Value;
                        else
                            GrossCS2Total = item.CS2Q2.Value;
                    }
                    //GrossCS2Total += (item.CS2Q2.HasValue == true ? item.CS2Q2.Value : item.CS2Q2);

                }
                else
                {
                    item.BaseQ2 = null;
                    item.CS1Q2 = null;
                    item.CS2Q2 = null;
                }
                if (quarters.Contains("Q3"))
                {
                    if (item.BaseQ3.HasValue)
                    {
                        if (TotalBase.HasValue)
                            TotalBase += item.BaseQ3.Value;
                        else
                            TotalBase = item.BaseQ3.Value;
                    }
                    //TotalBase += (item.BaseQ3.HasValue == true ? item.BaseQ3.Value : item.BaseQ3);
                    if (item.CS1Q3.HasValue)
                    {
                        if (TotalCS1.HasValue)
                            TotalCS1 += item.CS1Q3.Value;
                        else
                            TotalCS1 = item.CS1Q3.Value;
                    }
                    //TotalCS1 += (item.CS1Q3.HasValue == true ? item.CS1Q3.Value : item.CS1Q3);
                    if (item.CS2Q3.HasValue)
                    {
                        if (TotalCS2.HasValue)
                            TotalCS2 += item.CS2Q3.Value;
                        else
                            TotalCS2 = item.CS2Q3.Value;
                    }
                    //TotalCS2 += (item.CS2Q3.HasValue == true ? item.CS2Q3.Value : item.CS2Q3);
                    if (item.BaseQ3.HasValue)
                    {
                        if (GroupBaseQ3.HasValue)
                            GroupBaseQ3 += item.BaseQ3.Value;
                        else
                            GroupBaseQ3 = item.BaseQ3.Value;
                    }
                    //GroupBaseQ3 += (item.BaseQ3.HasValue == true ? item.BaseQ3.Value : item.BaseQ3);
                    if (item.CS1Q3.HasValue)
                    {
                        if (GroupCS1Q3.HasValue)
                            GroupCS1Q3 += item.CS1Q3.Value;
                        else
                            GroupCS1Q3 = item.CS1Q3.Value;
                    }
                    //GroupCS1Q3 += (item.CS1Q3.HasValue == true ? item.CS1Q3.Value : item.CS1Q3);
                    if (item.CS2Q3.HasValue)
                    {
                        if (GroupCS2Q3.HasValue)
                            GroupCS2Q3 += item.CS2Q3.Value;
                        else
                            GroupCS2Q3 = item.CS2Q3.Value;
                    }
                    //GroupCS2Q3 += (item.CS2Q3.HasValue == true ? item.CS2Q3.Value : item.CS2Q3);
                    if (item.BaseQ3.HasValue)
                    {
                        if (GroupBaseTotal.HasValue)
                            GroupBaseTotal += item.BaseQ3.Value;
                        else
                            GroupBaseTotal = item.BaseQ3.Value;
                    }
                    //GroupBaseTotal += (item.BaseQ3.HasValue == true ? item.BaseQ3.Value : item.BaseQ3);
                    if (item.CS1Q3.HasValue)
                    {
                        if (GroupCS1Total.HasValue)
                            GroupCS1Total += item.CS1Q3.Value;
                        else
                            GroupCS1Total = item.CS1Q3.Value;
                    }
                    //GroupCS1Total += (item.CS1Q3.HasValue == true ? item.CS1Q3.Value : item.CS1Q3);
                    if (item.CS2Q3.HasValue)
                    {
                        if (GroupCS2Total.HasValue)
                            GroupCS2Total += item.CS2Q3.Value;
                        else
                            GroupCS2Total = item.CS2Q3.Value;
                    }
                    //GroupCS2Total += (item.CS2Q3.HasValue == true ? item.CS2Q3.Value : item.CS2Q3);
                    if (item.BaseQ3.HasValue)
                    {
                        if (GrossBaseQ3.HasValue)
                            GrossBaseQ3 += item.BaseQ3.Value;
                        else
                            GrossBaseQ3 = item.BaseQ3.Value;
                    }
                    //GrossBaseQ3 += (item.BaseQ3.HasValue == true ? item.BaseQ3.Value : item.BaseQ3);
                    if (item.BaseQ3.HasValue)
                    {
                        if (GrossBaseTotal.HasValue)
                            GrossBaseTotal += item.BaseQ3.Value;
                        else
                            GrossBaseTotal = item.BaseQ3.Value;
                    }
                    //GrossBaseTotal += (item.BaseQ3.HasValue == true ? item.BaseQ3.Value : item.BaseQ3);
                    if (item.CS1Q3.HasValue)
                    {
                        if (GrossCS1Q3.HasValue)
                            GrossCS1Q3 += item.CS1Q3.Value;
                        else
                            GrossCS1Q3 = item.CS1Q3.Value;
                    }
                    //GrossCS1Q3 += (item.CS1Q3.HasValue == true ? item.CS1Q3.Value : item.CS1Q3);
                    if (item.CS1Q3.HasValue)
                    {
                        if (GrossCS1Total.HasValue)
                            GrossCS1Total += item.CS1Q3.Value;
                        else
                            GrossCS1Total = item.CS1Q3.Value;
                    }
                    //GrossCS1Total += (item.CS1Q3.HasValue == true ? item.CS1Q3.Value : item.CS1Q3);
                    if (item.CS2Q3.HasValue)
                    {
                        if (GrossCS2Q3.HasValue)
                            GrossCS2Q3 += item.CS2Q3.Value;
                        else
                            GrossCS2Q3 = item.CS2Q3.Value;
                    }
                    //GrossCS2Q3 += (item.CS2Q3.HasValue == true ? item.CS2Q3.Value : item.CS2Q3);
                    if (item.CS2Q3.HasValue)
                    {
                        if (GrossCS2Total.HasValue)
                            GrossCS2Total += item.CS2Q3.Value;
                        else
                            GrossCS2Total = item.CS2Q3.Value;
                    }
                    //GrossCS2Total += (item.CS2Q3.HasValue == true ? item.CS2Q3.Value : item.CS2Q3);


                }
                else
                {
                    item.BaseQ3 = null;
                    item.CS1Q3 = null;
                    item.CS2Q3 = null;
                }
                if (quarters.Contains("Q4"))
                {
                    if (item.BaseQ4.HasValue)
                    {
                        if (TotalBase.HasValue)
                            TotalBase += item.BaseQ4.Value;
                        else
                            TotalBase = item.BaseQ4.Value;
                    }
                    //TotalBase += (item.BaseQ4.HasValue == true ? item.BaseQ4.Value : item.BaseQ4);
                    if (item.CS1Q4.HasValue)
                    {
                        if (TotalCS1.HasValue)
                            TotalCS1 += item.CS1Q4.Value;
                        else
                            TotalCS1 = item.CS1Q4.Value;
                    }
                    //TotalCS1 += (item.CS1Q4.HasValue == true ? item.CS1Q4.Value : item.CS1Q4);
                    if (item.CS2Q4.HasValue)
                    {
                        if (TotalCS2.HasValue)
                            TotalCS2 += item.CS2Q4.Value;
                        else
                            TotalCS2 = item.CS2Q4.Value;
                    }
                    //TotalCS2 += (item.CS2Q4.HasValue == true ? item.CS2Q4.Value : item.CS2Q4);
                    if (item.BaseQ4.HasValue)
                    {
                        if (GroupBaseQ4.HasValue)
                            GroupBaseQ4 += item.BaseQ4.Value;
                        else
                            GroupBaseQ4 = item.BaseQ4.Value;
                    }
                    //GroupBaseQ4 += (item.BaseQ4.HasValue == true ? item.BaseQ4.Value : item.BaseQ4);
                    if (item.CS1Q4.HasValue)
                    {
                        if (GroupCS1Q4.HasValue)
                            GroupCS1Q4 += item.CS1Q4.Value;
                        else
                            GroupCS1Q4 = item.CS1Q4.Value;
                    }
                    //GroupCS1Q4 += (item.CS1Q4.HasValue == true ? item.CS1Q4.Value : item.CS1Q4);
                    if (item.CS2Q4.HasValue)
                    {
                        if (GroupCS2Q4.HasValue)
                            GroupCS2Q4 += item.CS2Q4.Value;
                        else
                            GroupCS2Q4 = item.CS2Q4.Value;
                    }
                    //GroupCS2Q4 += (item.CS2Q4.HasValue == true ? item.CS2Q4.Value : item.CS2Q4);
                    if (item.BaseQ4.HasValue)
                    {
                        if (GroupBaseTotal.HasValue)
                            GroupBaseTotal += item.BaseQ4.Value;
                        else
                            GroupBaseTotal = item.BaseQ4.Value;
                    }
                    //GroupBaseTotal += (item.BaseQ4.HasValue == true ? item.BaseQ4.Value : item.BaseQ4);
                    if (item.CS1Q4.HasValue)
                    {
                        if (GroupCS1Total.HasValue)
                            GroupCS1Total += item.CS1Q4.Value;
                        else
                            GroupCS1Total = item.CS1Q4.Value;
                    }
                    //GroupCS1Total += (item.CS1Q4.HasValue == true ? item.CS1Q4.Value : item.CS1Q4);
                    if (item.CS2Q4.HasValue)
                    {
                        if (GroupCS2Total.HasValue)
                            GroupCS2Total += item.CS2Q4.Value;
                        else
                            GroupCS2Total = item.CS2Q4.Value;
                    }
                    //GroupCS2Total += (item.CS2Q4.HasValue == true ? item.CS2Q4.Value : item.CS2Q4);
                    if (item.BaseQ4.HasValue)
                    {
                        if (GrossBaseQ4.HasValue)
                            GrossBaseQ4 += item.BaseQ4.Value;
                        else
                            GrossBaseQ4 = item.BaseQ4.Value;
                    }
                    //GrossBaseQ4 += (item.BaseQ4.HasValue == true ? item.BaseQ4.Value : item.BaseQ4);
                    if (item.BaseQ4.HasValue)
                    {
                        if (GrossBaseTotal.HasValue)
                            GrossBaseTotal += item.BaseQ4.Value;
                        else
                            GrossBaseTotal = item.BaseQ4.Value;
                    }
                    //GrossBaseTotal += (item.BaseQ4.HasValue == true ? item.BaseQ4.Value : item.BaseQ4);
                    if (item.CS1Q4.HasValue)
                    {
                        if (GrossCS1Q4.HasValue)
                            GrossCS1Q4 += item.CS1Q4.Value;
                        else
                            GrossCS1Q4 = item.CS1Q4.Value;
                    }
                    //GrossCS1Q4 += (item.CS1Q4.HasValue == true ? item.CS1Q4.Value : item.CS1Q4);
                    if (item.CS1Q4.HasValue)
                    {
                        if (GrossCS1Total.HasValue)
                            GrossCS1Total += item.CS1Q4.Value;
                        else
                            GrossCS1Total = item.CS1Q4.Value;
                    }
                    //GrossCS1Total += (item.CS1Q4.HasValue == true ? item.CS1Q4.Value : item.CS1Q4);
                    if (item.CS2Q4.HasValue)
                    {
                        if (GrossCS2Q4.HasValue)
                            GrossCS2Q4 += item.CS2Q4.Value;
                        else
                            GrossCS2Q4 = item.CS2Q4.Value;
                    }
                    //GrossCS2Q4 += (item.CS2Q4.HasValue == true ? item.CS2Q4.Value : item.CS2Q4);
                    if (item.CS2Q4.HasValue)
                    {
                        if (GrossCS2Total.HasValue)
                            GrossCS2Total += item.CS2Q4.Value;
                        else
                            GrossCS2Total = item.CS2Q4.Value;
                    }
                    //GrossCS2Total += (item.CS2Q4.HasValue == true ? item.CS2Q4.Value : item.CS2Q4);
                }
                else
                {
                    item.BaseQ4 = null;
                    item.CS1Q4 = null;
                    item.CS2Q4 = null;
                }


                Decimal? var1 = null;
                Decimal? var2 = null;
                if (TotalBase.HasValue)
                {
                    var1 = TotalBase;
                    if (TotalCS2.HasValue)
                        var1 = TotalBase.Value - TotalCS2.Value;
                    var2 = TotalBase;
                    if (TotalCS1.HasValue)
                        var2 = TotalBase.Value - TotalCS1.Value;
                }
                else
                {
                    if (TotalCS2.HasValue)
                        var1 = -1 * TotalCS2.Value;
                    if (TotalCS1.HasValue)
                        var2 = -1 * TotalCS1.Value;
                }

                respItem = item;
                respItem.OriginalProjectNo = item.ProjectNo;
                respItem.GroupingParametersCode = item.GroupingParametersCode;
                respItem.TotalBase = TotalBase;
                respItem.TotalCS1 = TotalCS1;
                respItem.TotalCS2 = TotalCS2;
                respItem.Var1 = var1;
                respItem.Var2 = var2;
                respItem.OriginalProjectName = $"{item.ProjectNo} - {item.ProjectName}";
                response.Add(respItem);



                groupName = item.GroupingParametersCode;
                _GroupingParametersName = item.GroupingParametersName;
                _LastGroupingParametersCode = item.GroupingParametersCode;
            }
            if (groupName != "")
            {

                Decimal? var11 = null;
                Decimal? var22 = null;
                if (GroupBaseTotal.HasValue)
                {
                    var11 = GroupBaseTotal.Value;
                    if (GroupCS2Total.HasValue)
                        var11 = GroupBaseTotal.Value - GroupCS2Total.Value;
                    var22 = GroupBaseTotal.Value;
                    if (GroupCS1Total.HasValue)
                        var22 = GroupBaseTotal.Value - GroupCS1Total.Value;
                }
                else
                {
                    if (GroupCS2Total.HasValue)
                        var11 = -1 * GroupCS2Total.Value;
                    if (GroupCS1Total.HasValue)
                        var22 = -1 * GroupCS1Total.Value;
                }
                if (config.SubTotalRequired == "Y")
                {
                    response.Add(
                        new VarianceAnalysisResponse()
                        {
                            RecordType = "GroupTotal",
                            ProjectNo = "SUBTOTAL " + _GroupingParametersName,
                            BaseQ1 = GroupBaseQ1,
                            BaseQ2 = GroupBaseQ2,
                            BaseQ3 = GroupBaseQ3,
                            BaseQ4 = GroupBaseQ4,
                            TotalBase = GroupBaseTotal,
                            CS1Q1 = GroupCS1Q1,
                            CS1Q2 = GroupCS1Q2,
                            CS1Q3 = GroupCS1Q3,
                            CS1Q4 = GroupCS1Q4,
                            TotalCS1 = GroupCS1Total,
                            CS2Q1 = GroupCS2Q1,
                            CS2Q2 = GroupCS2Q2,
                            CS2Q3 = GroupCS2Q3,
                            CS2Q4 = GroupCS2Q4,
                            TotalCS2 = GroupCS2Total,
                            Var1 = var11,
                            Var2 = var22,
                            OriginalProjectNo = _LastGroupingParametersCode,
                            GroupingParametersCode = _LastGroupingParametersCode,
                            OriginalProjectName = $"{_ProjectNo} - {_ProjectName}"

                        }
                        );
                }
            }

            responseGrid.GridResponse = response;
            List<VarianceAnalysisResponse> responseGrandTotal = new List<VarianceAnalysisResponse>();
            if (returnList.Count > 0)
            {
                Decimal? var11 = null;
                Decimal? var22 = null;
                if (GrossBaseTotal.HasValue)
                {
                    var11 = GrossBaseTotal.Value;
                    if (GrossCS2Total.HasValue)
                        var11 = GrossBaseTotal.Value - GrossCS2Total.Value;
                    var22 = GrossBaseTotal.Value;
                    if (GrossCS1Total.HasValue)
                        var22 = GrossBaseTotal.Value - GrossCS1Total.Value;
                }
                else
                {
                    if (GrossCS2Total.HasValue)
                        var11 = -1 * GrossCS2Total.Value;
                    if (GrossCS1Total.HasValue)
                        var22 = -1 * GrossCS1Total.Value;
                }

                responseGrandTotal.Add(
                    new VarianceAnalysisResponse()
                    {
                        RecordType = "GrandTotal",
                        ProjectNo = "Grand Total ",
                        BaseQ1 = GrossBaseQ1,
                        BaseQ2 = GrossBaseQ2,
                        BaseQ3 = GrossBaseQ3,
                        BaseQ4 = GrossBaseQ4,
                        TotalBase = GrossBaseTotal,
                        CS1Q1 = GrossCS1Q1,
                        CS1Q2 = GrossCS1Q2,
                        CS1Q3 = GrossCS1Q3,
                        CS1Q4 = GrossCS1Q4,
                        TotalCS1 = GrossCS1Total,
                        CS2Q1 = GrossCS2Q1,
                        CS2Q2 = GrossCS2Q2,
                        CS2Q3 = GrossCS2Q3,
                        CS2Q4 = GrossCS2Q4,
                        TotalCS2 = GrossCS2Total,
                        Var1 = var11,
                        Var2 = var22,
                        BaseScenario = baseScenario,
                        CS1 = cs1,
                        CS2 = cs2

                    }
                    );

                responseGrid.GrandTotal = responseGrandTotal;
            }
            return responseGrid;

        }

        public byte[] GetVarianceAnalysisReportExcel(VarianceAnalysisConfig config)
        {
            var quarters = config.Quarters.Split(',').OrderBy(c => c).ToList();
            var noOfQuarters = quarters.Count;

            var groupLevels = config.GroupLevels.Split(',');

            foreach (var level in groupLevels)
            {
                if (level == "Segment")
                {
                    config.GroupLevels = config.GroupLevels.Replace("Segment", "PS.ProjectSegmentName");
                }
                if (level == "BUCategory")
                {
                    config.GroupLevels = config.GroupLevels.Replace("BUCategory", "BU.BUCategoryName");
                }
                if (level == "Entity")
                {
                    config.GroupLevels = config.GroupLevels.Replace("Entity", "PE.ProjectEntityName");
                }
            }
            var reportData = _reportRepository.GetVarianceAnalysisReportData(config);

            if (reportData.VarianceAnalysisReportDatas.Count == 0)
            {
                throw new Exception("No ScenarioData Found.");
            }

            var baseScenario = reportData.Scenarios.FirstOrDefault(s => s.ScenarioID == config.BaseScenarioId);
            var compareScenarioA = reportData.Scenarios.FirstOrDefault(s => s.ScenarioID == config.CompareScenarioAId);
            var compareScenarioB = reportData.Scenarios.FirstOrDefault(s => s.ScenarioID == config.CompareScenarioBId);

            ExcelPackage excelPkg = new ExcelPackage();
            ExcelWorksheet workSheet = excelPkg.Workbook.Worksheets.Add("Variance Analysis Report");
            workSheet.Row(5).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(6).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(7).Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Row(5).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(6).Style.Fill.BackgroundColor.SetColor(Color.White);
            workSheet.Row(7).Style.Fill.BackgroundColor.SetColor(Color.White);

            workSheet.Cells["A5"].Value = "Project No";
            workSheet.Cells["A5:A7"].Merge = true;
            workSheet.Cells["A5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["A5"].Style.WrapText = true;

            workSheet.Cells["B5"].Value = "Project Name";
            workSheet.Cells["B5:B7"].Merge = true;
            workSheet.Cells["B5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["B5"].Style.WrapText = true;

            workSheet.Cells["C5"].Value = "Contract Status";
            workSheet.Cells["C5:C7"].Merge = true;
            workSheet.Cells["C5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["C5"].Style.WrapText = true;

            workSheet.Cells["D5"].Value = "Entity";
            workSheet.Cells["D5:D7"].Merge = true;
            workSheet.Cells["D5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["D5"].Style.WrapText = true;

            workSheet.Cells["E5"].Value = "ContractNature/Category";
            workSheet.Cells["E5:E7"].Merge = true;
            workSheet.Cells["E5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["E5"].Style.WrapText = true;

            workSheet.Cells["F5"].Value = "Segment";
            workSheet.Cells["F5:F7"].Merge = true;
            workSheet.Cells["F5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["F5"].Style.WrapText = true;

            workSheet.Cells["G5"].Value = "Sponsored/Non-Sponsored";
            workSheet.Cells["G5:G7"].Merge = true;
            workSheet.Cells["G5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["G5"].Style.WrapText = true;

            workSheet.Cells["H5"].Value = "Revenue";
            workSheet.Cells["H5:X5"].Merge = true;
            workSheet.Cells["H5:X5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["H5"].Style.Fill.BackgroundColor.SetColor(Color.Beige);

            workSheet.Cells["AA5"].Value = "Gross Margin";
            workSheet.Cells["AA5:AQ5"].Merge = true;
            workSheet.Cells["AA5:AQ5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["AA5"].Style.Fill.BackgroundColor.SetColor(Color.Beige);

            workSheet.Cells["H6"].Value = $"{compareScenarioB.ScenarioTypeName}";
            workSheet.Cells["H6:L6"].Merge = true;
            workSheet.Cells["H6:L6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["H6"].Style.Fill.BackgroundColor.SetColor(Color.Orange);

            workSheet.Cells["M6"].Value = $"{compareScenarioA.ScenarioTypeName}";
            workSheet.Cells["M6:Q6"].Merge = true;
            workSheet.Cells["M6:Q6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["M6"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            workSheet.Cells["R6"].Value = $"{baseScenario.ScenarioTypeName}";
            workSheet.Cells["R6:V6"].Merge = true;
            workSheet.Cells["R6:V6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["R6"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);

            workSheet.Cells["AA6"].Value = $"{compareScenarioB.ScenarioTypeName}";
            workSheet.Cells["AA6:AE6"].Merge = true;
            workSheet.Cells["AA6:AE6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["AA6"].Style.Fill.BackgroundColor.SetColor(Color.Orange);

            workSheet.Cells["AF6"].Value = $"{compareScenarioA.ScenarioTypeName}";
            workSheet.Cells["AF6:AJ6"].Merge = true;
            workSheet.Cells["AF6:AJ6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["AF6"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            workSheet.Cells["AK6"].Value = $"{baseScenario.ScenarioTypeName}";
            workSheet.Cells["AK6:AO6"].Merge = true;
            workSheet.Cells["AK6:AO6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Cells["AK6"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);

            //Revenue Headers - Quarters
            workSheet.Cells["H7"].Value = $"Q1 - {config.Year}";
            workSheet.Cells["I7"].Value = $"Q2 - {config.Year}";
            workSheet.Cells["J7"].Value = $"Q3 - {config.Year}";
            workSheet.Cells["K7"].Value = $"Q4 - {config.Year}";
            workSheet.Cells["L7"].Value = $"Total Cummulative";

            workSheet.Cells["H7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["I7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["J7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["K7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["L7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);

            workSheet.Cells["M7"].Value = $"Q1 - {config.Year}";
            workSheet.Cells["N7"].Value = $"Q2 - {config.Year}";
            workSheet.Cells["O7"].Value = $"Q3 - {config.Year}";
            workSheet.Cells["P7"].Value = $"Q4 - {config.Year}";
            workSheet.Cells["Q7"].Value = $"Total Cummulative";

            workSheet.Cells["M7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            workSheet.Cells["N7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            workSheet.Cells["O7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            workSheet.Cells["P7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            workSheet.Cells["Q7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            workSheet.Cells["R7"].Value = $"Q1 - {config.Year}";
            workSheet.Cells["S7"].Value = $"Q2 - {config.Year}";
            workSheet.Cells["T7"].Value = $"Q3 - {config.Year}";
            workSheet.Cells["U7"].Value = $"Q4 - {config.Year}";
            workSheet.Cells["V7"].Value = $"Total Cummulative";

            workSheet.Cells["R7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);
            workSheet.Cells["S7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);
            workSheet.Cells["T7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);
            workSheet.Cells["U7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);
            workSheet.Cells["V7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);

            //REVENUE VARIANCE
            workSheet.Cells["W6"].Value = $"Diff Curr to Budget";
            workSheet.Cells["W6:W7"].Merge = true;
            workSheet.Cells["W6:W7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["W6"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["W6"].Style.WrapText = true;

            workSheet.Cells["X6"].Value = $"Diff Curr to Prev ForeCast";
            workSheet.Cells["X6:X7"].Merge = true;
            workSheet.Cells["X6:X7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["X6"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["X6"].Style.WrapText = true;

            //GrossMargin Headers - Quarters
            workSheet.Cells["AA7"].Value = $"Q1 - {config.Year}";
            workSheet.Cells["AB7"].Value = $"Q2 - {config.Year}";
            workSheet.Cells["AC7"].Value = $"Q3 - {config.Year}";
            workSheet.Cells["AD7"].Value = $"Q4 - {config.Year}";
            workSheet.Cells["AE7"].Value = $"Total Cummulative";

            workSheet.Cells["AA7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["AB7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["AC7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["AD7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["AE7"].Style.Fill.BackgroundColor.SetColor(Color.Orange);

            workSheet.Cells["AF7"].Value = $"Q1 - {config.Year}";
            workSheet.Cells["AG7"].Value = $"Q2 - {config.Year}";
            workSheet.Cells["AH7"].Value = $"Q3 - {config.Year}";
            workSheet.Cells["AI7"].Value = $"Q4 - {config.Year}";
            workSheet.Cells["AJ7"].Value = $"Total Cummulative";

            workSheet.Cells["AF7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            workSheet.Cells["AG7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            workSheet.Cells["AH7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            workSheet.Cells["AI7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            workSheet.Cells["AJ7"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            workSheet.Cells["AK7"].Value = $"Q1 - {config.Year}";
            workSheet.Cells["AL7"].Value = $"Q2 - {config.Year}";
            workSheet.Cells["AM7"].Value = $"Q3 - {config.Year}";
            workSheet.Cells["AN7"].Value = $"Q4 - {config.Year}";
            workSheet.Cells["AO7"].Value = $"Total Cummulative";

            workSheet.Cells["AK7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);
            workSheet.Cells["AL7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);
            workSheet.Cells["AM7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);
            workSheet.Cells["AN7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);
            workSheet.Cells["AO7"].Style.Fill.BackgroundColor.SetColor(Color.Cyan);

            //GROSSMARGIN VARIANCE
            workSheet.Cells["AP6"].Value = $"Diff Curr to Budget";
            workSheet.Cells["AP6:AP7"].Merge = true;
            workSheet.Cells["AP6:AP7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["AP6"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["AP6"].Style.WrapText = true;

            workSheet.Cells["AQ6"].Value = $"Diff Curr to Prev ForeCast";
            workSheet.Cells["AQ6:AQ7"].Merge = true;
            workSheet.Cells["AQ6:AQ7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            workSheet.Cells["AQ6"].Style.Fill.BackgroundColor.SetColor(Color.Orange);
            workSheet.Cells["AQ6"].Style.WrapText = true;

            workSheet.Cells["A5:X7"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["A5:X7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["A5:X7"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["A5:X7"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

            workSheet.Cells["AA5:AQ7"].Style.Border.Top.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["AA5:AQ7"].Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["AA5:AQ7"].Style.Border.Left.Style = ExcelBorderStyle.Thick;
            workSheet.Cells["AA5:AQ7"].Style.Border.Right.Style = ExcelBorderStyle.Thick;

            workSheet.Column(25).Width = 3;
            workSheet.Column(26).Width = 3;
            workSheet.Column(12).AutoFit();
            workSheet.Column(17).AutoFit();
            workSheet.Column(22).AutoFit();
            workSheet.Column(31).AutoFit();
            workSheet.Column(36).AutoFit();
            workSheet.Column(41).AutoFit();

            var row = 8;
            var segment = "";
            var entity = "";
            var buCategory = "";

            foreach (var data in reportData.VarianceAnalysisReportDatas)
            {
                if (segment == data.ProjectSegmentName && entity == data.ProjectEntityName && buCategory == data.BUCategoryName)
                {
                    continue;
                }

                segment = data.ProjectSegmentName;
                entity = data.ProjectEntityName;
                buCategory = data.BUCategoryName;

                var dataList = reportData.VarianceAnalysisReportDatas
                    .Where(d => d.BUCategoryName == buCategory && d.ProjectEntityName == entity && d.ProjectSegmentName == segment)
                    .ToList();

                var secRow = row;
                if (dataList.Count == 0)
                    continue;

                foreach (var entry in dataList)
                {
                    workSheet.Cells[$"A{row}"].Value = data.IFSProjectCode;
                    workSheet.Cells[$"A{row}"].Style.WrapText = true;

                    workSheet.Cells[$"B{row}"].Value = data.ProjectName;
                    workSheet.Cells[$"B{row}"].Style.WrapText = true;

                    workSheet.Cells[$"C{row}"].Value = data.ContractStatusName;
                    workSheet.Cells[$"C{row}"].Style.WrapText = true;

                    workSheet.Cells[$"D{row}"].Value = data.ProjectEntityName;
                    workSheet.Cells[$"D{row}"].Style.WrapText = true;

                    workSheet.Cells[$"E{row}"].Value = data.BUCategoryName;
                    workSheet.Cells[$"E{row}"].Style.WrapText = true;

                    workSheet.Cells[$"F{row}"].Value = data.BUCategoryName;
                    workSheet.Cells[$"F{row}"].Style.WrapText = true;

                    workSheet.Cells[$"G{row}"].Value = data.ContractTypeName;
                    workSheet.Cells[$"G{row}"].Style.WrapText = true;


                    if (config.Quarters.Contains("Q1"))
                    {
                        workSheet.Cells[$"H{row}"].Value = entry.RV_S3Q1;
                        workSheet.Cells[$"M{row}"].Value = entry.RV_S2Q1;
                        workSheet.Cells[$"R{row}"].Value = entry.RV_S1Q1;

                        workSheet.Cells[$"AA{row}"].Value = entry.GM_S3Q1;
                        workSheet.Cells[$"AF{row}"].Value = entry.GM_S2Q1;
                        workSheet.Cells[$"AK{row}"].Value = entry.GM_S1Q1;
                    }
                    if (config.Quarters.Contains("Q2"))
                    {
                        workSheet.Cells[$"I{row}"].Value = entry.RV_S3Q2;
                        workSheet.Cells[$"N{row}"].Value = entry.RV_S2Q2;
                        workSheet.Cells[$"S{row}"].Value = entry.RV_S1Q2;

                        workSheet.Cells[$"AB{row}"].Value = entry.GM_S3Q2;
                        workSheet.Cells[$"AG{row}"].Value = entry.GM_S2Q2;
                        workSheet.Cells[$"AL{row}"].Value = entry.GM_S1Q2;
                    }
                    if (config.Quarters.Contains("Q3"))
                    {
                        workSheet.Cells[$"J{row}"].Value = entry.RV_S3Q3;
                        workSheet.Cells[$"O{row}"].Value = entry.RV_S2Q3;
                        workSheet.Cells[$"T{row}"].Value = entry.RV_S1Q3;

                        workSheet.Cells[$"AC{row}"].Value = entry.GM_S3Q3;
                        workSheet.Cells[$"AH{row}"].Value = entry.GM_S2Q3;
                        workSheet.Cells[$"AM{row}"].Value = entry.GM_S1Q3;
                    }
                    if (config.Quarters.Contains("Q4"))
                    {
                        workSheet.Cells[$"K{row}"].Value = entry.RV_S3Q4;
                        workSheet.Cells[$"P{row}"].Value = entry.RV_S2Q4;
                        workSheet.Cells[$"U{row}"].Value = entry.RV_S1Q4;

                        workSheet.Cells[$"AD{row}"].Value = entry.GM_S3Q4;
                        workSheet.Cells[$"AI{row}"].Value = entry.GM_S2Q4;
                        workSheet.Cells[$"AN{row}"].Value = entry.GM_S1Q4;
                    }

                    //Revenue - Total Cummulatives
                    workSheet.Cells[$"L{row}"].Formula = $"=SUM(H{row}:K{row})";
                    workSheet.Cells[$"Q{row}"].Formula = $"=SUM(M{row}:P{row})";
                    workSheet.Cells[$"V{row}"].Formula = $"=SUM(R{row}:U{row})";
                    //Gross Margin - Total Cummulatives
                    workSheet.Cells[$"AE{row}"].Formula = $"=SUM(AA{row}:AD{row})";
                    workSheet.Cells[$"AJ{row}"].Formula = $"=SUM(AF{row}:AI{row})";
                    workSheet.Cells[$"AO{row}"].Formula = $"=SUM(AK{row}:AN{row})";

                    //Revenue - Variance
                    workSheet.Cells[$"W{row}"].Formula = $"=V{row}-Q{row}";
                    workSheet.Cells[$"X{row}"].Formula = $"=V{row}-L{row}";
                    //GrossMargin - Variance
                    workSheet.Cells[$"AP{row}"].Formula = $"=AO{row}-AJ{row}";
                    workSheet.Cells[$"AQ{row}"].Formula = $"=AO{row}-AE{row}";

                    row++;
                }

                workSheet.Cells[$"A{row}"].Value = $"Sub Total";
                workSheet.Cells[$"A{row}:G{row}"].Merge = true;
                workSheet.Cells[$"A{row}:G{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                workSheet.Cells[$"H{row}"].Formula = $"=SUM(H{secRow}:H{row - 1})";
                workSheet.Cells[$"I{row}"].Formula = $"=SUM(I{secRow}:I{row - 1})";
                workSheet.Cells[$"J{row}"].Formula = $"=SUM(J{secRow}:J{row - 1})";
                workSheet.Cells[$"K{row}"].Formula = $"=SUM(K{secRow}:K{row - 1})";
                workSheet.Cells[$"L{row}"].Formula = $"=SUM(L{secRow}:L{row - 1})";
                workSheet.Cells[$"M{row}"].Formula = $"=SUM(M{secRow}:M{row - 1})";
                workSheet.Cells[$"N{row}"].Formula = $"=SUM(N{secRow}:N{row - 1})";
                workSheet.Cells[$"O{row}"].Formula = $"=SUM(O{secRow}:O{row - 1})";
                workSheet.Cells[$"P{row}"].Formula = $"=SUM(P{secRow}:P{row - 1})";
                workSheet.Cells[$"Q{row}"].Formula = $"=SUM(Q{secRow}:Q{row - 1})";
                workSheet.Cells[$"R{row}"].Formula = $"=SUM(R{secRow}:R{row - 1})";
                workSheet.Cells[$"S{row}"].Formula = $"=SUM(S{secRow}:S{row - 1})";
                workSheet.Cells[$"T{row}"].Formula = $"=SUM(T{secRow}:T{row - 1})";
                workSheet.Cells[$"U{row}"].Formula = $"=SUM(U{secRow}:U{row - 1})";
                workSheet.Cells[$"V{row}"].Formula = $"=SUM(V{secRow}:V{row - 1})";
                workSheet.Cells[$"W{row}"].Formula = $"=SUM(W{secRow}:W{row - 1})";
                workSheet.Cells[$"X{row}"].Formula = $"=SUM(X{secRow}:X{row - 1})";

                workSheet.Cells[$"AA{row}"].Formula = $"=SUM(AA{secRow}:AA{row - 1})";
                workSheet.Cells[$"AB{row}"].Formula = $"=SUM(AB{secRow}:AB{row - 1})";
                workSheet.Cells[$"AC{row}"].Formula = $"=SUM(AC{secRow}:AC{row - 1})";
                workSheet.Cells[$"AD{row}"].Formula = $"=SUM(AD{secRow}:AD{row - 1})";
                workSheet.Cells[$"AE{row}"].Formula = $"=SUM(AE{secRow}:AE{row - 1})";
                workSheet.Cells[$"AF{row}"].Formula = $"=SUM(AF{secRow}:AF{row - 1})";
                workSheet.Cells[$"AG{row}"].Formula = $"=SUM(AG{secRow}:AG{row - 1})";
                workSheet.Cells[$"AH{row}"].Formula = $"=SUM(AH{secRow}:AH{row - 1})";
                workSheet.Cells[$"AI{row}"].Formula = $"=SUM(AI{secRow}:AI{row - 1})";
                workSheet.Cells[$"AJ{row}"].Formula = $"=SUM(AJ{secRow}:AJ{row - 1})";
                workSheet.Cells[$"AK{row}"].Formula = $"=SUM(AK{secRow}:AK{row - 1})";
                workSheet.Cells[$"AL{row}"].Formula = $"=SUM(AL{secRow}:AL{row - 1})";
                workSheet.Cells[$"AM{row}"].Formula = $"=SUM(AM{secRow}:AM{row - 1})";
                workSheet.Cells[$"AN{row}"].Formula = $"=SUM(AN{secRow}:AN{row - 1})";
                workSheet.Cells[$"AO{row}"].Formula = $"=SUM(AO{secRow}:AO{row - 1})";
                workSheet.Cells[$"AP{row}"].Formula = $"=SUM(AP{secRow}:AP{row - 1})";
                workSheet.Cells[$"AQ{row}"].Formula = $"=SUM(AQ{secRow}:AQ{row - 1})";

                row += 2;
            }

            return excelPkg.GetAsByteArray(); ;
        }

        public ProjectLifeCycle ProjectLifeCycleReport(int projectid, string scenarioscope)
        {
            return new ReportRepository().ProjectLifeCycleReport(projectid, scenarioscope);

        }
        public ProjectLifeCycleDataModel ProjectLifeCycleReport1(int projectid, string scenarioscope)
        {
            return new ReportRepository().ProjectLifeCycleReport1(projectid, scenarioscope);

        }
        public byte[] ProjectLifeCycleReportDownload1(ProjectLifeCycle projectLife, string scenarioscope)
        {
            string positiveFormat = "#,##0.00_)";
            string negativeFormat = "(#,##0.00)";
            string zeroFormat = "-_)";
            string numberFormat = positiveFormat + ";" + negativeFormat;
            string fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat;
            string headerRevenue = "";
            string headerGM = "";
            int dataWidth = 15;
            if (scenarioscope == "PL")
            {
                headerRevenue = "PROFIT & LOSS - ACTUALS -  REVENUE(RV)";
                headerGM = "PROFIT & LOSS - ACTUALS -  GROSS MARGIN(GM)";
            }
            else
            {
                headerRevenue = "ORDER INTAKE - ACTUALS -  REVENUE(RV)";
                headerGM = "ORDER INTAKE - ACTUALS -  GROSS MARGIN(GM)";
            }

            var headerRevenuecount = projectLife.ProjectLifeCycleRevenueData.Count() - 1;
            var headerGMcount = projectLife.ProjectLifeCycleGMData.Count() - 1;
            ExcelPackage excelPkg = new ExcelPackage();
            ExcelWorksheet worksheet = excelPkg.Workbook.Worksheets.Add("ProjectLifeCycle Report");

            var colcount = 1;
            var rowcount = 1;
            Color colFromHexDarkBlue = System.Drawing.ColorTranslator.FromHtml("#5799FA");

            Color colFromHexGMHeader = System.Drawing.ColorTranslator.FromHtml("#B1E3F9");
            Color colFromHexMediumBlue = System.Drawing.ColorTranslator.FromHtml("#A0C4F9");


            Color colFromHexYellow = System.Drawing.ColorTranslator.FromHtml("#FADE65");
            Color colFromHexLightBlue = System.Drawing.ColorTranslator.FromHtml("#CADDFA");
            Color colFromHexLightRed = System.Drawing.ColorTranslator.FromHtml("#F9D2BD");
            Color colFromHexDarkGreen = System.Drawing.ColorTranslator.FromHtml("#4FD179");

            Color colFromHexThirdYear = System.Drawing.ColorTranslator.FromHtml("#CFF3F7");
            Color colFromHexFourthYear = System.Drawing.ColorTranslator.FromHtml("#FAEEDF");
            Color colFromHexFifthYear = System.Drawing.ColorTranslator.FromHtml("#F3D396 ");

            #region HeaderFirstRow

            //worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 3)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "PROJECT LIFECYCLE REPORT";
            // worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 3)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 3)].Style.Font.Bold = true;
            // worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            // worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 1)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkBlue);



            #endregion

            rowcount++;
            colcount = 1;

            #region HeaderSecondRow

            //worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 3)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "";
            // worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 3)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 3)].Style.Font.Bold = true;
            // worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            // worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 1)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkBlue);



            #endregion

            rowcount++;
            colcount = 1;
            #region HeaderThirdRow

            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "";
            //worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Font.Bold = true;
            //worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //worksheet.Cells[rowcount, colcount, rowcount, (colcount + (headerRevenuecount * 2) + 1)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkBlue);

            colcount = colcount + 2;

            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = headerRevenue;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);

            colcount = colcount + headerRevenuecount + 1;

            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = headerGM;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + headerRevenuecount)].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);

            colcount = colcount + headerRevenuecount + 1;
            #endregion


            rowcount++;
            colcount = 1;
            #region HeaderFourthRow
            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            foreach (var item in projectLife.ProjectLifeCycleRevenueData)
            {
                worksheet.Cells[rowcount, colcount].Value = item.HeaderText;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexThirdYear);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
            }
            foreach (var item in projectLife.ProjectLifeCycleGMData)
            {
                worksheet.Cells[rowcount, colcount].Value = item.HeaderText;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexThirdYear);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
            }
            #endregion

            rowcount++;
            colcount = 1;
            #region HeaderFifthRow
            worksheet.Cells[rowcount, colcount].Value = "Project Id";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = "Project Name";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;

            foreach (var item in projectLife.ProjectLifeCycleRevenueData)
            {
                worksheet.Cells[rowcount, colcount].Value = item.YearText;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
            }
            foreach (var item in projectLife.ProjectLifeCycleGMData)
            {
                worksheet.Cells[rowcount, colcount].Value = item.YearText;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
            }
            #endregion
            rowcount++;
            colcount = 1;

            #region Data Rows
            worksheet.Cells[rowcount, colcount].Value = projectLife.ProjectId;
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = projectLife.ProjectName;
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            // worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            foreach (var item in projectLife.ProjectLifeCycleRevenueData)
            {
                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                worksheet.Cells[rowcount, colcount].Value = item.Amount;
                if (item.Amount < 0)
                    worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                colcount++;
            }
            foreach (var item in projectLife.ProjectLifeCycleGMData)
            {
                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                worksheet.Cells[rowcount, colcount].Value = item.Amount;
                if (item.Amount < 0)
                    worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                colcount++;
            }
            #endregion

            rowcount++;
            colcount = 1;
            #region FifthRow - Blank Row
            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            // worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexMediumBlue);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = "";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexMediumBlue);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            foreach (var item in projectLife.ProjectLifeCycleRevenueData)
            {
                worksheet.Cells[rowcount, colcount].Value = "";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexMediumBlue);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
            }
            foreach (var item in projectLife.ProjectLifeCycleGMData)
            {
                worksheet.Cells[rowcount, colcount].Value = "";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
            }
            #endregion
            rowcount++;
            colcount = 1;
            #region Grand Total Row
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Merge = true;
            worksheet.Cells[rowcount, colcount].Value = "Grand Total";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount + 1)].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);

            colcount = colcount + 2;

            foreach (var item in projectLife.ProjectLifeCycleRevenueData)
            {
                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                worksheet.Cells[rowcount, colcount].Value = item.Amount;
                if (item.Amount < 0)
                    worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);

                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                colcount++;
            }
            foreach (var item in projectLife.ProjectLifeCycleGMData)
            {
                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                worksheet.Cells[rowcount, colcount].Value = item.Amount;
                if (item.Amount < 0)
                    worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                colcount++;
            }
            #endregion

            return excelPkg.GetAsByteArray();
        }

        public byte[] ProjectLifeCycleReportDownload(ProjectLifeCycle projectLife, string scenarioscope)
        {
            string positiveFormat = "#,##0.00_)";
            string negativeFormat = "(#,##0.00)";
            string zeroFormat = "-_)";
            string numberFormat = positiveFormat + ";" + negativeFormat;
            string fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat;
            string headerRevenue = "";
            string headerGM = "";
            string header = "";
            int dataWidth = 15;
            if (scenarioscope == "PL")
            {
                headerRevenue = "PROFIT & LOSS - ACTUALS -  REVENUE(RV)";
                headerGM = "PROFIT & LOSS - ACTUALS -  GROSS MARGIN(GM)";
                header = "PROFIT & LOSS - ACTUALS";
            }
            else
            {
                headerRevenue = "ORDER INTAKE - ACTUALS -  REVENUE(RV)";
                headerGM = "ORDER INTAKE - ACTUALS -  GROSS MARGIN(GM)";
                header = "ORDER INTAKE - ACTUALS";
            }

            var headerRevenuecount = projectLife.ProjectLifeCycleRevenueData.Count() - 1;
            var headerGMcount = projectLife.ProjectLifeCycleGMData.Count() - 1;
            ExcelPackage excelPkg = new ExcelPackage();
            ExcelWorksheet worksheet = excelPkg.Workbook.Worksheets.Add("ProjectLifeCycle Report");

            var colcount = 1;
            var rowcount = 1;
            Color colFromHexDarkBlue = System.Drawing.ColorTranslator.FromHtml("#5799FA");

            Color colFromHexGMHeader = System.Drawing.ColorTranslator.FromHtml("#B1E3F9");
            Color colFromHexMediumBlue = System.Drawing.ColorTranslator.FromHtml("#A0C4F9");


            Color colFromHexYellow = System.Drawing.ColorTranslator.FromHtml("#FADE65");
            Color colFromHexLightBlue = System.Drawing.ColorTranslator.FromHtml("#CADDFA");
            Color colFromHexLightRed = System.Drawing.ColorTranslator.FromHtml("#F9D2BD");
            Color colFromHexDarkGreen = System.Drawing.ColorTranslator.FromHtml("#4FD179");

            Color colFromHexThirdYear = System.Drawing.ColorTranslator.FromHtml("#CFF3F7");
            Color colFromHexFourthYear = System.Drawing.ColorTranslator.FromHtml("#FAEEDF");
            Color colFromHexFifthYear = System.Drawing.ColorTranslator.FromHtml("#F3D396 ");

            #region HeaderFirstRow

            worksheet.Cells[rowcount, colcount].Value = "PROJECT LIFECYCLE REPORT";
            worksheet.Cells[rowcount, colcount, rowcount, (colcount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount)].Style.Font.Bold = true;

            #endregion

            rowcount++;
            colcount = 1;

            #region HeaderSecondRow

            worksheet.Cells[rowcount, colcount].Value = header;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet.Cells[rowcount, colcount, rowcount, (colcount)].Style.Font.Bold = true;

            #endregion


            rowcount++;
            colcount = 1;

            #region HeaderThirdRow
            worksheet.Cells[rowcount, colcount].Value = "Project Id";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = "Project Name";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = "Type";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;

            foreach (var item in projectLife.ProjectLifeCycleRevenueData)
            {
                worksheet.Cells[rowcount, colcount].Value = item.YearText;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
            }

            worksheet.Cells[rowcount, colcount].Value = "Grand Total";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            #endregion


            #region Data Rows

            rowcount++;
            colcount = 1;
            worksheet.Cells[rowcount, colcount].Value = projectLife.ProjectId;
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = projectLife.ProjectName;
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = "REVENUE";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            var totalDecimal = Convert.ToDecimal(0);
            foreach (var item in projectLife.ProjectLifeCycleRevenueData)
            {
                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                worksheet.Cells[rowcount, colcount].Value = item.Amount;
                if (item.Amount < 0)
                    worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                colcount++;
                totalDecimal += item.Amount;
            }
            worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
            worksheet.Cells[rowcount, colcount].Value = totalDecimal;
            if (totalDecimal < 0)
                worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = dataWidth;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


            rowcount++;
            colcount = 1;
            worksheet.Cells[rowcount, colcount].Value = projectLife.ProjectId;
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = projectLife.ProjectName;
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            worksheet.Cells[rowcount, colcount].Value = "GROSS MARGIN";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = 20;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            colcount++;
            totalDecimal = Convert.ToDecimal(0);
            foreach (var item in projectLife.ProjectLifeCycleGMData)
            {
                worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                worksheet.Cells[rowcount, colcount].Value = item.Amount;
                if (item.Amount < 0)
                    worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = dataWidth;
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                totalDecimal += item.Amount;
                colcount++;
            }

            worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
            worksheet.Cells[rowcount, colcount].Value = totalDecimal;
            if (totalDecimal < 0)
                worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            worksheet.Column(colcount).Width = dataWidth;
            worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
            worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            #endregion
 
            return excelPkg.GetAsByteArray();
        }

        public ExtractResponseDataModel REPExtractReport(int year, string scenarioTypeCode, string isCurrencyConversionRequired)
        {
            return new ReportRepository().REPExtractReport(year, scenarioTypeCode, isCurrencyConversionRequired);
        }
        //public RepExtractForeCastFullResponse REPExtractForeCastReport(int year, string scenarioTypeCode, string isCurrencyConversionRequired)
        //{
        //    //return new ReportRepository().REPExtractForeCastReport(year, scenarioTypeCode, isCurrencyConversionRequired);
        //}

        public byte[] REPExtractReportDownload(ExtractResponseDataModel response, int Year, string scenarioTypeCode, string isCurrencyConversionRequired)
        {
            int dataWidth = 15;
            string positiveFormat = "#,##0.00_)";
            string negativeFormat = "(#,##0.00)";
            string zeroFormat = "-_)";
            string numberFormat = positiveFormat + ";" + negativeFormat;
            string fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat;
            ExcelPackage excelPkg = new ExcelPackage();
            ExcelWorksheet worksheet = excelPkg.Workbook.Worksheets.Add("REPExtract Report ");
            var headercolcount = 0;
            var colcount = 1;
            var rowcount = 1;
            Color colFromHexGMHeader = System.Drawing.ColorTranslator.FromHtml("#B1E3F9");
            Color colFromHexMediumBlue = System.Drawing.ColorTranslator.FromHtml("#A0C4F9");


            Color colFromHexYellow = System.Drawing.ColorTranslator.FromHtml("#FADE65");
            Color colFromHexLightBlue = System.Drawing.ColorTranslator.FromHtml("#CADDFA");
            Color colFromHexLightRed = System.Drawing.ColorTranslator.FromHtml("#F9D2BD");
            Color colFromHexDarkGreen = System.Drawing.ColorTranslator.FromHtml("#4FD179");

            Color colFromHexThirdYear = System.Drawing.ColorTranslator.FromHtml("#CFF3F7");
            Color colFromHexFourthYear = System.Drawing.ColorTranslator.FromHtml("#FAEEDF");
            Color colFromHexFifthYear = System.Drawing.ColorTranslator.FromHtml("#F3D396 ");

            string dollerName = "kUSD";
            if (isCurrencyConversionRequired == "Y")
                dollerName = "kEUR";
            if (scenarioTypeCode == "AC")
            {
                headercolcount = 12;
                #region HeaderFirstRow

                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "REP REPORT FOR ACTUALS";
                // worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Font.Bold = true;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkBlue);


                #endregion

                rowcount++;
                colcount = 1;
                #region HeaderSecondRow

                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "YEAR - " + Year.ToString();
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Font.Bold = true;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkBlue);


                #endregion

                rowcount++;
                colcount = 1;
                #region HeaderThirdRow

                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "SCENARIO " + response.ScenarioName + " (Profit & Loss Actuals)";
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Font.Bold = true;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkBlue);


                #endregion


                rowcount++;
                colcount = 1;
                #region HeaderFifthRow

                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;

                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "REVENUE (" + dollerName + ")";
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);

                colcount = colcount + 4;

                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "GROSS MARGIN (" + dollerName + ")";
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.BackgroundColor.SetColor(colFromHexGMHeader);

                colcount = colcount + 4;
                #endregion

                rowcount++;
                colcount = 1;
                #region Report Header Row
                worksheet.Cells[rowcount, colcount].Value = "Smart View Code";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Project Entity";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Project Segment";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "BU Category";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Statutory Category";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q1 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q2 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q3 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q4 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q1 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q2 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q3 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q4 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                #endregion

                foreach (var item in response.ListExtractDetailDataModel)
                {
                    rowcount++;
                    colcount = 1;
                    if (item.RecordType == "P")
                    {
                        worksheet.Cells[rowcount, colcount].Value = item.SmartViewName;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = item.ProjectEntityCode;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = item.ProjectSegmentCode;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = item.BUCategoryName;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = item.StatutoryCategoryName;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    else
                    {
                        //rowcount++;

                        worksheet.Cells[rowcount, colcount].Value = "Grand Total";
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount + 4)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount + 4)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount + 4)].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount + 4)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount + 4)].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);


                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = string.Empty;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = string.Empty;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = string.Empty;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = string.Empty;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearRevenueQ1New.Value;
                    if (item.FirstYearRevenueQ1New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearRevenueQ2New.Value;
                    if (item.FirstYearRevenueQ2New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearRevenueQ3New.Value;
                    if (item.FirstYearRevenueQ3New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearRevenueQ4New.Value;
                    if (item.FirstYearRevenueQ4New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearGMQ1New.Value;
                    if (item.FirstYearGMQ1New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearGMQ2New.Value;
                    if (item.FirstYearGMQ2New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearGMQ3New.Value;
                    if (item.FirstYearGMQ3New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearGMQ4New.Value;
                    if (item.FirstYearGMQ4New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    //if (item.RecordType != "P")
                    //{
                    //    worksheet.Cells[rowcount, 1, rowcount, 13].Style.Fill.BackgroundColor.SetColor(colFromHexMediumBlue);
                    //}

                }


                if (isCurrencyConversionRequired == "Y")
                {
                    rowcount++;
                    rowcount++;
                    colcount = 1;
                    worksheet.Cells[rowcount, colcount].Value = "Exchange Rate for Euro :";
                    worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;
                    var exchangeRate = "";
                    foreach (var item in response.ListCurrencyExchangeData)
                    {
                        if (item.Quarter == "Q1")
                        {
                            exchangeRate = "Q1 - " + item.AverageRate.Value.ToString(decimalformat);
                            rowcount++;
                            colcount = 1;
                            worksheet.Cells[rowcount, colcount].Value = exchangeRate;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;
                        }
                        if (item.Quarter == "Q2")
                        {
                            exchangeRate = "Q2 - " + item.AverageRate.Value.ToString(decimalformat);
                            rowcount++;
                            colcount = 1;
                            worksheet.Cells[rowcount, colcount].Value = exchangeRate;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;

                        }
                        if (item.Quarter == "Q3")
                        {
                            exchangeRate = "Q3 - " + item.AverageRate.Value.ToString(decimalformat);
                            rowcount++;
                            colcount = 1;
                            worksheet.Cells[rowcount, colcount].Value = exchangeRate;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;

                        }
                        if (item.Quarter == "Q4")
                        {
                            exchangeRate = "Q4 - " + item.AverageRate.Value.ToString(decimalformat);
                            rowcount++;
                            colcount = 1;
                            worksheet.Cells[rowcount, colcount].Value = exchangeRate;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;

                        }
                    }


                }

            }
            else
            {
                headercolcount = 26;
                #region HeaderFirstRow

                // worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "REP REPORT FOR FORECAST";
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Font.Bold = true;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkBlue);


                #endregion

                rowcount++;
                colcount = 1;
                #region HeaderSecondRow

                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "Current Year& all forcast";
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Font.Bold = true;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkBlue);


                #endregion

                rowcount++;
                colcount = 1;
                #region HeaderThirdRow

                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "SCENARIO " + response.ScenarioName + " (Current forecast scenarios)";
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Font.Bold = true;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount, rowcount, (colcount + headercolcount)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkBlue);


                #endregion

                rowcount++;
                colcount = 1;
                #region HeaderFifthRow

                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;

                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "REVENUE (" + dollerName + ")";
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);

                colcount = colcount + 11;

                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = "GROSS MARGIN (" + dollerName + ")";
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 10)].Style.Fill.BackgroundColor.SetColor(colFromHexGMHeader);

                colcount = colcount + 11;
                #endregion


                rowcount++;
                colcount = 1;
                #region HeaderSixthhRow

                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = string.Empty;
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;

                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = Year.ToString();
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);

                colcount = colcount + 4;

                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = (Year + 1).ToString();
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);

                colcount = colcount + 4;

                worksheet.Cells[rowcount, colcount].Value = (Year + 2).ToString();
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = (Year + 3).ToString();
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = (Year + 4).ToString() + "+";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexDarkGreen);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = Year.ToString();
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.BackgroundColor.SetColor(colFromHexGMHeader);

                colcount = colcount + 4;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Merge = true;
                worksheet.Cells[rowcount, colcount].Value = (Year + 1).ToString();
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount, rowcount, (colcount + 3)].Style.Fill.BackgroundColor.SetColor(colFromHexGMHeader);

                colcount = colcount + 4;
                worksheet.Cells[rowcount, colcount].Value = (Year + 2).ToString();
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexGMHeader);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = (Year + 3).ToString();
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexGMHeader);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = (Year + 4).ToString() + "+";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexGMHeader);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                #endregion

                rowcount++;
                colcount = 1;
                #region Report Header Row
                worksheet.Cells[rowcount, colcount].Value = "Smart View Code";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Project Entity";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Project Segment";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "BU Category";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Statutory Category";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q1 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q2 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q3 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q4 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q1 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q2 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q3 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q4 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "FY";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexThirdYear);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "FY";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexFourthYear);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "FY";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexFifthYear);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q1 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q2 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q3 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q4 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q1 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q2 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q3 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexYellow);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "Q4 (YTD)";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "FY";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexThirdYear);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "FY";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexFourthYear);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                colcount++;
                worksheet.Cells[rowcount, colcount].Value = "FY";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                worksheet.Column(colcount).Width = 20;
                worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexFifthYear);
                worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                #endregion

                foreach (var item in response.ListExtractDetailDataModel)
                {
                    rowcount++;
                    colcount = 1;
                    if (item.RecordType == "P")
                    {
                        worksheet.Cells[rowcount, colcount].Value = item.SmartViewName;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = item.ProjectEntityCode;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;

                        worksheet.Cells[rowcount, colcount].Value = item.ProjectSegmentCode;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;

                        worksheet.Cells[rowcount, colcount].Value = item.BUCategoryName;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;

                        worksheet.Cells[rowcount, colcount].Value = item.StatutoryCategoryName;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                    else
                    {
                        //rowcount++;
                        worksheet.Cells[rowcount, colcount].Value = "Grand Total";
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount)].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount, rowcount, (colcount)].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);

                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = string.Empty;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = string.Empty;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = string.Empty;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        colcount++;
                        worksheet.Cells[rowcount, colcount].Value = string.Empty;
                        worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        worksheet.Column(colcount).Width = 20;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        //worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                        worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearRevenueQ1New.Value;
                    if (item.FirstYearRevenueQ1New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearRevenueQ2New.Value;
                    if (item.FirstYearRevenueQ2New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearRevenueQ3New.Value;
                    if (item.FirstYearRevenueQ3New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearRevenueQ4New.Value;
                    if (item.FirstYearRevenueQ4New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    //Second Year Revenue

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.SecondYearRevenueQ1New.Value;
                    if (item.SecondYearRevenueQ1New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.SecondYearRevenueQ2New.Value;
                    if (item.SecondYearRevenueQ2New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.SecondYearRevenueQ3New.Value;
                    if (item.SecondYearRevenueQ3New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.SecondYearRevenueQ4New.Value;
                    if (item.SecondYearRevenueQ4New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    //Third Year Revenue

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.ThirdYearRevenueTC.Value;
                    if (item.ThirdYearRevenueTC.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexThirdYear);
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    //FourthYear Revenue
                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FourthYearRevenueTC.Value;
                    if (item.FourthYearRevenueTC.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexFourthYear);
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    //Fifth Year Revenue
                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FifthYearRevenueTC.Value;
                    if (item.FifthYearRevenueTC.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexFifthYear);
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                    //Gross Margin
                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearGMQ1New.Value;
                    if (item.FirstYearGMQ1New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearGMQ2New.Value;
                    if (item.FirstYearGMQ2New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearGMQ3New.Value;
                    if (item.FirstYearGMQ3New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FirstYearGMQ4New.Value;
                    if (item.FirstYearGMQ4New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    //Second Year GM

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.SecondYearGMQ1New.Value;
                    if (item.SecondYearGMQ1New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.SecondYearGMQ2New.Value;
                    if (item.SecondYearGMQ2New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.SecondYearGMQ3New.Value;
                    if (item.SecondYearGMQ3New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.SecondYearGMQ4New.Value;
                    if (item.SecondYearGMQ4New.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightBlue);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    //Third Year GM

                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.ThirdYearGMTC.Value;
                    if (item.ThirdYearGMTC.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexThirdYear);
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    //FourthYear GM
                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FourthYearGMTC.Value;
                    if (item.FourthYearGMTC.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexFourthYear);
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    //Fifth Year GM
                    colcount++;
                    worksheet.Cells[rowcount, colcount].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, colcount].Value = item.FifthYearGMTC.Value;
                    if (item.FifthYearGMTC.Value < 0)
                        worksheet.Cells[rowcount, colcount].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Column(colcount).Width = dataWidth;
                    if (item.RecordType != "P")
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexLightRed);
                        worksheet.Cells[rowcount, colcount].Style.Font.Bold = true;
                    }
                    else
                    {
                        worksheet.Cells[rowcount, colcount].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[rowcount, colcount].Style.Fill.BackgroundColor.SetColor(colFromHexFifthYear);
                    }
                    worksheet.Cells[rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    //if (item.RecordType != "P")
                    //{
                    //    worksheet.Cells[rowcount, 1, rowcount, 13].Style.Fill.BackgroundColor.SetColor(colFromHexMediumBlue);
                    //}

                }


                if (isCurrencyConversionRequired == "Y")
                {
                    rowcount++;
                    rowcount++;
                    colcount = 1;
                    worksheet.Cells[rowcount, colcount].Value = "Exchange Rate for Euro :";
                    worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;
                    var exchangeRate = "";
                    foreach (var item in response.ListCurrencyExchangeData)
                    {
                        if (item.Quarter == "Q1")
                        {
                            exchangeRate = "Q1 - " + item.AverageRate.Value.ToString(decimalformat);
                            rowcount++;
                            colcount = 1;
                            worksheet.Cells[rowcount, colcount].Value = exchangeRate;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;
                        }
                        if (item.Quarter == "Q2")
                        {
                            exchangeRate = "Q2 - " + item.AverageRate.Value.ToString(decimalformat);
                            rowcount++;
                            colcount = 1;
                            worksheet.Cells[rowcount, colcount].Value = exchangeRate;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;

                        }
                        if (item.Quarter == "Q3")
                        {
                            exchangeRate = "Q3 - " + item.AverageRate.Value.ToString(decimalformat);
                            rowcount++;
                            colcount = 1;
                            worksheet.Cells[rowcount, colcount].Value = exchangeRate;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;

                        }
                        if (item.Quarter == "Q4")
                        {
                            exchangeRate = "Q4 - " + item.AverageRate.Value.ToString(decimalformat);
                            rowcount++;
                            colcount = 1;
                            worksheet.Cells[rowcount, colcount].Value = exchangeRate;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worksheet.Cells[rowcount, colcount, rowcount, colcount].Style.Font.Bold = true;

                        }
                    }


                }

            }


            return excelPkg.GetAsByteArray();
        }

        //public ExtractResponseDataModel REPExtractReport(int year, string scenarioTypeCode, string isCurrencyConversionRequired)
        //{
        //    throw new NotImplementedException();
        //}
        public List<DevianceGridResponse> GetDevianceReport(DevianceReportConfig config)
        {
            //var quarters = config.Quarters.Split(',').OrderBy(c => c).ToList();
            var grid = new List<DevianceGridResponse>();
            var returnList = new ReportRepository().GetDevianceReport(config);
            Decimal? GrossQ1D = 0;
            Decimal? GrossQ2D = 0;
            Decimal? GrossQ3D = 0;
            Decimal? GrossQ4D = 0;
            Decimal? GrossTotalDep = 0;
            grid.Add(new DevianceGridResponse()
            {
                Q1 = returnList.OrgQ1,
                Q2 = returnList.OrgQ2,
                Q3 = returnList.OrgQ3,
                Q4 = returnList.OrgQ4,
                Total = returnList.TotalOrg,
                EntityName = "Organization",
                IsBold = true,
            });
            grid.Add(new DevianceGridResponse()
            {
                EntityName = "Departments",
                IsBold = true
            }); ;

            foreach (var item in returnList.DData)
            {
                grid.Add(new DevianceGridResponse()
                {
                    Q1 = item.DQ1,
                    Q2 = item.DQ2,
                    Q3 = item.DQ3,
                    Q4 = item.DQ4,
                    Total = item.TotalDep,
                    EntityName = item.DepartmentName.Decrypt(),
                });

                GrossQ1D = item.DQ1 + GrossQ1D;
                GrossQ2D = item.DQ2 + GrossQ2D;
                GrossQ3D = item.DQ3 + GrossQ3D;
                GrossQ4D= item.DQ4 + GrossQ4D;
                GrossTotalDep = item.TotalDep + GrossTotalDep;
            }
            grid.Add(new DevianceGridResponse()
            {
                Q1 = GrossQ1D,
                Q2 = GrossQ2D,
                Q3 = GrossQ3D,
                Q4 = GrossQ4D,
                Total = GrossTotalDep,
                EntityName = "Sub Total Department",
                IsBold = true,
            });

            Decimal? GrandTotal = GrossTotalDep;
            Decimal? GrossQ1C = 0;
            Decimal? GrossQ2C = 0;
            Decimal? GrossQ3C = 0;
            Decimal? GrossQ4C = 0;
            Decimal? GrossTotalClient = 0;
            // leter in pahse 2
            //if (returnList.CData.Count != 0) {
            //    grid.Add(new DevianceGridResponse()
            //    {
                    
            //        EntityName = "Clients",
            //    });
            //    foreach (var item in returnList.CData)
            //    {
            //        grid.Add(new DevianceGridResponse()
            //        {
            //            Q1 = item.CQ1,
            //            Q2 = item.CQ2,
            //            Q3 = item.CQ3,
            //            Q4 = item.CQ4,
            //            Total = item.TotalClient,
            //            EntityName = item.ClientName.Decrypt(),
            //        });
            //        GrossQ1C = item.CQ1 + GrossQ1C;
            //        GrossQ2C = item.CQ2 + GrossQ2C;
            //        GrossQ3C = item.CQ3 + GrossQ3C;
            //        GrossQ4C = item.CQ4 + GrossQ4C;
            //        GrossTotalClient = item.TotalClient + GrossTotalClient;
            //    }
            //    //grid.Add(new DevianceGridResponse()
            //    //{
            //    //    Q1 = GrossQ1C,
            //    //    Q2 = GrossQ2C,
            //    //    Q3 = GrossQ3C,
            //    //    Q4 = GrossQ4C,
            //    //    Total = GrossTotalClient,
            //    //    EntityName = "Sub Total Client",
            //    //});
            //    grid.Add(new DevianceGridResponse()
            //    {
            //        Q1 = GrossQ1C + GrossQ1D,
            //        Q2 = GrossQ2C + GrossQ2D,
            //        Q3 = GrossQ3C + GrossQ3D,
            //        Q4 = GrossQ4C + GrossQ4D,
            //        Total = GrossTotalClient + GrossTotalDep,
            //        EntityName = "Grand Total",
            //    });
            //}
            grid.Add(new DevianceGridResponse()
            {
                Q1 = returnList.OrgQ1 -(GrossQ1C + GrossQ1D),
                Q2 = returnList.OrgQ2 - (GrossQ2C + GrossQ2D),
                Q3 = returnList.OrgQ3 - (GrossQ3C + GrossQ3D),
                Q4 = returnList.OrgQ4 - (GrossQ4C + GrossQ4D),
                Total = returnList.TotalOrg - (GrossTotalClient + GrossTotalDep),
                EntityName = "Deviation",
            });

            return grid;

        }
        public byte[] GetDevianceReportExcel(DevianceReportConfig config, List<DevianceResponseModel> data)
        {
            // number formats
            string positiveFormat = "#,##0.00_)";
            string negativeFormat = "(#,##0.00)";
            string zeroFormat = "-_)";
            string numberFormat = positiveFormat + ";" + negativeFormat;
            string fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat;

            int projectCodeWidth = 30;
            int projectCodeElseWidth = 20;
            int dataWidth = 15;

            var quarters = config.Quarters.Split(',');
            var colcountQuarters = quarters.Length;
            var headercounter = colcountQuarters * 3 + 4;
            ExcelPackage excelPkg = new ExcelPackage();
            
            foreach(DevianceResponseModel item in data)
            {
                ExcelWorksheet worksheet = excelPkg.Workbook.Worksheets.Add(item.FinancialDataType);
                var colcount = 1;
                var rowcount = 1;
                worksheet.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(Color.Beige);
                #region blank row
                worksheet.Cells[rowcount, colcount].Value = "Entity Name";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
                worksheet.Column(colcount).Width = projectCodeWidth;
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = "Q1";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
                worksheet.Column(colcount).Width = projectCodeElseWidth;
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = "Q2";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
                worksheet.Column(colcount).Width = projectCodeElseWidth;
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = "Q3";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
                worksheet.Column(colcount).Width = projectCodeElseWidth;
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = "Q4";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
                worksheet.Column(colcount).Width = projectCodeElseWidth;
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = "Total";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
                worksheet.Column(colcount).Width = projectCodeElseWidth;
                colcount++;

                #endregion
                foreach (var row in item.GridResponse)
                {
                        rowcount++;
                        worksheet.Cells[rowcount, 1].Value = row.EntityName;
                        worksheet.Cells[rowcount, 2].Value = row.Q1;
                        worksheet.Cells[rowcount, 3].Value = row.Q2;
                        worksheet.Cells[rowcount, 4].Value = row.Q3;
                        worksheet.Cells[rowcount, 5].Value = row.Q4;
                        worksheet.Cells[rowcount, 6].Value = row.Total;
                }
            }
            
            return excelPkg.GetAsByteArray();
        }
        public byte[] GetFinanceReportExcel(DashboardConfig config,FinancePerformanceDataModel data)
        {
            //number formats
            string positiveFormat = "#,##0.00_)";
            string negativeFormat = "(#,##0.00)";
            string zeroFormat = "-_)";
            string numberFormat = positiveFormat + ";" + negativeFormat;
            string fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat;

            int projectCodeWidth = 30;
            int projectCodeElseWidth = 20;
            int dataWidth = 15;

            //var quarters = config.Quarters.Split(',');
            //var colcountQuarters = quarters.Length;
            //var headercounter = colcountQuarters * 3 + 4;
            ExcelPackage excelPkg = new ExcelPackage();

           
                ExcelWorksheet worksheet = excelPkg.Workbook.Worksheets.Add("Department");
                var colcount = 1;
                var rowcount = 1;
                worksheet.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                worksheet.Cells[rowcount, colcount].Value = "Department Name";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = "Q1";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = "Q2";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = "Q3";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                colcount++;

                worksheet.Cells[rowcount, colcount].Value = "Q4";
                worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                colcount++;

                

                
                foreach (var row in data.FinancialDataChart)
                {
                    rowcount++;
                    worksheet.Cells[rowcount, 1].Value = row.DepartmentName;
                    worksheet.Cells[rowcount, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    if (row.Q1.HasValue)
                    {
                        worksheet.Cells[rowcount, 2].Value = row.Q1;
                        if (row.Q1.Value < 0)
                            worksheet.Cells[rowcount, 2].Style.Font.Color.SetColor(Color.Red);
                        worksheet.Cells[rowcount, 2].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    if (row.Q2.HasValue)
                    {
                        worksheet.Cells[rowcount, 3].Value = row.Q2;
                        if (row.Q2.Value < 0)
                            worksheet.Cells[rowcount, 3].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, 3].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    if (row.Q3.HasValue)
                    {
                        worksheet.Cells[rowcount, 4].Value = row.Q3;
                        if (row.Q3.Value < 0)
                            worksheet.Cells[rowcount, 4].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, 4].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                    }
                    if (row.Q4.HasValue)
                    {
                        worksheet.Cells[rowcount, 5].Value = row.Q4;
                        if (row.Q4.Value < 0)
                            worksheet.Cells[rowcount, 5].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, 5].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                
                }
            

            return excelPkg.GetAsByteArray();
        }
        public byte[] GetProjectPerformanceReportExcel(DashboardConfig config, ProjectPerformanceDataModel data)
        {
            // number formats
            string positiveFormat = "#,##0.00_)";
            string negativeFormat = "(#,##0.00)";
            string zeroFormat = "-_)";
            string numberFormat = positiveFormat + ";" + negativeFormat;
            string fullNumberFormat = positiveFormat + ";" + negativeFormat + ";" + zeroFormat;

            //int projectCodeWidth = 30;
            //int projectCodeElseWidth = 20;
            //int dataWidth = 15;

            //var quarters = config.Quarters.Split(',');
            //var colcountQuarters = quarters.Length;
            //var headercounter = colcountQuarters * 3 + 4;
            ExcelPackage excelPkg = new ExcelPackage();


            ExcelWorksheet worksheet = excelPkg.Workbook.Worksheets.Add("Project");
            var colcount = 1;
            var rowcount = 1;
            worksheet.Cells["A1:E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells["A1:E1"].Style.Fill.BackgroundColor.SetColor(Color.LightCyan);
            #region blank row
            worksheet.Cells[rowcount, colcount].Value = "Project Name";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "Q1";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "Q2";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "Q3";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            colcount++;

            worksheet.Cells[rowcount, colcount].Value = "Q4";
            worksheet.Cells[rowcount, colcount].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.None);
            colcount++;



            #endregion
            foreach (var row in data.ProjectData)
            {
                rowcount++;
                worksheet.Cells[rowcount, 1].Value = row.DepartmentName;
                worksheet.Cells[rowcount, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                if (row.Q1.HasValue)
                {
                    worksheet.Cells[rowcount, 2].Value = row.Q1;
                    if (row.Q1.Value < 0)
                        worksheet.Cells[rowcount, 2].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, 2].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }
                if (row.Q2.HasValue)
                {
                    worksheet.Cells[rowcount, 3].Value = row.Q2;
                    if (row.Q2.Value < 0)
                        worksheet.Cells[rowcount, 3].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, 3].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }
                if (row.Q3.HasValue)
                {
                    worksheet.Cells[rowcount, 4].Value = row.Q3;
                    if (row.Q3.Value < 0)
                        worksheet.Cells[rowcount, 4].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, 4].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

                }
                if (row.Q4.HasValue)
                {
                    worksheet.Cells[rowcount, 5].Value = row.Q4;
                    if (row.Q4.Value < 0)
                        worksheet.Cells[rowcount, 5].Style.Font.Color.SetColor(Color.Red);
                    worksheet.Cells[rowcount, 5].Style.Numberformat.Format = fullNumberFormat;
                    worksheet.Cells[rowcount, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                }
            }


            return excelPkg.GetAsByteArray();
        }
        public List<FinancialDataType> GetAllFinancialDataTypesOfScenario(int id)
        {
            FinancialDataTypeRepository _financialDataTypeRepo = new FinancialDataTypeRepository();
            return _financialDataTypeRepo.GetAllFinancialDataTypesOfScenario(id);
        }

        public DeviationAnalysisYearWiseModel GetDeviationAnalysisReport(DevianceReportConfig config)
        {
            //var quarters = config.Quarters.Split(',').OrderBy(c => c).ToList();
            var grid = new DeviationAnalysisYearWiseModel();
            grid = new ReportRepository().GetDeviationAnalysisReport(config);
            

            return grid;

        }
        public DashboardDataModel GetDashboardData(DashboardConfig config)
        {
            //var quarters = config.Quarters.Split(',').OrderBy(c => c).ToList();
            var grid = new DashboardDataModel();
            grid = new ReportRepository().GetDashboardData(config);


            return grid;

        }
        public FinancePerformanceDataModel FinancePerformanceReport(DashboardConfig config)
        {
            //var quarters = config.Quarters.Split(',').OrderBy(c => c).ToList();
            var grid = new FinancePerformanceDataModel();
            grid = new ReportRepository().FinancePerformanceReport(config);


            return grid;

        }
        public ProjectPerformanceDataModel ProjectPerformanceReport(DashboardConfig config)
        {
            //var quarters = config.Quarters.Split(',').OrderBy(c => c).ToList();
            var grid = new ProjectPerformanceDataModel();
            grid = new ReportRepository().ProjectPerformanceReport(config);


            return grid;

        }
        public TrendReportData TrendAnalysisReport(DashboardConfig config)
        {
            //var quarters = config.Quarters.Split(',').OrderBy(c => c).ToList();
            var grid = new TrendReportData();
            grid = new ReportRepository().TrendAnalysisReport(config);


            return grid;

        }
        public List<ProjectDataModel> YearOverYear(int id1, int id2, string code)
        {
            //var quarters = config.Quarters.Split(',').OrderBy(c => c).ToList();
            var grid = new List<ProjectDataModel>();
            grid = new ReportRepository().YearOverYear(id1, id2, code);


            return grid;

        }
    }
}

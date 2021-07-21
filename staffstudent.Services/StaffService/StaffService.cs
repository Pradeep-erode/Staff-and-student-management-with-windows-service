using LinqToExcel;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using staffstudent.Core.IRepository;
using staffstudent.Core.IService;
using staffstudent.Core.staffEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace staffstudent.Services.StaffService
{
    public class StaffService : IServiceStaff
    {
        private readonly IRepositoryStaff _Irepositorystaff;
        public StaffService(IRepositoryStaff repositorystaff)
        {
            _Irepositorystaff = repositorystaff;
        }
        public StudentMarkEntity Studentcheck(int roll, string password)
        {
            return _Irepositorystaff.Studentcheck(roll, password);
        }
        public List<StudentInformationEntity> Getstudentlist()
        {
            return _Irepositorystaff.Getstudentlist();
        }
        public void Addstudentdetail(StudentInformationEntity studentdetail)
        {
            _Irepositorystaff.Addstudentdetail(studentdetail);
        }
        public StudentInformationEntity Getstudentbyrollno(int rollno)
        {
            return _Irepositorystaff.Getstudentbyrollno(rollno);
        }
        public void Deletestudentbyrollno(int rollno)
        {
            _Irepositorystaff.Deletestudentbyrollno(rollno);
        }
        public string UploadExclel(Fileupload uploadexcel)
        {
            #region Save to root folder

            string filename = uploadexcel.Filename;
            IFormFile docs = uploadexcel.excelfile;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Root", "Excel", filename);

            //if (File.Exists(path))
            //{
            //    File.Delete(path);
            //}
            ExcelWorksheet worksheet;

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                docs.CopyToAsync(fileStream);

                #endregion

            #region using Excelpackages for reading excel file

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(fileStream);
                worksheet = package.Workbook.Worksheets.FirstOrDefault();
                fileStream.Close();
            }
            if (worksheet == null)
            {
                return "please buy visual studio key to process yourfile... ";
            }
            // get number of rows and columns in the sheet
            int rows = worksheet.Dimension.Rows;
            int columns = worksheet.Dimension.Columns;
            if (columns != 7)
            {
                return "Required columns may be deleted...please check";
            }

            // loop through the worksheet rows and columns for each cell read and check null
            var counts = 0;
            for (int i = 2; i <= rows; i++)
            {
                for (int j = 1; j <= columns; j++)
                {
                    if (worksheet.Cells[i, j].Value != null)
                    {
                        counts++;
                    }
                    else
                    {
                        return worksheet.Cells[1, j].Value.ToString()+ " column contain null value...";
                    }
                }
            }

            List<Excelvalidation> artistAlbums = new List<Excelvalidation>();

            //add to our model..
            for (int i = 2; i <= rows; i++)
            {
                artistAlbums.Add(new Excelvalidation
                {
                    Student_Roll_no = worksheet.Cells[i, 1].Value.ToString(),
                    Tamil = worksheet.Cells[i, 2].Value.ToString(),
                    English = worksheet.Cells[i, 3].Value.ToString(),
                    Science = worksheet.Cells[i, 4].Value.ToString(),
                    Maths = worksheet.Cells[i, 5].Value.ToString(),
                    Total = worksheet.Cells[i, 6].Value.ToString(),
                    Average = worksheet.Cells[i, 7].Value.ToString()

                });

            }

            #endregion

            #region using OLeDB for excel read with Datatable

            //var connectionString = "";
            //if (filename.EndsWith(".xls"))
            //{
            //    connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", path);
            //}
            //else if (filename.EndsWith(".xlsx"))
            //{
            //    connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;";
            //}

            //var adapter = new OleDbDataAdapter("SELECT [Student_Roll_no],[Tamil],[English]," +
            // "[Science],[Maths],[Total],[Average] FROM [Sheet1$]", connectionString);

            //var ds = new DataSet();
            //adapter.Fill(ds, "ExcelTable");
            //DataTable dtable = ds.Tables["ExcelTable"];


            #endregion


            try
            {
                #region Using Linq To excel 

                //string sheetName = "Sheet1";
                //var excelFile = new ExcelQueryFactory(path);
                ////LinqToExcel querry to read data from excel file...
                //var artistAlbums = from a in excelFile.Worksheet<Excelvalidation>(sheetName) select a;

                #endregion

                List<StudentMarkEntity> excellist = new List<StudentMarkEntity>();

                #region Excel validation for null,datatype,max,min in foreach loop

                foreach (var mark in artistAlbums)
                {
                    #region for getting datatype we also use like this...

                    //if (mark.Tamil.GetType().Equals(typeof(int)))
                    //{
                    //    if (mark.Tamil.Count() < 4)
                    //    {
                    //        markk.Tamil = Convert.ToInt32(mark.Tamil);
                    //    }
                    //    else { return 6; }
                    //}
                    //else { return 5; }

                    #endregion

                    #region sample Regex validation for mail...

                    //for getting mail and string from client we use regex like here for validation...

                    //var regex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");
                    //if (regex.IsMatch(mark.mail))
                    //{
                    //    return 1;
                    //}

                    #endregion

                    StudentMarkEntity markk = new StudentMarkEntity();

                    if (mark.Student_Roll_no != null && mark.Tamil != null && mark.English != null && mark.Science != null && mark.Maths != null && mark.Total != null && mark.Average != null)
                    {
                        try
                        {
                            int rollno = Convert.ToInt32(mark.Student_Roll_no.Trim());
                            
                            if (mark.Student_Roll_no.Count() == 4)
                            {
                                markk.Student_Roll_no = rollno;
                            }
                            else { return "Student Roll no column should be only 4 digit..."; }
                        }
                        catch { 
                            return "Student Roll no column Contain special character..."; }
                        try
                        {
                            int Tamilmark = Convert.ToInt32(mark.Tamil.Trim());
                            if (Tamilmark < 100)
                            {
                                markk.Tamil = Tamilmark;
                            }
                            else { return "Tamil column contain invalid marks(>100)..."; }
                        }
                        catch { return "Tamil column Contain special character...!!!"; }
                        try
                        {
                            int Englishmark = Convert.ToInt32(mark.English.Trim());
                            if (Englishmark < 100)
                            {
                                markk.English = Englishmark;
                            }
                            else { return "English column contain invalid marks(>100)..."; }
                        }
                        catch { return "English column Contain special character...!!!"; }
                        try
                        {
                            int sciencemark = Convert.ToInt32(mark.Science.Trim());
                            if (sciencemark < 100)
                            {
                                markk.Science = sciencemark;
                            }
                            else { return "Science column contain invalid marks(>100)..."; }
                        }
                        catch { return "Science column Contain special character...!!!"; }
                        try
                        {
                            int Matmark = Convert.ToInt32(mark.Maths.Trim());
                            if (Matmark < 100)
                            {
                                markk.Maths = Matmark;
                            }
                            else { return "Maths column contain invalid marks(>100)..."; }
                        }
                        catch { return "Maths column Contain special character...!!!"; }
                        try
                        {
                            int Total = Convert.ToInt32(mark.Total.Trim());
                            if (Total <= 400)
                            {
                                markk.Total = Total;
                            }
                            else { return "Total column contain invalid marks(>400)..."; }
                        }
                        catch { return "Total column Contain special character...!!!"; }
                        try
                        {
                            double average = Convert.ToDouble(mark.Average.Trim());
                            if (mark.Average.Count() <= 100)
                            {
                                markk.Average = average;
                            }
                            else { return "Average column contain invalid average(>100)..."; }
                        }
                        catch { return "Average column Contain special characte...!!!"; }
                        excellist.Add(markk);

                    }
                    else
                    {
                        return "your excel table contain null cell...";
                    }
                }

                #endregion

                #region using OleDb with datatable and Datarow[] for excel validation

                //using datatable for validating each cell

                //var file = new OleDbConnection(connectionString);
                //file.Open();
                //var dt = file.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                //if (dt == null || dt.Rows.Count == 0) return 3;

                //string exp = "[Sheet1$]";

                //var fileStream = new FileStream(path, FileMode.Create);

                //using var package = new ExcelPackage(fileStream);
                //ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                //var rowcount = worksheet.Dimension.Rows;


                //DataRow[] fr = dt.Select(exp);
                //StudentMarkEntity markk = new StudentMarkEntity();

                //for (int a = 1; a > fr.Length; a++)
                //{
                //    for (int b = 1; b < dt.Columns.Count; b++)
                //    {
                //        if (fr[a][b] == null)
                //        {
                //            var nullcheck = fr[a][b] + "Contain null value";
                //            return 1;
                //        }
                //        else if (fr[a][b].GetType().Equals(typeof(int)))
                //        {
                //            markk.Student_Roll_no = Convert.ToInt32(fr[a][b]);
                //            return 1;
                //        }
                //        else
                //        {
                //            return 2;
                //        }

                //    }
                //}

                #endregion

                var list = excellist;
                var count = _Irepositorystaff.UploadExclel(list);
                return count;
            }
            catch
            {
                return "Linq to Excel package not intalled correctly...";
            }
        }
        public List<StudentMarkEntity> GetstudentMarkList()
        {
            return _Irepositorystaff.GetstudentMarkList();
        }
        public void ScheduleMail(StudentMarkEntity ScheduleMail)
        {
            _Irepositorystaff.ScheduleMail(ScheduleMail);
        }
    }
}

using Mail_scheduler_service.DLL.Datalogic;
using Mail_scheduler_service.DLL.Mymailmodel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mail_scheduler_service.BLL
{
    public class Mail_sender
    {
        // This function write log to LogFile.text when some error occurs.  
        public static void WriteErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\ErrorLogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + ex.Source.ToString().Trim() + "; " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        // This function write Message to log file.
        public static void WriteErrorLog(string Message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\MessageFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + ": " + Message);
                sw.Flush();
                sw.Close();
            }
            catch
            {
            }
        }
        // This function contains the logic to send mail.
        public static void SendEmail()
        {
            #region connection string

            //DataTable dt = new DataTable();
            //string query = "SELECT Subject FROM StudentMark";
            //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            //using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(constr))
            //{
            //    using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(query))
            //    {
            //        cmd.Connection = con;
            //        cmd.Parameters.AddWithValue("@Day", DateTime.Today.Day);
            //        cmd.Parameters.AddWithValue("@Month", DateTime.Today.Month);
            //        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            //        {
            //            sda.Fill(dt);
            //        }


            //    }
            //}

            ////foreach (DataRow row in dt.Rows)
            ////{
            ////    string namea = row["Subject"].ToString();
            ////    string date = row["Scheduled_time"].ToString();
            ////}


            //List<Studentdata> lista = new List<Studentdata>();
            //string connetionString;
            //SqlConnection cnn;
            //connetionString = constr;
            //cnn = new SqlConnection(connetionString);
            //cnn.Open();
            //SqlCommand commad;
            //SqlDataReader datareader;
            //string sql = query;
            //commad = new SqlCommand(sql,cnn);
            //datareader = commad.ExecuteReader();
            //while (datareader.Read())
            //{
            //    var stduent = new Studentdata();
            //    stduent.Subject= datareader[1].ToString();
            //}


            #endregion

            try
            {
                MailData hia = new MailData();
                var list = hia.GetStudentmail();
                foreach (var info in list)
                {
                    //var date = info.Date.Value.AddMinutes(10);
                    //string dates = string.Format("{0}/{1}/{2}", date.Day,date.Month,date.Year);
                    //string times= string.Format("{0}/{1}", date.Hour, date.Minute);
                    //string datess = string.Format("{0}/{1}/{2}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);
                    //string timess = string.Format("{0}/{1}", DateTime.Now.Hour, DateTime.Now.Minute);

                    if (info.Date == DateTime.Now) 
                    {
                        var Tomail = info.Email;
                        string name = info.Name;
                        string subject = info.Subject;

                        try
                        {
                            SmtpClient smtpClient = new SmtpClient();
                            smtpClient.EnableSsl = true; //cryptographic protocol encrypts the data that is exchanged between a web server and a user.
                            smtpClient.Timeout = 200000; //try to send mail for 200 seconds after it hit the catch exception.
                            MailMessage MailMsg = new MailMessage();
                            System.Net.Mime.ContentType HTMLType = new System.Net.Mime.ContentType("text/html");

                            string strBody = string.Format("Hi,{0} be ready for your {1} test...!!!", name, subject);

                            MailMsg.BodyEncoding = Encoding.Default;
                            MailMsg.To.Add(Tomail);
                            MailMsg.Priority = MailPriority.High;
                            MailMsg.Subject = "Test reminder";
                            MailMsg.Body = strBody;
                            MailMsg.IsBodyHtml = true;
                            AlternateView HTMLView = AlternateView.CreateAlternateViewFromString(strBody, HTMLType);

                            smtpClient.Send(MailMsg);
                            WriteErrorLog("Mail sent successfully! for "+ Tomail);
                        }
                        catch
                        {
                            WriteErrorLog("Mail not sended...");
                            throw;
                        }

                    }
                    else
                    {
                        WriteErrorLog("Wait for sometime...!!!");
                    }


                }
            }
            catch (Exception exe)
            {
                WriteErrorLog(exe.Message);
                throw;
            }
        }
    }
}



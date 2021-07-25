using Mail_scheduler_service.BLL;
using Mail_scheduler_service.DLL.Mail_entity;
using Mail_scheduler_service.DLL.Mymailmodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail_scheduler_service.DLL.Datalogic
{
    public class MailData
    {
        public List<Studentdata> GetStudentdata()
        {
            List<Studentdata> listofdata = new List<Studentdata>();

            using (var entity = new StaffmanagementEntitiesss())
            {
                var datematch = entity.StudentMarks.Where(m => m.Is_deleted == false &&m.Scheduled_time !=null).ToList();
                foreach (var datas in datematch)
                {
                    var datamodel = new Studentdata();
                    var lista = entity.Student_Information.Where(m => m.Student_Roll_No == datas.Student_Roll_no).FirstOrDefault();
                    datamodel.Name = lista.Student_First_name + " " + lista.Student_Last_Name;
                    datamodel.Email = lista.Email;
                    datamodel.Subject = datas.Subject;
                    datamodel.Date = datas.Scheduled_time;
                    listofdata.Add(datamodel);
                }
            }
            return listofdata;
        }
    }
}

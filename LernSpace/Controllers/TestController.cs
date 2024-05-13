using LernSpace.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LernSpace.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TestController : ApiController
    {
        SlowlernerDbEntities db = new SlowlernerDbEntities();
        public HttpResponseMessage AddNewTest(TestInfo info)
        {
            try
            {
                db.Test.Add(info.test);
                db.SaveChanges();
                foreach (var item in info.collectionsIds)
                {
                    item.testId = info.test.id;
                }
                db.TestCollection.AddRange(info.collectionsIds);

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,"Data Save");
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK,ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage userDefindTest(int Uid,int pid)
        {
            var data = db.Test.Where(e => e.createBy == Uid)
                .Join(db.TestCollection, test => test.id, testCollection => testCollection.testId, (test, testCollection) => new { test, testCollection })
                .Join(db.Collection, ttc => ttc.testCollection.collectId, collect => collect.id, (ppc, collect) => new
                {   
                    
                    collect.id,
                    collect.uText,
                    collect.eText,
                    collect.type,
                    collect.picPath,
                    collect.C_group,
                    collect.audioPath,
                    ppc.testCollection.questionTitle,
                    ppc.testCollection.op1,
                    ppc.testCollection.op2,
                    ppc.testCollection.op3,
                    ppc.test.title,
                    testId=ppc.test.id,
                    Op1ImagePath = db.Collection.Where(c => c.id == ppc.testCollection.op1).Select(c => c.picPath).FirstOrDefault(),
                    Op2ImagePath = db.Collection.Where(c => c.id == ppc.testCollection.op2).Select(c => c.picPath).FirstOrDefault(),
                    Op3ImagePath = db.Collection.Where(c => c.id == ppc.testCollection.op3).Select(c => c.picPath).FirstOrDefault(),
                    
                });
            if (pid != 0)
            {
                var patientAssignedTest = db.Patient.Where(e => e.pid == pid)
                    .Join(db.Appointment, patient => patient.pid, Appointment => Appointment.patientId, (patient, Appointment) => new { patient, Appointment })
                    .Join(db.AppointmentTest, Appointment => Appointment.Appointment.id, AppointmentTest => AppointmentTest.appointmentId, (PatientAppointment, AppointmentTest) => new { PatientAppointment, AppointmentTest })
                    .Join(db.Test, AppointTest => AppointTest.AppointmentTest.testId, Test => Test.id, (PatientAppointAppointTest, Test) => new
                    {
                        Test.id
                    }).ToList();
                var patientAssPracData = patientAssignedTest.Select(item => new PatienrpracticeTestData
                {
                    id = item.id,
                });
                var pracdata = data.Select(item => new TestData
                {
                   Id= item.id,
                   UText=item.uText,
                   EText=item.uText,
                   Type=item.type,
                   PicPath=item.picPath,
                   CGroup=item.C_group,
                   AudioPath=item.picPath,QuestionTitle=item.picPath,
                   Op1=item.op1,
                   Op2=item.op2,
                   Op3=item.op3,
                   Title=item.title,
                   TestId=item.id,
                   Op1ImagePath=item.Op1ImagePath,
                   Op2ImagePath=item.Op2ImagePath,
                   Op3ImagePath=item.Op3ImagePath,
                   flag=true
                }).ToList();

                foreach (var item in pracdata)
                {
                    if (patientAssPracData.Any(patient => patient.id == item.TestId))
                    {
                        // The pracId exists in patientAssignedPractices
                        // You can perform additional actions here if needed
                        item.flag = true; // For example, setting flag to true
                    }
                }
                if (pracdata.Any())
                {

                    return Request.CreateResponse(HttpStatusCode.OK, pracdata);
                }
            }

                if (data.Any())
            {

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "data not fornd");
        }
    }
}

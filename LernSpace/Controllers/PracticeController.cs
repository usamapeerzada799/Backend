using LernSpace.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI;

namespace LernSpace.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class PracticeController : ApiController
    {

        SlowlernerDbEntities db = new SlowlernerDbEntities();
        [HttpPost]
        /*Practic practic, [FromUri] int[] collection*/
        public HttpResponseMessage AddNewPractice([FromBody] PracticeInfo info)
        {
            try
            {
                db.Practice.Add(info.practice);
                db.SaveChanges();


                foreach (var item in info.collections)
                {
                    item.pracId = info.practice.id;


                }
                db.PracticeCollection.AddRange(info.collections);
                db.SaveChanges();
                return Request.CreateErrorResponse(HttpStatusCode.OK, "data save");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.OK, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage userDefindPractices(int Uid, int pid)
        {
            var data = db.Practice.Where(e => e.createBy == Uid)

                .Join(db.PracticeCollection, practic => practic.id, pracCollection => pracCollection.pracId, (practic, pracCollection) => new { practic, pracCollection })
                .Join(db.Collection, ppc => ppc.pracCollection.collectId, collect => collect.id, (ppc, collect) => new
                {

                    collect.id,
                    collect.uText,
                    collect.eText,
                    collect.type,
                    collect.picPath,
                    collect.C_group,
                    collect.audioPath,
                    ppc.practic.title,
                    pracId = ppc.practic.id,
                    flag = false

                }).ToList();
            if (pid != 0)
            {
                var patientAssignedPractices = db.Patient.Where(e => e.pid == pid)
                    .Join(db.Appointment, patient => patient.pid, Appointment => Appointment.patientId, (patient, Appointment) => new { patient, Appointment })
                    .Join(db.AppointmentPractic, Appointment => Appointment.Appointment.id, AppointmentPractice => AppointmentPractice.appointmentId, (PatientAppointment, AppointmentPractic) => new { PatientAppointment, AppointmentPractic })
                    .Join(db.Practice, AppointPractice => AppointPractice.AppointmentPractic.practiceId, Practice => Practice.id, (PatientAppointAppointPractice, Practice) => new
                    {
                        Practice.id
                    }).ToList();
                var patientAssPracData = patientAssignedPractices.Select(item => new PatienrpracticeTestData
                {
                    id = item.id,
                });
                var pracdata = data.Select(item => new PracticeData
                {
                    id = item.id,
                    uText = item.uText,
                    eText = item.eText,
                    type = item.type,
                    picPath = item.picPath,
                    C_group = item.C_group,
                    audioPath = item.audioPath,
                    title = item.title,
                    pracId = item.pracId,
                    flag = false // Assuming you want to set flag to false for all items
                }).ToList();

                foreach (var item in pracdata)
                {
                    if (patientAssPracData.Any(patient => patient.id == item.pracId))
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
            return Request.CreateResponse(HttpStatusCode.OK, "data not found");

        }
    }
}

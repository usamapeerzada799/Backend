using LernSpace.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;

namespace LernSpace.Controllers
{
    public class UserController : ApiController
    {
        SlowlernerDbEntities db= new SlowlernerDbEntities();
        [HttpGet]
        public HttpResponseMessage deleteAppdata(int appid)
        {
            var data=db.Appointment.Where(e=>e.id==appid).FirstOrDefault();
            db.Appointment.Remove(data);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK,"deleted");
        }
        [HttpPost]
        public HttpResponseMessage SignUp()
        {
            var request=HttpContext.Current.Request;
            var name = request["name"];
            var type = request["type"];
            var username= request["username"];
            var password= request["password"];
            var profilePic = request.Files["profilePic"];
            
            var validation=db.User.Where(e=>e.username==username).ToList();
            if(validation.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK,"username Already exist use another username");
            }
            string fileName = username.Split('.')[0] + "." + profilePic.FileName.Split('.')[1];
            profilePic.SaveAs(HttpContext.Current.Server.MapPath("~/Media/UsersImages/" + fileName));
            User user = new User();
            user.username = username;
            user.password = password;
            user.type = type;
            user.name = name;
            user.profPicPath = "/Media/UsersImages/" + fileName;
            db.User.Add(user);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK,"registerd");
        }


        [HttpGet]
        public HttpResponseMessage SignIn(string username,string password)
        {
            var data=db.User.Where(e=>e.username== username && e.password==password).Select(user=> new {user.uid,user.type,user.profPicPath,user.name}).FirstOrDefault();
            if(data==null)
            {
                return Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation,"invalid Username Or Password");
            }
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpGet]
        public HttpResponseMessage updateAppointmentFeedback(int appointid,string feedback)
        {
            var data=db.Appointment.Where(e=>e.id==appointid).FirstOrDefault();
            data.feedback = feedback;
            db.SaveChanges();
            var newData = new { data.id,data.patientId,data.feedback };
            return Request.CreateResponse(HttpStatusCode.OK,newData);
        }
        [HttpGet]
        public HttpResponseMessage GetAppointments(int Did, DateTime date)
        {
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;

            // Truncate time part from the date parameter
            DateTime startDate = date.Date;
            DateTime endDate = startDate.AddDays(1); // Assuming you want appointments for the entire day

            var data = db.Appointment
                          .Where(e => e.userId == Did && e.nextAppointDate >= startDate && e.nextAppointDate < endDate).
                          Join(db.Patient, app => app.patientId, pat => pat.pid, (app, pat) => new 
                          {
                              app.id,
                              app.patientId,
                              app.pracId,
                              app.testId,
                              app.feedback,
                              app.nextAppointDate,
                              pat.profPicPath,
                              pat.stage,
                              pat.name,
                              pat.pid
                          })
                         /* Select(appoint => new {
                              appoint.id,
                              appoint.patientId,
                              appoint.pracId,
                              appoint.testId,
                              appoint.userId,
                              appoint.feedback,
                              appoint.appointmentDate,
                              appoint.nextAppointDate
                          })*/
                          .ToList();

            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [HttpPost]
        public  HttpResponseMessage AddAppointment(Appointment appoint)
        {
            db.Appointment.Add(appoint);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        public HttpResponseMessage showSpacificAppointmentData(int AppointmentId,int pid)
        {
            var TestData = db.Appointment.Where(e => e.id == AppointmentId && e.patientId == pid)
                .Join(db.TestCollection, appoint => appoint.testId, testcollection => testcollection.testId, (appoint, testcollection) => new { appoint, testcollection }).
                Join(db.PatientTestCollectionFeedback, ttc => ttc.testcollection.id, ptcf => ptcf.id, (appointTestCollection, pTestColletFedback) => new { appointTestCollection, pTestColletFedback })
                .Join(db.Collection, ptcf => ptcf.pTestColletFedback.collectionId, collect => collect.id, (all, collect) => new
                {
                    collect.eText,
                    collect.uText,
                    all.pTestColletFedback.feedback,
                    
                   
                });
            var PracticeData = db.Appointment.Where(e => e.id == AppointmentId && e.patientId == pid)
                .Join(db.PracticeCollection, appoint => appoint.pracId, practicecollection => practicecollection.pracId, (appoint, pracCollection) => new { appoint, pracCollection })

                
                .Join(db.Collection, ptcf => ptcf.pracCollection.collectId, collect => collect.id, (all, collect) => new
                {
                    collect.eText,
                    collect.uText,
                    all.appoint.userId
                }).ToList();
            var uid = PracticeData[0].userId;
            var result = new {uid, PracticeData, TestData };
            
            return Request.CreateResponse(HttpStatusCode.OK,result );
        }
        [HttpPost]
        public HttpResponseMessage CaregiverRegisterPatient()
        {
           
            var request=HttpContext.Current.Request;
            var username = request["username"];
            var validation = db.Patient.Where(e => e.userName == username);

            if (validation==null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Username Already exist");
            }

            var name = request["name"];
            var age = int.Parse(request["age"]);
            var gender = request["gender"];
            var stage = int.Parse(request["stage"]);
           
            var password = request["password"]; 
            var profpic = request.Files["profpic"];
            var caregiverid = int.Parse(request["caregiverid"]);
            Patient patient= new Patient(); 
                
            patient.age = age;
            patient.gender = gender;
            patient.name = name;    
            patient.userName = username;
            patient.stage = stage;
            patient.firstTime = true;
            patient.password = password;
           

            
            string fileName = patient.userName.Split('.')[0] + "." + profpic.FileName.Split('.')[1];
            profpic.SaveAs(HttpContext.Current.Server.MapPath("~/Media/PatientsImages/" + fileName));
            patient.profPicPath = "/Media/PatientsImages/" + fileName;
            db.Patient.Add(patient);
            db.SaveChanges();
            UserPatient CaregivePatient = new UserPatient();
            CaregivePatient.userId = caregiverid;
            CaregivePatient.patientId = patient.pid;
            db.UserPatient.Add(CaregivePatient);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "registerd");
        }

    }
}

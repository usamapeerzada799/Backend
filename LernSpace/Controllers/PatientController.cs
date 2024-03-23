using LernSpace.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.ModelBinding;

namespace LernSpace.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PatientController : ApiController
    {

        SlowlernerDbEntities db = new SlowlernerDbEntities();
        [HttpGet]
        public HttpResponseMessage AssignedPractic(int Pid,string filter)
        {
            var data = db.Patient
             .Where(e => e.pid == Pid)
             .Join(db.Appointment, p => p.pid, ap => ap.patientId, (patien, appoint) => new { patien, appoint })
             .Join(db.PracticeCollection, ap => ap.appoint.pracId, pc => pc.pracId, (appoint, pracCollect) => new { appoint, pracCollect })
             .Join(db.Collection, prac => prac.pracCollect.collectId, c => c.id, (pracCollect, collect) => new
             {
                 AppointmentDate = pracCollect.appoint.appoint.id,
                 practiceCollectionId=pracCollect.pracCollect.id,
                 CollectId = collect.id,
                 Path = collect.picPath,
                 Etext = collect.eText,
                 Utext = collect.uText,
                 Group = collect.C_group,
                 Type = collect.type,
             })
             .OrderByDescending(e => e.AppointmentDate)
             .GroupBy(e => e.AppointmentDate)
             .Select(group => new
             {
                 AppointmentDate = group.Key,
                 Collections = group.Select(e => new
                 {  
                     e.practiceCollectionId,
                     e.CollectId,
                     e.Path,
                     e.Etext,
                     e.Utext,
                     e.Group,
                     e.Type,
                 }).ToList()
             });
            // .FirstOrDefault();
            if (filter == "all")
            {
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            
                
            return Request.CreateResponse(HttpStatusCode.OK, data.FirstOrDefault());
        }

        [HttpGet]
        public HttpResponseMessage AssignedTest(int Pid,string filter)
        {
            var data = db.Patient
             .Where(e => e.pid == Pid)
             .Join(db.Appointment, p => p.pid, ap => ap.patientId, (patien, appoint) => new { patien, appoint })
             .Join(db.TestCollection, ap => ap.appoint.testId, tc => tc.testId, (appoint, testcollect) => new { appoint, testcollect })
             .Join(db.Collection, test => test.testcollect.collectId, c => c.id, (testCollect, collect) => new
             {
                 AppointmentDate = testCollect.appoint.appoint.id,
                 testCollectionID= testCollect.testcollect.id,
                 CollectId = collect.id,
                 Path = collect.picPath,
                 Etext = collect.eText,
                 Utext = collect.uText,
                 Group = collect.C_group,
                 Type = collect.type,
                 Opt1 = testCollect.testcollect.op1,
                 Opt2 = testCollect.testcollect.op2,
                 Opt3 = testCollect.testcollect.op3
             })
             .OrderByDescending(e => e.AppointmentDate)
             .GroupBy(e => e.AppointmentDate)
             .Select(group => new
             {
                 AppointmentDate = group.Key,
                 Collections = group.Select(e => new
                 {
                     e.testCollectionID,
                     e.CollectId,
                     e.Path,
                     e.Etext,
                     e.Utext,
                     e.Group,
                     e.Type,
                     e.Opt1,
                     e.Opt2,
                     e.Opt3,


                 }).ToList()
             });
             

            if (filter == "all")
            {
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }


            return Request.CreateResponse(HttpStatusCode.OK, data.FirstOrDefault());

            
        }
        [HttpGet]
        public HttpResponseMessage computeTestResult([FromBody] int[] SelectedAns,int testId,int pid)
        {
           
            
            var data = db.TestCollection.Where(e => e.testId == testId)
                .Join(db.Collection, tc => tc.collectId, coll => coll.id, (testColl, coll) => new 
                {
                    testColl.id,
                    collectId=coll.id
                }).ToList();
            bool[] result=new bool[SelectedAns.Length];
            int count=0;
            foreach (var item in data)
            {
                if(item.collectId== SelectedAns[count])
                {
                    result[count] = true;
                }
                    count++;
            }
            List<PatientTestCollectionFeedback> patientTestCollectionFeedback = new List<PatientTestCollectionFeedback>();
            
            count = 0;
            foreach (var item in data)
            {
                PatientTestCollectionFeedback p = new PatientTestCollectionFeedback();
                p.patientId = pid;
                p.testCollectionId = item.id;
                p.collectionId = item.collectId;
                p.feedback = result[count];
                patientTestCollectionFeedback.Add(p);
                count++;
            }
            db.PatientTestCollectionFeedback.AddRange(patientTestCollectionFeedback);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK,patientTestCollectionFeedback);

        }
    }
}

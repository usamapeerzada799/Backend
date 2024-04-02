using LernSpace.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public HttpResponseMessage userDefindTest(int Uid)
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
                    Op3ImagePath = db.Collection.Where(c => c.id == ppc.testCollection.op3).Select(c => c.picPath).FirstOrDefault()
                });


            if (data.Any())
            {

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            return Request.CreateResponse(HttpStatusCode.OK, "data not fornd");
        }
    }
}

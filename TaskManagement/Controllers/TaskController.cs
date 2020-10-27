using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManagement_DAL;

namespace TaskManagement.Controllers
{
    public class TaskController : ApiController
    {
        [Authorize]
        public IEnumerable<Quote> Get()
        {
            using (TaskManagerEntities entities = new TaskManagerEntities())
            {
                return entities.Quotes.ToList();
            }
        }

        //Update Task by ID
        public HttpResponseMessage Put( int id, [FromBody] Quote quote)
        {
            try
            {
                using (TaskManagerEntities entities = new TaskManagerEntities())
                {
                    var entity = entities.Quotes.FirstOrDefault(q => q.QuoteID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Task with Id " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.QuoteType = quote.QuoteType;
                        entity.Contact = quote.Contact;
                        entity.DueDate = quote.DueDate;
                        entity.Task = quote.Task;
                        entity.TaskType = quote.TaskType;

                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //Delete Task by ID
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (TaskManagerEntities entities = new TaskManagerEntities())
                {
                    var entity = entities.Quotes.FirstOrDefault(q => q.QuoteID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Task with Id = " + id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Quotes.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //Create
        public HttpResponseMessage Post([FromBody] Quote quote)
        {
            try
            {
                using (TaskManagerEntities entities = new TaskManagerEntities())
                {
                    entities.Quotes.Add(quote);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, quote);
                    message.Headers.Location = new Uri(Request.RequestUri +
                        quote.QuoteID.ToString());

                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}



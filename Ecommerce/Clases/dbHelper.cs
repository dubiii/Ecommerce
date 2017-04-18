using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.Models;

namespace Ecommerce.Clases
{
    public class dbHelper
    {
        public static Response saveChanges(EcommerceContext db)
        {
            try
            {
                db.SaveChanges();
                return new Response {Succeded = true};
            }
            catch (Exception ex)
            {
                var response = new Response
                {
                    Succeded = false,
                };
                if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "Hay registros con los mismos valores";
                }else if (ex.InnerException != null && ex.InnerException.InnerException != null && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "Hay registros no se puede borrar porque hay datos relacionados";
                }
                else
                {
                    response.Message = ex.Message;
                }

                return response;
            }
        }

        public static int GetState(string description, EcommerceContext db)
        {
            var state = db.States.Where(s => s.Description == description).FirstOrDefault();
            if (state == null)
            {
                state = new State
                {
                    Description = description
                };
                db.States.Add(state);
                db.SaveChanges();
            }

            return state.StateID;
        }
    }
}
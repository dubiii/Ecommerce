using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ecommerce.Models;

namespace Ecommerce.Clases
{
    public class movementHelper : IDisposable
    {
        private static EcommerceContext db = new EcommerceContext();

        public void Dispose()
        {
            db.Dispose();
        }

        public static Response newOrder(NewOrderView view, string username)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == username).FirstOrDefault();
                    var order = new Order
                    {
                        CompanyID = user.CompanyID,
                        CustomerID = view.CustomerID,
                        Date = view.Date,
                        Remarks = view.Remarks,
                        StateID = dbHelper.GetState("Created", db),

                    };

                    db.Orders.Add(order);
                    db.SaveChanges();

                    var details = db.OrderDetailTemps.Where(o => o.UserName == username).ToList();
                    foreach (var detail in details)
                    {
                        var orderdetail = new OrderDetail
                        {
                            Description = detail.Description,
                            OrderID = order.CompanyID,
                            Price = detail.Price,
                            ProductID = detail.ProductID,
                            Quantity = detail.Quantity,
                            ImpuestoRate = detail.ImpuestoRate
                        };

                        db.OrderDetails.Add(orderdetail);
                        db.OrderDetailTemps.Remove(detail);
                    }

                    db.SaveChanges();
                    transaction.Commit();
                    return new Response {Succeded = true};
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new Response
                    {
                        Message = ex.Message,
                        Succeded = false,
                    };
                }
            }
        }
    }
}
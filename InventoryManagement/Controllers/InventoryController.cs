using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using InventoryManagement.Models;

namespace InventoryManagement.Controllers
{
    public class InventoryController : Controller
    {
        private readonly string cs =
        ConfigurationManager.ConnectionStrings["InventoryConnection"].ConnectionString;


    // READ: Display all products
    public ActionResult Index()
        {
            List<Inventory> products = new List<Inventory>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Inventory", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    products.Add(new Inventory
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Price = Convert.ToDecimal(dr["Price"])
                    });
                }
            }

            return View(products);
        }

        // CREATE: Display form
        public ActionResult Create()
        {
            return View();
        }

        // CREATE: Insert product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inventory item)
        {
            if (!ModelState.IsValid)
                return View(item);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Inventory
                (ProductName, Quantity, Price)
                VALUES (@name, @qty, @price)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@name", SqlDbType.VarChar, 100)
                    .Value = item.ProductName;
                cmd.Parameters.Add("@qty", SqlDbType.Int)
                    .Value = item.Quantity;
                cmd.Parameters.Add("@price", SqlDbType.Decimal)
                    .Value = item.Price;

                cmd.Parameters["@price"].Precision = 10;
                cmd.Parameters["@price"].Scale = 2;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // UPDATE: Display edit form
        public ActionResult Edit(int id)
        {
            Inventory item = GetProduct(id);

            if (item == null)
                return HttpNotFound();

            return View(item);
        }

        // UPDATE: Save changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inventory item)
        {
            if (!ModelState.IsValid)
                return View(item);

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE Inventory
                SET ProductName = @name,
                    Quantity = @qty,
                    Price = @price
                WHERE ProductID = @id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.Add("@name", SqlDbType.VarChar, 100)
                    .Value = item.ProductName;
                cmd.Parameters.Add("@qty", SqlDbType.Int)
                    .Value = item.Quantity;
                cmd.Parameters.Add("@price", SqlDbType.Decimal)
                    .Value = item.Price;

                cmd.Parameters["@price"].Precision = 10;
                cmd.Parameters["@price"].Scale = 2;

                cmd.Parameters.Add("@id", SqlDbType.Int)
                    .Value = item.ProductID;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // DELETE: Display confirmation
        public ActionResult Delete(int id)
        {
            Inventory item = GetProduct(id);

            if (item == null)
                return HttpNotFound();

            return View(item);
        }

        // DELETE: Remove product
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Inventory WHERE ProductID = @id", con);

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // Retrieve one product
        private Inventory GetProduct(int id)
        {
            Inventory item = null;

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Inventory WHERE ProductID = @id", con);

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    item = new Inventory
                    {
                        ProductID = Convert.ToInt32(dr["ProductID"]),
                        ProductName = dr["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Price = Convert.ToDecimal(dr["Price"])
                    };
                }
            }

            return item;
        }
    }


}

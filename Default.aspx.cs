using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

//References: CodAffection, Youtube, C# Asp.Net Gridview- Insert Update and Delete With SQL Server
//https:www.youtube.com/watch?v=vuoJeQ4L3Q4L3Wl&ab_channel=CodAffection
//https://abc20104675.azurewebsites.net
namespace ABC
{
    public partial class Default : System.Web.UI.Page
    {
        //Connecting the Azure database to the Application
        string connectionString = "Server=tcp:dbsvccldv621220104675.database.windows.net,1433;" +
            "Initial Catalog=DB_VC_CLDV6212_20104675;Persist Security Info=False;User ID=kfcldv20104675;Password=Alice@321;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        protected void Page_Load(object sender, EventArgs e)
        {
            //If statement to populate gridview
            if (!IsPostBack)
            {
                PopulateGridview();
            }
        }
        //Class to populate gridview
        void PopulateGridview()
        {
            DataTable dtbl = new DataTable();
            //connection string
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                //All information of Data retrieved from database
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Product", sqlCon);
               //fill gridview with database information
                sqlDa.Fill(dtbl);
            }
            if (dtbl.Rows.Count > 0)
            {
                gvABC.DataSource = dtbl;
                gvABC.DataBind();
            }
            else
            {
                //If there is no data to fill gridview
                dtbl.Rows.Add(dtbl.NewRow());
                gvABC.DataSource = dtbl;
                //problem
                gvABC.DataBind();
                gvABC.Rows[0].Cells.Clear();
                gvABC.Rows[0].Cells.Add(new TableCell());
                gvABC.Rows[0].Cells[0].ColumnSpan = dtbl.Columns.Count;
                gvABC.Rows[0].Cells[0].Text = "No Data Found ..!";
                gvABC.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        //To addnew item to table
        protected void gvABC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        string query = "INSERT INTO Product (Name,Details,Price) VALUES (@Name,@Details,@Price)";
                        SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                        sqlCmd.Parameters.AddWithValue("@Name", (gvABC.FooterRow.FindControl("txtNameFooter") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Details", (gvABC.FooterRow.FindControl("txtDetailsFooter") as TextBox).Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Price", (gvABC.FooterRow.FindControl("txtPriceFooter") as TextBox).Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        PopulateGridview();
                        lblSuccessMessage.Text = "New Record Added";
                        lblErrorMessage.Text = "";
                    }


                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }
        //editing items in table
        protected void gvABC_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvABC.EditIndex = e.NewEditIndex;
            PopulateGridview();
        }
        //to cancell editing items
        protected void gvABC_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvABC.EditIndex = -1;
            PopulateGridview();
        }
            //updating the row
        protected void gvABC_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {

                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {

                    sqlCon.Open();
                    string query = "UPDATE Product SET Name=@Name,Details=@Details,Price=@Price WHERE id = @id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Name", (gvABC.Rows[e.RowIndex].FindControl("txtName") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Details", (gvABC.Rows[e.RowIndex].FindControl("txtDetails") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@Price", (gvABC.Rows[e.RowIndex].FindControl("txtPrice") as TextBox).Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@id",Convert.ToInt32(gvABC.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    gvABC.EditIndex = -1;
                    PopulateGridview();
                    lblSuccessMessage.Text = "Selected Record Updated";
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }
            //Delete a row
        protected void gvABC_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionString))
                {
                    sqlCon.Open();
                    string query = "DELETE FROM Product WHERE ID = @ID";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@ID", Convert.ToInt32(gvABC.DataKeys[e.RowIndex].Value.ToString()));
                    sqlCmd.ExecuteNonQuery();
                    PopulateGridview();
                    lblSuccessMessage.Text = "Selected Record Deleted";
                    lblErrorMessage.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblSuccessMessage.Text = "";
                lblErrorMessage.Text = ex.Message;
            }
        }


        protected void gvABC_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
       
    }
}
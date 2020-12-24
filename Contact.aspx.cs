using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace ASP.NET_CRUD
{
    public partial class Contact : System.Web.UI.Page
    {
        SqlConnection sqlcon = new SqlConnection(@"Data Source =.; Integrated Security = true; Initial Catalog = AspDotNetCrud"); 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDelete.Enabled = false;
                FillGridView();
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            hfContactId.Value = "";
            txtName.Text = txtMobile.Text = txtAddress.Text = "";
            lblSuccessMessage.Text = lblErrorMessage.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("ContactCreateorUpdate", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ContactID", (hfContactId.Value ==""?0:Convert.ToInt32(hfContactId.Value)));
                sqlcmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();
                Clear();
                string contactId = hfContactId.Value;
                if (contactId == "")
                {
                    lblSuccessMessage.Text = "Saved Successfully";
                }
                else{
                    lblErrorMessage.Text = "Updated Successfully";
                }
                FillGridView();

            }
        }

        void FillGridView()
        {
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("ContactCreateorUpdate", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDa = new SqlDataAdapter("contactViewAll", sqlcon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                sqlcon.Close();
                gvContact.DataSource = dtbl;
                gvContact.DataBind(); 

            }
        }

        protected void lnk_OnClick(object sender, EventArgs e)
        {
            int contactID = Convert.ToInt32((sender as LinkButton).CommandArgument);
            if (sqlcon.State == ConnectionState.Closed)
            {
                sqlcon.Open();
                SqlCommand sqlcmd = new SqlCommand("contactViewByID", sqlcon);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@ContactID", contactID);
                SqlDataAdapter sqlDa = new SqlDataAdapter("contactViewAll", sqlcon);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                sqlcon.Close();
                hfContactId.Value = contactID.ToString();
                txtName.Text = dtbl.Rows[0]["Name"].ToString();
                txtMobile.Text = dtbl.Rows[0]["Name"].ToString();
                txtAddress.Text = dtbl.Rows[0]["Address"].ToString();
                btnSave.Text = "Update";
                btnDelete.Enabled = true; 
            }
        }
    }
}
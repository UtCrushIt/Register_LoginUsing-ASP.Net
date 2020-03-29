using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void submit_Click(object sender, EventArgs e)
    {
            String err = "";
            if (upass.Text.Length != 8)
            {
                err = "Password Should Be Of Length 8.";
            }
            else
            {
                OleDbConnection conn = new OleDbConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["data"].ConnectionString;
                OleDbCommand cmd = new OleDbCommand();

                cmd.CommandText = "SELECT count(*) FROM [USERS] WHERE UEMAIL = '" + uname.Text + "' and UPASS='" + upass.Text + "'";

                cmd.Connection = conn;
                try
                {

                    conn.Open();
                    string a = cmd.ExecuteScalar().ToString();
                    if (a == "0")
                    {
                        err = "You Are Not A Registered User Or Wrong Login Credentials.";
                    }
                    else
                    {
                        
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert(' Congratulations ! Login Successfull.')", true);
                        //Response.Redirect("~/Login.aspx");
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    err = ex.Message + ex.StackTrace;
                }
            }
        uname.Text = "";
        error.Text = err;

     
    }

    protected void forpass_Click(object sender, EventArgs e)
    {
        String err = "";
        if (uname.Text == "")
            err = "Please Provide Email Id.";
        else
        {
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["data"].ConnectionString;
            OleDbCommand cmd = new OleDbCommand();

            cmd.CommandText = "SELECT UPASS FROM [USERS] WHERE UEMAIL = '" + uname.Text + "'";

            cmd.Connection = conn;
            try
            {

                conn.Open();
                
                object a= cmd.ExecuteScalar();
                if (a==null)
                {
                    err = "This Email Is Not Registered With Us.";
                }
                else
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress("kumarrahul.allduniv@gmail.com");
                    message.To.Add(new MailAddress(uname.Text));
                    message.Subject = "Password Recovery For WWW.MYWEB.COM .";
                    message.IsBodyHtml = false; //to make message body as html  
                    message.Body = "We Have Received A Forgot Password Request Linked To This Email Id On WWW.MYWEB.COM.\n Here Is Your Password '" + a.ToString() + "'.\n Please Don't Share With Anyone.\n Thanks.";
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com"; //for gmail host  
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential("kumarrahul.allduniv@gmail.com", "Rm@1749001");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                    err = "A Mail Has Been Sended To Registered Email Id With The Needed Information.";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                err = ex.Message + ex.StackTrace;
            }
            error.Text = err;
        }
    }

}
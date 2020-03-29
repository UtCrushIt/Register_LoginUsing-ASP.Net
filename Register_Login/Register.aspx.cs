using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
    public static Regex regex = new Regex(
      "^(\\b[A-Za-z]*\\b(\\s+\\b[A-Za-z]*\\b)*(\\.[A-Za-z])?)$",
    RegexOptions.IgnoreCase
    | RegexOptions.CultureInvariant
    | RegexOptions.IgnorePatternWhitespace
    | RegexOptions.Compiled
    );

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    
    protected void register_Click(object sender, EventArgs e)
    {
        String err = "";
        if(regex.Match(uname.Text).Success)
        {
            if(Regex.Match(uphone.Text, "(0/91)?[6-9][0-9]{9}").Success && uphone.Text.Length==10)
            {
                if(ug.SelectedIndex==-1)
                {
                    err = "No Gender Selected.";
                }
                else
                {
                    if(upass.Text.Length!=8)
                    {
                        err = "Password Should Be Of Length 8.";
                    }

                    else
                    {
                        if (upass.Text.Equals(cupass.Text))
                        {
                            OleDbConnection conn = new OleDbConnection();
                            conn.ConnectionString = ConfigurationManager.ConnectionStrings["data"].ConnectionString;
                            OleDbCommand cmd = new OleDbCommand();

                            cmd.CommandText = "SELECT count(*) FROM [USERS] WHERE UEMAIL = '" + uemail.Text + "' or UPHONE='"+uphone.Text+"'";

                            cmd.Connection = conn;
                            try
                            {

                                conn.Open();
                                string a = cmd.ExecuteScalar().ToString();
                                if(a!="0")
                                {
                                    err = "User Already Registered With This Email or Phone.";
                                }
                                else
                                {
                                    cmd.CommandText = "insert into [USERS] values( '"+uname.Text+"','"+uemail.Text+"','"+upass.Text+"','"+uphone.Text+"','"+udob.Text+"','"+ug.SelectedValue.ToString()+"')";
                                    if (cmd.ExecuteNonQuery() > 0)
                                        err = "Registration Successful.";
                                    else
                                        err = "Can't Register Now Try Again.";
                                }
                            }
                            catch (Exception ex)
                            {
                                err = ex.Message+ex.StackTrace;
                            }
                            }
                        else
                        {
                            err = "Password And Confirm Passwords Are Not Same.";
                        }
                    }
                }
            }
            else
            {
                err = "Mobile Number Is Invalid.";
            }
        }
        else
        {
            err = "Name Is Invalid.";
        }
        uname.Text = "";
        uemail.Text = "";
        udob.Text = "";
        uphone.Text = "";
        upass.Text = "";
        cupass.Text = "";
        ug.SelectedIndex = -1;
        error.Text = err;
    }
}

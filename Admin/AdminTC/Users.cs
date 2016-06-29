using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AdminTC
{
    public partial class Users : UserControl,IUserControl
    {
        UserAttribute ua;
        bool edit = false;
        DataSet dsUsers;
        //DataView dvUsers;
        private static Users singleton_users = null;
       
        public Users()
        {
            InitializeComponent();
            RefreshDataset();
            
            if (singleton_users == null)
                singleton_users = this;
            //dsUsers = frmAdmin.GetSingletonAdminform().dsUsers;
        }

        public static Users GetSingletonUsers()
        {
            return singleton_users;
        }

        public void RefreshDataset()
        {
            //frmAdmin.GetSingletonAdminform().RefreshDataset();
            dsUsers = frmAdmin.GetSingletonAdminform().dsUsers;
        }

        public void ShowDetails(int id)
        {
            DataRow dr =  dsUsers.Tables[0].Rows.Find(id);
            UserAttribute ua = new UserAttribute();
            ua.id = (int)dr["ID"];
            ua.emailid = (string)dr["EMAILID"]; ;
            ua.active = (string)dr["ACTIVES"] == "T" ? true : false;
            ua.loginID = (string)dr["LOGINID"];
            ua.phoneno = (string)dr["PHONENO"];
            ua.username = (string)dr["USERNAME"];
            if (DBNull.Value != dr["VERSION"])
                ua.version = (string)dr["VERSION"];
           
            DisplayRecords(ua);
        }

        private void DisplayRecords(UserAttribute ua)
        {
            this.ua = ua;
            txtLoginName.Text = ua.username;;
            txtContactNo.Text = ua.phoneno;
            txtEmailID.Text = ua.emailid;
            txtLoginID.Text = ua.loginID;
            txtVersion.Text = ua.version;
            cmbActive.SelectedIndex = ua.active == true ? 0 : 1;
        }
        private void DisableForm()
        {
            txtContactNo.Enabled = false;
            txtEmailID.Enabled = false;
            txtLoginID.Enabled = false;
            txtLoginName.Enabled = false;
            cmbActive.Enabled = false;
            txtVersion.Enabled = false;
        }
        private void EnableForm()
        {
            txtContactNo.Enabled = true;
            txtEmailID.Enabled = true;
            txtLoginID.Enabled = true;
            txtLoginName.Enabled = true;
            cmbActive.Enabled = true;
            txtVersion.Enabled = true;
        }
        private void ClearForm()
        {
            txtContactNo.Text = "";
            txtEmailID.Text = "";
            txtLoginID.Text = "";
            txtLoginName.Text = "";
            cmbActive.SelectedIndex = 0;
            txtVersion.Text = "";
        }
        private new bool Validate()
        {
            string msgSting = "Mandatory Fields:";
            string dataError = "Validation Error:";
            if (txtLoginName.Text == "")
                msgSting = msgSting + Environment.NewLine + "Login name";
            if (txtLoginID.Text == "")
                msgSting = msgSting + Environment.NewLine + "Login ID";
            else
            {
                if (txtLoginID.Text.Trim().Length < 3)
                {
                    dataError = dataError + Environment.NewLine + "LoginID should be minimum 3 characters long";
                }
                if (txtLoginID.Text.Trim().Length > 50)
                {
                    dataError = dataError + Environment.NewLine + "LoginID should be maximum 15 characters long";
                }
                if (txtLoginID.Text.Trim().IndexOf(" ") >= 0)
                {
                    dataError = dataError + Environment.NewLine + "LoginID should not contain any spaces";
                }
            }
            if (txtEmailID.Text == "")
                msgSting = msgSting + Environment.NewLine + "Email ID";
            else
            {
                string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(strRegex);
                if (!re.IsMatch(txtEmailID.Text.Trim()))
                    dataError = dataError + Environment.NewLine + "Invalid Email ID";
            }
            if (msgSting != "Mandatory Fields:")
            {
                MessageBox.Show(msgSting,"TC Admin");
                return false;
            }
            else if (dataError != "Validation Error:")
            {
                MessageBox.Show(dataError, "TC Admin");
                return false;
            }
            else
                return true;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            btnNew.Enabled = false;
            btnDelete.Enabled = false;
            EnableForm();
            ClearForm();
            frmAdmin.GetSingletonAdminform().DG_Enabled = false;
            txtLoginID.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnEdit.Enabled = false;
            btnSave.Enabled = true;
            btnNew.Enabled = false;
            btnDelete.Enabled = false;
            EnableForm();
            frmAdmin.GetSingletonAdminform().DG_Enabled = false;
            edit = true;
            txtLoginID.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Validate())
            {
                int i = 0;
                try
                {
               
                    WSScalper.UserRow u = new WSScalper.UserRow();
                    u.LoginId = txtLoginID.Text.Trim();
                    u.EmailId = txtEmailID.Text.Trim();
                    u.Active = cmbActive.SelectedIndex == 0 ? true : false;
                    u.LoggedIn = false;
                    u.PhoneNo = txtContactNo.Text.Trim();
                    u.Username = txtLoginName.Text.Trim();
                    
                    WSScalper.WebServicesScalper ws = new WSScalper.WebServicesScalper();

                    if (!edit)
                    {
                        u.Password = "shark123";//deafault password for the new user
                        i = ws.CreateUser(u);
                        frmAdmin.GetSingletonAdminform().RefreshDataset(true);
                    }
                    else
                    {
                        u.Id = ua.id;
                        i = ws.EditUsers(u);
                        frmAdmin.GetSingletonAdminform().RefreshDataset(true);
                    }
                }
                catch (Exception ex)
                {
                    //if(ex.Message = "Unable to connect the remote server")
                    MessageBox.Show(ex.Message,"TC Admin");
                    //Application.Exit();
                    return;
                }

                if (i >= 1)
                {

                    if (ua == null) ua = new UserAttribute();
                    
                    ua.emailid = txtEmailID.Text.Trim();
                    ua.active = cmbActive.SelectedIndex == 0 ? true : false;
                    ua.loginID = txtLoginID.Text.Trim();
                    ua.phoneno = txtContactNo.Text.Trim();
                    ua.username = txtLoginName.Text.Trim();
                    

                    btnCancel.Enabled = false;
                    btnEdit.Enabled = true;
                    btnNew.Enabled = true;
                    btnSave.Enabled = false;
                    btnDelete.Enabled = true;
                    if (edit)
                    {
                        MessageBox.Show("Modified Successfully","TC Admin");
                        DisplayRecords(ua);

                    }
                    else
                    {
                        ua.id = i;
                        MessageBox.Show("Created Successfully","TC Admin");
                     }
                   
                    //frmAdmin.GetSingletonAdminform().RefreshGrid();
                    //RefreshDataset();

                    //refreah the dataset
                    //dsUsers = frmAdmin.GetSingletonAdminform().dsUsers;
                   
                    frmAdmin.GetSingletonAdminform().DG_Enabled = true;
                   
                    //frmAdmin.GetSingletonAdminform().DG_SelectRow(ua.id);
               
                    
                   
                    
                    DisableForm();
                    edit = false;
                }
                else if (i == -3)
                    MessageBox.Show("Login ID or Email ID already exist","TC Admin");
                else if (i == -4)
                    MessageBox.Show("Login Creation Failed","TC Admin");
            }
        }

        /*
        public void Update()
        {
            //frmAdmin.GetSingletonAdminform().RefreshGrid();
            RefreshDataset();

            //refreah the dataset
            dsUsers = frmAdmin.GetSingletonAdminform().dsUsers;

            frmAdmin.GetSingletonAdminform().DG_Enabled = true;
            frmAdmin.GetSingletonAdminform().DG_SelectRow(ua.id);
        }*/

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DisplayRecords(ua);
            btnCancel.Enabled = false;
            btnEdit.Enabled = true;
            btnSave.Enabled = false;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            DisableForm();
            frmAdmin.GetSingletonAdminform().DG_Enabled = true;
            edit = false;
        }

        private void Users_Load(object sender, EventArgs e)
        {
            DisableForm();
            btnCancel.Enabled = false;
            btnEdit.Enabled = true;
            btnNew.Enabled = true;
            btnSave.Enabled = false;
        }

        private void picRefresh_Click(object sender, EventArgs e)
        {
            frmAdmin.GetSingletonAdminform().RefreshDataset();
            frmAdmin.GetSingletonAdminform().DG_SelectRow(ua.id);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete user " + ua.username + " ?",
                "TC Admin", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                WSScalper.WebServicesScalper ws = new WSScalper.WebServicesScalper();
                int res = ws.CheckDependency(ua.loginID);
                if(res == 1)
                     result = MessageBox.Show("There are orders executed by user: " + ua.username + ". Are you sure you want to delete?",
                "TC Admin", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                 if (result == DialogResult.Yes || res == 2)
                 {

                     res = ws.DeleteUser(ua.id);
                     if (res > 0)
                     {
                         frmAdmin.GetSingletonAdminform().RefreshDataset();
                         frmAdmin.GetSingletonAdminform().ShowDefault();
                         MessageBox.Show("User " + ua.username + " deleted succesfully ", "TC Admin");

                     }
                     else
                     {
                         MessageBox.Show("Problem in deletion please try later", "TC Admin");
                     }
                 }
            }
     
        }
    }
}

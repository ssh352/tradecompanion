using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace AdminTC
{
     
    
    public partial class frmAdmin : Form
    {
        private static frmAdmin singleton_frmAdmin = null;
        //Users users = null;
        //Orders orders = null;
        public DataSet dsUsers;
        private bool userForm;

        DataTable dtYesNO;
        DataTable dtTrueFasle;
        IUserControl iUserControls;
    
        
        public frmAdmin()
        {
            InitializeComponent();

            dgDetails.MultiSelect = false;
            dgDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgDetails.ReadOnly = true;
            if (singleton_frmAdmin == null)
                singleton_frmAdmin = this;

            dtTrueFasle = new DataTable("TrueFalse");
            dtTrueFasle.Columns.Add("Description", typeof(String));
            dtTrueFasle.Columns.Add("Value", typeof(String));
            object[] ob = new object[2];
            ob[0] = "True";
            ob[1] = "T";
            dtTrueFasle.Rows.Add(ob);

            ob[0] = "False";
            ob[1] = "F";
            dtTrueFasle.Rows.Add(ob);

            ColumnActive.DataSource = dtTrueFasle.DefaultView;
            ColumnActive.DisplayMember = "Description";
            ColumnActive.ValueMember = "Value";
            
            
            dtYesNO = new DataTable("YesNo");
            dtYesNO.Columns.Add("Description", typeof(String));
            dtYesNO.Columns.Add("Value", typeof(String));
            ob[0] = "Yes";
            ob[1] = "Y";
            dtYesNO.Rows.Add(ob);

            ob[0] = "No";
            ob[1] = "N";
            dtYesNO.Rows.Add(ob);

            ColumnLoggedIn.DataSource = dtYesNO.DefaultView;
            ColumnLoggedIn.DisplayMember = "Description";
            ColumnLoggedIn.ValueMember = "Value";

            btnOrders.Click += new EventHandler(this.btnUsers_Click);
            userForm = false;
            btnPLTrade.Click += new EventHandler(this.btnUsers_Click);
          
        
        }

        public static frmAdmin GetSingletonAdminform()
        {
            return singleton_frmAdmin;
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            //show the user as default
            iUserControls = new Users();
            panelFormDisplay.Controls.Clear();
            panelFormDisplay.Controls.Add((System.Windows.Forms.UserControl)iUserControls);
            RefreshDataset();
        }

  

        public void RefreshDataset(bool userForm)
        {
            this.userForm = userForm;
            panelGrid.Controls.Clear();
            panelGrid.Controls.Add(spinningProgress1);
            AsyncCallWebServices();
        }
        public void RefreshDataset()
        {
            panelGrid.Controls.Clear();
            panelGrid.Controls.Add(spinningProgress1);
            AsyncCallWebServices();
        }

        public void AsyncCallWebServices()
        {
            try
            {
                WSScalper.WebServicesScalper wsServ = new WSScalper.WebServicesScalper();

                wsServ.GetUsersDSCompleted +=new AdminTC.WSScalper.GetUsersDSCompletedEventHandler(wsServ_GetUsersDSCompleted);
                wsServ.GetUsersDSAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TC Admin");
                return;
            }
        }

        // CallBack function
        public void wsServ_GetUsersDSCompleted(object sender, WSScalper.GetUsersDSCompletedEventArgs args)
        {
            
            if (args.Error != null)
                return;

            dsUsers = args.Result;
            panelGrid.Controls.Clear();
            panelGrid.Controls.Add(dgDetails);

            this.dgDetails.AutoGenerateColumns = false;
            this.dgDetails.DataSource = dsUsers.Tables[0].DefaultView;
            iUserControls.RefreshDataset();

            
            //Show Default if User Opened
            if(iUserControls.GetType().Name == "Users")
                ShowDefault();
            userForm = false;
        }


        
        private void dgDetails_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewSelectedRowCollection dc = this.dgDetails.SelectedRows;
                DataGridViewRow dvr = dc[0];
                int id = (int)dvr.Cells["ColumnID"].Value;
                iUserControls.ShowDetails(id);
            }
            catch//(Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        

        public void ShowDefault()
        {
            try
            {
                
                DataGridViewSelectedRowCollection dc = this.dgDetails.SelectedRows;
                DataGridViewRow dvr = dc[0];
                if (!userForm)
                {
                    int id = (int)dvr.Cells["ColumnID"].Value;
                    iUserControls.ShowDetails(id);
                    DG_SelectRow(id);
                }
                else
                    Users.GetSingletonUsers().Update();
            }
            catch
            {

            }
        }

        public int GetDefaultID()
        {

            try
            {
                DataGridViewSelectedRowCollection dc = this.dgDetails.SelectedRows;
                DataGridViewRow dvr = dc[0];
                return (int)dvr.Cells["ColumnID"].Value;
            }
            catch
            {
                return -1;
                //Exception
            }

        }

        public bool DG_Enabled
        {
            set
            {
                this.dgDetails.Enabled = value;
            }
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            RefreshDataset();
            
            Button button = (Button)sender;
            if (button.Name == "btnUsers")
                iUserControls = new Users();
            else if (button.Name == "btnOrders")
                iUserControls = new Orders();
            else if (button.Name == "btnPLTrade")
                iUserControls = new PLTrade();

            panelFormDisplay.Controls.Clear();
            panelFormDisplay.Controls.Add((System.Windows.Forms.UserControl)iUserControls);
            iUserControls.RefreshDataset();
        }

        public void DG_SelectRow(int id)
        {
            dgDetails.ClearSelection();
            int count = 0;
            foreach (DataGridViewRow dgvr in dgDetails.Rows)
            {
                
                if ((int)dgvr.Cells["ColumnID"].Value == id)
                {
                    dgvr.Selected = true;
                    dgDetails.FirstDisplayedScrollingRowIndex = count;
                    dgDetails.CurrentCell = dgvr.Cells[1];
                    break;
                }
                count++;
            }
        }

    }
}
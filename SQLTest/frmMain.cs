using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SQLTest
{
    public partial class frmMain : Form
    {
        Table _table;
        bool enabled = true;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _table = new Table();
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            TestConnection();

            FillFieldsList();
            FillAllData();
        }

        private void btnShowGroup_Click(object sender, EventArgs e)
        {
            ShowGrupTable();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearGroup();
            FillAllData();
        }



        private void ClearGroup()
        {
            foreach (ListViewItem item in lvFields.Items)
            {
                item.Checked = false;
            }
        }

        private void FillFieldsList()
        {
            lvFields.Items.Clear();
            foreach (var field in _table.Fields)
            {
                if (field.type != typeof(string) && field.type != typeof(DateTime))
                    continue;
                lvFields.Items.Add(field.Id, 0);
            }
        }

        private void TestConnection()
        {
            using (DataDriver dDriver = new DataDriver(_table))
            {
                enabled = dDriver.TestConnection();
            }
        }


        private void FillAllData()
        {
            if (!enabled)
                return;

            dgvData.DataSource = null;

            using (DataDriver dDriver = new DataDriver(_table))
            {
                var dtResult = dDriver.GetAllTable();
                dgvData.DataSource = dtResult;
            }
        }

        private void ShowGrupTable()
        {
            if (!enabled)
                return;

            dgvData.DataSource = null;

            List<Field> groupFields = new List<Field>();
            foreach (ListViewItem item in lvFields.Items)
            {
                if (item.Checked)
                    groupFields.Add(_table.Fields.Find(x => x.Id.Equals(item.Text))); //т.к. у нас единственное свойство id везде, можем себе позволить искать по тексту
            }

            using (DataDriver dDriver = new DataDriver(_table))
            {
                var dtResult = dDriver.GetGroupTable(groupFields);
                dgvData.DataSource = dtResult;
            }
        }

    }
}

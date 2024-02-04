using Rina.Client.Source_files;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Management;
using System.IO;
using System.Security.Cryptography;

namespace Rina.Client
{
    public partial class pForm : MetroFramework.Forms.MetroForm
    {
        public pForm()
        {
            Globals.pForm = this;
            InitializeComponent();
        }

        private void pForm_Shown(object sender, EventArgs e)
        {
            Functions.LoadLocalSettings();
            Functions.CheckPath();

            Networking.CheckNetwork();
        }

        private void pForm_Load(object sender, EventArgs e)
        {
          
        }

        private void completeProgressText_Click(object sender, EventArgs e)
        {

        }

        private void currentProgressText_Click(object sender, EventArgs e)
        {

        }

        private void currentProgress_Click(object sender, EventArgs e)
        {

        }
    }
}

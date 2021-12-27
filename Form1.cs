using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;

namespace BombCryptoBot
{
    public partial class Form1 : Form
    {
        public static System.Diagnostics.Process proc;

        public static DateTime? limit;

        public static System.Windows.Forms.Timer aTimer;



        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            proc = new Process();
            proc.StartInfo.FileName = "CMD.exe";
            proc.StartInfo.Arguments = "/k python index.py";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            proc.Start();
            //proc.WaitForExit();
            button1.Enabled = false;
            button2.Enabled = true;

            int hours = 0;
            try 
            {
                hours = Int32.Parse(comboBox1.SelectedItem.ToString());
                limit = DateTime.Now.AddHours(hours);
                label2.Text = limit.ToString();
            }
            catch (Exception)
            {
                limit = DateTime.MaxValue;
                label2.Text = limit.ToString();
            }


            

            aTimer = new System.Windows.Forms.Timer();
            aTimer.Tick += TimerEventProcessor;
            aTimer.Interval = 60000;
            aTimer.Enabled = true;

            this.WindowState = FormWindowState.Minimized;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
            label2.Text = "";
            aTimer.Stop();
            KillProcess();
        }


        private void TimerEventProcessor(object source, EventArgs e)
        {
            if (DateTime.Now > limit)
            {
                KillProcess();
                button1.Enabled = true;
                button2.Enabled = false;
                label2.Text = "";
                this.WindowState = FormWindowState.Normal;

            }
        }

        private void KillProcess()
        {
            proc.Kill();
            limit = null;

            Process[] p = Process.GetProcesses();

            foreach (Process process in Process.GetProcesses().Where(p =>
                                                        p.ProcessName.Contains("python")))
            {
                process.Kill();
            }



        }

        protected override void OnClosed(EventArgs e)
        {
            if(aTimer != null)
                aTimer.Stop();

            if(proc != null)
                KillProcess();

            base.OnClosed(e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

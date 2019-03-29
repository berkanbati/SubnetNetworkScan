using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Threading;
using System.Net;
using System.Management;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace Subnet_Scanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label6.ForeColor = Color.Red;
            label6.Text = "NULL";
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        Thread myThread = null;

        public void scanner(string subnet)
        {
            Ping myPing;
            PingReply reply;
            IPAddress addr;
            IPHostEntry host;

            progressBar1.Maximum = 254;
            progressBar1.Value = 0;
            listView1.Items.Clear();
            int count2 = 0;

            for (int i = 0; i < 255; i++)
            {
                string subnetv2 = "." + i.ToString();
                myPing = new Ping();
                reply = myPing.Send(subnet + subnetv2, 900);
                label6.ForeColor = Color.Green;
                label6.Text = "Scan.." + subnet + subnetv2;


                if (reply.Status == IPStatus.Success)
                {
                    try
                    {
                        addr = IPAddress.Parse(subnet + subnetv2);
                        host = Dns.GetHostEntry(addr);
                        listView1.Items.Add(new ListViewItem(new String[] { subnet + subnetv2, host.HostName, "Ready" }));
                        listView2.Items.Add(new ListViewItem(new String[] { subnet + subnetv2, host.HostName, "Online" }));

                    }
                    catch
                    {
                        if (i > 1)
                        {
                            listView2.Items.Add(new ListViewItem(new String[] { subnet + subnetv2, "Unknown", "Online" }));
                        }
                    }
                    if (i > 1)
                    {
                        label7.Text = listView2.Items.Count.ToString();
                    }

                }
                progressBar1.Value += 1;
            }

            start.Enabled = true;
            maskedTextBox1.Enabled = false;
            label6.Text = "FINISH";
            int count = listView1.Items.Count;
            MessageBox.Show("Scan Finished!\n " + label7.Text.ToString() + " Host Find.", "Finish", MessageBoxButtons.OK, MessageBoxIcon.Information);
            maskedTextBox1.Text = "";

        }

        private void start_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            label7.Text = "";
            listView1.Items.Clear();

            if (maskedTextBox1.Text.Trim() == "")
            {
                MessageBox.Show("You didn't give Ip Adress", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                myThread = new Thread(() => scanner(maskedTextBox1.Text));
                myThread.Start();

                if (myThread.IsAlive == true)
                {
                    stop.Enabled = true;
                    start.Enabled = false;
                    maskedTextBox1.Enabled = false;
                }
            }
        }

        private void stop_Click(object sender, EventArgs e)
        {
            myThread.Suspend();
            start.Enabled = true;
            stop.Enabled = false;
            maskedTextBox1.Enabled = true;
            label6.ForeColor = System.Drawing.Color.Red;
            label6.Text = "NULL";
            maskedTextBox1.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

    }
}

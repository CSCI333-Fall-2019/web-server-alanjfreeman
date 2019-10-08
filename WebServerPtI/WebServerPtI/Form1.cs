using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            Form main;

            string response = String.Empty;

            // Start the main object's listener
            main.listener = new TcpListener(IPAddress.Any, main.port);
            main.listener.Start();

            while (main.IsRunning)
            {
                try
                {
                    // Each new request to the listener gets a TcpClient
                    TcpClient client = main.listener.AcceptTcpClient();
                    // Send the TcpClient to its own thread to process
                    ThreadPool.QueueUserWorkItem(GetRequestedItem, client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error received: " + ex.Message);
                }
            }

            ThreadPool.QueueUserWorkItem(FunctionToCall());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void FunctionToCall()
        {
            Thread.Sleep(5000);
        }
    }
    // Threadsafe IsRunning flag to keep track of running state
    private bool _isRunning;
    static readonly object _isRunningLock = new object();
    public bool isRunning()
    {
        get
            {
            lock (_isRunningLock)
            {
                return this._isRunning;
            }
        }
        set
            {
            lock (_isRunningLock)
            {
                this._isRunning = value;
            }
            button1.Enabled = !value;
            button2.Enabled = ValueType;
            
        }
    }
}

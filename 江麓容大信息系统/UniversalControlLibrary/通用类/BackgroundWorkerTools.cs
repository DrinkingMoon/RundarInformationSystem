using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace UniversalControlLibrary
{
    public class BackgroundWorkerTools
    {
        static string m_strMsg = "";

        public static BackgroundWorker GetWorker(string msg)
        {
            m_strMsg = msg;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.WorkerSupportsCancellation = true;
            return worker;
        }

        static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            MessageForm mf = new MessageForm(m_strMsg);
            mf.Show();

            BackgroundWorker worker = (BackgroundWorker)sender;

            while (true)
            {
                mf.BringToFront();
                mf.Refresh();
                if (worker.CancellationPending)
                {
                    mf.Close();
                    e.Cancel = true;
                }
                else
                    e.Cancel = false;
                System.Threading.Thread.Sleep(10);
            }
        }

        static void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ((BackgroundWorker)sender).Dispose();
        }
    }
}

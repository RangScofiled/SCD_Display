﻿using System;
using System.Windows.Forms;
using WindowsFormsApplication3.View;

namespace WindowsFormsApplication3.Controller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Controller a = new Controller();

            a.Start(new VisualSCD());
        }
    }
}

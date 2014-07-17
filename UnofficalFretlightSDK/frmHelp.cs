using System;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;

namespace UnofficalFretlightSDK
{
    partial class frmHelp : Form
    {
        public frmHelp()
        {
            InitializeComponent();
            this.Text = String.Format("Help");
            
            LinkLabel.Link link = new LinkLabel.Link();
            link.LinkData = @"http://goo.gl/GM4dBc";
	        linkLabel1.Links.Add(link);
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }
        #endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Send the URL to the operating system.
            Process.Start(e.Link.LinkData as string);
        }
    }
}

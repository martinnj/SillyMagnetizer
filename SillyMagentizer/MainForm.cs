using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SillyMagentizer
{
    public partial class MainForm : Form
    {
        static System.Reflection.Assembly assembly = typeof(Program).Assembly;
        static GuidAttribute attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
        static string assemblyguid = attribute.Value;

        private Mutex _filemutex;

        public MainForm(Mutex memoryAccessMutex)
        {
            InitializeComponent();
            _filemutex = memoryAccessMutex;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var memorycontent = Shared.ReadMappedFile(_filemutex, assemblyguid);
            textBox1.Text = memorycontent;
        }
    }
}

using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SillyMagentizer
{
    static class Program
    {
        // Make sure the mutex we use for instance management is created before we execute program code.
        static System.Reflection.Assembly assembly = typeof(Program).Assembly;
        static GuidAttribute attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
        static string assemblyguid = attribute.Value;
        static Mutex singleinstancemutex = new Mutex(true, "instance_mutex_" + assemblyguid);
        static Mutex memoryaccessmutex = new Mutex(true, "memory_access_mutex_" + assemblyguid);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                // Check if we can get the mutex, if we can, we're the first instance so we run.
                if (singleinstancemutex.WaitOne(TimeSpan.Zero, true))
                {
                    MemoryMappedFile mmf = MemoryMappedFile.CreateNew(assemblyguid, 10000, MemoryMappedFileAccess.ReadWrite);
                    memoryaccessmutex.ReleaseMutex();
                    Shared.WriteToMappedFile(memoryaccessmutex, assemblyguid, args[0]);

                    try
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new MainForm(memoryaccessmutex));
                    }
                    finally
                    {
                        // Clean up after ourselves, dispose of memory file, then release mutex.
                        mmf.Dispose();
                        singleinstancemutex.ReleaseMutex();
                    }
                }
                else
                {
                    Shared.WriteToMappedFile(memoryaccessmutex, assemblyguid, args[0]);
                }
            }
        }

    }
}

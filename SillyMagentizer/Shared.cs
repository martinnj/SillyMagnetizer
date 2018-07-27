using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;

namespace SillyMagentizer
{
    internal static class Shared
    {
        internal static void WriteToMappedFile(Mutex fileAccessMutex, string memoryMappedFilename, string line)
        {
            var precontent = ReadMappedFile(fileAccessMutex, memoryMappedFilename);
            fileAccessMutex.WaitOne();
            using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(memoryMappedFilename))
            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
            {
                stream.Seek(0, SeekOrigin.Begin);
                if (!String.IsNullOrEmpty(precontent)) writer.Write(precontent);
                writer.WriteLine(line);
                writer.Flush();

            }
            fileAccessMutex.ReleaseMutex();
        }

        internal static string ReadMappedFile(Mutex fileAccessMutex, string memoryMappedFilename)
        {
            fileAccessMutex.WaitOne();
            var retval = "";
            using (MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(memoryMappedFilename))
            using (MemoryMappedViewStream stream = mmf.CreateViewStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                retval = reader.ReadToEnd();

            }
            fileAccessMutex.ReleaseMutex();
            return retval.Trim('\0');
        }
    }
}

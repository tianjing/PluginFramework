using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace Log2Net
{
    public class TextLog : Log2Service.ITextLog
    {
        private static string filePath = AppDomain.CurrentDomain.BaseDirectory;
        private static string filenameEx = ".txt";
        private static string file = filePath + DateTime.Today.ToString("yyy-MM-dd") + filenameEx;
        private object locobj = new object();
        public void OutLog(string message, Exception e)
        {
            lock (locobj)
            {
                System.IO.File.AppendAllText(file, string.Format("errTile:{0},ExceptionMessage:{1}", message, e.Message));
            }
        }
        private bool CreateTxt()
        {
            bool result = false;
            try
            {
                result = true;
            }
            catch { }
            return result;
        }
        public void OutLog(string message)
        {
            lock (locobj)
            {
                System.IO.File.AppendAllText(file, string.Format("errTile:{0}", message));
            }
        }
    }
}

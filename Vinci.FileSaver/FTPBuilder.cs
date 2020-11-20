using System;
using System.Collections.Generic;
using System.Text;

namespace Vinci.FileSaver
{
    public class FTPBuilder
    {
        public Interface.IStorage Build(string url)
        {
            var client = new FluentFTP.FtpClient(url);

            return new FTPStorage { Client = client };
        }

        public Interface.IStorage Build(string url, string username, string password)
        {
            if (username is null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var netCredential = new System.Net.NetworkCredential(username, password);
            var client = new FluentFTP.FtpClient(url,netCredential);

            return new FTPStorage { Client = client };
        }
    }
}

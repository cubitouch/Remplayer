using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Management;

namespace Remplayer.Service
{
    public class PathModel
    {
        public List<Tuple<string, string>> Drives
        {
            get
            {
                List<Tuple<string, string>> temp = new List<Tuple<string, string>>();

                foreach (DriveInfo hd in System.IO.DriveInfo.GetDrives())
                {
                    temp.Add(new Tuple<string, string>(Uri.EscapeUriString(hd.Name), hd.Name));
                }

                return temp;
            }
            set { }
        }
        public List<Tuple<string, string>> NetworkHosts
        {
            get
            {
                List<Tuple<string, string>> temp = new List<Tuple<string, string>>();

                foreach (string computer in new ListNetworkComputers.NetworkBrowser().getNetworkComputers())
                {
                    if (computer.ToString() != System.Environment.MachineName)
                    {

                        temp.Add(new Tuple<string, string>(Uri.EscapeUriString("$" + computer), computer));
                    }
                }

                return temp;
            }
            set { }
        }
        public List<Tuple<string, string, bool>> Listing
        {
            get
            {
                List<Tuple<string, string, bool>> temp = new List<Tuple<string, string, bool>>();

                string path = Path.Replace("$", "\\\\");
                bool wasFile = false;

                try
                {
                    FileAttributes attr = File.GetAttributes(path);
                    if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                    {
                        //file loaded by player
                        MainWindow._viewModel.Source = path;
                        path = System.IO.Path.GetDirectoryName(path);
                        wasFile = true;
                    }
                }
                catch (Exception)
                {

                }

                if (path.Split('/')[path.Split('/').Length - 1].StartsWith("\\\\") && path.Split('\\').Length == 3)
                {
                    Trinet.Networking.ShareCollection shi = Trinet.Networking.ShareCollection.GetShares(path.Split('/')[path.Split('/').Length - 1]);
                    if (shi != null)
                    {
                        foreach (Trinet.Networking.Share si in shi)
                        {
                            temp.Add(new Tuple<string, string, bool>(Uri.EscapeUriString(path.Replace("\\\\", "$") + "\\" + si.NetName), path + "\\" + si.NetName, false));
                        }
                    }
                }
                else
                {
                    DirectoryInfo di = new System.IO.DirectoryInfo(path + "/");

                    foreach (System.IO.DirectoryInfo d in di.GetDirectories())
                    {
                        temp.Add(new Tuple<string, string, bool>(Uri.EscapeUriString((!wasFile ? Request.Path.ToString().Substring(5) : System.IO.Path.GetDirectoryName(Request.Path.ToString().Substring(5))) + "/" + d.Name), d.Name, false));
                    }
                    foreach (System.IO.FileInfo f in di.GetFiles("*.*"))
                    {
                        temp.Add(new Tuple<string, string, bool>(Uri.EscapeUriString((!wasFile ? Request.Path.ToString().Substring(5) : System.IO.Path.GetDirectoryName(Request.Path.ToString().Substring(5))) + "/" + f.Name), f.Name, (Request.Path.ToString().Substring(6).Replace("$", "//") == f.FullName.Replace("\\", "/"))));
                    }
                }

                return temp;
            }
            set { }
        }

        public string Path { get; set; }
        public Nancy.Request Request { get; set; }

        public bool IsCurrentPath(string actual = "")
        {
            return actual.Trim() == Path.Trim();
        }

        public PathModel(string p = "", Nancy.Request r = null)
        {
            Path = p;
            Request = r;
        }
    }
}

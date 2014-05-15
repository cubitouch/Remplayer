using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Management;


namespace Remplayer.Service
{
    public class ServiceModule : Nancy.NancyModule
    {
        /// <summary>
        /// Gets the rooted initial directory to use to access the host.
        /// Returns an empty string if the server is unavailable.
        /// </summary>
        /// <param name="serverName">The server to connect to.</param>

        public ServiceModule()
        {
            Get["/"] = parameter =>
            {
                return View["home.html", new PathModel()];
            };
            Get["/base"] = parameter =>
            {
                return View["filesystem.html", new PathModel()];
            };
            Get["/cmd/{c}"] = parameter =>
            {
                string cmd = parameter.c.ToString();
                switch (cmd)
                {
                    case "stop":
                        MainWindow._viewModel.State = System.Windows.Controls.MediaState.Stop;
                        break;
                    case "pause":
                        MainWindow._viewModel.State = System.Windows.Controls.MediaState.Pause;
                        break;
                    case "lecture":
                        MainWindow._viewModel.State = System.Windows.Controls.MediaState.Play;
                        break;
                    case "volumne_up":
                        MainWindow._viewModel.Volume += 0.1;
                        break;
                    case "volumne_down":
                        MainWindow._viewModel.Volume -= 0.1;
                        break;
                    default:
                        break;
                }

                return "";
            };
            Get["/path/"] = parameter =>
            {
                return new Nancy.Responses.RedirectResponse("/");
            };
            Get["/path/{d*}"] = parameter =>
            {
                return View["path.html", new PathModel(parameter.d.ToString(), Request)];
            };
        }
    }
}

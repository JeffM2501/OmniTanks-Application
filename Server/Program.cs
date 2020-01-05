using System;
using System.IO;
using System.Reflection;

using Urho;
using Urho.Desktop;

namespace Server
{
    class Program
    {
        private static void Log(string text)
        {
            Console.WriteLine(text);
        }

        static void Main(string[] args)
        {
            string dataPath = string.Empty; // TODO, parse this from optional command line

            if (dataPath == string.Empty || !Directory.Exists(dataPath))
                dataPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");

            if (!Directory.Exists(dataPath))
            {
                Log("Unable to locate data path " + dataPath);
                return;
            }

            if (Directory.Exists(dataPath))
            {
                DesktopUrhoInitializer.AssetsDirectory = dataPath;

                ApplicationOptions options = new ApplicationOptions("Data");
                options.Orientation = ApplicationOptions.OrientationType.Landscape;
                options.ResizableWindow = false;
                options.WindowedMode = true;
                options.AdditionalFlags = "-headless";


                int exitCode = 0;
                ServerApp app = null;
                try
                {
                    app = new ServerApp(options);
                    exitCode = app.Run();
                }
                catch (Exception ex)
                {
                    if (app == null || !app.IsExiting)
                    {
                        Log(ex.ToString());
                    }
                }
            }
        }
    }
}

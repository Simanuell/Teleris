using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Teleris.Core;
using Teleris.Resources;
using System.Reflection;

namespace Teleris.Systems.Systems
{
    class ResourceSystem : ISystem
    {
        private IEngine _engine;
        private FileSystemWatcher watcher;
        private bool EffectFileChanged;
        private string EffectFileName;

        public override void AddToGame(IEngine Engine)
        {
            //string path = "C:/Users/ilkka.jahnukainen/Desktop/own project/Teleris framework/dx11/Resources/Effects/Shaders/";
            string path = "C:/Users/ilkkaj/Documents/GitHub/Teleris/Teleris_framework/dx11/Resources/Effects/Shaders/";
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "dx11\Resources\Effects\Shaders\");
            
            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.fx";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            //watcher.Created += new FileSystemEventHandler(OnChanged);
            //watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;          
        
        
        }

        public override void RemoveFromGame(IEngine Engine)
        {
            System.Console.WriteLine("Printer removed!");
        }
       

        public override void Update(double time)
        {

            if (EffectFileChanged) 
            {

                EffectPool.pool.Recompile(EffectFileName);
                EffectFileChanged = false;
            }



        }



        // Define the event handlers. 
        void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            //Debug.WriteLine("File: " +  e.FullPath + " " + e.ChangeType);
            Debug.WriteLine("File: " + e.Name + " " + e.ChangeType);
            EffectFileName = e.Name;
            EffectFileChanged = true;

        }
    
    }
}

using System.IO;

namespace Creek.Messaging
{
    internal class EventHelper
    {
        private readonly FileSystemWatcher _watcher;

        public event Recieve Recieveing;

        protected virtual void OnRecieveing(Message m)
        {
            var handler = Recieveing;
            if (handler != null) handler(m);
        }

        public EventHelper()
        {
            _watcher = new FileSystemWatcher();
        }

        public void Listen()
        {
            _watcher.EnableRaisingEvents = true;
            _watcher.NotifyFilter = NotifyFilters.FileName;
            _watcher.Created += (sender, args) => Recieveing(Message.Create(args.Name, File.ReadAllText(args.FullPath)));
        }

        public void StopListening()
        {
            _watcher.EnableRaisingEvents = false;
        }

        public delegate void Recieve(Message m);
        
    }
}

namespace Creek.Extensibility.Addins
{
    using System.Collections.Generic;
    using System.Linq;

    public class ExtensionNode
    {
        #region Fields

        public Dictionary<string, object> Commands = new Dictionary<string, object>();

        public string Path;

        internal List<ExtensionCommand> Nodes = new List<ExtensionCommand>();

        #endregion

        #region Public Methods and Operators

        public T[] CreateInstances<T>() where T : class
        {
            return this.Commands.Values.ToArray().Select(source => source as T).ToArray();
        }

        public ExtensionCommand GetCommand(string name)
        {
            return this.Nodes.FirstOrDefault(extensionCommand => extensionCommand.Name == name);
        }

        #endregion
    }
}
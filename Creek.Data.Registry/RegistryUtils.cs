using System;

namespace Creek.Data.Registry
{
    /// <summary>
    /// This class provides some usefull method for the IEntry interface.
    /// </summary>
    public class RegistryUtils
    {
        /// <summary>
        /// Gets a string value from the node.
        /// </summary>
        /// <param name="node">the node.</param>
        /// <returns>the value as string.</returns>
        /// <exception cref="RegistryException">
        /// Throws a RegistryException if the ValueFormat does not match or 
        /// the node is a folder.
        /// </exception>
        public static string GetStringValue(IEntry node)
        {
            CheckNode(node);

            var format = node.GetValueFormat();
            if (format != ValueFormat.String)
            {
                throw new RegistryException("ValueFormat does not match.");
            }

            var value = node.GetValue();
            var valueString = Convert.ToString(value);
            if (valueString == null)
            {
                throw new RegistryException("Cannot cast value to string.");
            }

            return valueString;
        }

        /// <summary>
        /// Gets a long value from the node.
        /// </summary>
        /// <param name="node">the node.</param>
        /// <returns>the value as long.</returns>
        /// <exception cref="RegistryException">
        /// Throws a RegistryException if the ValueFormat does not match or 
        /// the node is a folder.
        /// </exception>
        public static long GetLongValue(IEntry node)
        {
            CheckNode(node);

            var format = node.GetValueFormat();
            if (format != ValueFormat.Long)
            {
                throw new RegistryException("ValueFormat does not match.");
            }

            var value = node.GetValue();
            var valueLong = Convert.ToInt64(value);

            return valueLong;
        }

        /// <summary>
        /// Gets a int value from the node.
        /// </summary>
        /// <param name="node">the node.</param>
        /// <returns>the value as int.</returns>
        /// <exception cref="RegistryException">
        /// Throws a RegistryException if the ValueFormat does not match or 
        /// the node is a folder.
        /// </exception>
        public static int GetIntValue(IEntry node)
        {
            CheckNode(node);

            var format = node.GetValueFormat();
            if (format != ValueFormat.Int)
            {
                throw new RegistryException("ValueFormat does not match.");
            }

            var value = node.GetValue();
            var valueInt = Convert.ToInt32(value);
            return valueInt;
        }

        /// <summary>
        /// Gets a double value from the node.
        /// </summary>
        /// <param name="node">the node.</param>
        /// <returns>the value as double.</returns>
        /// <exception cref="RegistryException">
        /// Throws a RegistryException if the ValueFormat does not match or 
        /// the node is a folder.
        /// </exception>
        public static double GetDoubleValue(IEntry node)
        {
            CheckNode(node);

            var format = node.GetValueFormat();
            if (format != ValueFormat.Double)
            {
                throw new RegistryException("ValueFormat does not match.");
            }

            var value = node.GetValue();
            var valueDouble = Convert.ToDouble(value);
            return valueDouble;
        }

        /// <summary>
        /// Adds a new value node to a given folder node.
        /// </summary>
        /// <param name="key">the key of the new value node.</param>
        /// <param name="value">the value.</param>
        /// <param name="format">the format.</param>
        /// <param name="root">the folder node.</param>
        /// <returns>the new value node.</returns>
        public static IEntry AddValueNode(string key,object value, ValueFormat format,IEntry root)
        {
            if (root == null)
            {
                throw new RegistryException("root is null");
            }

            if (!root.IsFolder)
            {
                throw new RegistryException("root is a value node!");
            }

            var keyExists = root.Contains(key);
            if (keyExists)
            {
                throw new RegistryException("the key already exists.");
            }

            var node = root.AddValue(key);
            node.SetValue(value,format);

            return node;
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="expression">the expression that describes the path.</param>
        /// <param name="root">the root node.</param>
        /// <returns>the node at the specified expression.</returns>
        public static IEntry GetNode(string expression,IEntry root)
        {
            if (string.IsNullOrEmpty(expression))
            {
                throw new RegistryException("expression is null or empty!");
            }

            var nodeFound = true;
            var currentNode = root;
            var nodes = expression.Split(new []{'/'});
            foreach (var node in nodes)
            {
                nodeFound = currentNode.Contains(node);
                if (nodeFound)
                {
                    currentNode = currentNode[node];
                }
            }

            if (!nodeFound)
            {
                throw new RegistryException("cannot validate epxression");
            }

            return currentNode;
        }

        private static void CheckNode(IEntry node)
        {
            if (node.IsFolder)
            {
                throw new RegistryException("Cannot access a value from a folder.");
            }
        }
    }
}

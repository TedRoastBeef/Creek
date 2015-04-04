//////////////////////////////////////////////////////////////////////////////
// This source code and all associated files and resources are copyrighted by
// the author(s). This source code and all associated files and resources may
// be used as long as they are used according to the terms and conditions set
// forth in The Code Project Open License (CPOL), which may be viewed at
// http://www.blackbeltcoder.com/Legal/Licenses/CPOL.
//
// Copyright (c) 2011 Jonathan Wood
//

namespace Creek.Text
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public enum QueryTypes
    {
        Select,
        Insert,
        Update,
        Delete
    }

    public enum JoinTypes
    {
        InnerJoin,
        LeftJoin,
        RightJoin,
        OuterJoin
    }

    public enum ConditionOperators
    {
        And,
        Or
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }

    /// <summary>
    /// Class to construct SQL queries
    /// </summary>
    internal class QueryBuilder
    {
        // Private members to track current settings
        private readonly List<ColumnInfo> _columns = new List<ColumnInfo>();
        private readonly List<ConditionInfo> _conditions = new List<ConditionInfo>();
        private readonly List<KeyValuePair<string, string>> _nameValuePairs = new List<KeyValuePair<string, string>>();
        private readonly List<SortInfo> _sortColumns = new List<SortInfo>();
        private readonly List<TableInfo> _tables = new List<TableInfo>();

        // Type of query (SELECT, INSERT, UPDATE, DELETE)

        /// <summary>
        /// Construction
        /// </summary>
        public QueryBuilder()
        {
            this.Reset();
        }

        public QueryTypes QueryType { get; set; }

        /// <summary>
        /// Restores this instance to default settings.
        /// </summary>
        public void Reset()
        {
            this.QueryType = QueryTypes.Select;
            this._columns.Clear();
            this._nameValuePairs.Clear();
            this._tables.Clear();
            this._tables.Clear();
        }

        /// <summary>
        /// Adds a column for a SELECT query
        /// </summary>
        /// <param name="colName">Column name</param>
        public void AddColumn(string colName)
        {
            this.AddColumn(colName, null, null);
        }

        /// <summary>
        /// Adds a column for a SELECT query
        /// </summary>
        /// <param name="colName">Column name</param>
        /// <param name="tableName">Name of table</param>
        public void AddColumn(string colName, string tableName)
        {
            this.AddColumn(colName, tableName, null);
        }

        /// <summary>
        /// Adds a column for a SELECT query
        /// </summary>
        /// <param name="colName">Column name</param>
        /// <param name="tableName">Name of table</param>
        /// <param name="alias">Column alias name</param>
        public void AddColumn(string colName, string tableName, string alias)
        {
            this._columns.Add(new ColumnInfo {Name = colName, TableName = tableName, Alias = alias});
        }

        /// <summary>
        /// Adds a column name/value pair for INSERT or UPDATE queries
        /// </summary>
        /// <param name="name">Column name</param>
        /// <param name="value">Column value</param>
        public void AddNameValuePair(string name, string value)
        {
            this._nameValuePairs.Add(new KeyValuePair<string, string>(name, value));
        }

        /// <summary>
        /// Adds an initial or only table to the query.
        /// </summary>
        /// <param name="tableName"></param>
        public void AddTable(string tableName)
        {
            if (this._tables.Count > 0)
                throw new Exception("Must supply JOIN parameters in additional tables");
            this.AddTable(null, null, tableName, null, JoinTypes.InnerJoin);
        }

        /// <summary>
        /// Adds a subsequent table to the query
        /// </summary>
        /// <param name="leftTable">Name of table on left side of join</param>
        /// <param name="leftColumn">Name of column on left side of join</param>
        /// <param name="newTable">Name of table being added</param>
        /// <param name="rightColumn">Name of column on right side of join</param>
        /// <param name="type">Join type</param>
        public void AddTable(string leftTable, string leftColumn, string newTable, string rightColumn, JoinTypes type)
        {
            this._tables.Add(new TableInfo
                            {
                                Name = newTable,
                                JoinType = type,
                                LeftTable = leftTable,
                                LeftColumn = leftColumn,
                                RightColumn = rightColumn
                            });
        }

        /// <summary>
        /// Adds a WHERE condition to the current query
        /// </summary>
        /// <param name="text">Text of WHERE condition</param>
        public void AddCondition(string text)
        {
            this.AddCondition(text, ConditionOperators.And);
        }

        /// <summary>
        /// Adds a WHERE condition to the current query
        /// </summary>
        /// <param name="text">Text of WHERE condition</param>
        /// <param name="op">Logical operator joining this condition with previous one</param>
        public void AddCondition(string text, ConditionOperators op)
        {
            this._conditions.Add(new ConditionInfo {Text = text, Operator = op});
        }

        /// <summary>
        /// Adds an ORDER BY column to the current query
        /// </summary>
        /// <param name="colName">Name of column used for sorting</param>
        public void AddSortColumn(string colName)
        {
            this.AddSortColumn(colName, SortOrder.Ascending);
        }

        /// <summary>
        /// Adds an ORDER BY column to the current query
        /// </summary>
        /// <param name="colName">Name of column used for sorting</param>
        /// <param name="direction">Sort direction</param>
        public void AddSortColumn(string colName, SortOrder direction)
        {
            this.AddSortColumn(colName, null, direction);
        }

        /// <summary>
        /// Adds an ORDER BY column to the current query
        /// </summary>
        /// <param name="colName">Name of column used for sorting</param>
        /// <param name="tableName">Name of table this column is part of</param>
        /// <param name="direction">Sort direction</param>
        public void AddSortColumn(string colName, string tableName, SortOrder direction)
        {
            this._sortColumns.Add(new SortInfo {Name = colName, Table = tableName, Direction = direction});
        }

        /// <summary>
        /// Returns a query string using the current settings
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            switch (this.QueryType)
            {
                case QueryTypes.Select:
                    builder.Append("SELECT");
                    builder.AppendFormat(" {0}", this.BuildColumnString());
                    builder.AppendFormat(" FROM {0}", this.BuildTableString());
                    builder.Append(this.BuildConditionString());
                    builder.Append(this.BuildSortString());
                    break;

                case QueryTypes.Insert:
                    builder.Append("INSERT");
                    builder.AppendFormat(" INTO {0}", this.BuildTableString(true));
                    builder.AppendFormat(" ({0}) VALUES ({1})",
                                         this.BuildPairNameStrings(this._nameValuePairs),
                                         this.BuildPairValueString(this._nameValuePairs));
                    break;

                case QueryTypes.Update:
                    builder.Append("UPDATE");
                    builder.AppendFormat(" {0} SET ", this.BuildTableString(true));
                    builder.Append(this.BuildPairNameValueString(this._nameValuePairs));
                    builder.Append(this.BuildConditionString());
                    break;

                case QueryTypes.Delete:
                    builder.Append("DELETE");
                    builder.AppendFormat(" FROM {0}", this.BuildTableString(true));
                    builder.Append(this.BuildConditionString());
                    break;
            }
            return builder.ToString();
        }

        /// <summary>
        /// Constructs a string with list of columns
        /// </summary>
        /// <returns></returns>
        protected string BuildColumnString()
        {
            if (this._columns.Count == 0)
                return "*";

            var builder = new StringBuilder();
            foreach (ColumnInfo info in this._columns)
            {
                if (builder.Length > 0)
                    builder.Append(", ");
                builder.Append(info);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Constructs a string with list of tables
        /// </summary>
        /// <returns></returns>
        protected string BuildTableString()
        {
            return this.BuildTableString(false);
        }

        /// <summary>
        /// Constructs a string with list of tables
        /// </summary>
        /// <param name="singleTable">Set to true if all but first table should be ignored</param>
        /// <returns></returns>
        protected string BuildTableString(bool singleTable)
        {
            if (this._tables.Count == 0)
                throw new Exception("No table specified");

            if (singleTable)
                return this._tables[0].ToString(true);

            var builder = new StringBuilder();
            foreach (TableInfo info in this._tables)
            {
                builder.Append(info.ToString(builder.Length == 0));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Construcuts a string with a list of WHERE conditions
        /// </summary>
        /// <returns></returns>
        protected string BuildConditionString()
        {
            if (this._conditions.Count == 0)
                return String.Empty;

            var builder = new StringBuilder();
            foreach (ConditionInfo info in this._conditions)
            {
                builder.Append(info.ToString(builder.Length == 0));
            }
            builder.Insert(0, " WHERE");
            return builder.ToString();
        }

        /// <summary>
        /// Constructs a string with a list of ORDER BY columns
        /// </summary>
        /// <returns></returns>
        protected string BuildSortString()
        {
            if (this._sortColumns.Count == 0)
                return String.Empty;

            var builder = new StringBuilder();
            foreach (SortInfo info in this._sortColumns)
            {
                builder.Append(info.ToString(builder.Length == 0));
            }
            builder.Insert(0, " ORDER BY");
            return builder.ToString();
        }

        /// <summary>
        /// Constructs a list of names from name/value pairs
        /// </summary>
        /// <param name="pairs">Name/value pairs from which to construct string</param>
        /// <returns></returns>
        protected string BuildPairNameStrings(List<KeyValuePair<string, string>> pairs)
        {
            var builder = new StringBuilder();
            foreach (var pair in pairs)
            {
                if (builder.Length > 0)
                    builder.Append(", ");
                builder.AppendFormat("[{0}]", pair.Key);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Constructs a list of values from name/values pairs
        /// </summary>
        /// <param name="pairs">Name/value pairs from which to construct string</param>
        /// <returns></returns>
        protected string BuildPairValueString(List<KeyValuePair<string, string>> pairs)
        {
            var builder = new StringBuilder();
            foreach (var pair in pairs)
            {
                if (builder.Length > 0)
                    builder.Append(", ");
                builder.Append(pair.Value);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Constructs a list of name/values from name/values pairs
        /// </summary>
        /// <param name="pairs">Name/value pairs from which to construct string</param>
        /// <returns></returns>
        protected string BuildPairNameValueString(List<KeyValuePair<string, string>> pairs)
        {
            var builder = new StringBuilder();
            foreach (var pair in this._nameValuePairs)
            {
                if (builder.Length > 0)
                    builder.Append(", ");
                builder.AppendFormat("[{0}] = {1}", pair.Key, pair.Value);
            }
            return builder.ToString();
        }

        #region Nested type: ColumnInfo

        /// <summary>
        /// Class to track column information
        /// </summary>
        private class ColumnInfo
        {
            public string Name { get; set; }
            public string Alias { get; set; }
            public string TableName { get; set; }

            public override string ToString()
            {
                var builder = new StringBuilder();
                if (this.TableName != null)
                    builder.AppendFormat("[{0}].", this.TableName);
                builder.AppendFormat("[{0}]", this.Name);
                if (this.Alias != null)
                    builder.AppendFormat(" AS [{0}]", this.Alias);
                return builder.ToString();
            }
        }

        #endregion

        #region Nested type: ConditionInfo

        /// <summary>
        /// Class to track WHERE condition information
        /// </summary>
        private class ConditionInfo
        {
            public string Text { get; set; }
            public ConditionOperators Operator { get; set; }

            public override string ToString()
            {
                return this.ToString(true);
            }

            public string ToString(bool isFirstCondition)
            {
                // First condition has no logical operator
                if (isFirstCondition)
                    return String.Format(" {0}", this.Text);

                // Condition with logical operator
                var builder = new StringBuilder();
                switch (this.Operator)
                {
                    case ConditionOperators.And:
                        builder.Append(" AND");
                        break;
                    case ConditionOperators.Or:
                        builder.Append(" OR");
                        break;
                }
                builder.AppendFormat(" {0}", this.Text);
                return builder.ToString();
            }
        }

        #endregion

        #region Nested type: SortInfo

        /// <summary>
        /// Class to track sort information
        /// </summary>
        private class SortInfo
        {
            public string Name { get; set; }
            public string Table { get; set; }
            public SortOrder Direction { get; set; }

            public override string ToString()
            {
                return this.ToString(true);
            }

            public string ToString(bool isFirstColumn)
            {
                var builder = new StringBuilder();
                if (isFirstColumn)
                    builder.Append(" ");
                else
                    builder.Append(", ");
                if (!String.IsNullOrEmpty(this.Table))
                    builder.AppendFormat("[{0}].", this.Table);
                builder.AppendFormat("[{0}]", this.Name);
                if (this.Direction == SortOrder.Descending)
                    builder.Append(" DESC");
                return builder.ToString();
            }
        }

        #endregion

        #region Nested type: TableInfo

        /// <summary>
        /// Class to track table information
        /// </summary>
        private class TableInfo
        {
            public string Name { get; set; }
            public JoinTypes JoinType { get; set; }
            public string LeftTable { get; set; }
            public string LeftColumn { get; set; }
            public string RightColumn { get; set; }

            public override string ToString()
            {
                return this.ToString(true);
            }

            public string ToString(bool isFirstTable)
            {
                // First table has no join
                if (isFirstTable)
                    return String.Format("[{0}]", this.Name);

                // Table with join
                var builder = new StringBuilder();
                switch (this.JoinType)
                {
                    case JoinTypes.InnerJoin:
                        builder.Append(" INNER JOIN");
                        break;
                    case JoinTypes.LeftJoin:
                        builder.Append(" LEFT OUTER JOIN");
                        break;
                    case JoinTypes.RightJoin:
                        builder.Append(" RIGHT OUTER JOIN");
                        break;
                    case JoinTypes.OuterJoin:
                        builder.Append(" FULL OUTER JOIN");
                        break;
                }
                builder.AppendFormat(" [{0}] ON [{1}].[{2}] = [{0}].[{3}]",
                                     this.Name, this.LeftTable, this.LeftColumn, this.RightColumn);
                return builder.ToString();
            }
        }

        #endregion
    }
}
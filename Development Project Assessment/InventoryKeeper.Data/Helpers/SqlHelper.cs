using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace InventoryKeeper.Data.Helpers
{
    #region SqlHelper

    public static class SqlHelper
    {
        #region Query

        public static IEnumerable<dynamic> QuerySP(string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, bool buffered = true, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.Query(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<dynamic> QuerySQL(string sql, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, bool buffered = true, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.Query(sql, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.Text);
            }
        }

        public static IEnumerable<T> QuerySP<T>(string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, bool buffered = true, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.Query<T>(storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<T> QuerySQL<T>(string sql, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, bool buffered = true, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.Query<T>(sql, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.Text);
            }
        }

        public static IEnumerable<object> QuerySP(Type type, string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, bool buffered = true, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.Query(type, storedProcedure, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.StoredProcedure);
            }
        }

        public static IEnumerable<object> QuerySQL(Type type, string sql, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, bool buffered = true, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.Query(type, sql, param: (object)param, transaction: transaction, buffered: buffered, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.Text);
            }
        }

        #endregion

        #region ExecuteScalar

        public static object ExecuteScalarSP(string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.ExecuteScalar(storedProcedure, param: (object)param, transaction: transaction, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.StoredProcedure);
            }
        }

        public static object ExecuteScalarSQL(string sql, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.ExecuteScalar(sql, param: (object)param, transaction: transaction, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.Text);
            }
        }

        public static T ExecuteScalarSP<T>(string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.ExecuteScalar<T>(storedProcedure, param: (object)param, transaction: transaction, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.StoredProcedure);
            }
        }

        public static T ExecuteScalarSQL<T>(string sql, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.ExecuteScalar<T>(sql, param: (object)param, transaction: transaction, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.Text);
            }
        }

        #endregion

        #region Execute

        public static int ExecuteSP(string storedProcedure, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.Execute(storedProcedure, param: (object)param, transaction: transaction, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.StoredProcedure);
            }
        }

        public static int ExecuteSQL(string sql, dynamic param = null, dynamic outParam = null, SqlTransaction transaction = null, int? commandTimeout = null, string connectionString = null)
        {
            CombineParameters(ref param, outParam);

            using (SqlConnection connection = new SqlConnection(GetConnectionString(connectionString)))
            {
                connection.Open();
                return connection.Execute(sql, param: (object)param, transaction: transaction, commandTimeout: GetTimeout(commandTimeout), commandType: CommandType.Text);
            }
        }

        #endregion

        #region CombineParameters

        private static void CombineParameters(ref dynamic param, dynamic outParam = null)
        {
            if (outParam != null)
            {
                if (param != null)
                {
                    param = new DynamicParameters(param);
                    ((DynamicParameters)param).AddDynamicParams(outParam);
                }
                else
                {
                    param = outParam;
                }
            }
        }

        #endregion

        #region Connection String & Timeout


        public static string GetConnectionString(string connectionStringName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionStringName))
                    connectionStringName = "cnn_IK";

                return System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ToString();
            }
            catch (NullReferenceException nex)
            {
                throw new Exception($"The ConnectionString for '{connectionStringName}' was not setup correctly");
            }
        }

        public static int ConnectionTimeout { get; set; }

        public static int GetTimeout(int? commandTimeout = null)
        {
            if (commandTimeout.HasValue)
                return commandTimeout.Value;

            return ConnectionTimeout;
        }

        #endregion

        #region ToEnumerable

        public delegate object ValueHandler(string columnName, Type columnType, object value);

        #endregion

        #region ToProperties

        public static IDictionary<string, object> ToProperties(this IDictionary<string, object> obj, params string[] columnNames)
        {
            return ToProperties(obj, null, columnNames);
        }

        public static IDictionary<string, object> ToProperties(this IDictionary<string, object> obj, ValueHandler getValue, params string[] columnNames)
        {
            if (columnNames != null && columnNames.Length > 0)
            {
                IDictionary<string, object> props = new Dictionary<string, object>();
                if (getValue != null)
                {
                    foreach (var pair in obj)
                    {
                        if (columnNames.Contains(pair.Key))
                            props.Add(pair.Key, getValue(pair.Key, (pair.Value != null ? pair.Value.GetType() : typeof(object)), pair.Value));
                    }
                }
                else
                {
                    foreach (var pair in obj)
                    {
                        if (columnNames.Contains(pair.Key))
                            props.Add(pair.Key, pair.Value);
                    }
                }
                return props;
            }
            else if (getValue != null)
            {
                IDictionary<string, object> props = new Dictionary<string, object>();
                foreach (var pair in obj)
                    props.Add(pair.Key, getValue(pair.Key, (pair.Value != null ? pair.Value.GetType() : typeof(object)), pair.Value));
                return props;
            }
            else
            {
                return obj;
            }
        }

        public static IDictionary<string, object> ToProperties(object obj, params string[] columnNames)
        {
            return ToProperties(obj, null, columnNames);
        }

        public static IDictionary<string, object> ToProperties(object obj, ValueHandler getValue, params string[] columnNames)
        {
            if (obj is IDictionary<string, object>)
            {
                if (getValue != null || (columnNames != null && columnNames.Length > 0))
                    return ToProperties((IDictionary<string, object>)obj, getValue, columnNames);
                else
                    return (IDictionary<string, object>)obj;
            }

            Type type = obj.GetType();

            var columns =
                type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Select(f => new
                    {
                        ColumnName = f.Name,
                        ColumnType = f.FieldType,
                        IsField = true,
                        MemberInfo = (MemberInfo)f
                    })
                    .Union(
                        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => p.CanRead)
                            .Where(p => p.GetGetMethod(true).IsPublic)
                            .Where(p => p.GetIndexParameters().Length == 0)
                            .Select(p => new
                            {
                                ColumnName = p.Name,
                                ColumnType = p.PropertyType,
                                IsField = false,
                                MemberInfo = (MemberInfo)p
                            })
                    )
                    .Where(c => (columnNames != null && columnNames.Length > 0 ? columnNames.Contains(c.ColumnName) : true)); // columns exist

            IDictionary<string, object> values = new Dictionary<string, object>();
            if (getValue != null)
            {
                foreach (var column in columns)
                    values.Add(column.ColumnName, getValue(column.ColumnName, column.ColumnType, (column.IsField ? ((FieldInfo)column.MemberInfo).GetValue(obj) : ((PropertyInfo)column.MemberInfo).GetValue(obj, null))));
            }
            else
            {
                foreach (var column in columns)
                    values.Add(column.ColumnName, (column.IsField ? ((FieldInfo)column.MemberInfo).GetValue(obj) : ((PropertyInfo)column.MemberInfo).GetValue(obj, null)));
            }
            return values;
        }

        public static IEnumerable<IDictionary<string, object>> ToProperties(this IEnumerable<IDictionary<string, object>> objs, params string[] columnNames)
        {
            return ToProperties(objs, null, columnNames);
        }

        public static IEnumerable<IDictionary<string, object>> ToProperties(this IEnumerable<IDictionary<string, object>> objs, ValueHandler getValue, params string[] columnNames)
        {
            if (columnNames != null && columnNames.Length > 0)
            {
                List<IDictionary<string, object>> values = new List<IDictionary<string, object>>();
                if (getValue != null)
                {
                    foreach (IDictionary<string, object> obj in objs)
                    {
                        IDictionary<string, object> props = new Dictionary<string, object>();
                        foreach (var pair in obj)
                        {
                            if (columnNames.Contains(pair.Key))
                                props.Add(pair.Key, getValue(pair.Key, (pair.Value != null ? pair.Value.GetType() : typeof(object)), pair.Value));
                        }
                        values.Add(props);
                    }
                }
                else
                {
                    foreach (IDictionary<string, object> obj in objs)
                    {
                        IDictionary<string, object> props = new Dictionary<string, object>();
                        foreach (var pair in obj)
                        {
                            if (columnNames.Contains(pair.Key))
                                props.Add(pair.Key, pair.Value);
                        }
                        values.Add(props);
                    }
                }
                return values;
            }
            else if (getValue != null)
            {
                List<IDictionary<string, object>> values = new List<IDictionary<string, object>>();
                foreach (IDictionary<string, object> obj in objs)
                {
                    IDictionary<string, object> props = new Dictionary<string, object>();
                    foreach (var pair in obj)
                        props.Add(pair.Key, getValue(pair.Key, (pair.Value != null ? pair.Value.GetType() : typeof(object)), pair.Value));
                    values.Add(props);
                }
                return values;
            }
            else
            {
                return objs;
            }
        }

        public static IEnumerable<IDictionary<string, object>> ToProperties<T>(this IEnumerable<T> objs, params string[] columnNames)
        {
            return ToProperties<T>(objs, null, columnNames);
        }

        public static IEnumerable<IDictionary<string, object>> ToProperties<T>(this IEnumerable<T> objs, ValueHandler getValue, params string[] columnNames)
        {
            if (objs is IEnumerable<IDictionary<string, object>>)
            {
                if (getValue != null || (columnNames != null && columnNames.Length > 0))
                    return ToProperties((IEnumerable<IDictionary<string, object>>)objs, getValue, columnNames);
                else
                    return (IEnumerable<IDictionary<string, object>>)objs;
            }

            Type type = typeof(T);

            var columns =
                type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Select(f => new
                    {
                        ColumnName = f.Name,
                        ColumnType = f.FieldType,
                        IsField = true,
                        MemberInfo = (MemberInfo)f
                    })
                    .Union(
                        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => p.CanRead)
                            .Where(p => p.GetGetMethod(true).IsPublic)
                            .Where(p => p.GetIndexParameters().Length == 0)
                            .Select(p => new
                            {
                                ColumnName = p.Name,
                                ColumnType = p.PropertyType,
                                IsField = false,
                                MemberInfo = (MemberInfo)p
                            })
                    )
                    .Where(c => (columnNames != null && columnNames.Length > 0 ? columnNames.Contains(c.ColumnName) : true)); // columns exist

            List<IDictionary<string, object>> values = new List<IDictionary<string, object>>();
            if (getValue != null)
            {
                foreach (var obj in objs)
                {
                    var dic = new Dictionary<string, object>();
                    foreach (var column in columns)
                        dic.Add(column.ColumnName, getValue(column.ColumnName, column.ColumnType, (column.IsField ? ((FieldInfo)column.MemberInfo).GetValue(obj) : ((PropertyInfo)column.MemberInfo).GetValue(obj, null))));
                    values.Add(dic);
                }
            }
            else
            {
                foreach (var obj in objs)
                {
                    var dic = new Dictionary<string, object>();
                    foreach (var column in columns)
                        dic.Add(column.ColumnName, (column.IsField ? ((FieldInfo)column.MemberInfo).GetValue(obj) : ((PropertyInfo)column.MemberInfo).GetValue(obj, null)));
                    values.Add(dic);
                }
            }
            return values;
        }

        public static IEnumerable<IDictionary<string, object>> ToProperties(this IEnumerable<object> objs, params string[] columnNames)
        {
            return ToProperties(objs, null, columnNames);
        }

        public static IEnumerable<IDictionary<string, object>> ToProperties(this IEnumerable<object> objs, ValueHandler getValue, params string[] columnNames)
        {
            if (getValue == null && (columnNames == null || columnNames.Length == 0) && objs is IEnumerable<IDictionary<string, object>>)
                return (IEnumerable<IDictionary<string, object>>)objs;

            List<IDictionary<string, object>> values = new List<IDictionary<string, object>>();
            foreach (var obj in objs)
                values.Add(ToProperties(obj, getValue, columnNames));
            return values;
        }

        #endregion
    }

    #endregion

    #region DynamicParameters Extensions

    public static class DynamicParametersExtensions
    {
        // http://msdn.microsoft.com/en-us/library/cc716729(v=vs.100).aspx
        static readonly Dictionary<SqlDbType, DbType?> sqlDbTypeMap = new Dictionary<SqlDbType, DbType?>
        {
            {SqlDbType.BigInt, DbType.Int64},
            {SqlDbType.Binary, DbType.Binary},
            {SqlDbType.Bit, DbType.Boolean},
            {SqlDbType.Char, DbType.AnsiStringFixedLength},
            {SqlDbType.DateTime, DbType.DateTime},
            {SqlDbType.Decimal, DbType.Decimal},
            {SqlDbType.Float, DbType.Double},
            {SqlDbType.Image, DbType.Binary},
            {SqlDbType.Int, DbType.Int32},
            {SqlDbType.Money, DbType.Decimal},
            {SqlDbType.NChar, DbType.StringFixedLength},
            {SqlDbType.NText, DbType.String},
            {SqlDbType.NVarChar, DbType.String},
            {SqlDbType.Real, DbType.Single},
            {SqlDbType.UniqueIdentifier, DbType.Guid},
            {SqlDbType.SmallDateTime, DbType.DateTime},
            {SqlDbType.SmallInt, DbType.Int16},
            {SqlDbType.SmallMoney, DbType.Decimal},
            {SqlDbType.Text, DbType.String},
            {SqlDbType.Timestamp, DbType.Binary},
            {SqlDbType.TinyInt, DbType.Byte},
            {SqlDbType.VarBinary, DbType.Binary},
            {SqlDbType.VarChar, DbType.AnsiString},
            {SqlDbType.Variant, DbType.Object},
            {SqlDbType.Xml, DbType.Xml},
            {SqlDbType.Udt,(DbType?)null}, // Dapper will take care of it
            {SqlDbType.Structured,(DbType?)null}, // Dapper will take care of it
            {SqlDbType.Date, DbType.Date},
            {SqlDbType.Time, DbType.Time},
            {SqlDbType.DateTime2, DbType.DateTime2},
            {SqlDbType.DateTimeOffset, DbType.DateTimeOffset}
        };

        public static void Add(this DynamicParameters parameter, string name, object value, SqlDbType? sqlDbType, ParameterDirection? direction, int? size)
        {
            parameter.Add(name, value, (sqlDbType != null ? sqlDbTypeMap[sqlDbType.Value] : (DbType?)null), direction, size);
        }

        public static void Add(this DynamicParameters parameter, string name, object value = null, SqlDbType? sqlDbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
        {
            parameter.Add(name, value, (sqlDbType != null ? sqlDbTypeMap[sqlDbType.Value] : (DbType?)null), direction, size, precision, scale);
        }

        public static Dictionary<string, object> Get(this DynamicParameters parameter)
        {
            return parameter.Get<object>();
        }

        // all the parameters are of the same type T
        public static Dictionary<string, T> Get<T>(this DynamicParameters parameter)
        {
            Dictionary<string, T> values = new Dictionary<string, T>();
            foreach (string parameterName in parameter.ParameterNames)
                values.Add(parameterName, parameter.Get<T>(parameterName));
            return values;
        }
    }

    #endregion
}

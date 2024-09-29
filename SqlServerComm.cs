using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Windows;

namespace Lambda_Release01._01._01.BackEnd.DataAccessLayer
{
    public static class SqlServerComm
    {
        internal static SqlConnection _connection = new SqlConnection();
        internal static string _connectionString;
        internal static string _commandString;
        public static void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                try
                {
                    _connection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while trying to open connection. \n" + ex.Message);
                }
            }
        }
        public static void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                try
                {
                    _connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while trying to close connection. \n" + ex.Message);
                }
            }
        }
        private static void PrepareConnection(string connectionStringName, string storedProcedureName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            _connection.ConnectionString = _connectionString;
            _commandString = "Exec " + storedProcedureName;
        }
        private static void PrepareParameters(DynamicParameters dynamicParameters)
        {
            if (dynamicParameters.ParameterNames.Count() > 0)
            {
                foreach (string parameterName in dynamicParameters.ParameterNames)
                {
                    if (parameterName == dynamicParameters.ParameterNames.Last<string>())
                    {
                        _commandString += " @" + parameterName;
                    }
                    else
                    {
                        _commandString += " @" + parameterName + ", ";
                    }
                }
            }
        }
        public static int ExecuteCommand(string storedProcedureName, DynamicParameters dynamicParameters = null, string connectionStringName = "default")
        {
            PrepareConnection(connectionStringName, storedProcedureName);
            dynamicParameters = dynamicParameters ?? new DynamicParameters();
            if (dynamicParameters.ParameterNames.Count() > 0)
            {
                PrepareParameters(dynamicParameters);
                return _connection.Execute(_commandString, dynamicParameters);
            }
            else
            {
                return _connection.Execute(_commandString);
            }
        }
        public static IEnumerable<T> ExecuteStoredProcedure<T>(string storedProcedureName, DynamicParameters dynamicParameters = null, string connectionStringName = "default")
        {
            PrepareConnection(connectionStringName, storedProcedureName);
            dynamicParameters = dynamicParameters ?? new DynamicParameters();
            if (dynamicParameters.ParameterNames.Count() > 0)
            {
                PrepareParameters(dynamicParameters);
                return _connection.Query<T>(_commandString, dynamicParameters);
            }
            else
            {
                return _connection.Query<T>(_commandString);
            }
        }
    }
}
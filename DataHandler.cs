using Dapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lambda_Release01._01._01.BackEnd.DataAccessLayer.DataHandlers
{
    public static class DataHandler
    {
        public static ObservableCollection<T> FetchData<T>(string storedProcedureName, DynamicParameters parameters = null)
        {
            return new ObservableCollection<T>(SqlServerComm.ExecuteStoredProcedure<T>(storedProcedureName, parameters));
        }
        public static int SaveData(string storedProcedureName, DynamicParameters parameters)
        {
            return SqlServerComm.ExecuteCommand(storedProcedureName, parameters);
        }
    }
}

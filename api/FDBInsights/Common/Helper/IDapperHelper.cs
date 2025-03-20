using System.Data;
using System.Data.Common;
using Dapper;

namespace FDBInsights.Common.Helper;

public interface IDapperHelper
{
    DbConnection GetDbconnection();

    // Execute Methods
    Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

    Task<int> ExecuteWithTransactionAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    Task<int> ExecuteNonQueryAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    // Query Methods (Single Row)
    Task<T?> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);

    Task<T?> GetSingleRowAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    Task<T?> GetFirstRowAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    // Query Methods (Multiple Rows)
    Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    Task<IEnumerable<T>> GetMultipleAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    // Scalar Methods
    Task<T?> GetScalarAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    Task<T?> GetColumnAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    // Insert/Update Methods
    Task<T?> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    Task<T?> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

    // Multiple Result Sets
    Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> GetMultipleResultAsync<T1, T2>(
        string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

    Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> GetMultipleResultAsync<T1, T2, T3>(
        string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

    // DataTable Methods
    Task<DataTable> GetDataTableAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    Task<List<DataTable>> GetMultipleDataTablesAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure);

    // Bulk Operations
    Task<int> BulkInsertAsync<T>(string tableName, IEnumerable<T> entities) where T : class;
}
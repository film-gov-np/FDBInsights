using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;

namespace FDBInsights.Common.Helper;

public class DapperHelper(string? connectionString) : IDapperHelper
{
    #region Bulk Operations

    public async Task<int> BulkInsertAsync<T>(string tableName, IEnumerable<T> entities) where T : class
    {
        // This is a simplified implementation. For production use, consider using SqlBulkCopy
        using IDbConnection db = new SqlConnection(connectionString);
        try
        {
            if (db.State == ConnectionState.Closed)
                db.Open();

            using var tran = db.BeginTransaction();
            try
            {
                var properties = typeof(T).GetProperties();
                var columns = string.Join(", ", properties.Select(p => p.Name));
                var values = string.Join(", ", properties.Select(p => "@" + p.Name));
                var sql = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";

                var result = await db.ExecuteAsync(sql, entities, tran);
                tran.Commit();
                return result;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
        finally
        {
            if (db.State == ConnectionState.Open)
                db.Close();
        }
    }

    #endregion

    public DbConnection GetDbconnection()
    {
        return new SqlConnection(connectionString);
    }

    #region Execute Methods

    public async Task<int> ExecuteAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        return await db.ExecuteAsync(sp, parms, commandType: commandType);
    }

    public async Task<int> ExecuteWithTransactionAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        try
        {
            if (db.State == ConnectionState.Closed)
                db.Open();

            using var tran = db.BeginTransaction();
            try
            {
                var result = await db.ExecuteAsync(sp, parms, tran, commandType: commandType);
                tran.Commit();
                return result;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
        finally
        {
            if (db.State == ConnectionState.Open)
                db.Close();
        }
    }

    public async Task<int> ExecuteNonQueryAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        return await db.ExecuteAsync(sp, parms, commandType: commandType);
    }

    #endregion

    #region Query Methods (Single Row)

    public async Task<T?> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        return await db.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType);
    }

    public async Task<T?> GetSingleRowAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        return await db.QuerySingleOrDefaultAsync<T>(sp, parms, commandType: commandType);
    }

    public async Task<T?> GetFirstRowAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        return await db.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType);
    }

    #endregion

    #region Query Methods (Multiple Rows)

    public async Task<List<T>> GetAllAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        return (await db.QueryAsync<T>(sp, parms, commandType: commandType)).ToList();
    }

    public async Task<IEnumerable<T>> GetMultipleAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        return await db.QueryAsync<T>(sp, parms, commandType: commandType);
    }

    #endregion

    #region Scalar Methods

    public async Task<T?> GetScalarAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        return await db.ExecuteScalarAsync<T>(sp, parms, commandType: commandType);
    }

    public async Task<T?> GetColumnAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        // For Dapper, GetColumn is equivalent to ExecuteScalar
        return await db.ExecuteScalarAsync<T>(sp, parms, commandType: commandType);
    }

    #endregion

    #region Insert/Update Methods

    public async Task<T?> InsertAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        T? result;
        using IDbConnection db = GetDbconnection();
        try
        {
            if (db.State == ConnectionState.Closed)
                db.Open();
            using var tran = db.BeginTransaction();
            try
            {
                result = await db.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType, transaction: tran);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
        finally
        {
            if (db.State == ConnectionState.Open)
                db.Close();
        }

        return result;
    }

    public async Task<T?> UpdateAsync<T>(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        T? result;
        using IDbConnection db = new SqlConnection(connectionString);
        try
        {
            if (db.State == ConnectionState.Closed)
                db.Open();
            using var tran = db.BeginTransaction();
            try
            {
                result = await db.QueryFirstOrDefaultAsync<T>(sp, parms, commandType: commandType, transaction: tran);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
        finally
        {
            if (db.State == ConnectionState.Open)
                db.Close();
        }

        return result;
    }

    #endregion

    #region Multiple Result Sets

    public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>>> GetMultipleResultAsync<T1, T2>(
        string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        var result = await db.QueryMultipleAsync(sp, parms, commandType: commandType);
        var item1 = await result.ReadAsync<T1>();
        var item2 = await result.ReadAsync<T2>();

        return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
    }

    public async Task<Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> GetMultipleResultAsync<T1, T2, T3>(
        string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        var result = await db.QueryMultipleAsync(sp, parms, commandType: commandType);
        var item1 = await result.ReadAsync<T1>();
        var item2 = await result.ReadAsync<T2>();
        var item3 = await result.ReadAsync<T3>();

        return new Tuple<IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(item1, item2, item3);
    }

    #endregion

    #region DataTable Methods

    public async Task<DataTable> GetDataTableAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        var reader = await db.ExecuteReaderAsync(sp, parms, commandType: commandType);
        var table = new DataTable();
        table.Load(reader);
        return table;
    }

    public async Task<List<DataTable>> GetMultipleDataTablesAsync(string sp, DynamicParameters parms,
        CommandType commandType = CommandType.StoredProcedure)
    {
        using IDbConnection db = new SqlConnection(connectionString);
        var result = new List<DataTable>();
        var reader = await db.ExecuteReaderAsync(sp, parms, commandType: commandType);

        do
        {
            var table = new DataTable();
            table.Load(reader);
            result.Add(table);
        } while (!reader.IsClosed && reader.NextResult());

        return result;
    }

    #endregion
}
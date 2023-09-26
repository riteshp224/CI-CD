using Microsoft.Extensions.Options;
using Platinum.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace Platinum.Data
{
    public abstract class BaseRepository
    {
        #region Fields
        public readonly IOptions<DataConfig> _connectionString;
        #endregion

        #region Constructor
        public BaseRepository(IOptions<DataConfig> connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region SQL Methods

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString.Value.DefaultConnection))
            {
                await con.OpenAsync();
                return await con.QueryFirstOrDefaultAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString.Value.DefaultConnection))
            {
                await con.OpenAsync();
                return await con.QueryAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<object> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString.Value.DefaultConnection))
            {
                await con.OpenAsync();
                return await con.ExecuteScalarAsync<object>(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> ExecuteAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString.Value.DefaultConnection))
            {
                await con.OpenAsync();
                return await con.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<dynamic> QueryMultipleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (SqlConnection con = new SqlConnection(_connectionString.Value.DefaultConnection))
            {
                await con.OpenAsync();
                return await con.QueryMultipleAsync(sql, param, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion

    }
}

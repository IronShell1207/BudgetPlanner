using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BudgetPlanner.Constants;
using BudgetPlanner.Objects;
using Microsoft.Data.Sqlite;

namespace BudgetPlanner
{
    public class AppDbContext : TheDisposable
    {
        private readonly string ConnectionString = $"Data source={ProgPathes.DataFolderPath}budgetPlanner.db";

        public async Task<int> AddOperationAsync(MoneyOperation moneyMove)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                return await AddOperationAsync(moneyMove, connection);
            }
        }

        public async Task<int> AddOperationAsync(MoneyOperation moneyMove, SqliteConnection connection)
        {
            var operation = connection.CreateCommand();
            operation.CommandText = $"INSERT INTO Operations (OperationCategory, Sum, Type, Comment, DateTime) VALUES " +
                                    $"(\"{moneyMove.OperationCategory}\", \"{moneyMove.Sum}\", \"{moneyMove.Type}\", \"{moneyMove.Comment}\", \"{moneyMove.DateTime}\" )";
            return operation.ExecuteNonQuery();
        }

        public async Task<List<MoneyOperation>> GetOperationsAsync(int limit = 10, int offset = 0 )
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                return await GetOperationsAsync(limit, offset, connection);
            }
        }

        public async Task<List<MoneyOperation>> GetOperationsAsync(int limit, int offset, SqliteConnection connection)
        {
            var operationsList = new List<MoneyOperation>();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT id, operationcategory, sum, type, comment, datetime FROM Operations LIMIT {limit} OFFSET {offset}";
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    var operation = new MoneyOperation();
                    operation.Id = reader.GetInt64(0);
                    operation.OperationCategory = reader.GetString(1);
                    operation.Sum = reader.GetDouble(2);
                    operation.Type = reader.GetBoolean(3);
                    operation.Comment = reader.GetString(4);
                    operation.DateTime = reader.GetDateTime(5);
                    operationsList.Add(operation);
                }
            }
            return operationsList;
        }
        public AppDbContext()
        {
            
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var read = connection.CreateCommand();
                read.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Operations' COLLATE NOCASE";

                var result = read.ExecuteReader();
                if (!result.HasRows)
                {
                    var operationsTableCreatingCommand = connection.CreateCommand();
                    operationsTableCreatingCommand.CommandText = @"
CREATE TABLE IF NOT EXISTS Operations
(   ID INTEGER PRIMARY key autoincrement NOT NULL,
    OperationCategory CHAR(100) NOT NULL,
    Sum REAL NOT NULL,
    Type INT NOT NULL,
    Comment TEXT,
    DateTime TEXT NOT NULL
);";
                    operationsTableCreatingCommand.ExecuteNonQuery();

                }
            }
            
        }
    }
}

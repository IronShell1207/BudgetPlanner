using System;
using System.Collections.Generic;
using System.Globalization;
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

        public async Task<int> AddOperationAsync(MoneyOperations moneyMove)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                return await AddOperationAsync(moneyMove, connection);
            }
        }
        public async Task<int> AddOperationAsync(MoneyOperations moneyMove, SqliteConnection connection)
        {
            CultureInfo ci = new CultureInfo("en");
            Thread.CurrentThread.CurrentCulture = ci;
            var operation = connection.CreateCommand();
            var type = moneyMove.Type ? 1 : 0;
            var sum = moneyMove.Type ? moneyMove.Sum : -moneyMove.Sum;
            operation.CommandText = $"INSERT INTO Operations (OperationCategory, Sum, Type, Comment, DateTime) VALUES " +
                                    $"(\"{moneyMove.OperationCategory}\", \"{sum}\", \"{type}\", \"{moneyMove.Comment}\", \"{moneyMove.DateTime}\" )";
            return operation.ExecuteNonQuery();
        }
        public async Task<List<double>> GetAllMoneyMoves()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                List<double> operations = new List<double>();
                connection.Open();
                var operation = connection.CreateCommand();
                operation.CommandText = $"SELECT Sum FROM Operations";
                var reader = await operation.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        operations.Add(reader.GetDouble(0));
                    }
                }
                return operations;
            }
        }
        public async Task<List<double>> GetMoneyMovesByDate(DateTime date)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                List<double> operations = new List<double>();
                connection.Open();
                var operation = connection.CreateCommand();
                CultureInfo ci = new CultureInfo("en");
                Thread.CurrentThread.CurrentCulture = ci;
                operation.CommandText = $"SELECT Sum FROM Operations WHERE DateTime BETWEEN ('{date.ToShortDateString()}') AND ('{(date+TimeSpan.FromDays(1)).ToShortDateString()}' )";
                var reader = await operation.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        operations.Add(reader.GetDouble(0));
                    }
                }
                return operations;
            }
        }
        public async Task<List<MoneyOperations>> GetOperationsAsync(int limit = 10, int offset = 0 )
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                return await GetOperationsAsync(limit, offset, connection);
            }
        }

        public async Task<List<MoneyOperations>> GetOperationsAsync(int limit, int offset, SqliteConnection connection)
        {
            var operationsList = new List<MoneyOperations>();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT id, operationcategory, sum, type, comment, datetime FROM Operations LIMIT {limit} OFFSET {offset}";
            var reader = await command.ExecuteReaderAsync();
            if (reader.HasRows)
            {
                while (await reader.ReadAsync())
                {
                    var operation = new MoneyOperations();
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

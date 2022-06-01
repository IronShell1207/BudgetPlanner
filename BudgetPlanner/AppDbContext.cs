using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            CultureInfo ci = new CultureInfo("en");
            Thread.CurrentThread.CurrentCulture = ci;
            var operation = connection.CreateCommand();
            var type = moneyMove.Type ? 1 : 0;
            var sum = moneyMove.Type ? moneyMove.Sum : -moneyMove.Sum;
            operation.CommandText = $"INSERT INTO Operations (OperationCategory, Sum, Type, Comment, DateTime) VALUES " +
                                    $"(\"{moneyMove.OperationCategory}\", \"{sum}\", \"{type}\", \"{moneyMove.Comment}\", \"{moneyMove.DateTime}\" )";
            return operation.ExecuteNonQuery();
        }
        public async Task<int> EditOperationAsync(MoneyOperation moneyMove)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                return await EditOperationAsync(moneyMove, connection);
            }
        }
        public async Task<int> EditOperationAsync(MoneyOperation moneyMove, SqliteConnection connection)
        {
            CultureInfo ci = new CultureInfo("en");
            Thread.CurrentThread.CurrentCulture = ci;
            var operation = connection.CreateCommand();
            var type = moneyMove.Type ? 1 : 0;
            var sum = moneyMove.Type ? moneyMove.Sum : -moneyMove.Sum;
            operation.CommandText = $"UPDATE Operations SET " +
                                    $" Sum = \"{moneyMove.Sum}\"," +
                                    $" Type = \"{moneyMove.Type}\"," +
                                    $" Comment = \"{moneyMove.Comment}\"," +
                                    $" DateTime = \"{moneyMove.DateTime}\"," +
                                    $" OperationCategory = \"{moneyMove.OperationCategory}\"" +
                                    $"WHERE Id = {moneyMove.Id};";
            return operation.ExecuteNonQuery();
        }

        public async Task<int> DeleteOperationAsync(MoneyOperation moneyMove)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                return await DeleteOperationAsync(moneyMove, connection);
            }
        }

        public async Task<int> DeleteOperationAsync(MoneyOperation moneyMove, SqliteConnection connection)
        {
            CultureInfo ci = new CultureInfo("en");
            Thread.CurrentThread.CurrentCulture = ci;
            var operation = connection.CreateCommand();
            operation.CommandText = $"DELETE FROM Operations WHERE Id = {moneyMove.Id}";
            return operation.ExecuteNonQuery();
        }

        public async Task<List<double>> GetAllMoneyMovesAsync()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                List<double> operations = new List<double>();
                connection.Open();
                var operation = connection.CreateCommand();
                operation.CommandText = $"SELECT Sum FROM Operations ORDER BY DateTime DESC";
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
        public async Task<List<double>> GetMoneyMovesByDateAsync(DateTime dateFrom, DateTime dateTo)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                List<double> operations = new List<double>();
                connection.Open();
                var operation = connection.CreateCommand();
                CultureInfo ci = new CultureInfo("en");
                Thread.CurrentThread.CurrentCulture = ci;
                operation.CommandText = $"SELECT Sum FROM Operations WHERE DateTime BETWEEN ('{dateFrom.ToShortDateString()}') AND ('{(dateTo).ToShortDateString()}' )";
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
        public async Task<List<MoneyOperation>> GetDataByTimePeriodAsync(DateTime dateFrom, DateTime dateTo, string orderBy= "")
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                var operationsList = new List<MoneyOperation>();
                connection.Open();
                var sqliteCommand = connection.CreateCommand();
                CultureInfo ci = new CultureInfo("en");
                Thread.CurrentThread.CurrentCulture = ci;
                sqliteCommand.CommandText = $"SELECT id, operationcategory, sum, type, comment, datetime FROM Operations WHERE DateTime BETWEEN ('{dateFrom.ToShortDateString()}') AND ('{(dateTo).ToShortDateString()}') {orderBy}";
                var reader = await sqliteCommand.ExecuteReaderAsync();
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
        }
        public async Task<List<MoneyOperation>> GetOperationsAsync(int limit = 50, int offset = 0, string orderBy ="")
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                return await GetOperationsAsync(limit, offset, connection, orderBy);
            }
        }

        public async Task<List<MoneyOperation>> GetOperationsAsync(int limit, int offset, SqliteConnection connection, string orderBy)
        {
            var operationsList = new List<MoneyOperation>();
            var command = connection.CreateCommand();
            command.CommandText = $"SELECT id, operationcategory, sum, type, comment, datetime FROM Operations {orderBy} LIMIT {limit} OFFSET {offset} ";
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

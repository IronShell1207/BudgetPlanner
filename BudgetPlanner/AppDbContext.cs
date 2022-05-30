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
        public AppDbContext()
        {
            using (var connection = new SqliteConnection($"Data source={ProgPathes.DataFolderPath}budgetPlanner.db"))
            {
                connection.Open();
                var operationsTableCreatingCommand = connection.CreateCommand();
                operationsTableCreatingCommand.CommandText = @"
CREATE TABLE IF NOT EXISTS Operations
(   ID INT PRIMARY KEY NOT NULL,
    OperationCategory CHAR(100) NOT NULL,
    Sum REAL NOT NULL,
    Type INT NOT NULL,
    Comment TEXT,
    UserID INT NOT NULL,
    DateTime TEXT NOT NULL,
    CONSTRAINT FK_Operations_Users_UserId FOREIGN KEY (
        UserId
    )
    REFERENCES Users (Id) ON DELETE CASCADE
);";
                var tableUsersCreationCommand = connection.CreateCommand();
                tableUsersCreationCommand.CommandText = 
@"
    CREATE TABLE IF NOT EXISTS Users
(   ID INT PRIMARY KEY NOT NULL,
    NAME CHAR(50)      NOT NULL
);";
                operationsTableCreatingCommand.ExecuteNonQuery();
                tableUsersCreationCommand.ExecuteNonQuery();
            }
        }
    }
}

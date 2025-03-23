using System.Data.SqlClient;
using FDBInsights.Models;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;

namespace FDBInsights.Data;

public class SubscribeProductTableDependency : ISubscribeTableDependency
{
    private readonly string _connectionString;
    private readonly DashboardHub _dashboardHub;
    private SqlTableDependency<TicketTransaction>? _tableDependency;

    public SubscribeProductTableDependency(DashboardHub dashboardHub, string connectionString)
    {
        _dashboardHub = dashboardHub;
        _connectionString = connectionString;
    }

    public void SubscribeTableDependency(string? connectionString)
    {
        // Dispose previous instance if exists
        _tableDependency?.Dispose();
        var uniqueId = Guid.NewGuid().ToString();
        // Create a new instance and configure the mapper
        var mapper = new ModelToTableMapper<TicketTransaction>();
        mapper.AddMapping(model => model.TheaterName, "TheaterName");
        mapper.AddMapping(model => model.TheaterCode, "TheaterCode");
        mapper.AddMapping(model => model.ScreenName, "ScreenName");
        mapper.AddMapping(model => model.ScreenID, "ScreenID");
        mapper.AddMapping(model => model.ShowTypeName, "ShowTypeName");
        mapper.AddMapping(model => model.ShowTypeID, "ShowTypeID");
        mapper.AddMapping(model => model.MovieName, "MovieName");
        mapper.AddMapping(model => model.MovieCode, "MovieCode");
        mapper.AddMapping(model => model.FiscalYear, "FiscalYear");
        mapper.AddMapping(model => model.FiscalYearID, "FiscalYearID");
        mapper.AddMapping(model => model.ShowDateTime, "ShowDateTime");
        mapper.AddMapping(model => model.ShowID, "ShowID");
        mapper.AddMapping(model => model.PrintDateTime, "PrintDateTime");
        mapper.AddMapping(model => model.TicketTypeName, "TicketTypeName");
        mapper.AddMapping(model => model.TicketTypeID, "TicketTypeID");
        mapper.AddMapping(model => model.PaymentTypeName, "PaymentTypeName");
        mapper.AddMapping(model => model.PaymentTypeID, "PaymentTypeID");
        mapper.AddMapping(model => model.TicketCode, "TicketCode");
        mapper.AddMapping(model => model.SeatNo, "SeatNo");
        mapper.AddMapping(model => model.TicketStatusName, "TicketStatusName");
        mapper.AddMapping(model => model.TicketStatusValue, "TicketStatusValue");
        mapper.AddMapping(model => model.TicketPrice, "TicketPrice");
        mapper.AddMapping(model => model.TicketsTax, "TicketsTax");
        mapper.AddMapping(model => model.TicketsCharge, "TicketsCharge");
        mapper.AddMapping(model => model.DistributorCode, "DistributorCode");
        mapper.AddMapping(model => model.DistributorCommissionValue, "DistributorCommissionValue");
        mapper.AddMapping(model => model.TicketCancelledReason, "TicketCancelledReason");
        mapper.AddMapping(model => model.TicketCancelledDateTime, "TicketCancelledDateTime");
        mapper.AddMapping(model => model.AddedDateTime, "AddedDateTime");
        mapper.AddMapping(model => model.Header, "Header");
        mapper.AddMapping(model => model.Extracted, "Extracted");
        mapper.AddMapping(model => model.UpdatedOn, "UpdatedOn");
        mapper.AddMapping(model => model.IPAddress, "IPAddress");
        mapper.AddMapping(model => model.TicketNetPrice, "TicketNetPrice");

        // Skip permission check and set executeUserPermissionCheck to false
        _tableDependency = new SqlTableDependency<TicketTransaction>(
            connectionString,
            "TicketTransaction",
            "Report",
            mapper,
            null,
            null,
            TableDependency.SqlClient.Base.Enums.DmlTriggerType.All,
            false);
        _tableDependency.OnChanged += TableDependency_OnChanged;
        _tableDependency.OnError += TableDependency_OnError;
        _tableDependency.Start();
    }

    private void TableDependency_OnChanged(object sender,
        TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<TicketTransaction> e)
    {
        if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            //_dashboardHub.SendProducts();
            Console.WriteLine("Change detected as:{0}", e.ChangeType);
        CleanupTableDependencyObjects();
        // Restart the dependency
        //SubscribeTableDependency(_connectionString);
    }

    private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
    {
        Console.WriteLine($"Error occurred: {e.Error.Message}");

        // Attempt to restart the Table Dependency
        if (_tableDependency != null)
        {
            Console.WriteLine("Attempting to restart SqlTableDependency...");
            try
            {
                // Clean up existing objects if conflict occurs
                CleanupTableDependencyObjects();

                _tableDependency.Stop(); // Stop the current dependency
                _tableDependency.Start(); // Start it again
                Console.WriteLine("TableDependency restarted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error restarting TableDependency: {ex.Message}");
            }
        }
    }

    private void CleanupTableDependencyObjects()
    {
        try
        {
            // Query the database to identify existing dependency objects
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Get the list of objects related to SqlTableDependency
                var command = new SqlCommand(
                    "SELECT name FROM sys.objects WHERE name LIKE 'Report_TicketTransaction_%';", connection);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var objectName = reader.GetString(0);
                    Console.WriteLine($"Cleaning up: {objectName}");

                    // Drop the related service queue, message type, and triggers
                    DropDependencyObject(connection, objectName);
                }
            }

            Console.WriteLine("Existing table dependency objects cleaned up.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cleaning up table dependency objects: {ex.Message}");
        }
    }

    private void DropDependencyObject(SqlConnection connection, string objectName)
    {
        try
        {
            // Drop service queue, message type, and triggers dynamically
            using (var command = new SqlCommand($"DROP QUEUE {objectName};", connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new SqlCommand($"DROP MESSAGE TYPE {objectName}/StartMessage/Insert;", connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new SqlCommand($"DROP TRIGGER {objectName};", connection))
            {
                command.ExecuteNonQuery();
            }

            Console.WriteLine($"Dropped: {objectName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error dropping {objectName}: {ex.Message}");
        }
    }
}
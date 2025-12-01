using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAO.Database {
    public abstract class BaseDAO {
        protected void ExecuteReader(string query, Action<MySqlDataReader> handleRow, Dictionary<string, object> parameters = null) {
            MySqlConnection connection = null;
            MySqlCommand command = null;
            MySqlDataReader reader = null;

            try {
                connection = DatabaseConnection.GetConnection();
                connection.Open();
                command = new MySqlCommand(query, connection);

                if (parameters != null) {
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                reader = command.ExecuteReader();
                while (reader.Read()) {
                    handleRow(reader);
                }
            } catch (MySqlException ex) {
                throw new Exception($"Lỗi database khi thực thi query: {ex.Message}\nQuery: {query}", ex);
            } catch (Exception ex) {
                throw new Exception($"Lỗi không xác định: {ex.Message}\nQuery: {query}", ex);
            } finally {
                reader?.Close();
                command?.Dispose();
                connection?.Close();
            }
        }

        protected DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null) {
            MySqlConnection connection = null;
            MySqlCommand command = null;
            MySqlDataAdapter adapter = null;

            try {
                connection = DatabaseConnection.GetConnection();
                command = new MySqlCommand(query, connection);

                if (parameters != null) {
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            } catch (MySqlException ex) {
                throw new Exception($"Lỗi database khi thực thi query: {ex.Message}\nQuery: {query}", ex);
            } finally {
                adapter?.Dispose();
                command?.Dispose();
                connection?.Close();
            }
        }

        protected int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null) {
            MySqlConnection connection = null;
            MySqlCommand command = null;

            try {
                connection = DatabaseConnection.GetConnection();
                connection.Open();
                command = new MySqlCommand(query, connection);

                if (parameters != null) {
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                return command.ExecuteNonQuery();
            } catch (MySqlException ex) {
                throw new Exception($"Lỗi database khi thực thi non-query: {ex.Message}\nQuery: {query}", ex);
            } finally {
                command?.Dispose();
                connection?.Close();
            }
        }

        protected object ExecuteScalar(string query, Dictionary<string, object> parameters = null) {
            MySqlConnection connection = null;
            MySqlCommand command = null;

            try {
                connection = DatabaseConnection.GetConnection();
                connection.Open();
                command = new MySqlCommand(query, connection);

                if (parameters != null) {
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                return command.ExecuteScalar();
            } catch (MySqlException ex) {
                throw new Exception($"Lỗi database khi thực thi scalar: {ex.Message}\nQuery: {query}", ex);
            } finally {
                command?.Dispose();
                connection?.Close();
            }
        }

        protected int ExecuteInsertAndGetId(string query, Dictionary<string, object> parameters = null) {
            MySqlConnection connection = null;
            MySqlCommand command = null;

            try {
                connection = DatabaseConnection.GetConnection();
                connection.Open();
                command = new MySqlCommand(query, connection);

                if (parameters != null) {
                    foreach (var param in parameters)
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }

                command.ExecuteNonQuery();
                return Convert.ToInt32(command.LastInsertedId);
            } catch (MySqlException ex) {
                throw new Exception($"Lỗi database khi insert: {ex.Message}\nQuery: {query}", ex);
            } finally {
                command?.Dispose();
                connection?.Close();
            }
        }

        protected bool ExecuteTransaction(List<Action<MySqlConnection, MySqlTransaction>> actions) {
            MySqlConnection connection = null;
            MySqlTransaction transaction = null;

            try {
                connection = DatabaseConnection.GetConnection();
                connection.Open();
                transaction = connection.BeginTransaction();

                foreach (var action in actions)
                    action(connection, transaction);

                transaction.Commit();
                return true;
            } catch (Exception ex) {
                transaction?.Rollback();
                throw new Exception($"Lỗi khi thực thi transaction: {ex.Message}", ex);
            } finally {
                transaction?.Dispose();
                connection?.Close();
            }
        }

        protected T GetValueOrDefault<T>(object value, T defaultValue = default(T)) {
            if (value == null || value == DBNull.Value)
                return defaultValue;
            return (T)Convert.ChangeType(value, typeof(T));
        }

        protected string GetString(MySqlDataReader reader, string columnName) {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        protected int GetInt32(MySqlDataReader reader, string columnName) {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
        }

        protected DateTime? GetDateTime(MySqlDataReader reader, string columnName) {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (DateTime?)null : reader.GetDateTime(ordinal);
        }

        protected decimal? GetDecimal(MySqlDataReader reader, string columnName) {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? (decimal?)null : reader.GetDecimal(ordinal);
        }

        protected void LogQuery(string query, Dictionary<string, object> parameters = null) {
        }
    }
}


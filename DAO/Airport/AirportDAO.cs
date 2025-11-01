using DAO.Database;
using System;
using System.Data;

namespace DAO.Airport
{
    public class AirportDAO : BaseDAO
    {
        #region Singleton Pattern
        private static AirportDAO _instance;
        private static readonly object _lock = new object();
        private AirportDAO() { }
        public static AirportDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new AirportDAO();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        public DataTable GetAllAirportsForComboBox()
        {
            string query = @"
                SELECT 
                    airport_id, 
                    CONCAT(airport_name, ' (', airport_code, ')') AS DisplayName
                FROM 
                    Airports 
                ORDER BY 
                    airport_name";
            try
            {
                return ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sân bay: " + ex.Message, ex);
            }
        }
        public DataTable GetArrivalAirportsByDeparture(int departureAirportId)
        {
            string query = @"
                SELECT DISTINCT
                    arr.airport_id,
                    CONCAT(arr.airport_name, ' (', arr.airport_code, ')') AS DisplayName
                FROM 
                    Routes r
                JOIN 
                    Airports arr ON r.arrival_place_id = arr.airport_id
                WHERE
                    r.departure_place_id = @departureAirportId
                ORDER BY
                    arr.airport_name";

            var parameters = new Dictionary<string, object>
            {
                { "@departureAirportId", departureAirportId }
            };

            try
            {
                return ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sân bay đến: " + ex.Message, ex);
            }
        }
    }
}
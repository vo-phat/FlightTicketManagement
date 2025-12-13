using DTO.Baggage;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.BaggageDAO
{
    public class TicketBaggageDAO
    {
        // ===============================
        // GET BY TICKET ID (JOIN)
        // ===============================
        public List<TicketBaggageDTO> GetByTicketId(int ticketId)
        {
            List<TicketBaggageDTO> list = new List<TicketBaggageDTO>();

            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    -- Carry-on
                    SELECT 
                        tb.id,
                        tb.ticket_id,
                        tb.baggage_type,
                        tb.carryon_id,
                        tb.checked_id,
                        tb.quantity,
                        tb.note,
                        c.weight_kg AS kg,
                        0 AS price,
                        c.description
                    FROM ticket_baggage tb
                    JOIN carryon_baggage c ON tb.carryon_id = c.carryon_id
                    WHERE tb.ticket_id = @ticket_id AND tb.baggage_type = 'carry_on'

                    UNION ALL

                    -- Checked baggage
                    SELECT 
                        tb.id,
                        tb.ticket_id,
                        tb.baggage_type,
                        tb.carryon_id,
                        tb.checked_id,
                        tb.quantity,
                        tb.note,
                        ck.weight_kg AS kg,
                        ck.price AS price,
                        ck.description
                    FROM ticket_baggage tb
                    JOIN checked_baggage ck ON tb.checked_id = ck.checked_id
                    WHERE tb.ticket_id = @ticket_id AND tb.baggage_type = 'checked'
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ticket_id", ticketId);

                var rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    list.Add(new TicketBaggageDTO
                    {
                        Id = rd.GetInt32("id"),
                        TicketId = rd.GetInt32("ticket_id"),
                        BaggageType = rd.GetString("baggage_type"),
                        CarryOnId = rd["carryon_id"] == DBNull.Value ? null : (int?)rd.GetInt32("carryon_id"),
                        CheckedId = rd["checked_id"] == DBNull.Value ? null : (int?)rd.GetInt32("checked_id"),
                        Quantity = rd.GetInt32("quantity"),
                        Note = rd.GetString("note"),

                        Kg = rd.GetInt32("kg"),
                        Price = rd.GetDecimal("price"),
                        Description = rd.GetString("description")
                    });
                }
            }

            return list;
        }

        // ===============================
        // INSERT
        // ===============================
        public bool Insert(TicketBaggageDTO dto)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = @"
                    INSERT INTO ticket_baggage
                    (ticket_id, baggage_type, carryon_id, checked_id, quantity, note)
                    VALUES (@ticket, @type, @carry, @checked, @qty, @note)
                ";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ticket", dto.TicketId);
                cmd.Parameters.AddWithValue("@type", dto.BaggageType);
                cmd.Parameters.AddWithValue("@carry", dto.CarryOnId);
                cmd.Parameters.AddWithValue("@checked", dto.CheckedId);
                cmd.Parameters.AddWithValue("@qty", dto.Quantity);
                cmd.Parameters.AddWithValue("@note", dto.Note);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // ===============================
        // DELETE
        // ===============================
        public bool Delete(int id)
        {
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM ticket_baggage WHERE id=@id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}

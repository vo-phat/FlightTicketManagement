using DAO.Seat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Seat
{
    public class PriceSeatFareBUS
    {
        private readonly PriceSeatFareDAO _fareDao;

        public PriceSeatFareBUS()
        {
            _fareDao = new PriceSeatFareDAO();
        }

        /// <summary>
        /// Lấy giá phụ trội theo flight (fare rule)
        /// - Không có rule -> 0
        /// - Có nhiều rule -> đã được DAO xử lý ưu tiên
        /// </summary>
        public decimal GetFarePrice(int flightId)
        {
            if (flightId <= 0)
                throw new ArgumentException("flightId không hợp lệ");

            return _fareDao.GetFarePriceByFlight(flightId, DateTime.Now);
        }

        /// <summary>
        /// Lấy giá phụ trội theo flight và thời điểm chỉ định
        /// (dùng cho test / tính giá theo ngày bay)
        /// </summary>
        public decimal GetFarePrice(int flightId, DateTime pricingTime)
        {
            if (flightId <= 0)
                throw new ArgumentException("flightId không hợp lệ");

            return _fareDao.GetFarePriceByFlight(flightId, pricingTime);
        }
    }
}

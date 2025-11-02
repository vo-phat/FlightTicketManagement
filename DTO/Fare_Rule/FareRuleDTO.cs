using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Fare_Rule
{
    public class FareRuleDTO
    {
        public int RuleId { get; set; }
        public int RouteId { get; set; }
        public int ClassId { get; set; }

        public string RouteName { get; set; }     // SGN → PQC
        public string CabinClass { get; set; }    // PHỔ THÔNG
        public string FareType { get; set; }
        public string Season { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public FareRuleDTO() { }

        public FareRuleDTO(int ruleId, int routeId, int classId,
                           string routeName, string cabinClass,
                           string fareType, string season,
                           DateTime effectiveDate, DateTime expiryDate,
                           string description, decimal price)
        {
            RuleId = ruleId;
            RouteId = routeId;
            ClassId = classId;
            RouteName = routeName;
            CabinClass = cabinClass;
            FareType = fareType;
            Season = season;
            EffectiveDate = effectiveDate;
            ExpiryDate = expiryDate;
            Description = description;
            Price = price;
        }
    }
}

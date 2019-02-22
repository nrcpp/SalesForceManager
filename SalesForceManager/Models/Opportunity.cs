using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Siemplify
{
    public class Opportunity
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Stage { get; set; }
        public string BillingCountry { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Owner { get; set; }
        public int FYCV { get; set; }
        public string CreatedBy { get; set; }
        public List<OpportunityStageHistory> StagesHistory { get; set; } = new List<OpportunityStageHistory>();
    }

    public class OpportunityStageHistory
    {
        public string OpportunityName { get; set; }
        public string ToStage { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

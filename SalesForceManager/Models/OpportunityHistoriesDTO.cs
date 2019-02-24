using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesForceManager.Models
{
    public class Attributes
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Attributes2
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    public class OpportunityHistoryDTO
    {
        public Attributes2 attributes { get; set; }
        public string OpportunityId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string StageName { get; set; }
    }

    public class OpportunityHistoriesListDTO
    {
        public int totalSize { get; set; }
        public bool done { get; set; }
        public List<OpportunityHistoryDTO> records { get; set; }
    }

    public class OpportunityHistoriesClass
    {
        public Attributes attributes { get; set; }
        public string Id { get; set; }
        public OpportunityHistoriesListDTO OpportunityHistories { get; set; }
    }

    public class OpportunityHistoriesRootObject
    {
        public int totalSize { get; set; }
        public bool done { get; set; }
        public List<OpportunityHistoriesClass> records { get; set; }
    }
}

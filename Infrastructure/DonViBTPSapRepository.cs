using Data;
using System.Collections.Generic;

namespace Infrastructure
{
    public class DonViBTPSapRepository : RepositoryBase<DonViBTPSap>
    {
        public DonViBTPSapRepository(KHKDEntities entities) : base(entities)
        {
        }
    }
}
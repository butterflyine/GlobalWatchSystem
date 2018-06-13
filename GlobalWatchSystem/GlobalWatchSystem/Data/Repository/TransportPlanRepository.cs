using GlobalWatchSystem.Models;
using SharpRepository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalWatchSystem.Data.Repository
{
    public interface ITransportPlanRepository : IRepository<TransportPlan>
    {
    }

    public class TransportPlanRepository : ConfigurationBasedRepository<TransportPlan, int>, ITransportPlanRepository
    {
    }
}
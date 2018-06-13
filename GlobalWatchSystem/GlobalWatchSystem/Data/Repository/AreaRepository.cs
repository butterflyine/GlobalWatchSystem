using System.Collections.Generic;
using System.Linq;
using GlobalWatchSystem.Models;
using SharpRepository.Repository;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IAreaRepository : IRepository<Area>
    {
        IEnumerable<Area> GetChildAreas(int areaId);
    }
    public class AreaRepository : ConfigurationBasedRepository<Area, int>, IAreaRepository
    {

        public IEnumerable<Area> GetChildAreas(int areaId)
        {
            var result = new List<Area>();
            GetChildAreasRecursive(result, areaId);
            return result;
        }

        private void GetChildAreasRecursive(List<Area> accumulator, int areaId)
        {
            Area current = Get(areaId);
            if (!accumulator.Exists(area => area.Id == areaId) && current != null)
            {
                accumulator.Add(current);
            }

            List<Area> directChildren = FindAll(area => area.ParentId == areaId).ToList();
            foreach (Area child in directChildren)
            {
                if (!accumulator.Exists(area => area.Id == child.Id))
                {
                    accumulator.Add(child);
                    GetChildAreasRecursive(accumulator, child.Id);
                }
            }
        }
    }
}
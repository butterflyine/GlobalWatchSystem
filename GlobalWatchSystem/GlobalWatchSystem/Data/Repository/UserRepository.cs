using System;
using System.Collections.Generic;
using System.Linq;
using GlobalWatchSystem.Models;
using SharpRepository.Repository;

namespace GlobalWatchSystem.Data.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUserName(String userName);
        IEnumerable<User> FindUsersInAreas(IEnumerable<int> areaIds);
    }

    public class UserRepository : ConfigurationBasedRepository<User, int>, IUserRepository
    {
        public User GetByUserName(string userName)
        {
            return Find(user => user.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<User> FindUsersInAreas(IEnumerable<int> areaIds)
        {
            if (areaIds != null && areaIds.Any())
            {
                return FindAll(user => areaIds.Contains(user.AreaId));
            }
            return new List<User>();
        }
    }
}
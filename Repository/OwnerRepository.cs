using Entities.Models;
using Contracts;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            :base(repositoryContext)
        {

        }

        public IEnumerable<Owner> GetAllOwners()
        {
            return FindAll().OrderBy(owner => owner.Name);
        }

        public Owner GetOwnerById(Guid ownerId)
        {
            return FindByCondition(owner => owner.Id.Equals(ownerId))
                .DefaultIfEmpty(new Owner())
                .FirstOrDefault();
        }
    }
}

using Entities.Models;
using Contracts;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using Entities.ExtendedModels;
using Entities.Extensions;

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

        public OwnerExtended GetOwnerWithDetails(Guid ownerId)
        {
            return new OwnerExtended(GetOwnerById(ownerId))
            {
                Accounts = RepositoryContext.Accounts.Where(account => account.OwnerId == ownerId)
            };
        }

        public void CreateOwner(Owner owner)
        {
            owner.Id = Guid.NewGuid();
            Create(owner);
            Save();
        }

        public void UpdateOwner(Owner dbOwner, Owner owner)
        {
            dbOwner.Map(owner);
            Update(dbOwner);
            Save();
        }
    }
}

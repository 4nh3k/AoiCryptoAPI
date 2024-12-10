using AoiCryptoAPI.Data;
using AoiCryptoAPI.Models;
using System.Collections.Generic;

namespace AoiCryptoAPI.Services
{
    public class AllowlistService
    {
        private readonly MongoRepository<AllowlistEntry> _repository;

        public AllowlistService(MongoRepository<AllowlistEntry> repository)
        {
            _repository = repository;
        }

        public List<AllowlistEntry> Get() => _repository.GetAll();

        public AllowlistEntry GetById(string id) => _repository.Get(id);

        public void Create(AllowlistEntry entry) => _repository.Create(entry);

        public void Update(string id, AllowlistEntry entry) => _repository.Update(id, entry);

        public void Delete(string id) => _repository.Delete(id);

        public List<AllowlistEntry> GetByPoolAddress(string poolAddress) =>
            _repository.Find(entry => entry.PoolAddress == poolAddress);

        public List<AllowlistEntry> GetByUserAddress(string userAddress) =>
            _repository.Find(entry => entry.UserAddress == userAddress);

        public void CreateBulk(List<AllowlistEntry> entries)
        {
            if (entries != null && entries.Count > 0)
            {
                _repository.InsertMany(entries);
            }
        }
    }
}

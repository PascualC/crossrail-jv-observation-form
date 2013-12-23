using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Crossrail.ObservationForm.DataLayer;
using Crossrail.ObservationForm.DataLayer.Models;

namespace Crossrail.ObservationForm.Business
{
    public class ContractService
    {
        private readonly Repository<Contract> _contractRepository;

        public ContractService(ObservationDbContext context)
        {
            _contractRepository = new Repository<Contract>(context);
        }

        public List<Domain.Contract> GetAll()
        {
            return _contractRepository.GetAll().Project().To<Domain.Contract>().ToList();
        }

        public Domain.Contract GetById(int id)
        {
            var contract = _contractRepository.GetById(id);
            return Mapper.Map<Domain.Contract>(contract);
        }
    }
}
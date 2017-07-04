using System;
using System.Collections.Generic;
using System.Linq;
using MI.Data;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MI.Core.Domain;

namespace MI.Application.ISType
{
    public class STypeService : ISTypeService
    {
        private readonly IBaseRepository<Core.Domain.SType> _repository;
        public STypeService(IBaseRepository<Core.Domain.SType> repository)
        {
            _repository = repository;
        }

        public void AddsType(SType sType)
        {
            throw new NotImplementedException();
        }

        public IList<SType> GetsType()
        {
            throw new NotImplementedException();
        }
        //public IList<NoticeDto> GetNotices()
        //{
        //    IList<NoticeDto> dtoList = Mapper.Map<IList<NoticeDto>>(_repository.GetAll().ToList());

        //    return dtoList;
        //}
        public List<SType> QureyByCondition(Func<SType, bool> predicate)
        {
            IList<SType> dtoList = _repository.GetAll().ToList();
            return dtoList.Where(predicate).OrderByDescending(u => u.f_tID).ToList();
        }

        //public void AddsType(SType sType)
        //{
        //    _repository.Insert(sType);
        //}

        //public IList<SType> GetsType()
        //{
        //    IList<SType> sTyList = Mapper.Map<IList<SType>>(_repository.GetAll().ToList());
        //    return sTyList;
        //}

        //public List<SType> QureyByCondition(Func<SType, bool> predicate)
        //{
        //    List<SType> sTyList = Mapper.Map<List<SType>>(_repository.GetAll().Where(predicate).ToList());
        //    return sTyList;
        //}
    }
}

using System;
using AutoMapper;
using MI.Application.ContentServerce.Dto;
using MI.Application.Dto;
using MI.Core.Domain;
using MI.Application.ChangeRoomService.Dto;

namespace MI.Application.Mappers
{
    public class ApplicationMapperConfiguraton : IMapperConfiguration
    {
        public void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                

                cfg.CreateMap<EmpRent, EmpRentDto>();//员工租房
                cfg.CreateMap<t_Dormitory, t_DormitoryDto>();
                cfg.CreateMap<CarRegister, CarRegisterDto>();
                cfg.CreateMap<Grant, GrantDto>();//外租补助
                cfg.CreateMap<ReturnTicket, ReturnTicketDto>();

                //cfg.CreateMap<WorkDistribution, WorkDistributionDto>();
                cfg.CreateMap<t_Dormitory, t_DormitoryDto>();//宿舍登记表
                cfg.CreateMap<t_LatticeContent, t_LatticeContentDto>();//宿舍格子视图
                cfg.CreateMap<t_ModifyRecord, ModifyRecordDto>();
                cfg.CreateMap<t_LaundryPwd, t_LaundryPwdDto>();
                //   cfg.CreateMap<t_ChangeRoom, t_ChangeRoomDto>();
                cfg.CreateMap<Employee, EmployeeDto>();
                cfg.CreateMap<Tariff, TariffDto>();//电费超支
                cfg.CreateMap<CashRegister, CashRegisterListDto>();//现金登记
            });
        }

    }
}

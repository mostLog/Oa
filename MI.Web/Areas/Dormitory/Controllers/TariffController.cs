using MI.Application;
using MI.Application.Dto;
using MI.Application.File;
using MI.Application.OASession;
using MI.Application.OASession.Dto;
using MI.Core.Domain;
using MI.Core.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using TSOfficeSystem_New.Controllers;

namespace TSOfficeSystem_New.Areas.Dormitory.Controllers
{
    [AjaxFunc("/Dormitory")]
    public class TariffController : BaseController
    {
        //电费超支服务
        private readonly ITariffService _tariffService;
        //文件操作服务
        private readonly ITariffFileService _fileService;
        //类型服务
        private readonly ISTypeService _typeService;
        //楼栋信息服务
        private readonly IDormitoryService _dormitoryService;
        public TariffController(
            ITariffService tariffService,
            ITariffFileService fileService,
            ISTypeService typeService,
            IDormitoryService dormitoryService,
            ISession session, IEmployeeService emoloyee) : base(session, emoloyee)
        {
            _tariffService = tariffService;
            _fileService = fileService;
            _typeService = typeService;
            _dormitoryService = dormitoryService;
        }
        public ActionResult Index(TariffPagedInputDto input)
        {
            var list = _tariffService.GetPagedAllTariffs(input);
            //收款地点
            ViewBag.WorkLoaction = _typeService.GetsTypeByWhere((int)sTypeEnum.上班地点类型);
            //社区
            ViewBag.Community = _dormitoryService.GetTariffbyCommunity();
            ViewBag.InputSearch = input;
            return View(list);
        }
        /// <summary>
        /// 根据id获取Tariff
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public JsonResult GetTariffById(int id = 0)
        {
            if (id == 0)
            {
                return Json(new
                {
                    status = 0
                });
            }
            Tariff model = _tariffService.GetObjectById(id);
            return Json(new
            {
                Id = model.f_Id,
                DormitoryId=model.f_DormitoryId,
                Community = model.t_dormitory?.f_Community ?? string.Empty,
                Building = model.t_dormitory?.f_Building ?? string.Empty,
                RoomNo = model.t_dormitory?.f_RoomNo ?? string.Empty,
                TariffStandard=model.f_TariffStandard,
                RealBill=model.f_RealBill,
                Overruns=model.f_Overruns,
                TariffDate=model.f_TariffDate!=DateTime.MinValue? model.f_TariffDate.ToString("yyyy-MM-dd"):string.Empty,
                Registrant=model.f_Registrant,
                IsPayment=model.f_IsPayment,
                Rate=model.f_Rate,
                AddressId=model.f_AddressId,
                Remark=model.f_Remark
            });
        }
        /// <summary>
        /// 添加修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult AddOrEdit(Tariff model)
        {
            int result = 0;
            OAUser loginUser = base._session.GetCurrUser();
            model.f_operator = loginUser.NickName;
            model.f_operatorTime = DateTime.Now;
           
            if (model.f_Id > 0)
            {
                result = _tariffService.UpdateObject(model);
            }
            else
            {
                //收费人
                model.f_Rate = loginUser.NickName;
                //登记人
                model.f_Registrant = loginUser.NickName;
                result = _tariffService.AddObject(model);
            }

            return Json(result);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        public ActionResult Delete(int id)
        {
            int result = _tariffService.DeleteObject(id);

            return Json(result);
        }
        [HttpPost]
        [AjaxGetOrPost(MI.Core.Proxy.Type.Post)]
        /// <summary>
        /// 根据社区获取楼栋
        /// </summary>
        /// <param name="community"></param>
        /// <returns></returns>
        public ActionResult GetBuildingsByCommunity(string community)
        {
            IList<string> list = _dormitoryService.GetTariffbyCommunity(community);

            return Json(list);
        }
        [HttpPost]
        /// <summary>
        /// 判断房间是否存在
        /// </summary>
        /// <param name="community"></param>
        /// <param name="building"></param>
        /// <param name="roomNo"></param>
        /// <returns></returns>
        public ActionResult RoomIsExist(string community,string building,string roomNo)
        {
            t_Dormitory dormitory= _dormitoryService.GetTariffbyRoomNo(community,building,roomNo);
            int rId = 0;
            if (dormitory!=null)
            {
                rId = dormitory.f_DormitoryId;
            }
            return Json(rId);
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <returns></returns>
        public FileResult ExportExcel(TariffPagedInputDto input)
        {
            ExcelDto dto = _fileService.ExcelExport(input);
            string path = Path.Combine(HttpContext.Server.MapPath("~/Temp/Downloads"), dto.FileName);
            return File(path, "application/vnd.ms-excel", dto.FileName);
        }
    }
}
using System;
using System.Collections.Generic;

namespace MI.Application.Dto
{
    public class BedLinenDto
    {
        public BedLinenDto()
        {
            listRemarks = new List<string>();
        }

        /// <summary>
        /// 自增id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public int Eid { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime RegistrationDate { get; set; }
        /// <summary>
        /// 社区
        /// </summary>
        public string Community { get; set; }
        /// <summary>
        /// 楼栋
        /// </summary>
        public string Building { get; set; }
        /// <summary>
        /// 房间号
        /// </summary>
        public string RoomNo { get; set; }
        /// <summary>
        /// 床位类型（双人床Queen、单人床Single等）
        /// </summary>
        public string Hostels { get; set; }
        /// <summary>
        /// 床单数量
        /// </summary>
        public int? BunkNo { get; set; }
        /// <summary>
        /// 被单数量
        /// </summary>
        public int? SheetsNo { get; set; }
        /// <summary>
        /// 被子数量
        /// </summary>
        public int? QuiltNo { get; set; }
        /// <summary>
        /// 枕头数量
        /// </summary>
        public int? PillowcaseNo { get; set; }
        /// <summary>
        /// 当月送洗次数
        /// </summary>
        public int? Cont { get; set; }
        /// <summary>
        /// 加送被单数量
        /// </summary>
        public int? AddBunkNo { get; set; }
        /// <summary>
        /// 加送被子数量
        /// </summary>
        public int? AddSheetsNo { get; set; }
        /// <summary>
        /// 加送枕头数量
        /// </summary>
        public int? AddQuiltNo { get; set; }
        /// <summary>
        /// 加送枕头套数量
        /// </summary>
        public int? AddPillowcaseNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

        public List<string> listRemarks { get; set; }
    }
}

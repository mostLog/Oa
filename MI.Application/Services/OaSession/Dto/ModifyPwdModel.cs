using System.ComponentModel.DataAnnotations;

namespace MI.Application.Dto
{
    public class ModifyPwdModel
    {
        /// <summary>
        /// 空构造函数
        /// </summary>
        public ModifyPwdModel()
        { }
        /// <summary>
        /// 有一个参数的构造函数
        /// </summary>
        /// <param name="iEid"></param>
        public ModifyPwdModel(int iEid)
        {
            eid = iEid;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int eid { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "旧密码")]
        public string oldPwd { get; set; }  

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        [StringLength(10, MinimumLength = 3)]
        public string newPwd { get; set; } 
    }
}

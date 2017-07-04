using System.ComponentModel.DataAnnotations;

namespace MI.Application.Dto
{
    /// <summary>
    /// 登陆参数Dto
    /// </summary>
    public class LoginDto
    {
        [Required(ErrorMessage ="账号不能为空！")]
        [Display(Name ="账号")]
        public string AccountName { get; set; }
        [Required(ErrorMessage ="密码不能为空！")]
        [Display(Name ="密码")]
        public string Password { get; set; }
    }
}

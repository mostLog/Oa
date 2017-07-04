using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI.Core.Common
{
    public enum ErrorEnum : int
    {
        /// <summary>
        /// 失败
        /// </summary>
        [IODescription("失败")]
        Fail = 0,
        /// <summary>
        /// 成功
        /// </summary>
        [IODescription("成功")]
        Success = 1,
        /// <summary>
        /// 用户名或密码错误
        /// </summary>
        [IODescription("用户名或密码错误")]
        Wrong = 2,
        /// <summary>
        /// 不是管理员
        /// </summary>
        [IODescription("不是管理员")]
        NoAdmin = 3,
        /// <summary>
        /// 图片大小
        /// </summary>
        [IODescription("图片大小")]
        ImageSize = 4,
        /// <summary>
        /// 图片格式
        /// </summary>
        [IODescription("图片格式")]
        ImageFormat = 5,
        /// <summary>
        /// 图片为空
        /// </summary>
        [IODescription("图片为空")]
        ImageIsNull = 6,
        /// <summary>
        /// 图片宽高超出 1920x1080
        /// </summary>
        imgWidthHeightError = 7,
        /// <summary>
        /// 内部程序异常
        /// </summary>
        [IODescription("内部程序异常")]
        Error = 500,
        /// <summary>
        /// 其他表中有引用该表数据
        /// </summary>
        Quote = 600,
        /// <summary>
        /// 删除关联记录异常
        /// </summary>
        [IODescription("删除关联记录异常")]
        DelError = 501,
        /// <summary>
        /// 个人信息昵称为空
        /// </summary>
        [IODescription("个人信息昵称为空")]
        NickNameIsNull = 8,
        /// <summary>
        /// 原操作者不是当前用户，不能修改
        /// </summary>
        [IODescription("原操作者不是当前用户，不能修改")]
        OperationIsNotCurrent = 9,
        /// <summary>
        /// 资料不存在
        /// </summary>
        [IODescription("资料不存在")]
        InformationIsNull = 10,
        /// <summary>
        /// 资料无变动
        /// </summary>
        [IODescription("资料无变动")]
        InformationNotChange = 11,
        /// <summary>
        /// 长度超出规定限制
        /// </summary>
        [IODescription("长度超出规定限制")]
        LengthOutRange = 12,
        /// <summary>
        /// 记录已被删除，无法恢复
        /// </summary>
        [IODescription("记录已被删除")]
        RecordDeleted = 13
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SB.Common
{
    /* 公共的常用变量
     * add by ale to 2106/12/9 */
    public class PublicConstant
    {
        /*這裡的權限字符串只作用于是否有權限訪問該頁面，具體級別訪問數據的需要在頁面自己處理 */

        /// <summary>
        /// 经理
        /// </summary>
        public const int MANAGER = 1;
        /// <summary>
        /// 金流经理权限字符串
        /// (金流考核/客服考核/人員管理/考勤管理)
        /// </summary>
        public const string JL_MANAGER_REMARKS = "GeneralComment,RewardAndPunishment,UploadGroup,QueryGroup,CommentScoreAvg,AssessmentForAllItem,AssessmentForMultiItem,AssessmentForOneItem,AssessmentDirect,CheckTimeByEmp,CheckTimeByManage,StaffManage,JLCommentConfig,";
        /// <summary>
        /// 客服经理权限字符串
        /// (金流考核/客服考核/人員管理/考勤管理/考勤打卡)
        /// </summary>
        public const string KF_MANAGER_REMARKS = "GeneralComment,RewardAndPunishment,UploadGroup,QueryGroup,CommentScoreAvg,AssessmentForAllItem,AssessmentForMultiItem,AssessmentForOneItem,AssessmentDirect,CheckTimeByEmp,CheckTimeByManage,StaffManage,JLCommentConfig";
        /// <summary>
        /// 主任
        /// </summary>
        public const int DIRECTOR = 2;
        /// <summary>
        /// 金流主任权限字符串
        /// (金流考核/人員管理/考勤管理/考勤打卡)
        /// </summary>
        public const string JL_DIRECTOR_REMARKS = "GeneralComment,RewardAndPunishment,UploadGroup,QueryGroup,CommentScoreAvg,CheckTimeByEmp,CheckTimeByManage,StaffManage,JLCommentConfig,";
        /// <summary>
        /// 客服主任权限字符串
        /// (金流考核/客服考核/人員管理/考勤管理/考勤打卡)
        /// </summary>
        public const string KF_DIRECTOR_REMARKS = "GeneralComment,RewardAndPunishment,UploadGroup,QueryGroup,CommentScoreAvg,AssessmentForAllItem,AssessmentForMultiItem,AssessmentForOneItem,AssessmentDirect,CheckTimeByEmp,CheckTimeByManage,StaffManage,JLCommentConfig,";

        /// <summary>
        /// 副主任
        /// </summary>
        public const int VICEDIRECTOR = 2;
        /// <summary>
        /// 组长
        /// </summary>
        public const int LEADER = 3;
        /// <summary>
        /// 金流组长权限字符串
        /// (金流考核/人員管理/考勤管理/考勤打卡)
        /// </summary>
        public const string JL_LEADER_REMARKS = "GeneralComment,RewardAndPunishment,UploadGroup,QueryGroup,CommentScoreAvg,CheckTimeByEmp,CheckTimeByManage,StaffManage,JLCommentConfig";
        /// <summary>
        /// 客服组长权限字符串
        /// (客服考核/人員管理/考勤管理/考勤打卡)
        /// </summary>
        public const string KF_LEADER_REMARKS = "AssessmentForAllItem,AssessmentForMultiItem,AssessmentForOneItem,AssessmentDirect,CheckTimeByEmp,CheckTimeByManage,StaffManage,";

        /// <summary>
        /// 副组长
        /// </summary>
        public const int VICELEADER = 3;

        /// <summary>
        ///金流普通员工权限字符串
        ///大于组长3的都属于普通员工
        ///(金流考核/考勤打卡)
        /// </summary>
        public const string JL_EMPLOYEE_REMARKS = "GeneralComment,RewardAndPunishment,CommentScoreAvg,CheckTimeByEmp,WhetherCadre";/*WhetherCadre 是普通员工*/

        /// <summary>
        ///客服普通员工权限字符串
        ///大于组长3的都属于普通员工
        ///(客服考核/考勤打卡)
        /// </summary>
        public const string KF_EMPLOYEE_REMARKS = "AssessmentForAllItem,AssessmentForMultiItem,AssessmentForOneItem,CheckTimeByEmp,";

        /// <summary>
        ///行政普通员工权限字符串
        ///行政目前還未得知需要哪些 暫時為空，後續可能會在加行政方面的權限
        /// </summary>
        public const string XZ_EMPLOYEE_REMARKS = "";

        /// <summary>
        ///  默認XZ1001帳號 最高權限
        /// </summary>
        public const string HIGHEST_REMARKS = "GeneralComment,RewardAndPunishment,UploadGroup,QueryGroup,CommentScoreAvg,AssessmentForAllItem,AssessmentForMultiItem,AssessmentForOneItem,AssessmentDirect,CheckTimeByEmp,CheckTimeByManage,StaffManage,JLCommentConfig,";
        /// <summary>
        /// 最高權限帳號。(弃用)不需要使用xz1001帐号
        /// </summary>
        public static string HIGHEST_ACCOUNTNAME = "";
    }
}

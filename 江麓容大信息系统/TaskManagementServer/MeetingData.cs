using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using GlobalObject;
using TaskServerModule;

namespace TaskManagementServer
{
    /// <summary>
    /// 任务重要性
    /// </summary>
    public enum TaskImportance : byte
    {
        /// <summary>
        /// 紧急重要
        /// </summary>
        紧急重要 = 1,

        /// <summary>
        /// 紧急
        /// </summary>
        紧急 = 2,

        /// <summary>
        /// 重要
        /// </summary>
        重要 = 3,

        /// <summary>
        /// 普通
        /// </summary>
        普通
    }

    /// <summary>
    /// 会议提醒方式
    /// </summary>
    public enum MeetingAlarmMode : int
    {
        /// <summary>
        /// 短信及消息框提醒
        /// </summary>
        短信及消息框提醒 = 5,

        /// <summary>
        /// 仅短信提醒
        /// </summary>
        仅短信提醒 = 4,

        /// <summary>
        /// 仅消息框提醒
        /// </summary>
        仅消息框提醒 = 3
    }

    /// <summary>
    /// 会议状态
    /// </summary>
    public enum MeetingStatus
    {
        /// <summary>
        /// 待发
        /// </summary>
        待发,

        /// <summary>
        /// 已发
        /// </summary>
        已发,

        /// <summary>
        /// 撤销
        /// </summary>
        撤销,

        /// <summary>
        /// 等待分管领导审核
        /// </summary>
        待审
    }

    /// <summary>
    /// 日常事务类别
    /// </summary>
    public enum DailyWorkType : int
    {
        /// <summary>
        /// 会议
        /// </summary>
        会议 = 1
    }

    /// <summary>
    /// 会议数据
    /// </summary>
    public class MeetingData
    {
        /// <summary>
		/// 会议编号
        /// </summary>
		private string _会议编号;
		
        /// <summary>
        /// 会议编号
        /// </summary>
        public string 会议编号
        {
            get{ return _会议编号; }
            set{ _会议编号 = value; }
        }
        
		/// <summary>
		/// 事务类别
        /// </summary>
        private DailyWorkType _事务类别;

        /// <summary>
        /// 事务类别
        /// </summary>
        public DailyWorkType 事务类别
        {
            get{ return _事务类别; }
            set{ _事务类别 = value; }
        }
        
		/// <summary>
		/// 标题
        /// </summary>
		private string _标题;

        /// <summary>
        /// 标题
        /// </summary>
        public string 标题
        {
            get{ return _标题; }
            set{ _标题 = value; }
        }
        
		/// <summary>
		/// 开始时间
        /// </summary>
		private DateTime _开始时间;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime 开始时间
        {
            get{ return _开始时间; }
            set{ _开始时间 = value; }
        }
        
		/// <summary>
		/// 结束时间
        /// </summary>
		private DateTime _结束时间;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime 结束时间
        {
            get{ return _结束时间; }
            set{ _结束时间 = value; }
        }
        
		/// <summary>
        /// 会议资源对象集
        /// </summary>
		private List<View_PRJ_Resource> _会议资源对象集;

        /// <summary>
        /// 会议资源对象集
        /// </summary>
        public List<View_PRJ_Resource> 会议资源对象集
        {
            get { return _会议资源对象集; }
            set { _会议资源对象集 = value; }
        }

        /// <summary>
        /// 会议资源(将会议资源用逗号分隔)
        /// </summary>
        private string _会议资源;

        /// <summary>
        /// 会议资源(将会议资源用逗号分隔)
        /// </summary>
        public string 会议资源
        {
            get { return _会议资源; }
            set { _会议资源 = value; }
        }
  
		/// <summary>
		/// 创建人工号
        /// </summary>
		private string _创建人工号;

        /// <summary>
        /// 创建人工号
        /// </summary>
        public string 创建人工号
        {
            get{ return _创建人工号; }
            set{ _创建人工号 = value; }
        }
        
		/// <summary>
		/// 创建人姓名
        /// </summary>
		private string _创建人姓名;

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string 创建人姓名
        {
            get{ return _创建人姓名; }
            set{ _创建人姓名 = value; }
        }
        
		/// <summary>
		/// 创建日期
        /// </summary>
		private DateTime _创建日期;

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime 创建日期
        {
            get{ return _创建日期; }
            set{ _创建日期 = value; }
        }
        
		/// <summary>
		/// 重要性
        /// </summary>
        private TaskImportance _重要性;

        /// <summary>
        /// 重要性
        /// </summary>
        public TaskImportance 重要性
        {
            get{ return _重要性; }
            set{ _重要性 = value; }
        }
        
		/// <summary>
		/// 会议状态
        /// </summary>
        private MeetingStatus _会议状态;

        /// <summary>
        /// 会议状态
        /// </summary>
        public MeetingStatus 会议状态
        {
            get{ return _会议状态; }
            set{ _会议状态 = value; }
        }
        
		/// <summary>
		/// 主持人工号
        /// </summary>
		private string _主持人工号;

        /// <summary>
        /// 主持人工号
        /// </summary>
        public string 主持人工号
        {
            get{ return _主持人工号; }
            set{ _主持人工号 = value; }
        }
        
		/// <summary>
		/// 主持人姓名
        /// </summary>
		private string _主持人姓名;

        /// <summary>
        /// 主持人姓名
        /// </summary>
        public string 主持人姓名
        {
            get{ return _主持人姓名; }
            set{ _主持人姓名 = value; }
        }
        
		/// <summary>
		/// 记录人工号
        /// </summary>
		private string _记录人工号;

        /// <summary>
        /// 记录人工号
        /// </summary>
        public string 记录人工号
        {
            get{ return _记录人工号; }
            set{ _记录人工号 = value; }
        }
        
		/// <summary>
		/// 记录人姓名
        /// </summary>
		private string _记录人姓名;

        /// <summary>
        /// 记录人姓名
        /// </summary>
        public string 记录人姓名
        {
            get{ return _记录人姓名; }
            set{ _记录人姓名 = value; }
        }

        /// <summary>
        /// 与会人员对象集
        /// </summary>
        private List<PersonnelBasicInfo> _与会人员对象集;

        /// <summary>
        /// 与会人员对象集
        /// </summary>
        public List<PersonnelBasicInfo> 与会人员对象集
        {
            get { return _与会人员对象集; }
            set { _与会人员对象集 = value; }
        }

        /// <summary>
        /// 与会人员(将与会人员姓名用逗号分隔)
        /// </summary>
        private string _与会人员;

        /// <summary>
        /// 与会人员(将与会人员姓名用逗号分隔)
        /// </summary>
        public string 与会人员
        {
            get { return _与会人员; }
            set { _与会人员 = value; }
        }

		/// <summary>
		/// 提醒方式
        /// </summary>
        private MeetingAlarmMode _提醒方式;

        /// <summary>
        /// 提醒方式
        /// </summary>
        public MeetingAlarmMode 提醒方式
        {
            get{ return _提醒方式; }
            set{ _提醒方式 = value; }
        }

		/// <summary>
		/// 提醒提前分钟数
        /// </summary>
		private int _提醒提前分钟数;

        /// <summary>
        /// 提醒提前分钟数
        /// </summary>
        public int 提醒提前分钟数
        {
            get{ return _提醒提前分钟数; }
            set{ _提醒提前分钟数 = value; }
        }

		/// <summary>
		/// 会议正文
        /// </summary>
		private string _会议正文;

        /// <summary>
        /// 会议正文
        /// </summary>
        public string 会议正文
        {
            get{ return _会议正文; }
            set{ _会议正文 = value; }
        }
    }
}

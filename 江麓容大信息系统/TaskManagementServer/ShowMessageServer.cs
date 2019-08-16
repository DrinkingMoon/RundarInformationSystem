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
    /// 短信状态枚举
    /// </summary>
    public enum ShortMessageStatus
    {
        /// <summary>
        /// 只是保存起来而不发送
        /// </summary>
        暂存,

        /// <summary>
        /// 到了发送的时间就会立即发送短信
        /// </summary>
        待发送,

        /// <summary>
        /// 短信已经发送完成
        /// </summary>
        已发送
    }

    /// <summary>
    /// 短信操作服务
    /// </summary>
    class ShowMessageServer : TaskManagementServer.IShowMessageServer
    {
        /// <summary>
        /// 获取短信数据
        /// </summary>
        /// <param name="beginTime">发送短信的开始时间</param>
        /// <param name="endTime">发送短信的结束时间</param>
        public List<View_Task_ShortMessage> GetData(DateTime beginTime, DateTime endTime)
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var result = from r in ctx.View_Task_ShortMessage
                         where r.发送时间 >= beginTime && r.发送时间 <= endTime
                         select r;

            return result.ToList();
        }

        /// <summary>
        /// 获取所有的短信类别
        /// </summary>
        /// <returns>返回获取到的短信类别列表</returns>
        public Task_ShortMessageType[] GetShortMsgType()
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var result = from r in ctx.Task_ShortMessageType
                         select r;

            return result.ToArray();
        }

        /// <summary>
        /// 根据短信类别名称获取短信类别信息
        /// </summary>
        /// <param name="typeName">短信类别名称</param>
        /// <returns>返回获取到的短信类别信息</returns>
        private Task_ShortMessageType GetShortMsgType(string typeName)
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var result = from r in ctx.Task_ShortMessageType
                         where r.TypeName == typeName
                         select r;

            return result.Single();
        }

        /// <summary>
        /// 添加短信
        /// </summary>
        /// <param name="lstShortMsg">短信列表</param>
        /// <param name="status">短信状态</param>
        public void Add(List<View_Task_ShortMessage> lstShortMsg, ShortMessageStatus status)
        {
            //signing messages
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            Task_ShortMessage shortMsg = new Task_ShortMessage();

            DateTime date = ServerTime.Time;

            var result = from r in ctx.Task_ShortMessage
                     where r.ID.Substring(3, 4) == date.Year.ToString()
                     select r;

            int maxValue = 1;

            if (result.Count() > 0)
            {
                maxValue += (from r in ctx.Task_ShortMessage
                             where r.ID.Substring(3, 4) == date.Year.ToString()
                             select Convert.ToInt32(r.ID.Substring(9))).Max();
            }

            foreach (var item in lstShortMsg)
            {
                Task_ShortMessage addItem = new Task_ShortMessage();

                // HYX : 短信项
                addItem.ID = string.Format("SMX{0:D4}{1:D2}{2:D5}", date.Year, date.Month, maxValue);

                maxValue++;

                addItem.CreaterID = BasicInfo.LoginID;
                addItem.CreateTime = date;
                addItem.Receiver = item.接收人姓名;

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(item.接收人手机号) || !StapleFunction.IsHandset(item.接收人手机号))
                {
                    throw new Exception(string.Format("不正确的手机号码：{0}", item.接收人手机号));
                }

                addItem.MobileNo = item.接收人手机号;
                addItem.Content = item.短信内容;
                addItem.SendTime = item.发送时间;
                addItem.Status = status.ToString();
                addItem.Remark = item.备注;
                addItem.ShortMsgTypeID = GetShortMsgType(item.短信类别).TypeID;

                ctx.Task_ShortMessage.InsertOnSubmit(addItem);
            }

            ctx.SubmitChanges();
        }
         
        /// <summary>
        /// 删除短信
        /// </summary>
        /// <param name="lstShortMsgID">短信ID列表</param>
        public void Delete(List<string> lstShortMsgID)
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            foreach (var item in lstShortMsgID)
            {
                var result = from r in ctx.Task_ShortMessage
                             where r.ID == item
                             select r;

                ctx.Task_ShortMessage.DeleteAllOnSubmit(result);
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 更新短信列表中所有短信的状态
        /// </summary>
        /// <param name="lstShortMsgID">短信ID列表</param>
        /// <param name="status">更新后的状态</param>
        public void Update(List<string> lstShortMsgID, ShortMessageStatus status)
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            foreach (var item in lstShortMsgID)
            {
                var result = from r in ctx.Task_ShortMessage
                             where r.ID == item
                             select r;

                result.Single().Status = status.ToString();
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 更新指定短信的数据
        /// </summary>
        /// <param name="shortMsgID">短信ID</param>
        /// <param name="shortMsg">更新后的短信内容</param>
        public void Update(string shortMsgID, View_Task_ShortMessage shortMsg)
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var result = from r in ctx.Task_ShortMessage
                         where r.ID == shortMsgID
                         select r;

            Task_ShortMessage msg = result.Single();

            msg.Status = shortMsg.状态;
            msg.Content = shortMsg.短信内容;

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(shortMsg.接收人手机号) || !StapleFunction.IsHandset(shortMsg.接收人手机号))
            {
                throw new Exception(string.Format("不正确的手机号码：{0}", shortMsg.接收人手机号));
            }

            msg.MobileNo = shortMsg.接收人手机号;
            msg.Receiver = shortMsg.接收人姓名;
            msg.SendTime = shortMsg.发送时间;
            msg.Remark = shortMsg.备注;
            msg.CreateTime = ServerTime.Time;
            msg.ShortMsgTypeID = GetShortMsgType(shortMsg.短信类别).TypeID;

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 获取用户手机号码
        /// </summary>
        /// <param name="lstUser">用户基础信息列表</param>
        /// <returns>用户姓名与手机号码的字典</returns>
        public Dictionary<string, string> GetMobileNo(List<PersonnelBasicInfo> lstUser)
        {
            PlatformManagement.IUserManagement userManagement = PlatformManagement.PlatformFactory.GetUserManagement();

            Dictionary<string, string> dicUserInfo = new Dictionary<string, string>();

            foreach (var user in lstUser)
            {
                PlatformManagement.View_Auth_User authUser = userManagement.GetUser(user.工号);

                if (authUser != null)
                    dicUserInfo.Add(user.姓名, authUser.手机号码);
                else
                    dicUserInfo.Add(user.姓名, "");
            }

            return dicUserInfo;
        }
    }
}

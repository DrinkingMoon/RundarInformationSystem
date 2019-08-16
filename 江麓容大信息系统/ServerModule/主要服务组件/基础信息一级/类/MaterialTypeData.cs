using System;
using System.Collections.Generic;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 材料类别信息类
    /// </summary>
    public class MaterialTypeData : ICloneable
    {
        #region variants

        /// <summary>
        /// 材料类别编码
        /// </summary>
        string m_materialTypeCode;

        /// <summary>
        /// 材料类别名称
        /// </summary>
        string m_materialTypeName;

        /// <summary>
        /// 材料类别级数(用于构成树形结构)
        /// </summary>
        int m_materialTypeGrade;

        /// <summary>
        /// 是否末级(用于构成树形结构)
        /// </summary>
        bool m_isEnd;

        #endregion variants

        #region properties

        /// <summary>
        /// 获取或设置材料类别编码
        /// </summary>
        public string MaterialTypeCode
        {
            get { return m_materialTypeCode; }
            set { m_materialTypeCode = value; }
        }

        /// <summary>
        /// 获取或设置材料类别名称
        /// </summary>
        public string MaterialTypeName
        {
            get { return m_materialTypeName; }
            set { m_materialTypeName = value; }
        }

        /// <summary>
        /// 获取或设置材料类别级数(用于构成树形结构)
        /// </summary>
        public int MaterialTypeGrade
        {
            get { return m_materialTypeGrade; }
            set { m_materialTypeGrade = value; }
        }

        /// <summary>
        /// 获取或设置是否末级(用于构成树形结构)
        /// </summary>
        public bool IsEnd
        {
            get { return m_isEnd; }
            set { m_isEnd = value; }
        }

        #endregion properties

        #region ICloneable 成员

        /// <summary>
        /// 克隆对象信息
        /// </summary>
        /// <returns>克隆后的对象</returns>
        public object Clone()
        {
            MaterialTypeData info = new MaterialTypeData();

            info.MaterialTypeCode = this.MaterialTypeCode;
            info.MaterialTypeGrade = this.MaterialTypeGrade;
            info.MaterialTypeName = this.MaterialTypeName;
            info.IsEnd = this.IsEnd;

            return info;
        }

        #endregion
    }
}

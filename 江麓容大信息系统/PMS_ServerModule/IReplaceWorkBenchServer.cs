using System;
namespace ServerModule
{
    public interface IReplaceWorkBenchServer
    {
        /// <summary>
        /// 获得所有替换工位的信息
        /// </summary>
        /// <returns>获取成功返回dt，获取失败返回null</returns>
        System.Collections.Generic.List<ZPX_ReplaceWorkBench> GetAllData();

        /// <summary>
        /// 添加可替换工位的信息
        /// </summary>
        /// <param name="replace">可替换工位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool Insert(ZPX_ReplaceWorkBench replace, out string error);

        /// <summary>
        /// 删除可替换工位的信息
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool Delete(int id, out string error);

        /// <summary>
        /// 修改可替换工位的信息
        /// </summary>
        /// <param name="replace">可替换工位信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true 失败返回false</returns>
        bool Update(ZPX_ReplaceWorkBench replace, out string error);
    }
}

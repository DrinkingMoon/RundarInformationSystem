/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  DynamicCompiler.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;

namespace GlobalObject
{
    /// <summary>
    /// 动态编译代码
    /// </summary>
    public class DynamicCompiler
    {
        //C#代码提供对象
        CSharpCodeProvider m_cShrapProvider = new CSharpCodeProvider();

        /// <summary>
        /// 编译对象参数
        /// </summary>
        CompilerParameters m_comParameters = new CompilerParameters();

        /// <summary>
        /// 构造函数
        /// </summary>
        public DynamicCompiler()
        {
            //设置外部信息
            m_comParameters.CompilerOptions = "/optimize";
            m_comParameters.GenerateInMemory = true;
            m_comParameters.OutputAssembly = "DynamicCompiler";
            m_comParameters.ReferencedAssemblies.Add("System.dll");
            m_comParameters.ReferencedAssemblies.Add("System.Data.dll");
            m_comParameters.ReferencedAssemblies.Add("System.Xml.dll");

        }

        /// <summary>
        /// 编译代码处理
        /// </summary>
        /// <param name="code">方法代码</param>
        /// <returns>返回字符串</returns>
        string OperatorCode(string code)
        {
            string strCode = @"
            using System;
            namespace DynamicCompiler
            {
                  public class DynamicCompilerServer
                  {"
                   + code + 
                  @"
                  }
            }";

            return strCode;
        }

        /// <summary>
        /// 执行动态编译
        /// </summary>
        /// <param name="cSharpCode"></param>
        /// <param name="methodName"></param>
        /// <param name="paramters"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        object DynamicExecute(string cSharpCode,string methodName, object[] paramters, out string error)
        {
            error = null;

            string strCode = OperatorCode(cSharpCode);

            //编译
            CompilerResults results = m_cShrapProvider.CompileAssemblyFromSource(m_comParameters, strCode);

            //错误
            if (results.Errors.HasErrors)
            {
                foreach (CompilerError item in results.Errors)
                {
                    error += item.ErrorText + "\r\n";
                }

                return null;
            }
            else
            {
                //通过反射获得已编译的程序集
                Assembly ass = results.CompiledAssembly;

                //获得动态生成类的对象
                object objClass = ass.CreateInstance("DynamicCompiler.DynamicCompilerServer");

                //获取对象的方法
                MethodInfo objMethod = objClass.GetType().GetMethod(methodName);

                //调用对象的方法/获取值
                object objResult = objMethod.Invoke(objClass, paramters);

                return objResult;
            }
        }

        public object DynamicMath(Dictionary<string, object> dicParamter, string mathFormula, out string error)
        {
            error = null;
            //comParameters.ReferencedAssemblies.Add(System.AppDomain.CurrentDomain.BaseDirectory
            //    + "/DynamicCalculate.dll");

            //设置内部信息
            string methodName = null;

            string strCSharpCode = MathCode(dicParamter, mathFormula, out methodName);

            object objResult = DynamicExecute(strCSharpCode, methodName, null, out error);

            //object objResult = OperatorCode(strCSharpCode) ;

            if (objResult == null)
            {
                return null;
            }
            else
            {
                return objResult;
            }

        }

        string MathCode(Dictionary<string, object> dicParamter, string mathFormula, out string methodName)
        {
            methodName = "GetMathResult";

            string strMathFormulaTemp = mathFormula;

            if (mathFormula.Contains("if("))
            {
                strMathFormulaTemp = strMathFormulaTemp.Replace("{", "{ objValue = ");
                strMathFormulaTemp = strMathFormulaTemp.Replace("}", ";}");

                foreach (KeyValuePair<string, object> item in dicParamter)
                {
                    if (mathFormula.Contains(item.Key))
                    {
                        strMathFormulaTemp = strMathFormulaTemp.Replace(item.Key, item.Value.ToString());
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<string, object> item in dicParamter)
                {
                    if (mathFormula.Contains(item.Key))
                    {
                        strMathFormulaTemp = strMathFormulaTemp.Replace(item.Key, item.Value.ToString());
                    }
                }

                strMathFormulaTemp = "objValue = " + strMathFormulaTemp + ";";
            }


            string code = @"
                      public object GetMathResult()
                      {
                            object objValue = null;

                            " + strMathFormulaTemp + @"

                            return objValue;
                      }";
            return code;
        }
    }
}

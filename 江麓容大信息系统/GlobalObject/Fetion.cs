//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace GlobalObject
//{
//    class Fetion
//    {
//    }
//}

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Net;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public partial class Fetion
{
    static private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)   
    {   
        return true;   
    }

    internal class AcceptAllCertificatePolicy : ICertificatePolicy
    {
        public AcceptAllCertificatePolicy()
        {
        }

        public bool CheckValidationResult(ServicePoint sPoint, X509Certificate cert, WebRequest wRequest, int certProb)
        {
            // Always accept   
            return true;
        }
    }

    static public void Send(string phoneNumber, string content)
    {
        string url = @"https://sms.api.bz/fetion.php?username=13787176273&password=@Hello123World&sendto=" + phoneNumber + "&message=" + content;
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
        req.Method = "GET";

        //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
        ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

        using (WebResponse wr = req.GetResponse())
        {
            Console.WriteLine("短信发送成功");
            //Response.Write("短信发送成功！");
        }
    }
}
 


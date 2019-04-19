using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using System.Text.RegularExpressions;
using Microsoft.SharePoint;
using Microsoft.Office.Server.Security;
using System.Security.Cryptography;

namespace YR.Auth
{
    /// <summary>
    /// 自定义成员资格提供程序
    /// </summary>
    public class FBAMember : LdapMembershipProvider
    {
        public override bool ValidateUser(string username, string password)
        {
            if(password == Md5Hash(username))
            {
                return true;
            }
            else
            {
                return base.ValidateUser(username, password);
            }
        }

        public static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}

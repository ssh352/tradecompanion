Imports Microsoft.VisualBasic

Public Class ClsPswdGenerator

    '    using System.Security.Cryptography;
    'public static string getRandomAlphaNumeric()
    '        {

    '            RandomNumberGenerator rm;
    '            rm = RandomNumberGenerator.Create();

    '            byte[] data = new byte[3];

    '            rm.GetNonZeroBytes(data);

    '            string sRand = "";
    '            string sTmp = "";

    '            for (int nCnt = 0; nCnt <= data.Length - 1; nCnt++)
    '            {
    '                int nVal = Convert.ToInt32(data.GetValue(nCnt));

    '                if ((nVal >= 48 && nVal <= 57) || (nVal >= 65 && nVal <= 90) || (nVal >= 97 && nVal <= 122))
    '                {
    '                    sTmp = Convert.ToChar(nVal).ToString(); 
    '                }
    '                else
    '                {
    '                    sTmp = nVal.ToString();
    '                }

    '                sRand += sTmp.ToString();
    '            }

    '            return sRand;
    '        }



End Class

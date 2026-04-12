namespace Umbra.School.Shared
{
    public static class UtilityHelper
    {
        public static string GetFullName(string? firstName, string? lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
                return string.Empty;
            // Logic: If either name contains Chinese characters, use Chinese format
            if (UtilityHelper.ContainsChinese(firstName!) || UtilityHelper.ContainsChinese(lastName!))
            {
                return $"{lastName}{firstName}"; // No space for Chinese names
            }
            return $"{firstName} {lastName}"; // Space for English names
        }

        public static bool ContainsChinese(string text)
        {
            // Unicode range for Chinese characters
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"[\u4e00-\u9fa5]");
        }

        public static string AutoPassword()
        {
            return GeneratePassword("ALL", 8);
        }

        private static string GeneratePassword(string type, int length)
        {
            //定义密码字符的范围,小写,大写字母,数字以及特殊字符
            string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numnberChars = "0123456789";
            string specialCahrs = "~!@#$%^&*()_+|-=,./[]{}:;':";   //"\" 转义字符不添加 "号不添加

            string tmpStr = "";

            int iRandNum;
            Random rnd = new Random();

            length = length < 6 ? 6 : length; //密码长度必须大于6,否则自动取6

            // LOWER为小写 UPPER为大写 NUMBER为数字 NUMCHAR为数字和字母 ALL全部包含 五种方式
            //只有当选择UPPER才会有大写字母产生,其余方式中的字母都为小写,避免有些时候字母不区分大小写
            if (type == "LOWER")
            {
                for (int i = 0; i < length; i++)
                {
                    iRandNum = rnd.Next(lowerChars.Length);
                    tmpStr += lowerChars[iRandNum];
                }
            }
            else if (type == "UPPER")
            {
                for (int i = 0; i < length; i++)
                {
                    iRandNum = rnd.Next(upperChars.Length);
                    tmpStr += upperChars[iRandNum];
                }
            }
            else if (type == "NUMBER")
            {
                for (int i = 0; i < length; i++)
                {
                    iRandNum = rnd.Next(numnberChars.Length);
                    tmpStr += numnberChars[iRandNum];
                }
            }
            else if (type == "NUMCHAR")
            {
                int numLength = rnd.Next(length);
                //去掉随机数为0的情况
                if (numLength == 0)
                {
                    numLength = 1;
                }
                int charLength = length - numLength;
                string rndStr = "";
                for (int i = 0; i < numLength; i++)
                {
                    iRandNum = rnd.Next(numnberChars.Length);
                    tmpStr += numnberChars[iRandNum];
                }
                for (int i = 0; i < charLength; i++)
                {
                    iRandNum = rnd.Next(lowerChars.Length);
                    tmpStr += lowerChars[iRandNum];
                }
                //将取得的字符串随机打乱
                for (int i = 0; i < length; i++)
                {
                    int n = rnd.Next(tmpStr.Length);
                    //去除n随机为0的情况
                    //n = (n == 0) ? 1 : n;
                    rndStr += tmpStr[n];
                    tmpStr = tmpStr.Remove(n, 1);
                }
                tmpStr = rndStr;
            }
            else if (type == "ALL")
            {
                int numLength = rnd.Next(length - 1);
                //去掉随机数为0的情况
                if (numLength == 0)
                {
                    numLength = 1;
                }
                int charLength = rnd.Next(length - numLength);
                if (charLength == 0)
                {
                    charLength = 1;
                }
                int specCharLength = length - numLength - charLength;
                string rndStr = "";
                for (int i = 0; i < numLength; i++)
                {
                    iRandNum = rnd.Next(numnberChars.Length);
                    tmpStr += numnberChars[iRandNum];
                }
                for (int i = 0; i < charLength; i++)
                {
                    iRandNum = rnd.Next(upperChars.Length);
                    tmpStr += upperChars[iRandNum];
                }
                for (int i = 0; i < charLength; i++)
                {
                    iRandNum = rnd.Next(lowerChars.Length);
                    tmpStr += lowerChars[iRandNum];
                }
                for (int i = 0; i < specCharLength; i++)
                {
                    iRandNum = rnd.Next(specialCahrs.Length);
                    tmpStr += specialCahrs[iRandNum];
                }
                //将取得的字符串随机打乱
                for (int i = 0; i < length; i++)
                {
                    int n = rnd.Next(tmpStr.Length);
                    //去除n随机为0的情况
                    //n = (n == 0) ? 1 : n;
                    rndStr += tmpStr[n];
                    tmpStr = tmpStr.Remove(n, 1);
                }
                tmpStr = rndStr;
            }
            //默认将返回数字类型的密码
            else
            {
                for (int i = 0; i < length; i++)
                {
                    iRandNum = rnd.Next(numnberChars.Length);
                    tmpStr += numnberChars[iRandNum];
                }
            }
            return tmpStr;
        }
    }
}

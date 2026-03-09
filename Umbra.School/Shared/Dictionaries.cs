namespace Umbra.School.Shared
{
    public static class Dictionaries
    {
        public static Dictionary<string, string> WordsAssessmentScopes = new Dictionary<string, string>
        {
            { "shsv", "高中词汇" },
            { "shsp", "高中词组" },
            { "shpsv", "小学词汇" },
            { "shjhsv", "初中词汇" },
            { "myu", "我的生词库" },
        };
        public static Dictionary<string, string> WordsAssessmentSources = new Dictionary<string, string>
        {
            { "Library", "词库" },
            { "Unfamiliar", "生词（★<=3）" },
            { "ErrorNotes", "以往错题"}
        };
        public static Dictionary<string, string> WordsAssessmentMethods = new Dictionary<string, string>
        {
            { "random", "随机" },
            { "order", "顺序" }
        };
    }



    }

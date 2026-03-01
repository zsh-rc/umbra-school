using System.ComponentModel;
using System.Reflection;

namespace Umbra.School.Shared
{
    public enum AssessmentStatus
    {
        [Description("未开始")]
        NotStarted,
        [Description("正在进行测试")]
        InProgress,
        [Description("已提交审批")]
        Submitted,
        [Description("审批完毕")]
        Reviewed
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}

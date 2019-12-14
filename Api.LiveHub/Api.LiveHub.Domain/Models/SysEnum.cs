using System.ComponentModel;

namespace Api.LiveHub.Domain.Models
{

    public enum AMorPM
    {
        [Description("上午")]
        Forenoon = 0,
        [Description("下午")]
        Afternoon = 1
    }
    public enum AccountStatus
    {
        Disabled = 0,
        Enabled = 1,
        Leave = -1
    }
    public enum BusinessType
    {
        [Description("出差")]
        Away = 0,
        [Description("回程")]
        Return = 1,
        [Description("中转")]
        Transit = 2
    }

    public enum DownloadDate
    {
        Yesterday = 0,
        Today = 1,
        LastWeek = 2,   
        ThisWeek = 3,
        LastMonth = 4,
        ThisMonth =5
    }

    public enum BSType
    {
        [Description("出差")]
        出差 = 0,
        [Description("回程")]
        回程 = 1,
        [Description("中转")]
        中转 = 2
    }

    public enum Time
    {
        [Description("上午")]
        上午 = 0,
        [Description("下午")]
        下午 = 1
    }
    public enum BusinessStatus
    {
        已提交 = 0,
        已确认 = 1,
        已退回 = 2,
    }

    public enum ToDoStatus
    {
        未完成 = 0,
        已完成 = 1,
        已删除 = 2,
    }

    public enum ToDoType
    {
        ToDo = 0,
        Things = 1,
        Training = 2,
    }

}

namespace CoursesManagment.Core.Enums
{
    public enum UserStatusEnum
    {
        Active,
        NotActive

    }
    public enum CourseStatusEnum
    {
        Pending = 1,
        Publish = 2
    }
    public enum DaysEnum
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7,
    }
    public enum FrequencySettingEnum
    {
        Hourly = 1,
        Daily = 2,
        Monthly = 3,
        Quarterly = 4,
        Yearly = 5,
    }

    public enum FrequencyEnum
    {
        Year = 1,
        Quarter = 2,
        Month = 3,
        Week = 4,
        LastWeek = 5
    }

}

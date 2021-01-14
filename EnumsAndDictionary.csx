static class Constant
{
    [Flags]
    public enum DeviceEnum
    {
        电网 = 0,
        风机 = 1
    }

    public static void getEnum(int i)
    {
        var dic = new Dictionary<int, DeviceEnum>{
            {1,DeviceEnum.电网},
            {2,DeviceEnum.风机}
        };
        dic[i].Dump();
    }
}

Constant.DeviceEnum.电网.Dump();
Constant.getEnum(1);


public enum RoleEnum
{
    未知 = 0,
    超级管理员 = 1,
    管理员 = 2,
    普通用户 = 4,
    临时用户 = 8,
    审核员 = 16,
    审查员 = 32
}

Enum.Parse(typeof(RoleEnum), "1").Dump();
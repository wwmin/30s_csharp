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

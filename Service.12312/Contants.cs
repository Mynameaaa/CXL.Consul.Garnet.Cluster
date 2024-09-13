namespace Service._12312;

public class Contants
{

    private static int Count = 0;

    public static void GetAndSet()
    {
        Console.WriteLine("当前调用次数：【{0}】", Count += 1);
    }
}

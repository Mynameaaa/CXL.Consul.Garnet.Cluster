using StackExchange.Redis;
using System.Linq;

//string[] connections = new string[]
//{
//    "localhost:9100,Password=Password",
//    "localhost:9101,Password=Password",
//    "localhost:9102,Password=Password",
//    "localhost:9103,Password=Password",
//    "localhost:9104,Password=Password",
//    "localhost:9105,Password=Password",
//};

//var connection = ConnectionMultiplexer.Connect(string.Join(",", connections));
//var db = connection.GetDatabase();

//for (int i = 0; i < 10000; i++)
//{
//    await db.StringSetAsync($"Key_{i}", $"Value_{i}");
//}

////List<AAA> numbers = new List<AAA>();

////var result = numbers.DefaultIfEmpty();

////foreach (var number in result)
////{
////    Console.WriteLine(number);
////}

Console.WriteLine(5 / 3);


Console.WriteLine("Garnet End....");
Console.Read();


class AAA
{
    public string Name { get; set; } = "aaa";
}

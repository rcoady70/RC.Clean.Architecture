
//Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}");
//var tea = await MakeTeaAsync();
//Console.WriteLine(tea);

using RC.CA.Scratch;

TreeTest _tree = new TreeTest();
_tree.Run();


async Task<string> MakeTeaAsync()
{
    var waterTASK = BoillWaterAsync();
    Console.WriteLine($"Take the cups out t-{Thread.CurrentThread.ManagedThreadId}");

    int a = 0;
    for (int i = 0; i < 10000000000; i++)
    {
        a += i;
    }

    Console.WriteLine($"Put tea in cups t-{Thread.CurrentThread.ManagedThreadId}");
    var water = await waterTASK;
    var tea = $"pour {water} in cups t-{Thread.CurrentThread.ManagedThreadId}";

    return tea;
}

async Task<string> BoillWaterAsync()
{
    Console.WriteLine($"Kettle start t-{Thread.CurrentThread.ManagedThreadId}");
    Console.WriteLine($"Kettle boiling t-{Thread.CurrentThread.ManagedThreadId}");
    await Task.Delay(2000);
    Console.WriteLine($"Kettle finished t-{Thread.CurrentThread.ManagedThreadId}");

    return "water";
}

class Node
{
    public int Id { get; set; }
    public List<int> Children = new List<int>();
}

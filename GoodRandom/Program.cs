// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using GoodRandom;

var random = new GoodRandomGenerator(new SecureRandomProvider(1000));
var fastRandom = new GoodRandomGenerator(new PseudoRandomProvider(1000));
void rollTester(int sides, int count, GoodRandomGenerator generator)
{
    //var rolls = Enumerable.Range(1, sides).Select(i => (i, 0)).ToDictionary(x => x.i, x => x.Item2);

    var stopwatch = Stopwatch.StartNew();
    //foreach (var choice in generator.GetDiceRolls(sides, count))
    //{
    //    //rolls[choice] =  rolls[choice] + 1;
    //}
    for(var i = 0; i < count; i++) { 
        var roll = generator.GetFairInt(1, sides+1);
        //rolls[roll] =  rolls[roll] + 1;
    }
    stopwatch.Stop();
    //var sb = new StringBuilder();
    //foreach (var roll in rolls)
    //{
    //    sb.AppendLine($"{roll.Key}:{roll.Value} {(double)(roll.Value) / count}");
    //}
    //Console.WriteLine(sb.ToString());
    Console.WriteLine($"Rolling took {stopwatch.ElapsedMilliseconds}ms");
}

void testBasicRandomNumberGenerator(int sides, int count) {
    //var rolls = Enumerable.Range(1, sides).Select(i => (i, 0)).ToDictionary(x => x.i, x => x.Item2);

    var stopwatch = Stopwatch.StartNew();
    for(var i = 0; i < count; i++) { 
        var roll = RandomNumberGenerator.GetInt32(1, sides+1);
        //rolls[roll] =  rolls[roll] + 1;
    }
    stopwatch.Stop();
    //var sb = new StringBuilder();
    //foreach (var roll in rolls)
    //{
    //    sb.AppendLine($"{roll.Key}:{roll.Value} {(double)(roll.Value) / count}");
    //}
    //Console.WriteLine(sb.ToString());
    Console.WriteLine($"Rolling took {stopwatch.ElapsedMilliseconds}ms");
    Console.WriteLine("------");
}

List<int> roll(int times, int sides) => random.GetDiceRolls(sides, times).ToList();

List<int> stats()
{
    List<int> stats = new List<int>();
    for (var i = 0; i < 6; i++)
    {
        var rolls = roll(4, 6);
        var top3 = rolls.ToList().OrderByDescending(r => r).Take(3);
        var sum = top3.Sum();
        stats.Add(sum);
    }

    Console.WriteLine($"{string.Join(',', stats.OrderByDescending(r=>r))}");
    return stats;
}

void badStats()
{
    List<int> rolls;
    do
    {
        rolls = stats();
    } while (rolls.Min() > 5);
}

void roller() 
{
    ConsoleKeyInfo key = Console.ReadKey();
    
    while(key.Key != ConsoleKey.Q)
    {
        Console.WriteLine(roll(1,20).First());
        key = Console.ReadKey(true);
    }
}

void unfair() {
    var sides = 3;
    var count = 100000000;
    var rolls = Enumerable.Range(0, sides).Select(i => (i, 0)).ToDictionary(x => x.i, x => x.Item2);
    for (var i = 0; i < count; i++) {
        var choice = RandomNumberGenerator.GetInt32(int.MaxValue) % sides;
        rolls[choice] = rolls[choice] + 1;
    }
    foreach (var roll in rolls)
    {
        Console.WriteLine($"{roll.Key}:{roll.Value} {(double)(roll.Value) / count}");
    }
}

var stopwatch = Stopwatch.StartNew();
var testSize = 1000000;
var sides = 20; //int.MaxValue - 1;
Console.WriteLine($"Testing {testSize} iterations");
Console.WriteLine("System.Cryptography Random Number Generator");
rollTester(sides, testSize, random);
Console.WriteLine("System Random Number Generator");
rollTester(sides, testSize, fastRandom);
Console.WriteLine("System.Crytography.RandomNumberGenerator.GetInt32()");
testBasicRandomNumberGenerator(sides, testSize);
Console.WriteLine($"all took {stopwatch.ElapsedMilliseconds}ms");
//badStats();
//Console.WriteLine(roll(1,8).First());
//Console.WriteLine($"{string.Join(' ', roll(4,8))}");
//roller();
//Console.WriteLine(roll(1, 15).First());
//stats();

Console.WriteLine("End");

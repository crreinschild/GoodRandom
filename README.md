# GoodRandom

Small project to fairly choose from a set of objects (numbers, letters, objects, etc). Specifically removes bias that arise from using the modulus of a random number that unfairly favors lower numbers.

## Usage

```CSharp
var r = new GoodRandomGenerator(new SecureRandomProvider());

// simple dice roll
var oneD20 = r.GetDiceRoll(20);
Console.WriteLine($"D20: {oneD20}"); // D20: 13

// multiple dice rolls
var twoD4 = r.GetDiceRolls(4, 2);
Console.WriteLine($"2d4: {string.Join(',', twoD4)}"); // 2d4: 2,1

// another method that shows how the dice roll is implemented
var diceSides = Enumerable.Range(1,6).ToArray();
var fourD6 = r.GetFairRandomSet(diceSides, 4);
Console.WriteLine($"4d6: {string.Join(',', fourD6)}"); // 4d6: 3,2,2,6

// can use strings too!
var stringSet = "Hello World";
var randomString = new string(r.GetFairRandomSet(stringSet.ToCharArray(), 10).ToArray());
Console.WriteLine($"string: {randomString}"); // string:  r HlHWW W

// spicy
var objects = new object[] { 2, "42", new Random(), new Exception() };
var randomObjects = r.GetFairRandomSet(objects, 3);
Console.WriteLine($"objects??: {string.Join(',', randomObjects.Select(o => o.GetType()))}"); 
// objects??: System.Exception,System.Int32,System.Exception 
```

## Goals

This project was literally just me wanting to roll dice for DnD that I know isn't going to secretly cheat me. Technically there is now a much easier way to choose a fair random number in dotnet with the new RandomNumberGenerator class and the GetInt32(min, max) function, but this was me messing about trying to roll as many fairly choosen randoms as quickly as possible.

Also, I've been putting off maintaining a portfolio, so I decided to put some of my various projects out for people to see, use, and share. I know this project would have been very useful in my previous role.

### Guarantee Pick a Fair Random Number

Once upon a time, it wasn't as trivial to fairly choose a secure random number, like generating random strings for temporary passwords and such. It involved messing around with something like RNGCryptographicProvider and getting a random int and using the modulo of that: `int choice = random % max;`. This has an underlying (though very small) bias towards the lower numbers. If you divide a large number by a small number, you are likely to have a remainder. If you don't discard this remainder, you will end up assigning that number to one of your choices, giving that lower choice a (possibly insignificant) advantage. *When it comes to either security or gaming, this bias may not be so insignificant; especially when you roll a nat 1.*

### Assign Choices Among Any Given Set

Once upon I time, I tasked myself with updating some pretty bad code for generating random temporary passwords/activation codes. I spent a lot of time on picking fair numbers, but never really had a nice way to assign those fair numbers to the actual letters, numbers, and symbols. I settled for the typical random indexes into an string "alphabet" solution. Here I do the same thing, but I want to make it much easier to allow the caller to provide the set to choose from. This means you can use the same function to generate passwords, any kind of dice roll, or if you're feeling spicy, any instance of type `object`. 

### Gotta Go Fast

I really want it to be as fast as possible without eating much memory. So far, it's still roughly on par with RandomNumberGenerator GetInt32, being faster on rolling for smaller numbers, and slightly slower when the number of sides expands higher than can fit in a byte. On my machine, it can roll 100 million d20's in about 8 seconds, while RandomNumberGenerator.GetInt32 takes 14 seconds. Rolling 100 million d(int.MaxValue-1)'s roughly flips these results.

A couple of things have helped though. For one, using IEnumerable and lazy returns make it so that pulling 1 value from the results costs virtually nothing, as with the next million values. Another improvement is random byte batching, but this only really helps for extreme cases (like 100 million randoms), and can be turned off to minimize keeping these numbers in memory in the case of security concerns.

*I haven't added an asynchronous interface yet because I've found that introducing asynchronicity to things that take very little time just adds way too much overhead; especially when you can generate 1 million randoms in 80ms a single i7 core. It's not off the table though, just not a high priority for me.*

### General Cleanness and Testability

It's honestly a mess right now. I mainly used Program.cs to roll stats for my DnD characters and running various manual performance tests. I also moved the actual RNG providers out with some IoC so that I can manually test things easier, and provide testability in the (hopefully) near future. I plan on cleaning it up, turning Program.cs into a proper command-line interface and add a wide variety of unit tests. I also want to properly document everything and design a nice interface that makes sense.

### Use in Other Projects

This will likely just be for me and my own projects. I probably won't create a package for it, and honestly have other projects I much rather work on than maintaining this as a public library. That being said, if there really is anyone out there who wants to use this code, feel free to copy/fork/reference any bits of this code for your projects. Also, if you read this far, congratulations, you win no prizes!
namespace ConsoleApp1;

// The challenge tells me to slightly modify the program from Level 36 to use lambda expressions for the constructor
// instead of named methods.

/*
        Sieve newsomething = new(Sieve.TenMultiple);
        Console.WriteLine("Enter a number to test it out!");
        while (true)
            if (int.TryParse(Console.ReadLine(), out int x)) Console.WriteLine(newsomething.IsGood(x).ToString());
*/

// Instead of Sieve newsomething = new(MethodName), we could use
// n => n % 10 == 0 for TenMultiple method,
// n => n % 2 == 0 for EvenNumber method,
// n => n % 2 != 0 for OddNumber method.

// Question 1: Does this change make the program shorter or longer?
// Answer: Shorter, obviously?

// Question 2: Does this change make the program easier to read or harder?
// Answer: I guess it makes it a bit easier to read.
// (Author has a notable answer: https://csharpplayersguide.com/solutions/5th-edition/the-lambda-sieve)
﻿// See https://aka.ms/new-console-template for more information


using ChattApi;

Console.WriteLine("Hello, World!");

var api = new Api(Authentication, s => Task.Run(() => Console.WriteLine(s)));
while (true)
{
    Console.WriteLine("Write something to server:");
    api.SendMessage(Console.ReadLine());
    Console.WriteLine("Sending");
}



return;

string Authentication()
{
    while (true)
    {
        Console.WriteLine("What's your username?");
        var name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name)) return name;

        Console.WriteLine("Illegal username. Try again!");
    }
}
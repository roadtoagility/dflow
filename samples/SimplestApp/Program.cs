﻿// Copyright (C) 2020  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using SimplestApp.Operations;
using SimplestApp.Services;
using SimplestApp.Specifications;

namespace SimplestApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("== Simple App to Create a User");

            var spec = new UserValidSpecification();
            var us = new UserService(spec);

            var user = us.Add(new AddUser { Name = "My name", Mail = "my@mail.com" });

            Console.WriteLine();
            Console.WriteLine($"My name is {user.Name}, my mail is {user.Mail} i'm a valid {user.IsValid}");
            Console.WriteLine();
            Console.WriteLine("press any key to exit.");
            Console.ReadKey();
        }
    }
}
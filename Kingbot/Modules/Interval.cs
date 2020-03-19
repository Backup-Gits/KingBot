﻿using Kingbot.Helpers.Data;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Kingbot.Modules
{
    class Interval
    {
        // Dictionary for storing all the timers. This is the heart of the interval system
        private static Dictionary<string, Timer> intervals = new Dictionary<string, Timer>();

        // Handles command string given from the CommandHandler
        public static void Handle(List<String> words)
        {
            string instruction = words[1].ToLower();
            string name = words[2];

            switch (instruction)
            {
                case "start":
                    string ms = words[3];

                    StartInterval(name, int.Parse(ms));
                    break;
                case "stop":
                    StopInterval(name);
                    break;
                case "add":
                    words.RemoveRange(0, 3);
                    string message = String.Join(" ", words.ToArray());
                    AddInterval(name, message);
                    break;
                case "remove":
                    DataHelper.Delete("intervals", name);
                    break;
            }
        }

        // Add a new interval phrase and message into the database

        // TODO: Add ensure check
        private static void AddInterval(string name, string message)
        {
            DataHelper.Write("intervals", name, message);
        }

        /*
         * Flow
         * 1. Store the name and the interval timer inside the dictionary for easy access
         * 2. Set the interval's ms, event, and start it
         * 3. Keep posting a message to the channel until the stop command is executed
         */
        private static void StartInterval(string name, int ms)
        {
            string message = DataHelper.Read("intervals", name);
            intervals[name] = new Timer(ms);
            intervals[name].Elapsed += (sender, e) => OnTimedEvent(sender, message);
            intervals[name].AutoReset = true;
            intervals[name].Start();
        }

        private static void OnTimedEvent(object sender, string message)
        {
            TwitchBot.client.SendMessage(TwitchBot.channel, message);
        }

        // Stops already existing interval message. Checks if the interval even exists
        private static void StopInterval(string name)
        {
            if (intervals.ContainsKey(name))
                intervals[name].Stop();
            else
                TwitchBot.client.SendMessage(TwitchBot.channel, "This timer doesn't exist! Perhaps you never started it?");
        }
    }
}

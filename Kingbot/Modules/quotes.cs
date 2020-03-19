﻿using System;
using Kingbot.Helpers.Data;
using System.Collections.Generic;

namespace Kingbot.Modules
{
    class Quotes
    {
        /*
         * Handles all quote related commands
         * Links up to the mongodb database for reading
         * and saying in the twitch client.
         */

        public static void Handle(List<String> words)
        {
            string instruction = words[1].ToLower();
            string index = words[2];

            switch (instruction)
            {
                case "retrieve":
                    TwitchBot.client.SendMessage(TwitchBot.channel, ReturnQuote(index));
                    break;
                case "add":
                    words.RemoveRange(0, 3);
                    string message = String.Join(" ", words.ToArray());
                    AddQuote(index, message);
                    break;
                case "delete":
                    DataHelper.Delete("quotes", index);
                    break;
            }
        }

        /*
         * Get the quote from the database and return it to the
         * calling function.
         */
        private static string ReturnQuote(string index)
        {
            var result = DataHelper.Read("quotes", index);

            if (result == null)
                return "This quote doesn't exist! Try adding it?";
            else
                return result;
        }

        /*
         * Add a new quote into the database
         * 
         * TODO: Use the EnsureQuote check to see if the quote exists
         * already to prevent overwriting.
         * 
         * Quotes cannot be updated unless executed by an admin
         */

        private static void AddQuote(string index, string message)
        {
            //TODO: Add ensurequote here

            DataHelper.Write("quotes", index, message);
        }
    }
}

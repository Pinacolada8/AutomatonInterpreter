using System;
using System.IO;
using App.Extensions;
using App.Logging;
using Business.Error;
using Business.Input;
using Newtonsoft.Json;

namespace App
{
    class Program
    {
        public static void Main(string[] args)
        {
            LogController.Setup();

            var logger = LogController.GetLogger();

            if (args == null || args.Length < 1)
            {
                logger.Error(@"Missing input file argument");
                System.Environment.Exit((int)ReturnCodes.MISSING_FILE_ARGUMENT);
                return;
            }

            var inputFilePath = args[0];
            logger.Debug(inputFilePath);

            JsonInput inputJson;

            // Reading File
            try
            {
                var fileStr = File.ReadAllText(inputFilePath);
                inputJson = JsonConvert.DeserializeObject<JsonInput>(fileStr);
            }
            catch (Exception exception)
            {
                logger.Error(@"Failed reading input file");
                logger.Error(exception);
                System.Environment.Exit((int)ReturnCodes.FILE_READING_ERROR);
                return;
            }

            var automaton = inputJson.AP.ToDeterministicStackAutomaton();
            var running = true;

            while (running)
            {
                var firstKey = Console.ReadKey();
                if (firstKey.Modifiers == ConsoleModifiers.Control && firstKey.Key == ConsoleKey.D)
                {
                    logger.Debug(@"Closing");
                    System.Environment.Exit((int)ReturnCodes.SUCCESS);
                }
                if(firstKey.Key == ConsoleKey.Enter)
                    continue;

                var word = firstKey.KeyChar + Console.ReadLine();
                logger.Debug(word);
                try
                {
                    var result = automaton.TestWord(word);
                    logger.Info(result ? @"Sim" : @"Não");
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    logger.Info(@"Não");
                }
            }
        }
    }
}
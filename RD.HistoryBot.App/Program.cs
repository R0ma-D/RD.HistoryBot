using System.Reflection;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RD.HistoryBot.App.DAL;
using RD.HistoryBot.App.DAL.Google;
using RD.HistoryBot.App.DAL.InMemory;
using RD.HistoryBot.App.Model;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace RD.HistoryBot.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Log("Starting history bot...");
            
            try
            {
                var memoryCache = new MemoryCache(new MemoryCacheOptions());
                
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets(typeof(Program).GetTypeInfo().Assembly)
                    .AddEnvironmentVariables()
                    .Build();

                var testStudents = config["Bot:Admins"]?
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Select(x => new Student
                    {
                        Login = x,
                        UserName = x,
                        ExpirationDate = DateTime.UtcNow.AddDays(1),
                        AvailableThemes = "1-5",
                    }).ToArray() ?? Array.Empty<Student>();

                var sheetService = GetSheetsService(config);
                var topicAndQuestionRepository = new GoogleTopicAndQuestionRepository(sheetService, config["Google:QuestionFile"]!);
                var studentRepository = new GoogleStudentRepository(sheetService, config["Google:StudentFile"]!);
                await Task.WhenAll(topicAndQuestionRepository.Reload(), studentRepository.Reload());

                var messageHandler = new MessageHandler(studentRepository, topicAndQuestionRepository, topicAndQuestionRepository, memoryCache);

                var botClient = GetBotClient(config);

                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                var receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = { }, // receive all update types
                };
                
                botClient.StartReceiving(
                    messageHandler.HandleUpdateAsync,
                    messageHandler.HandleErrorAsync,
                    receiverOptions,
                    cancellationToken
                );
                var me = await botClient.GetMeAsync(cancellationToken);

                Log($"Bot {me.Username} is ready");

                while (true)
                {
                    var input = Console.ReadLine()?.ToLower();
                    if (input == "exit")
                    {
                        cts.Cancel();
                        return;
                    }
                    Console.WriteLine("To stop bot, type 'exit' and press Enter");
                }
            }
            catch (Exception ex)
            {
                Log(ex);
                throw;
            }
        }

        private static ITelegramBotClient GetBotClient(IConfiguration config)
        {
            var token = config["Bot:Token"];
            return new TelegramBotClient(token!);
        }

        private static SheetsService GetSheetsService(IConfiguration config)
        {
            var serviceAccountJson = config["Google:Account"];
            return new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "history-bot",
                HttpClientInitializer = GoogleCredential.FromJson(serviceAccountJson)
                    .CreateScoped(new[] { SheetsService.Scope.Spreadsheets }),
            });
        }

        private static void Log(string message)
        {
            Console.WriteLine(message);
        }

        private static void Log(Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Reflection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;
using System.Threading;
using System.Runtime.InteropServices;

namespace Magic_Conch
{
    public class Program
    {
        private CommandService commands;
        private DiscordSocketClient client;
        private static QuestionHandler questionHandler;
        private IServiceProvider services;
        private ISocketMessageChannel currentChannel;
        private SocketUser lastUser;
       
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            Console.WriteLine("Serializing Memory");       

            Thread.Sleep(2000);

            questionHandler.Save();

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }

        static void Main(string[] args)
        {
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            new Program().Start().GetAwaiter().GetResult();
        }

        public async Task Start()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            questionHandler = new QuestionHandler();
            
            client.Log += Log;

            string token = "NDQyMjMxNTg5MDIwMTA2NzUz.Dc-g7w.prK7budzdl9vd-lRe2qZhZDm__g";

            services = new ServiceCollection()
                    .BuildServiceProvider();

            SimpleHandelQuestions();

            await InstallCommands();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            //TestMemory(); //Used to test new aspects of memory serialization and deserialization

            await Task.Delay(-1);            
        }

        //Allows for a more customized response based on type.
        private void HandelQuestions()
        {
            questionHandler.ChoiceAwnsered += (s, e) =>
            {
                
            };

            questionHandler.FavoriteAwnsered += (s, e) =>
            {

            };

            questionHandler.KnowEverythingAsked += (s, e) =>
            {

            };

            questionHandler.YesNoQuestion += (s, e) =>
            {

            };

            questionHandler.UnknownQuestion += (s, e) =>
            {

            };

            //Optional: Set Frustrated Threshold To 0 to disable
            questionHandler.Frustrated += (s, e) =>
            {

            };

            //Optional: When Insulted Will Respond With Random Insult
            questionHandler.Insulted += (s, e) =>
            {
                currentChannel.SendMessageAsync(e.awnser);
            };
        }

        //Allows for quick setup but generic responses
        private void SimpleHandelQuestions()
        {
            questionHandler.QuestionAwnsered += (s, e) =>
            {
                currentChannel.SendMessageAsync(e.awnser);
            };

            questionHandler.UnknownQuestion += (s, e) =>
            {
                currentChannel.SendMessageAsync(e.message);
            };

            //Optional: Set Frustrated Threshold To 0 to disable
            questionHandler.Frustrated += (s, e) =>
            {
                currentChannel.SendMessageAsync(e.awnser);
            };
            
            //Optional: When Insulted Will Respond With Random Insult
            questionHandler.Insulted += (s, e) =>
            {
                currentChannel.SendMessageAsync(e.awnser);
            };

            questionHandler.EncodedMessage += (s, e) =>
            {
                lastUser.SendMessageAsync($"Key: {e.key} -- Message: {e.message}");
            };
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public async Task InstallCommands()
        {
            // Hook the MessageReceived Event into our Command Handler
            client.MessageReceived += HandleCommand;

            // Discover all of the commands in this assembly and load them.
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommand(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            currentChannel = message.Channel;

            if (message.Content == "!" || message.Content.Contains("!!") || message.Content.Contains("!_") || message.Content.Contains("! "))
                return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix('!', ref argPos)))
            {
                lastUser = message.Author;
                if(message.HasMentionPrefix(client.CurrentUser, ref argPos))
                {
                    if (message.Channel.Id == 117057226199400451)
                    {
                        await message.Channel.SendMessageAsync("Cause Keegan doesnt like it please use the bots channel so i dont get banned. Thank you.");
                        return;
                    }

                    List<string> parts = message.Content.Split(' ').ToList();
                    parts.RemoveAt(0);
                    StringBuilder sb = new StringBuilder();
                    foreach(string s in parts)
                    {
                        sb.Append(s + " ");
                    }

                    questionHandler.HandleQuesiton(sb.ToString());                    
                }

                return;
            }
            // Create a Command Context
            var context = new CommandContext(client, message);
            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await commands.ExecuteAsync(context, argPos);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
       
    }
}
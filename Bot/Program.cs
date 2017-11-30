using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bot
{
    class Program
    {
        private DiscordSocketClient DiscordSocketClient;

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }
           
        public async Task MainAsync()
        {
            DiscordSocketClient = new DiscordSocketClient();

            DiscordSocketClient.Log += Log;
            DiscordSocketClient.MessageReceived += MessageReceived;

            string token;
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader("token.txt");
                token = file.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            await DiscordSocketClient.LoginAsync(TokenType.Bot, token);
            await DiscordSocketClient.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask; 
        }

        private async Task MessageReceived(SocketMessage message)
        {
            var channel = message.Channel;
            var help_commands = "```" +
                "\n!hola: El bot 100% real no feik te saluda y te bendice :v" +
                "\n!gay: Descubre tu mismo lo que hace" +
                "\n!real: Questionas la realidad del bot" +
                "\n!borrar: Borra los ultimos 100 mensajes" +
                "\n!" +
                "```";

            switch (message.Content)
            {
                case "!comandos":
                    await message.Channel.SendMessageAsync(help_commands);
                    break;
                case "!hola":
                    await message.Channel.SendMessageAsync("Hola " + message.Author.Mention + " te bendigo papu :v");
                    break;
                case "!gay":
                    await message.Channel.SendMessageAsync(message.Author.Mention + " eres gay y lo sabes!");
                    break;
                case "!real":
                    await message.Channel.SendMessageAsync("Soy el bot 100% real no feik 1 link MEGA en la descripción");
                    break;
                case "!borrar":
                    await PurgeChat(message, 100);
                    break;
            }
        }
        public async Task PurgeChat(SocketMessage message, int amount)
        {
            var messages = await message.Channel.GetMessagesAsync(amount + 1).Flatten();

            await message.Channel.DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await message.Channel.SendMessageAsync("M Deleted");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
    }
}

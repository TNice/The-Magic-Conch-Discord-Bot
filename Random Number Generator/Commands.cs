using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Magic_Conch
{
    // Create a module with no prefix
    public class Commands : ModuleBase
    {   
        [Command("endorsments")]
        public async Task Endorsments(string person, int number)
        {
            string html = "https://playoverwatch.com/en-us/career/pc/" + person + "-" + number.ToString();

            int endorsmnetValue = MagicConch.GetAttributeValue(MagicConch.IsolateElements(MagicConch.FindElements(MagicConch.ScrapeHTML(html)))[0]);
            await Context.Channel.SendMessageAsync($"{person} Has {endorsmnetValue} endorsments");
        }       
    }
}
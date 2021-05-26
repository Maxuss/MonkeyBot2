using System;
using Discord.Commands;
using System.Threading.Tasks;
using static MonkeyBot.Data.Constants;
using Discord;
using MonkeyBot.Helpers;
using Newtonsoft.Json.Linq;
using MonkeyBot.Requests;
using Embed = MonkeyBot.Helpers.Embed;
using EmbedField = MonkeyBot.Helpers.EmbedField;

namespace MonkeyBot.Commands.Hypixel
{
    [Group("sb")]
    public class Bazaar : ModuleBase<SocketCommandContext>
    {
        private string KEY = Inner.GetHypixelKey();

        [Command("bz")]
        [Summary("Gets data of certain item")]
        public async Task BzAsync([Remainder] [Summary("The item too look for")]
            string item)
        {
            IUserMessage message = await ReplyAsync($"Getting data on item {item.ToUpper().Replace(" ", "_")}");
            try
            {
                Embed e = await Emb(item);
                await message.ModifyAsync(m =>
                {
                    m.Content = "";
                    m.Embed = e.E;
                });
            }
            catch (NullReferenceException e)
            {
                ErrorEmbed eE = new ErrorEmbed(e);
                await message.ModifyAsync(m =>
                {
                    m.Content = "";
                    m.Embed = eE.Embed;
                });

            }
        }

        private async Task<Embed> Emb(string item)
        {
            string ti = item.ToUpper().Replace(" ", "_");
            JObject obj = await HypixelRequest.GetSerializedData(HypixelRequestType.Bazaar, ti);
            JObject quickStatus = (JObject) obj["products"][ti]["quick_status"];
            JArray sellOrders = (JArray) obj["products"][ti]["sell_summary"];
            JArray buyOrders = (JArray) obj["products"][ti]["buy_summary"];
            
            float hSell = (float)Math.Round((float)sellOrders.First["pricePerUnit"], 1);
            float lSell = (float)Math.Round((float) sellOrders.Last["pricePerUnit"], 1);
            
            float lBuy = (float)Math.Round((float) buyOrders.First["pricePerUnit"], 1);
            float hBuy = (float)Math.Round((float) buyOrders.Last["pricePerUnit"], 1);
            
            EmbedField f0 = new EmbedField("Success: ", (string) obj["success"], true);
            EmbedField f1 = new EmbedField("Price", 
                $"Buy: {Math.Round((float)quickStatus["buyPrice"], 1)}\n" +
                $"Sell: {Math.Round((float)quickStatus["sellPrice"], 1)}");
            EmbedField f2 = new EmbedField("Volume",
                $"Buy: {quickStatus["buyVolume"]}\nSell: {quickStatus["sellVolume"]}");
            EmbedField f3 = new EmbedField("Total orders",
                $"Buy: {quickStatus["buyOrders"]}\nSell: {quickStatus["sellOrders"]}"
            );
            EmbedField f4 = new EmbedField("Highest price per unit",
                $"Buy: {hBuy}\nSell: {hSell}");
            EmbedField f5 = new EmbedField("Lowest price per unit",
                $"Buy: {lBuy}\nSell: {lSell}");
            Embed e = new Embed("HySb Bazaar Entry summary", Color.Orange, new[]{f0,f1,f2,f3,f4,f5}, $"{EMBED_FOOTER} | Info on {ti}");
            return e;
        }
    }
}
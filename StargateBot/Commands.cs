using System;
using Discord.Commands;
using System.Threading.Tasks;
using System.IO;
using Discord.WebSocket;
using Discord;
using System.Linq;

namespace StargateBot
{
    public class Commands : ModuleBase<SocketCommandContext>
    {

            [Command("square")]
            [Summary("Squares a number.")]
            public async Task SquareAsync([Summary("The number to square.")] int num)
            {
                // We can also access the channel from the Command Context.
                await Context.Channel.SendMessageAsync($"{num}^2 = {Math.Pow(num, 2)}");
            }

        [Command("aki")]
        public async Task Aki(string cmd = "")
        {

            if (cmd.Length == 0)
            {
                await Context.Channel.SendMessageAsync($"Have fun, {Context.User.Username}. And good luck.");
            }
            
        }

        [Command("role")]
        public async Task SetRole(string cmd = "")
        {
            Console.WriteLine($"{Context.User.Username} has requested a role.");
            bool done = false;
            while (done == false)
            {

                var user = Context.User as SocketGuildUser;
                var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == "ancients (admin)");
                if (user.Roles.Contains(role))
                {
                    Console.WriteLine("Denying request. User is admin.");
                    await Context.Channel.SendMessageAsync($"You're an admin, {user.Nickname}. Request denied.");
                    return;
                }

                done = true;
            }
            var command = cmd.ToLower();

            switch (command)
            {

                case "tau'ri":
                    await AddRole("Tau'ri");
                    break;
                case "nox":
                    await AddRole("The Nox");
                    break;
                case "tollan":
                    await AddRole("Tollan");
                    break;
                case "wraith":
                    await AddRole("Wraith");
                    break;
                case "furlings":
                    await AddRole("Furlings");
                    break;
                default:
                    await Context.Channel.SendMessageAsync($"Hey {Context.User.Mention}. Thank you for opting to choose a role.\n\n" +
                        $"Currently you can use the following commands to select a role. This is purely cosmetic at the moment.\n\n" +
                        $"!role tau'ri - Become a member of the SGC from Earth.\n" +
                        $"!role tollan - Join the ranks of Earth's first advanced ally.\n" +
                        $"!role nox - Hide from the goa'uld like a true nox\n" +
                        $"!role wraith - Feel like sapping the life out of the humans of the galaxy? This role is for you.\n" +
                        $"!role furlings - Become the mysterious 4th race of the great alliance.\n" +
                        $"More are coming at a later date. Enjoy.");
                    break;

            }
                
            
        }


        public async Task AddRole(string therole, ulong theuser = 0)
        {
            SocketUser user;
            if (theuser == 0)
            {
                user = Context.User;
            }
            else
            {
                user = Context.Client.GetUser(theuser);
            }


            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == therole);
            await (user as IGuildUser).RemoveRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == "Tau'ri"));
            await (user as IGuildUser).RemoveRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == "The Nox"));
            await (user as IGuildUser).RemoveRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == "Tollan"));
            await (user as IGuildUser).RemoveRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == "Wraith"));
            await (user as IGuildUser).RemoveRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name == "Furlings"));
            await (user as IGuildUser).AddRoleAsync(role);
            await Context.Channel.SendMessageAsync($"All done, {Context.User.Mention}.");
        }


        [Command("hi")]
            [Summary("Say hi to the user.")]
            public async Task HiUser()
            {
                var userinfo = Context.User;
                // We can also access the channel from the Command Context.
                await Context.Channel.SendMessageAsync($"Hello {userinfo.Mention}!");
            }



            [Command("random")]
            [Summary("Returns a random number.")]
            public async Task RandomNumber([Summary("The lowest number.")] int min, [Summary("The highest number.")] int max)
            {
                var rnd = new Random();
                await Context.Channel.SendMessageAsync($"🎲 Your random number is: {rnd.Next(min, max)}");
            }

            [Command("pi")]
            [Summary("What is pi?")]
            public async Task TellMePi()
            {
                double pi = Math.PI;
                
                await Context.Channel.SendMessageAsync($"```You can't calculate pi yourself, {Context.User.Mention}? Pi is: {pi}```");

            }


            [Command("memes")]
            [Summary("Show a random meme")]
            public async Task ShowAMeme()
            {

                var rnd = new Random();
                int next = rnd.Next(0, Global.memes.Length - 1);
                string chosen = Global.memes[next];
                Console.WriteLine($"Meme requested by {Context.User.Username}\nArray length: {Global.memes.Length}\nRandom Number chosen: {next}\nMeme file: {chosen}.\n\n");
                if (File.Exists(chosen))
                {
                    await Context.Channel.SendFileAsync(chosen, "Feel free to send memes to AussieEevee for inclusion.");
                }
                else
                {
                    await Context.Channel.SendMessageAsync($"I had trouble accessing the meme database. Try again.");
                }

                //await Context.Channel.SendMessageAsync($"I'm sorry. Memes are unavailable at the moment. Blame the lazy developer.");

            }

           

            [Command("help")]
            [Summary("Show help")]
            public async Task HelpMe()
            {
                await Context.Channel.SendMessageAsync($"Help system offline.");

            }

            [Command("admin")]
            public async Task AdminCommand(string cmd = "")
            {
                string thecmd = cmd.ToLower();
            var user = Context.User as SocketGuildUser;
            var role = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == "ancients (admin)");
            if (user.Roles.Contains(role))
            {
                switch (thecmd)
                {
                    case "test":
                        await Context.Channel.SendMessageAsync($"Admin Command Test");
                        break;


                    case "reloadmemes":
                        Console.WriteLine("Reload Meme directory requested.");
                        Global.memes = Directory.GetFiles(@"Memes\");
                        await Context.Channel.SendMessageAsync($"Memes refreshed. I have {Global.memes.Length} memes in my database.");
                        break;

                    case "version":
                        await Context.Channel.SendMessageAsync($"This is {Global.appname} v{Global.version}");
                        break;

                    case "ping":
                        var userinfo = Context.User;
                        var ping = Context.Client.Latency;
                        // We can also access the channel from the Command Context.
                        await Context.Channel.SendMessageAsync($"Pong! I can see you, {userinfo.Mention}. The bots ping is: {ping}ms.");
                        break;

                    case "listmemes":
                        Console.WriteLine("\nList of memes in the array requested. Processing....");
                        for (int i = 0; i < Global.memes.Length; i++)
                        {
                            Console.WriteLine($"{i} = {Global.memes[i]}");
                        }
                        await Context.Channel.SendMessageAsync($"I have listed the memes array in the console window.");
                        break;

                    default:
                        await Context.User.SendMessageAsync("Valid Commands are: test, reloadmemes, ping, version.");
                        await Context.Channel.SendMessageAsync($"Command vacant or not recognized. Valid commands have been PMed to you.");
                        break;


                }
                return;
            } else
                {
                    await Context.Channel.SendMessageAsync($"Only admins can use these commands. Sorry, {Context.User.Username}.");
                }



            }




            

        }

    }
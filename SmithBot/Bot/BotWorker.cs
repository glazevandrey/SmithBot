﻿using NLog;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Extensions.Polling;

namespace SmithBot.Bot
{
    public class BotWorker
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public BotWorker()
        {
        }

        public static void InitBot(IServiceProvider serviceProvider)
        {

            Program.botService = serviceProvider.GetService<BotService>();
            logger.Info("STARTED");


            Program.botClient.DeleteWebhookAsync();
            List<UpdateType> types = new List<UpdateType>();

            types.Add(UpdateType.Message);
            var options = new ReceiverOptions
            {
                AllowedUpdates = new[] { UpdateType.Message, UpdateType.CallbackQuery, UpdateType.InlineQuery, UpdateType.MyChatMember, UpdateType.ChatMember }
            };

            Program.botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            options,
            cancellationToken: new CancellationToken()
            );

        }
        private static async Task HandleUpdateAsync(
                  ITelegramBotClient botClient,
                  Update update,
                  CancellationToken cancellationToken)
        {

            var handler = update.Type switch
            {
                UpdateType.Message => OnMessageReceived(botClient, update),
                UpdateType.MyChatMember => OnMessageReceived(botClient, update),
                UpdateType.ChatMember => OnMessageReceived(botClient, update),
                UpdateType.InlineQuery => OnInlineReceived(botClient, update),
                UpdateType.CallbackQuery => OnInlineReceived(botClient, update),
            };

            try
            {
                await handler;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(botClient, ex, cancellationToken);
            }
        }
        private static async Task OnInlineReceived(ITelegramBotClient botClient, Update updateMessage)
        {
            await Program.botService.InlineStart(updateMessage);
        }
        private static async Task OnMessageReceived(ITelegramBotClient botClient, Update updateMessage)
        {
            logger.Info("received message");
            await Program.botService.Start(updateMessage);
        }
        private static Task HandleErrorAsync(
 ITelegramBotClient botClient,
 Exception update,
 CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }
}

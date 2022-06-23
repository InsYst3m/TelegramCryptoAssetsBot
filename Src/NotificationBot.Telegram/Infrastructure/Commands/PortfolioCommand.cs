﻿using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class PortfolioCommand : IBotCommand
    {
        private readonly ParsedMessage _parsedMessage;
        private readonly INotificationService _notificationService;

        public PortfolioCommand(
            ParsedMessage parsedMessage,
            INotificationService notificationService)
        {
            ArgumentNullException.ThrowIfNull(parsedMessage);
            ArgumentNullException.ThrowIfNull(notificationService);

            _parsedMessage = parsedMessage;
            _notificationService = notificationService;
        }

        public async Task ExecuteAsync(params string[] arguments)
        {
            ReplyKeyboardMarkup keyboard = new(new[]
            {
                new KeyboardButton[] { "Add", "Remove" },
                new KeyboardButton[] { "Update", "View" }
            })
            {
                ResizeKeyboard = true
            };

            await _notificationService.SendMarkupNotificationAsync(
                _parsedMessage.Message.Chat.Id,
                string.Empty,
                keyboard);
        }
    }
}

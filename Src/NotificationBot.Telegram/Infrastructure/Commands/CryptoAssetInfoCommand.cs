﻿using NotificationBot.DataAccess.Entities;
using NotificationBot.DataAccess.Services;
using NotificationBot.Telegram.Infrastructure.Generators;
using NotificationBot.Telegram.Infrastructure.Parsers.Models;
using NotificationBot.Telegram.Infrastructure.Services.Interfaces;
using NotificationBot.Telegram.Infrastructure.ViewModels;

namespace NotificationBot.Telegram.Infrastructure.Commands
{
    public class CryptoAssetInfoCommand : IBotCommand
    {
        private readonly ParsedMessage _parsedMessage;
        private readonly IDataAccessService _dataAccessService;
        private readonly IGraphService _graphService;
        private readonly IMessageGenerator _messageGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoAssetInfoCommand"/> class.
        /// </summary>
        /// <param name="parsedMessage">The parsed telegram bot message.</param>
        /// <param name="dataAccessService">The data access service.</param>
        /// <param name="graphService">The graph service.</param>
        /// <param name="messageGenerator">The message generator.</param>
        public CryptoAssetInfoCommand(
            ParsedMessage parsedMessage,
            IDataAccessService dataAccessService,
            IGraphService graphService,
            IMessageGenerator messageGenerator)
        {
            ArgumentNullException.ThrowIfNull(parsedMessage);
            ArgumentNullException.ThrowIfNull(dataAccessService);
            ArgumentNullException.ThrowIfNull(graphService);
            ArgumentNullException.ThrowIfNull(messageGenerator);

            _parsedMessage = parsedMessage;
            _dataAccessService = dataAccessService;
            _graphService = graphService;
            _messageGenerator = messageGenerator;
        }

        /// <inheritdoc cref="IBotCommand.ExecuteAsync(string[])" />
        public async Task<string> ExecuteAsync(params string[] arguments)
        {
            List<CryptoAsset> supportedCryptoAssets = await _dataAccessService.GetCryptoAssetsLookupAsync();

            if (!supportedCryptoAssets.Exists(x => x.Abbreviation == _parsedMessage.CommandText!))
            {
                return "Crypto Asset is not supported.";
            }

            CryptoAssetViewModel? cryptoAsset = await _graphService.GetCryptoAssetAsync(_parsedMessage.CommandText!);

            if (cryptoAsset is null)
            {
                return "Crypto Asset not found.";
            }

            return _messageGenerator.GenerateCryptoAssetInfoMessageAsync(cryptoAsset);
        }
    }
}

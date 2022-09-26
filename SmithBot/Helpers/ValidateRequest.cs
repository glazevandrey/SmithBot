namespace SmithBot.Helpers
{
    public class ValidateRequest
    {
        public bool IsValid(Telegram.Bot.Types.Update update)
        {
            if (update.Type != Telegram.Bot.Types.Enums.UpdateType.InlineQuery && update.Type != Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                return false;
            }

            return true;
        }
    }
}

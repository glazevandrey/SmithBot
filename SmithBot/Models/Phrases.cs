namespace SmithBot.Models
{
    public class Phrases
    {
        public string check { get; set; } = "Проверить";
        public string chooseLanguage { get; set; } = "Выберите ваш язык:";
        public string subscribeOnChannel { get; set; } = "Чтобы продолжить, Вам необходимо подписаться на канал \n\n{0}";
        public string noUsername { get; set; } = "У вас не установлен @username в Телеграм";
        public string registered { get; set; } = "Вы успешно зарегистрировались!";
        public string checkBalance { get; set; } = "Баланс";
        public string instriction { get; set; } = "Инструкция";
        public string referralActivated { get; set; } = "@{0} активировал вашу реферальную ссылку";
        public string yourBalance { get; set; } = "Ваш баланс: {0}";
        public string information { get; set; } = "Информация";
        public string refSystem { get; set; } = "Реферальная система";
        public string profile { get; set; } = "Личный кабинет";
        public string toMainMenu { get; set; } = "В главное меню";
        public string youInMainMenu { get; set; } = "Вы в главном меню";
        public string setWalletPls { get; set; } = "Введите адрес кошелька:";

        public string yourRefLink { get; set; } = "Ваша реферальная ссылка: \n\n{0}";
        public string share { get; set; } = "Поделиться";
        public string selectRef { get; set; } = "Выберите категорию";
        public string referrals { get; set; } = "Рефералы";
        public string purchases { get; set; } = "Покупки";
        public string goToChannel { get; set; } = "Перейти в канал \n{0}";
        public string inlineSendText { get; set; } = "Отправить реферальную ссылку";
        public string GetNFT { get; set; } = "Получить NFT";
        public string Withdraw { get; set; } = "Вывести";
        public string cancel { get; set; } = "Отменить";
        public string back { get; set; } = "Назад";
        public string mainInfo { get; set; } = "Розыгрыш выигрышной НФТ произойдет:\n<b>{4}</b>\n\nДо конца игры осталось:\n<b>{5} дней {6} часа(ов) {7} минут {8} секунд</b>\n\nСумма банка: <b>{0}</b>\n\nСумма выигрыша: <b>{1}</b>\n\nСамый близкий к победе купленный NFT: <b>{2}</b>\n\nNFT который выигрывает: <b>{3}</b>";

        public string congratsNFT { get; set; } = "Поздравялем! Вы успешно получили NFT!";
        public string invalidNumber { get; set; } = "Некорректное число";
        public string NotEnoughFunds { get; set; } = "Недостаточно средств";
        public string sendCommentToUs { get; set; } = "Отправьте в чат комментарий, в котором укажите:\n<b>Ваш адрес TON кошелька</b>\nВаши пожелания к выводу";
        public string orderRegistered { get; set; } = "Ваша заявка на вывод средств успешно зарегистрирована!";
        public string newOrder { get; set; } = "Новая заявка на вывод средств:\n\nПользователь: @{0}\nСумма к выводу: {1}\nКомментарий: {2}";
        public string youHaveOrder { get; set; } = "У Вас уже есть активная заявка на вывод средств!";
        public string youHaveNFTorder { get; set; } = "У Вас уже есть активная заявка на покупку NFT!";
        public string notenoughtRare { get; set; } = "В данный момент у нас нет такого кол-ва NFT.\nВведите меньшее колличество";
        public string sendTon { get; set; } = "Отправьте в чат кол-во TON которое вы хотите вывести";
        public string sendNFT { get; set; } = "Отправьте в чат кол-во NFT которое вы хотите приобрести";
        public string havePaid { get; set; } = "Я оплатил";
        public string iWantBuy { get; set; } = "Приобрести";

        public string Common { get; set; } = "Обычная";
        public string Rare { get; set; } = "Редкая";
        public string succsessPayment { get; set; } = "Отлично! При успешной оплате, в течении нескольких минут мы пришлем NFT в данный чат!\n\nПри возникновении вопросов по оплате обращаться: {0}";
        public string transactionInfo { get; set; } = "Вы собираетесь купить:\n\n<b>{0} шт. NFT</b>\nК оплате: <b>{1} TON</b>" +
            "\n\nУникальный комментарий для оплаты данных NFT:\n\n<code><b>{2}</b></code>" +
            "\n\nАдрес нашего кошелька: \n\n<code><b>{3}</b></code>" +
            "\n\nПосле того как вы совершите оплату, нажмите кнопку <b>{4}</b>";

        public string chooseNFT { get; set; } = "Выберите какую NFT вы хотите приобрести:";
        public string profileInfo { get; set; } = "Ваш Id: <b>{0}</b>" +
            "\n\nУ вас имеется <b>{1} шт. NFT</b>, на сумму <b>{2} TON</b>" +
            "\n\nКол-во рефералов: {3}" +
            "\n\nДата регистрации в боте: {4}" +
            "\n\nТех. поддержка: {5}";



        public string purchasesInfo { get; set; } = "В данный момент, у нас действует трёхуровневая реферальная система:" +
            "\n\nЗа тех, кого Вы пригласили по своей ссылке <b>(первый уровень)</b>, Вы будете получать <b>10%</b> их покупок." +
            "\n\nЗа тех, кого пригласили ваши рефералы с первого уровня <b>(второй уровень)</b>, вы получите <b>5%</b> от покупок." +
            "\n\nЗа рефералов <b>третьего уровня</b>, вы будете получать <b>1%</b>." +
            "\n\nКогда на реферальном кошельке наберется определенная сумма (в данный момент это <b>{0} TON</b>), Вы сможете получить одну из наших NFT за внутренний баланс или же вывести средства на свой кошелек";

        public string referralsInfo { get; set; } = "Информация по вашим рефералам: " +
            "\n\nВаши рефералы первого уровня:" +
            "\n\n{0}" +
            "\n\nВторого уровня:" +
            "\n\n{1}" +
            "\n\nТретьего уровня:" +
            "\n\n{2}" +
            "\n\n";


        public string tonWalletInfo { get; set; } = "В данный момент:\n\n1 шт. Обычной NFT = <b>{1} TON</b>\n1 шт. Редкой NFT = <b>{2} TON</b>\n\nАдрес нашего кошелька:" +
            "\n\n<code><b>{3}</b></code>" + "\n\n" + "Чтобы купить нашу NFT, Вам необходимо:" +
            "\n\n1. Нажать кнопку {0}" +
            "\n\n2. Указать тип NFT" +
            "\n\n3. Указать кол-во NFT которое вы хотите купить" +
            "\n\n4. Указать уникальный комментарий, который будет сгенерирован, чтобы мы поняли что нам перевели именно Вы:" +
            "\n\n5. После того как вы оплатили, вам <b>необходимо нажать кнопку {4}</b>\nДалее, в течении нескольких минут, Вам будет привязана и отослана NFT." +
            "\n\nВнимание! К каждому заказу выдается свой уникальный комментарий!\n\n";

    }

}

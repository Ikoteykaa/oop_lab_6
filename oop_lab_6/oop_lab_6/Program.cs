using System;
using System.Collections.Generic;
using System.Globalization;


namespace oop_lab_1
{
    public class Program
    {

        public static void AddGame(List<VideoGames> games, VideoGames game)
        {
            if (games == null)
                throw new ArgumentNullException(nameof(games), "Колекція не може бути null.");
            if (game == null)
                throw new ArgumentNullException(nameof(game), "Не можна додати порожній об'єкт (null).");

            games.Add(game);
        }

        public static bool RemoveGameAt(List<VideoGames> games, int index)
        {
            if (games == null)
                throw new ArgumentNullException(nameof(games), "Колекція не може бути null.");

            if (index < 0 || index >= games.Count)
                return false;

            games.RemoveAt(index);
            return true;
        }

        public static int RemoveGamesByGenre(List<VideoGames> games, GameGenre genre)
        {
            if (games == null)
                throw new ArgumentNullException(nameof(games), "Колекція не може бути null.");

            return games.RemoveAll(g => g.Genre == genre);
        }

        public static List<VideoGames> FindByGenre(List<VideoGames> games, GameGenre genre)
        {
            if (games == null)
                throw new ArgumentNullException(nameof(games), "Колекція не може бути null.");

            return games.FindAll(g => g.Genre == genre);
        }

        public static List<VideoGames> FindByYear(List<VideoGames> games, int year)
        {
            if (games == null)
                throw new ArgumentNullException(nameof(games), "Колекція не може бути null.");
            if (year < 1900 || year > DateTime.Now.Year)
                throw new ArgumentOutOfRangeException(nameof(year), "Рік має бути від 1900 до поточного.");

            return games.FindAll(g => g.ReleaseYear.Year == year);
        }

        public static bool TryGetGenre(int number, out GameGenre genre)
        {
            if (Enum.IsDefined(typeof(GameGenre), number))
            {
                genre = (GameGenre)number;
                return true;
            }
            genre = GameGenre.RPG;
            return false;
        }

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<VideoGames> games = new List<VideoGames>();
            int doChoice;

            do
            {
                Console.WriteLine("\nГОЛОВНЕ МЕНЮ (Lab 6)");
                Console.WriteLine("1 – Додати об'єкт");
                Console.WriteLine("2 – Переглянути всі об'єкти");
                Console.WriteLine("3 – Знайти об'єкт");
                Console.WriteLine("4 – Продемонструвати поведінку");
                Console.WriteLine("5 – Видалити об'єкт");
                Console.WriteLine("6 – Продемонструвати static-методи");
                Console.WriteLine("0 – Вийти з програми");
                Console.Write("Ваш вибір: ");

                if (!int.TryParse(Console.ReadLine(), out doChoice))
                {
                    PrintError("Потрібно ввести число!");
                    doChoice = -1;
                    continue;
                }

                switch (doChoice)
                {
                    case 1:
                        VideoGames newGame = CreateGameFromConsole();
                        if (newGame != null)
                        {
                            AddGame(games, newGame);
                            Console.WriteLine("\nГру успішно додано!");
                        }
                        break;

                    case 2:
                        PrintGamesTable(games);
                        break;

                    case 3:
                        SearchGames(games);
                        break;

                    case 4:
                        DemonstrateBehavior(games);
                        break;

                    case 5:
                        DeleteGame(games);
                        break;

                    case 6:
                        DemonstrateStaticMethods();
                        break;

                    case 0:
                        Console.WriteLine("\nПрограму завершено. До побачення!");
                        break;

                    default:
                        PrintError("Некоректний вибір! Оберіть пункт від 0 до 6.");
                        break;
                }

            } while (doChoice != 0);
        }

        static void PrintError(string message)
        {
            Console.WriteLine($"\n[Помилка] {message} Спробуйте ще раз.\n");
        }

        static GameGenre ReadGenre()
        {
            while (true)
            {
                Console.Write("Оберіть жанр (1-Action, 2-Adventure, 3-RPG, 4-Strategy, 5-Sport, 6-Horror, 7-Simulator): ");
                if (int.TryParse(Console.ReadLine(), out int number) && TryGetGenre(number, out GameGenre genre))
                {
                    return genre;
                }
                PrintError("Оберіть число від 1 до 7.");
            }
        }

        static VideoGames CreateGameFromConsole()
        {
            while (true)
            {
                Console.WriteLine("\n--- Оберіть спосіб додавання гри ---");
                Console.WriteLine("1 - Ввести характеристики покроково");
                Console.WriteLine("2 - Ввести об'єкт одним рядком (через TryParse)");
                Console.WriteLine("0 - Скасувати додавання");
                Console.Write("Ваш вибір: ");

                string choice = Console.ReadLine()?.Trim();

                if (choice == "1")
                {
                    return AddGameInteractive();
                }
                else if (choice == "2")
                {
                    while (true)
                    {
                        Console.WriteLine("\nФормат рядка: Назва;Жанр;Рейтинг;Дата(РРРР-ММ-ДД);Ціна");
                        Console.WriteLine("Приклад: Witcher 3;RPG;4.9;2015-05-19;599");
                        Console.Write("Введіть рядок (або '0' для відміни): ");
                        string inputStr = Console.ReadLine()?.Trim();

                        if (inputStr == "0") return null;

                        if (VideoGames.TryParse(inputStr, out VideoGames parsedGame))
                        {
                            return parsedGame;
                        }
                        PrintError("Не вдалося створити об'єкт з введеного рядка! Перевірте формат та значення.");
                    }
                }
                else if (choice == "0")
                {
                    return null;
                }
                else
                {
                    PrintError("Некоректний вибір! Введіть 1, 2 або 0.");
                }
            }
        }

        static VideoGames AddGameInteractive()
        {
            Console.WriteLine("\n--- Покрокове додавання нової гри ---");

            string name;
            while (true)
            {
                Console.Write("Введіть назву гри (2-50 символів): ");
                name = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(name) && name.Length >= 2 && name.Length <= 50)
                    break;
                PrintError("Назва має містити від 2 до 50 символів.");
            }

            GameGenre genre = ReadGenre();

            double rating;
            while (true)
            {
                Console.Write("Введіть рейтинг гри (від 0 до 5, напр. 4.5): ");
                string rInput = Console.ReadLine()?.Replace(',', '.');
                if (double.TryParse(rInput, NumberStyles.Any, CultureInfo.InvariantCulture, out rating) && rating >= 0 && rating <= 5)
                    break;
                PrintError("Рейтинг повинен бути числом від 0 до 5.");
            }

            DateOnly releaseYear;
            while (true)
            {
                Console.Write("Введіть дату релізу (рррр-мм-дд): ");
                if (DateOnly.TryParse(Console.ReadLine(), out releaseYear) && releaseYear <= DateOnly.FromDateTime(DateTime.Now))
                    break;
                PrintError("Некоректний формат дати або дата у майбутньому.");
            }

            double price;
            while (true)
            {
                Console.Write("Введіть ціну гри (більше 0): ");
                string pInput = Console.ReadLine()?.Replace(',', '.');
                if (double.TryParse(pInput, NumberStyles.Any, CultureInfo.InvariantCulture, out price) && price > 0)
                    break;
                PrintError("Ціна повинна бути числом, більшим за 0.");
            }

            try
            {
                return new VideoGames(name, genre, rating, releaseYear, price);
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
                return null;
            }
        }

        static void PrintHeader()
        {
            Console.WriteLine(new string('-', 96));
            Console.WriteLine($"| {"№",-2} | {"Назва",-20} | {"Жанр",-12} | {"Рейтинг",-7} | {"Дата релізу",-11} | {"Ціна",-8} | {"Гравці",-6} | {"TopTier",-7} |");
            Console.WriteLine(new string('-', 96));
        }

        static void PrintRow(VideoGames game, int number)
        {
            Console.WriteLine($"| {number,-2} | {game.Name,-20} | {game.Genre,-12} | {game.Rating,-7:F1} | {game.ReleaseYear,-11} | {game.Price,-8:F2} | {game.CountPlayers,-6} | {game.IsTopTier,-7} |");
        }

        static void PrintGamesTable(List<VideoGames> games)
        {
            Console.WriteLine($"\n[СТАТИСТИКА] Загальна кількість створених об'єктів (CreatedObjectsCount): {VideoGames.CreatedObjectsCount}");
            Console.WriteLine($"[СТАТИСТИКА] Наразі збережено ігор у списку: {games.Count}");

            if (games.Count == 0)
            {
                Console.WriteLine("\nСписок ігор порожній! Спочатку додайте об'єкти.");
                return;
            }

            Console.WriteLine("\n============================================= СПИСОК ІГОР ==============================================");
            PrintHeader();
            for (int i = 0; i < games.Count; i++)
            {
                PrintRow(games[i], i + 1);
            }
            Console.WriteLine(new string('-', 96));
        }

        static void PrintSearchResult(List<VideoGames> allGames, List<VideoGames> found)
        {
            if (found.Count == 0)
            {
                Console.WriteLine("\nЗа цим критерієм нічого не знайдено!");
                return;
            }

            Console.WriteLine("\nРезультати пошуку:");
            PrintHeader();
            foreach (VideoGames game in found)
            {
                PrintRow(game, allGames.IndexOf(game) + 1);
            }
            Console.WriteLine(new string('-', 96));
        }

        static void SearchGames(List<VideoGames> games)
        {
            if (games.Count == 0)
            {
                Console.WriteLine("\nНемає ігор для пошуку.");
                return;
            }

            while (true)
            {
                Console.WriteLine("\n--- Пошук ---");
                Console.WriteLine("1 - За жанром");
                Console.WriteLine("2 - За роком випуску");
                Console.WriteLine("0 - Повернутися назад");
                Console.Write("Оберіть критерій: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    PrintError("Введіть число!");
                    continue;
                }

                if (choice == 0) return;

                if (choice == 1)
                {
                    GameGenre genre = ReadGenre();
                    PrintSearchResult(games, FindByGenre(games, genre));
                    return;
                }
                else if (choice == 2)
                {
                    while (true)
                    {
                        Console.Write("Введіть рік випуску (напр. 2020): ");
                        if (int.TryParse(Console.ReadLine(), out int year) && year >= 1900 && year <= DateTime.Now.Year)
                        {
                            PrintSearchResult(games, FindByYear(games, year));
                            return;
                        }
                        PrintError("Введіть коректний рік (між 1900 та поточним).");
                    }
                }
                else
                {
                    PrintError("Некоректний вибір критерію. Введіть 1, 2 або 0.");
                }
            }
        }

        static void DeleteGame(List<VideoGames> games)
        {
            if (games.Count == 0)
            {
                Console.WriteLine("\nНемає об'єктів для видалення.");
                return;
            }

            while (true)
            {
                Console.WriteLine("\n--- Видалення гри ---");
                Console.WriteLine("1 - Видалити за номером у таблиці");
                Console.WriteLine("2 - Видалити всі ігри певного жанру");
                Console.WriteLine("0 - Відміна");
                Console.Write("Ваш вибір: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    PrintError("Введіть число!");
                    continue;
                }

                if (choice == 0) return;

                if (choice == 1)
                {
                    PrintGamesTable(games);
                    while (true)
                    {
                        Console.Write("\nВведіть номер гри для видалення (або 0 для скасування): ");
                        if (int.TryParse(Console.ReadLine(), out int number) && number >= 0 && number <= games.Count)
                        {
                            if (number == 0) return;

                            RemoveGameAt(games, number - 1);
                            Console.WriteLine("\nОб'єкт успішно видалено!");
                            return;
                        }
                        PrintError($"Некоректний номер! Номер має бути від 1 до {games.Count}.");
                    }
                }
                else if (choice == 2)
                {
                    GameGenre genre = ReadGenre();
                    int removed = RemoveGamesByGenre(games, genre);

                    if (removed == 0)
                        Console.WriteLine($"\nІгор з жанром {genre} не знайдено!");
                    else
                        Console.WriteLine($"\nУспішно видалено {removed} об'єкт(ів).");
                    return;
                }
                else
                {
                    PrintError("Некоректний вибір. Оберіть 1, 2 або 0.");
                }
            }
        }

        static void DemonstrateBehavior(List<VideoGames> games)
        {
            if (games.Count == 0)
            {
                Console.WriteLine("\nСписок порожній.");
                return;
            }

            PrintGamesTable(games);
            int index;

            while (true)
            {
                Console.Write("\nВведіть номер гри для взаємодії (або 0 для відміни): ");
                if (int.TryParse(Console.ReadLine(), out index) && index >= 0 && index <= games.Count)
                {
                    break;
                }
                PrintError($"Номер має бути від 1 до {games.Count} (або 0 для відміни).");
            }

            if (index == 0) return;

            VideoGames selectedGame = games[index - 1];
            int subChoice = -1;

            while (subChoice != 0)
            {
                Console.WriteLine($"\n--- Дії з грою: {selectedGame.Name} ---");
                Console.WriteLine("1 - Запустити гру (додати 1 гравця)");
                Console.WriteLine("2 - Запустити гру (додати декілька гравців)");
                Console.WriteLine("3 - Вийти з гри (відняти гравця)");
                Console.WriteLine("4 - Змінити рейтинг");
                Console.WriteLine("5 - Переглянути ToString() об'єкта");
                Console.WriteLine("0 - Повернутися до головного меню");
                Console.Write("Вибір: ");

                if (!int.TryParse(Console.ReadLine(), out subChoice))
                {
                    PrintError("Потрібно ввести число!");
                    continue;
                }

                switch (subChoice)
                {
                    case 1:
                        selectedGame.StartGame();
                        Console.WriteLine("Гру запущено! Кількість гравців збільшено на 1.");
                        break;

                    case 2:
                        while (true)
                        {
                            Console.Write("Введіть кількість гравців, що приєдналися (більше 0): ");
                            if (int.TryParse(Console.ReadLine(), out int players) && players > 0)
                            {
                                selectedGame.StartGame(players);
                                Console.WriteLine($"Гру запущено! Додано {players} гравців.");
                                break;
                            }
                            PrintError("Кількість гравців має бути цілим додатним числом.");
                        }
                        break;

                    case 3:
                        if (selectedGame.ExitGame())
                            Console.WriteLine("Один гравець вийшов з гри.");
                        else
                            Console.WriteLine("Помилка! У грі наразі немає активних гравців.");
                        break;

                    case 4:
                        while (true)
                        {
                            Console.Write("Введіть новий рейтинг (0 - 5): ");
                            string rInput = Console.ReadLine()?.Replace(',', '.');
                            try
                            {
                                if (!double.TryParse(rInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double newRating))
                                {
                                    throw new FormatException("Введено некоректний формат числа.");
                                }
                                selectedGame.Rating = newRating;
                                Console.WriteLine("Рейтинг успішно оновлено!");
                                break;
                            }
                            catch (Exception ex)
                            {
                                PrintError(ex.Message);
                            }
                        }
                        break;

                    case 5:
                        Console.WriteLine($"\nФорматований рядок ToString(): {selectedGame}");
                        break;

                    case 0:
                        break;

                    default:
                        PrintError("Некоректна дія! Оберіть від 0 до 5.");
                        break;
                }
            }
        }

        static void DemonstrateStaticMethods()
        {
            Console.WriteLine("\n================ ДЕМОНСТРАЦІЯ СТАТИЧНИХ ЧЛЕНІВ КЛАСУ ================");

            Console.WriteLine($"1. Загальна кількість створених об'єктів (CreatedObjectsCount): {VideoGames.CreatedObjectsCount}");
            Console.WriteLine($"2. Поточна платформа за замовчуванням (DefaultPlatform): {VideoGames.DefaultPlatform}");

            while (true)
            {
                Console.Write("\nБажаєте змінити платформу за замовчуванням? (1 – так, 0 – ні): ");
                string choice = Console.ReadLine()?.Trim();

                if (choice == "1")
                {
                    while (true)
                    {
                        Console.Write("Введіть нову платформу: ");
                        string newPlatform = Console.ReadLine()?.Trim();
                        try
                        {
                            VideoGames.DefaultPlatform = newPlatform;
                            Console.WriteLine($"Платформу успішно змінено на: {VideoGames.DefaultPlatform}");
                            break;
                        }
                        catch (Exception ex)
                        {
                            PrintError(ex.Message);
                        }
                    }
                    break;
                }
                else if (choice == "0")
                {
                    break;
                }
                else
                {
                    PrintError("Некоректне значення! Введіть 1 (так) або 0 (ні).");
                }
            }

            Console.WriteLine("\n3. Предметний static-метод (Розрахунок ціни зі знижкою):");
            double price;
            while (true)
            {
                Console.Write("Введіть початкову ціну гри: ");
                string pInput = Console.ReadLine()?.Replace(',', '.');
                if (double.TryParse(pInput, NumberStyles.Any, CultureInfo.InvariantCulture, out price) && price > 0)
                {
                    break;
                }
                PrintError("Ціна повинна бути числом, більшим за 0.");
            }

            double discount;
            while (true)
            {
                Console.Write("Введіть відсоток знижки (0-100): ");
                string dInput = Console.ReadLine()?.Replace(',', '.');
                if (double.TryParse(dInput, NumberStyles.Any, CultureInfo.InvariantCulture, out discount) && discount >= 0 && discount <= 100)
                {
                    break;
                }
                PrintError("Відсоток знижки має бути числом від 0 до 100.");
            }

            try
            {
                double discountedPrice = VideoGames.CalculateDiscountedPrice(price, discount);
                Console.WriteLine($"Ціна {price:F2} грн зі знижкою {discount}% становить: {discountedPrice:F2} грн");
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

            Console.WriteLine("\n4. Демонстрація методу Parse(string s):");
            Console.WriteLine("Формат: Назва;Жанр;Рейтинг;Дата;Ціна (наприклад: Doom;Action;4.7;2016-05-13;400)");
            Console.Write("Введіть рядок для перевірки Parse: ");
            string parseInput = Console.ReadLine()?.Trim();

            try
            {
                VideoGames parsedGame = VideoGames.Parse(parseInput);
                Console.WriteLine($"[Успіх Parse] Створено об'єкт: {parsedGame.Name} ({parsedGame.Genre}), Ціна: {parsedGame.Price}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Очікуваний виняток Parse]: {ex.GetType().Name} - {ex.Message}");
            }

            Console.WriteLine("\n5. Демонстрація методу TryParse(string s, out VideoGames obj):");
            Console.Write("Введіть рядок для перевірки TryParse: ");
            string tryParseInput = Console.ReadLine()?.Trim();

            if (VideoGames.TryParse(tryParseInput, out VideoGames tryParsedGame))
            {
                Console.WriteLine($"[Успіх TryParse] Успішно створено об'єкт: {tryParsedGame.Name} ({tryParsedGame.Genre})");
            }
            else
            {
                Console.WriteLine("[Помилка TryParse] Не вдалося перетворити рядок (метод повернув false).");
            }
        }
    }
}
using System;
using System.Globalization;

namespace oop_lab_1
{
    public class VideoGames
    {

        private static int _createdObjectsCount = 0;
        private static string _defaultPlatform = "PC (Windows)";

        public static int CreatedObjectsCount => _createdObjectsCount;

        public static string DefaultPlatform
        {
            get => _defaultPlatform;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Назва платформи не може бути порожньою.");
                _defaultPlatform = value;
            }
        }

        private string _name;
        private GameGenre _genre;
        private double _rating;
        private DateOnly _releaseYear;
        private double _price;

        public string Publisher { get; set; } = "Unknown";
        public int CountPlayers { get; private set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 2 || value.Length > 50)
                    throw new ArgumentException("Назва має містити від 2 до 50 символів.");
                _name = value;
            }
        }

        public GameGenre Genre
        {
            get => _genre;
            set
            {
                if (!Enum.IsDefined(typeof(GameGenre), value))
                    throw new ArgumentException("Недопустимий жанр гри.");
                _genre = value;
            }
        }

        public double Rating
        {
            get => _rating;
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(Rating), "Рейтинг повинен бути числом від 0 до 5.");
                _rating = value;
            }
        }

        public DateOnly ReleaseYear
        {
            get => _releaseYear;
            set
            {
                if (value > DateOnly.FromDateTime(DateTime.Now))
                    throw new ArgumentException("Дата релізу не може бути у майбутньому.");
                _releaseYear = value;
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Ціна повинна бути більшою за 0.");
                _price = value;
            }
        }

        public bool IsTopTier => Rating >= 4.5 && CountPlayers > 0;

        public VideoGames()
        {
            _name = "Без назви";
            _genre = GameGenre.Action;
            _rating = 0;
            _releaseYear = new DateOnly(2000, 1, 1);
            _price = 1;
            CountPlayers = 0;

            _createdObjectsCount++;
            Console.WriteLine("\n[Система] Спрацював конструктор БЕЗ параметрів.");
        }

        public VideoGames(string name, GameGenre genre)
            : this(name, genre, 0.0, new DateOnly(2000, 1, 1), 1.0)
        {
            Console.WriteLine("[Система] Спрацював конструктор з 2 параметрами (через this()).");
        }

        public VideoGames(string name, GameGenre genre, double rating, DateOnly releaseYear, double price)
        {
            Name = name;
            Genre = genre;
            Rating = rating;
            ReleaseYear = releaseYear;
            Price = price;
            CountPlayers = 0;

            _createdObjectsCount++;
            Console.WriteLine("\n[Система] Спрацював конструктор з УСІМА параметрами.");
        }


        public static double CalculateDiscountedPrice(double originalPrice, double discountPercent)
        {
            if (originalPrice <= 0)
                throw new ArgumentException("Ціна повинна бути більшою за 0.");
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentOutOfRangeException(nameof(discountPercent), "Відсоток знижки має бути від 0 до 100.");

            return originalPrice * (1.0 - discountPercent / 100.0);
        }


        public static VideoGames Parse(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentNullException(nameof(s), "Рядок для парсингу не може бути порожнім.");
            }

            string[] parts = s.Split(';');
            if (parts.Length != 5)
            {
                throw new FormatException("Некоректний формат рядка! Очікується 5 елементів, розділених ';': Назва;Жанр;Рейтинг;Дата(РРРР-ММ-ДД);Ціна");
            }

            string name = parts[0].Trim();

            if (!Enum.TryParse(parts[1].Trim(), true, out GameGenre genre) || !Enum.IsDefined(typeof(GameGenre), genre))
            {
                throw new FormatException($"Некоректний жанр '{parts[1].Trim()}'. Допустимі значення: Action, Adventure, RPG, Strategy, Sport, Horror, Simulator.");
            }

            if (!double.TryParse(parts[2].Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double rating))
            {
                throw new FormatException($"Некоректний формат рейтингу '{parts[2].Trim()}'. Потрібно ввести число.");
            }

            if (!DateOnly.TryParse(parts[3].Trim(), out DateOnly releaseYear))
            {
                throw new FormatException($"Некоректний формат дати релізу '{parts[3].Trim()}'. Очікується формат РРРР-ММ-ДД.");
            }

            if (!double.TryParse(parts[4].Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
            {
                throw new FormatException($"Некоректний формат ціни '{parts[4].Trim()}'. Потрібно ввести число.");
            }

            return new VideoGames(name, genre, rating, releaseYear, price);
        }


        public static bool TryParse(string s, out VideoGames obj)
        {
            try
            {
                obj = Parse(s);
                return true;
            }
            catch (Exception)
            {
                obj = null;
                return false;
            }
        }

        public override string ToString()
        {
            return $"{Name};{Genre};{Rating};{ReleaseYear:yyyy-MM-dd};{Price}";
        }

        private void ChangePlayerCount(int delta)
        {
            CountPlayers += delta;
        }

        public void StartGame()
        {
            ChangePlayerCount(1);
        }

        public void StartGame(int playersToAdd)
        {
            if (playersToAdd > 0)
            {
                ChangePlayerCount(playersToAdd);
            }
        }

        public bool ExitGame()
        {
            if (CountPlayers > 0)
            {
                ChangePlayerCount(-1);
                return true;
            }
            return false;
        }
    }
}
using oop_lab_1;
using GameProgram = oop_lab_1.Program;

namespace Test_Video_Game
{
    [TestClass]
    public sealed class Test_Program_Collections
    {
        private List<VideoGames> _games = new();

        [TestInitialize]
        public void Setup()
        {
            // Перед кожним тестом створюємо новий список із 4 ігор
            _games = new List<VideoGames>
            {
                new VideoGames("Minecraft", GameGenre.Adventure, 4.5, new DateOnly(2011, 11, 18), 20.0),
                new VideoGames("Doom", GameGenre.Action, 4.2, new DateOnly(2016, 5, 13), 25.0),
                new VideoGames("Witcher", GameGenre.RPG, 4.8, new DateOnly(2015, 5, 19), 30.0),
                new VideoGames("Doom Eternal", GameGenre.Action, 4.6, new DateOnly(2020, 3, 20), 40.0)
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            _games.Clear();
        }

        //AddGame 

        [TestMethod]
        public void AddGame_Valid_IncreasesCount()
        {
            var game = new VideoGames("Tetris", GameGenre.Simulator);

            GameProgram.AddGame(_games, game);

            Assert.HasCount(5, _games);
            Assert.Contains(game, _games);
        }

        [TestMethod]
        public void AddGame_ToEmptyList_Works()
        {
            var empty = new List<VideoGames>();

            GameProgram.AddGame(empty, new VideoGames("Tetris", GameGenre.Simulator));

            Assert.HasCount(1, empty);
        }

        [TestMethod]
        public void AddGame_NullGame_Throws()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => GameProgram.AddGame(_games, null!));
            Assert.HasCount(4, _games);
        }

        [TestMethod]
        public void AddGame_NullList_Throws()
        {
            Assert.ThrowsExactly<ArgumentNullException>(
                () => GameProgram.AddGame(null!, new VideoGames()));
        }

        //RemoveGameAt

        [TestMethod]
        public void RemoveGameAt_ValidIndex_RemovesAndReturnsTrue()
        {
            bool result = GameProgram.RemoveGameAt(_games, 1);

            Assert.IsTrue(result);
            Assert.HasCount(3, _games);
            Assert.AreEqual("Witcher", _games[1].Name);
        }

        [TestMethod]
        public void RemoveGameAt_FirstAndLast_Work()
        {
            Assert.IsTrue(GameProgram.RemoveGameAt(_games, 0));
            Assert.IsTrue(GameProgram.RemoveGameAt(_games, _games.Count - 1));

            Assert.HasCount(2, _games);
            Assert.AreEqual("Doom", _games[0].Name);
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(4)]
        [DataRow(100)]
        public void RemoveGameAt_InvalidIndex_ReturnsFalse(int index)
        {
            bool result = GameProgram.RemoveGameAt(_games, index);

            Assert.IsFalse(result);
            Assert.HasCount(4, _games);
        }

        [TestMethod]
        public void RemoveGameAt_EmptyList_ReturnsFalse()
        {
            var empty = new List<VideoGames>();

            Assert.IsFalse(GameProgram.RemoveGameAt(empty, 0));
        }

        [TestMethod]
        public void RemoveGameAt_NullList_Throws()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => GameProgram.RemoveGameAt(null!, 0));
        }

        // RemoveGamesByGenre

        [TestMethod]
        public void RemoveGamesByGenre_TwoMatches_RemovesBoth()
        {
            int removed = GameProgram.RemoveGamesByGenre(_games, GameGenre.Action);

            Assert.AreEqual(2, removed);
            Assert.HasCount(2, _games);
            Assert.IsFalse(_games.Exists(g => g.Genre == GameGenre.Action));
        }

        [TestMethod]
        public void RemoveGamesByGenre_OneMatch_RemovesOne()
        {
            int removed = GameProgram.RemoveGamesByGenre(_games, GameGenre.RPG);

            Assert.AreEqual(1, removed);
            Assert.HasCount(3, _games);
        }

        [TestMethod]
        public void RemoveGamesByGenre_NoMatches_ReturnsZero()
        {
            int removed = GameProgram.RemoveGamesByGenre(_games, GameGenre.Horror);

            Assert.AreEqual(0, removed);
            Assert.HasCount(4, _games);
        }

        [TestMethod]
        public void RemoveGamesByGenre_EmptyList_ReturnsZero()
        {
            var empty = new List<VideoGames>();

            Assert.AreEqual(0, GameProgram.RemoveGamesByGenre(empty, GameGenre.Action));
        }

        [TestMethod]
        public void RemoveGamesByGenre_NullList_Throws()
        {
            Assert.ThrowsExactly<ArgumentNullException>(
                () => GameProgram.RemoveGamesByGenre(null!, GameGenre.Action));
        }

        //FindByGenre

        [TestMethod]
        public void FindByGenre_TwoMatches_ReturnsTwo()
        {
            List<VideoGames> found = GameProgram.FindByGenre(_games, GameGenre.Action);

            Assert.HasCount(2, found);
            Assert.AreEqual("Doom", found[0].Name);
            Assert.AreEqual("Doom Eternal", found[1].Name);
        }

        [TestMethod]
        public void FindByGenre_NoMatches_ReturnsEmptyList()
        {
            List<VideoGames> found = GameProgram.FindByGenre(_games, GameGenre.Sport);

            Assert.IsNotNull(found);
            Assert.IsEmpty(found);
        }

        [TestMethod]
        public void FindByGenre_DoesNotChangeOriginalList()
        {
            GameProgram.FindByGenre(_games, GameGenre.Action);

            Assert.HasCount(4, _games);
        }

        [TestMethod]
        public void FindByGenre_NullList_Throws()
        {
            Assert.ThrowsExactly<ArgumentNullException>(
                () => GameProgram.FindByGenre(null!, GameGenre.Action));
        }

        //FindByYear

        [TestMethod]
        public void FindByYear_OneMatch_ReturnsOne()
        {
            List<VideoGames> found = GameProgram.FindByYear(_games, 2015);

            Assert.HasCount(1, found);
            Assert.AreEqual("Witcher", found[0].Name);
        }

        [TestMethod]
        public void FindByYear_NoMatches_ReturnsEmptyList()
        {
            List<VideoGames> found = GameProgram.FindByYear(_games, 2000);

            Assert.IsEmpty(found);
        }

        [TestMethod]
        public void FindByYear_EmptyList_ReturnsEmptyList()
        {
            var empty = new List<VideoGames>();

            Assert.IsEmpty(GameProgram.FindByYear(empty, 2020));
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1899)]
        [DataRow(-5)]
        public void FindByYear_TooSmallYear_Throws(int year)
        {
            Assert.ThrowsExactly<ArgumentOutOfRangeException>(
                () => GameProgram.FindByYear(_games, year));
        }

        [TestMethod]
        public void FindByYear_FutureYear_Throws()
        {
            int future = DateTime.Now.Year + 1;

            Assert.ThrowsExactly<ArgumentOutOfRangeException>(
                () => GameProgram.FindByYear(_games, future));
        }

        [TestMethod]
        public void FindByYear_NullList_Throws()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => GameProgram.FindByYear(null!, 2020));
        }

        // TryGetGenre

        [TestMethod]
        [DataRow(1, GameGenre.Action)]
        [DataRow(3, GameGenre.RPG)]
        [DataRow(7, GameGenre.Simulator)]
        public void TryGetGenre_ValidNumber_ReturnsTrue(int number, GameGenre expected)
        {
            bool result = GameProgram.TryGetGenre(number, out GameGenre genre);

            Assert.IsTrue(result);
            Assert.AreEqual(expected, genre);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(8)]
        [DataRow(-1)]
        [DataRow(999)]
        public void TryGetGenre_InvalidNumber_ReturnsFalse(int number)
        {
            bool result = GameProgram.TryGetGenre(number, out GameGenre genre);

            Assert.IsFalse(result);
        }
    }
}
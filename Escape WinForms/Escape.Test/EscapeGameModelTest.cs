using Escape.Model;
using Escape.Persistence;
using Moq;

namespace Escape.Test
{
    [TestClass]
    public class EscapeGameModelTest
    {
        private EscapeGameModel _model = null!;
        private EscapeTable _mockedTable = null!;
        private Mock<IEscapeDataAccess> _mock = null!;
        [TestInitialize]
        public void Initialize()
        {
            _mockedTable = new EscapeTable(15);
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                    _mockedTable.SetValue(j, i, 0, "start");
            }
            _mockedTable.SetValue(7, 0, 3, "start");
            _mockedTable.SetValue(0, 14, 3, "start");
            _mockedTable.SetValue(14, 14, 5, "start");
            for (int i = 0; i < 15; i++)
            {
                _mockedTable.SetValue(i, 7, 2, "start");
            }
            _mock = new Mock<IEscapeDataAccess>();
            _mock.Setup(mock => mock.LoadAsync(It.IsAny<String>()))
                .Returns(() => Task.FromResult(_mockedTable));

            _model = new EscapeGameModel(_mock.Object);
            _model.GameAdvanced += new EventHandler<EscapeEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<EscapeEventArgs>(Model_GameOver);
        }
        [TestMethod]
        public void EscapeNewNormalGameTest()
        {
            _model.NewGame();
            Assert.AreEqual(Difficulty.Medium, _model.Difficulty);
            Assert.AreEqual(15, _model.Table.Size);
            Assert.AreEqual(0, _model.GameTime);
            Assert.AreEqual(3, _model.Table.GetValue(_model.Table.Size / 2, 0));
            Assert.AreEqual(4, _model.Table.GetValue(0, _model.Table.Size - 1));
            Assert.AreEqual(5, _model.Table.GetValue(_model.Table.Size - 1, _model.Table.Size - 1));
        }
        [TestMethod]
        public void EscapeNewEasyGameTest()
        {
            _model.Difficulty = Difficulty.Easy;
            _model.NewGame();
            Assert.AreEqual(Difficulty.Easy, _model.Difficulty);
            Assert.AreEqual(11, _model.Table.Size);
            Assert.AreEqual(0, _model.GameTime);
            Assert.AreEqual(3, _model.Table.GetValue(_model.Table.Size / 2, 0));
            Assert.AreEqual(4, _model.Table.GetValue(0, _model.Table.Size - 1));
            Assert.AreEqual(5, _model.Table.GetValue(_model.Table.Size - 1, _model.Table.Size - 1));
        }
        [TestMethod]
        public void EscapeNewHardGameTest()
        {
            _model.Difficulty = Difficulty.Hard;
            _model.NewGame();
            Assert.AreEqual(Difficulty.Hard, _model.Difficulty);
            Assert.AreEqual(21, _model.Table.Size);
            Assert.AreEqual(0, _model.GameTime);
            Assert.AreEqual(3, _model.Table.GetValue(_model.Table.Size / 2, 0));
            Assert.AreEqual(4, _model.Table.GetValue(0, _model.Table.Size - 1));
            Assert.AreEqual(5, _model.Table.GetValue(_model.Table.Size - 1, _model.Table.Size - 1));
        }

        [TestMethod]
        public void EscapeGameModelAdvanceTimeTest()
        {
            _model.NewGame();
            int time = _model.GameTime;

            _model.AdvanceTime();
            time++;
            Assert.AreEqual(time, _model.GameTime);
        }
        [TestMethod]
        public void EscapeGameModelStepTest()
        {
            _model.NewGame();

            _model.Step(7, 0, 3, "down"); // lépünk lefelé
            Assert.AreEqual(0, _model.Table.GetValue(7, 0)); //törölte-e mögötte?
            Assert.AreEqual(3, _model.Table.GetValue(7, 1)); //lépett-e?
        }
        [TestMethod]
        public async Task EscapeGameModelLoadTest()
        {
            _model.NewGame();
            await _model.LoadGameAsync(string.Empty);
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Assert.AreEqual(_mockedTable.GetValue(i, j), _model.Table.GetValue(i, j));
                }
            }
            Assert.AreEqual(0, _model.GameTime);
            _mock.Verify(dataAccess => dataAccess.LoadAsync(string.Empty), Times.Once());
        }
        private void Model_GameAdvanced(Object? sender, EscapeEventArgs e)
        {
            Assert.AreEqual(e.GameTime, _model.GameTime);
            Assert.IsFalse(e.isWon);
        }
        private void Model_GameOver(Object? sender, EscapeEventArgs e)
        {
            Assert.IsTrue(_model.IsGameOver);
            Assert.AreEqual(0, e.GameTime);
            Assert.IsFalse(e.isWon);
        }
    }
}
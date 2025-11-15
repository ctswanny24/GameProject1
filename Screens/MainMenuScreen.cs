using Microsoft.Xna.Framework;
using GameProject1.StateManagement;
using Microsoft.Xna.Framework.Media;
using System.Text.Json;
using GameProject1.Saving;

namespace GameProject1.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenuScreen : MenuScreen
    {
        private Song menuSong;
        private Game _game;

        public MainMenuScreen(Game game) : base("Barry the Bowling Ball")
        {
            _game = game;

            var playGameMenuEntry = new MenuEntry("Start New Game");
            var resumeGameMenuEntry = new MenuEntry("Load Saved Game");
            //var optionsMenuEntry = new MenuEntry("Options (Currently Under Development)");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            resumeGameMenuEntry.Selected += LoadSavedGameEntrySelected;
            //optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(resumeGameMenuEntry);
            //MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        private void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Stop();
            TopDownScreen newGame = new TopDownScreen(ScreenManager.GraphicsDevice, _game, null);
            //RockslideMinigame newGame = new RockslideMinigame(ScreenManager.GraphicsDevice, _game);
            //MinigameScreen newGame = new MinigameScreen(ScreenManager.GraphicsDevice);
            newGame.Initialize();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, newGame);

        }

        private void LoadSavedGameEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Stop();
            TopDownScreen newGame = new TopDownScreen(ScreenManager.GraphicsDevice, _game, SaveStateManager.LoadGame());
            //RockslideMinigame newGame = new RockslideMinigame(ScreenManager.GraphicsDevice, _game);
            //MinigameScreen newGame = new MinigameScreen(ScreenManager.GraphicsDevice);
            newGame.Initialize();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, newGame);
        }

        private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit this game?";
            var confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        private void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}

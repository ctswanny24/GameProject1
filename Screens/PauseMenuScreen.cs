using GameProject1.Player;
using GameProject1.Saving;
using GameProject1.StateManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameProject1.Screens
{


    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class PauseMenuScreen : MenuScreen
    {
        private Game _game;
        private BowlingBallMan _player;
        private List<Villian> _villians;

        public PauseMenuScreen(Game game) : base("Paused")
        {
            _game = game;
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        public PauseMenuScreen(Game game, BowlingBallMan player, List<Villian> villians) : base("Paused")
        {
            _game = game;
            _player = player;
            _villians = villians;
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var saveGameMenuEntry = new MenuEntry("Save Game");
            var quitGameMenuEntry = new MenuEntry("Quit Game");

            resumeGameMenuEntry.Selected += OnCancel;
            saveGameMenuEntry.Selected += OnSave;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(saveGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);

        }

        private void OnSave(object sender, PlayerIndexEventArgs e)
        {
            List<Tuple<float, float, bool>> villianInfo = new List<Tuple<float, float, bool>>();
            foreach (Villian v in _villians)
                villianInfo.Add(new Tuple<float, float, bool>(v.Position.X, v.Position.Y, v.Dead));
            SaveStateManager.SaveGame(new SaveData(_player.Position.X, _player.Position.Y, _player.Health, villianInfo));
        }

        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        // This uses the loading screen to transition from the game back to the main menu screen.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(_game));
        }
    }
}

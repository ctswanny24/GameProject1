using System;
using GameProject1.StateManagement;

namespace GameProject1
{
    // Our game's implementation of IScreenFactory which can handle creating the screens
    public class ScreenFactory : IScreenFactory
    {
        public StateManagement.GameScreen CreateScreen(Type screenType)
        {
            // All of our screens have empty constructors so we can just use Activator
            return Activator.CreateInstance(screenType) as StateManagement.GameScreen;
        }
    }
}

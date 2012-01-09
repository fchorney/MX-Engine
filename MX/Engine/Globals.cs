namespace MX
{
    public static class GameEnvironment
    {
        public static double ElapsedTime
        {
            get { return Engine.GetInstance.ElapsedTime; }
        }

        public static Vector2D Camera
        {
            get { return Graphics.GetInstance.Camera; }
        }

        public static Graphics Screen
        {
            get { return Graphics.GetInstance; }
        }

        public static Mouse Mouse
        {
            get { return Mouse.GetInstance; }
        }

        public static Keyboard Keyboard
        {
            get { return Keyboard.GetInstance; }
        }
    }

    public static class GameResources
    {
        public static T Load<T>(string name) where T : IResource<T>, new()
        {
            return ResourceCache<T>.Get(name);
        }
    }

    /*
    public static class GameVariables
    {

    }
    */
}

namespace ProgPart3
{
    public class User
    {
        public string Name { get; private set; }

        public void AskName()
        {
            Name = "User"; // You can expand this later to use WPF input
        }
    }
}

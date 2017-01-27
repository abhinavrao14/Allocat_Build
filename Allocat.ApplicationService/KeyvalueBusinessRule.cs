namespace Allocat.ApplicationService
{
    public class KeyValueBusinessRule : ValidationRules
    {
        public KeyValueBusinessRule()
        {
        }

        public bool ValidateType(string Type)
        {
            return true;
        }
    }
}
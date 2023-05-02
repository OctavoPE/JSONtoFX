namespace Converter
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool UseNormals = false;
            float outlineMulti = 1.0F;

            while (true)
            {
                Console.WriteLine("Please enter the name of the JSON file within this directory to convert to a .fx file: ");
                String fileName = Console.ReadLine();

                Console.WriteLine("Use default options? (Y/N) Y: uses defaults, N: enter own options");
                String userdefault = Console.ReadLine();
                if (userdefault.ToUpper().Contains("N"))
                {
                    Console.WriteLine("OPTIONAL: Would you like to use normals? Please note: Current functionality does not have a normal texture provided to the .fx file. Enter (Y/N): (N is recommended for now)");
                    string userNormals = Console.ReadLine();
                    if (userNormals.ToUpper().Contains("Y"))
                    {
                        UseNormals = true;
                    }
                    Console.WriteLine("OPTIONAL: Enter a number to use as your outline multiplier: (default is 1):");
                    try
                    {
                        outlineMulti = float.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Invalid syntax for float. Will use default of 1.");
                    }
                }

                if (!fileName.ToLower().Contains(".json"))
                {
                    fileName = fileName + ".json";
                }
                Console.WriteLine("Reading JSON data from " + fileName);

                try
                {
                    JSONReader Reader = new JSONReader(fileName);
                    Reader.WriteFXFile(fileName.Replace(".json", ".fx"), UseNormals, outlineMulti);
                    Console.WriteLine("Successfully generated .fx file.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not read JSON data! Error output: " + ex);
                }
            }
        }
    }
}

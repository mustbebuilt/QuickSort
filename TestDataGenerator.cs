namespace QuickSort
{
    public enum DataDistribution
    {
        Random = 1,
        AlreadySorted = 2,
        ReverseSorted = 3,
        DuplicateHeavy = 4
    }

    public static class TestDataGenerator
    {
        public static readonly int[] SupportedSizes = [100, 1000, 5000];

        public static int[] Generate(int size, DataDistribution distribution)
        {
            Random rand = new Random();
            int[] data = new int[size];

            switch (distribution)
            {
                case DataDistribution.Random:
                    for (int i = 0; i < size; i++)
                    {
                        data[i] = rand.Next(1, size * 10 + 1);
                    }
                    break;

                case DataDistribution.AlreadySorted:
                    for (int i = 0; i < size; i++)
                    {
                        data[i] = (i + 1);
                    }
                    break;

                case DataDistribution.ReverseSorted:
                    for (int i = 0; i < size; i++)
                    {
                        data[i] = (size - i);
                    }
                    break;

                case DataDistribution.DuplicateHeavy:
                    // Only 5 distinct values (e.g., 1 to 5) across the entire array
                    const int distinctValues = 5;
                    for (int i = 0; i < size; i++)
                    {
                        data[i] = rand.Next(1, distinctValues + 1);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(distribution), "Unknown distribution type.");
            }

            return data;
        }

        public static string GetDistributionName(DataDistribution distribution)
        {
            return distribution switch
            {
                DataDistribution.Random => "Random",
                DataDistribution.AlreadySorted => "Already Sorted",
                DataDistribution.ReverseSorted => "Reverse-Sorted",
                DataDistribution.DuplicateHeavy => "Duplicate-Heavy (few unique values)",
                _ => distribution.ToString()
            };
        }
    }
}

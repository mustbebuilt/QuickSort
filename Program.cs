namespace QuickSort
{
    internal class Program
    {
        #region MergeSort
        static void MergeSort(int[] A, int left, int right, ref long comparisons, ref long recursions)
        {
            if (left < right)
            {
                int mid = left + (right - left) / 2;

                recursions++;
                MergeSort(A, left, mid, ref comparisons, ref recursions);

                recursions++;
                MergeSort(A, mid + 1, right, ref comparisons, ref recursions);

                Merge(A, left, mid, right, ref comparisons);
            }
        }

        static void Merge(int[] A, int left, int mid, int right, ref long comparisons)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            int[] L = new int[n1];
            int[] R = new int[n2];

            Array.Copy(A, left, L, 0, n1);
            Array.Copy(A, mid + 1, R, 0, n2);

            int i = 0, j = 0, k = left;

            while (i < n1 && j < n2)
            {
                comparisons++;
                if (L[i] <= R[j])
                {
                    A[k++] = L[i++];
                }
                else
                {
                    A[k++] = R[j++];
                }
            }

            while (i < n1)
            {
                A[k++] = L[i++];
            }

            while (j < n2)
            {
                A[k++] = R[j++];
            }
        }
        #endregion

        #region QuickSort (Pivot at Start)
        static void QuickSortPivotStart(int[] A, int left, int right, ref long comparisons, ref long recursions)
        {
            if (left < right)
            {
                int p = PartitionPivotStart(A, left, right, ref comparisons);

                recursions++;
                QuickSortPivotStart(A, left, p - 1, ref comparisons, ref recursions);

                recursions++;
                QuickSortPivotStart(A, p + 1, right, ref comparisons, ref recursions);
            }
        }

        static int PartitionPivotStart(int[] A, int left, int right, ref long comparisons)
        {
            int pivot = A[left];
            int i = left + 1;
            int j = right;

            while (i <= j)
            {
                while (i <= right)
                {
                    comparisons++;
                    if (A[i] <= pivot)
                        i++;
                    else
                        break;
                }

                while (j > left)
                {
                    comparisons++;
                    if (A[j] > pivot)
                        j--;
                    else
                        break;
                }

                if (i < j)
                {
                    int temp = A[i];
                    A[i] = A[j];
                    A[j] = temp;
                }
            }

            int tempPivot = A[left];
            A[left] = A[j];
            A[j] = tempPivot;

            return j;
        }
        #endregion

        #region QuickSort (Pivot at End)
        static void QuickSortPivotEnd(int[] A, int left, int right, ref long comparisons, ref long recursions)
        {
            if (left < right)
            {
                int p = PartitionPivotEnd(A, left, right, ref comparisons);

                recursions++;
                QuickSortPivotEnd(A, left, p - 1, ref comparisons, ref recursions);

                recursions++;
                QuickSortPivotEnd(A, p + 1, right, ref comparisons, ref recursions);
            }
        }

        static int PartitionPivotEnd(int[] A, int left, int right, ref long comparisons)
        {
            int pivot = A[right];
            int i = left - 1;

            for (int j = left; j < right; j++)
            {
                comparisons++;
                if (A[j] <= pivot)
                {
                    i++;
                    int temp = A[i];
                    A[i] = A[j];
                    A[j] = temp;
                }
            }

            int tempPivot = A[i + 1];
            A[i + 1] = A[right];
            A[right] = tempPivot;

            return i + 1;
        }
        #endregion

        static bool IsSorted(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] > array[i + 1])
                    return false;
            }
            return true;
        }

        static (int size, DataDistribution distribution) ConfigureTestData()
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("           Configure Test Dataset                 ");
            Console.WriteLine("--------------------------------------------------");

            // 1. Choose size
            int size = 100;
            while (true)
            {
                Console.WriteLine("Choose array size:");
                Console.WriteLine("  1. 100 integers");
                Console.WriteLine("  2. 1,000 integers");
                Console.WriteLine("  3. 5,000 integers");
                Console.Write("Select size (1-3) [default: 1]: ");

                string? choice = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(choice) || choice == "1")
                {
                    size = 100;
                    break;
                }
                if (choice == "2")
                {
                    size = 1000;
                    break;
                }
                if (choice == "3")
                {
                    size = 5000;
                    break;
                }
                Console.WriteLine("[!] Invalid choice. Please enter 1, 2, or 3.\n");
            }

            // 2. Choose distribution
            DataDistribution distribution = DataDistribution.Random;
            while (true)
            {
                Console.WriteLine("\nChoose data distribution:");
                Console.WriteLine("  1. Random");
                Console.WriteLine("  2. Already sorted");
                Console.WriteLine("  3. Reverse-sorted");
                Console.WriteLine("  4. Duplicate heavy");
                Console.Write("Select distribution (1-4) [default: 1]: ");

                string? choice = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(choice) || choice == "1")
                {
                    distribution = DataDistribution.Random;
                    break;
                }
                if (choice == "2")
                {
                    distribution = DataDistribution.AlreadySorted;
                    break;
                }
                if (choice == "3")
                {
                    distribution = DataDistribution.ReverseSorted;
                    break;
                }
                if (choice == "4")
                {
                    distribution = DataDistribution.DuplicateHeavy;
                    break;
                }
                Console.WriteLine("[!] Invalid choice. Please enter 1, 2, 3, or 4.");
            }

            Console.WriteLine($"\n[✓] Selected: {size:N0} integers with {TestDataGenerator.GetDistributionName(distribution)} distribution.");
            return (size, distribution);
        }

        static void Main()
        {
            // Initial configuration
            var (currentSize, currentDistribution) = ConfigureTestData();
            int[] sampleArray = TestDataGenerator.Generate(currentSize, currentDistribution);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("==================================================");
                Console.WriteLine("           Sorting Algorithm Selector             ");
                Console.WriteLine("==================================================");
                Console.WriteLine($"Active Dataset: {sampleArray.Length:N0} integers | {TestDataGenerator.GetDistributionName(currentDistribution)}");
                Console.WriteLine("1. MergeSort");
                Console.WriteLine("2. QuickSort (Pivot at Start)");
                Console.WriteLine("3. QuickSort (Pivot at End)");
                Console.WriteLine("4. Change Test Data (Size / Distribution)");
                Console.WriteLine("5. Regenerate Array (with current settings)");
                Console.WriteLine("6. Exit");
                Console.WriteLine("==================================================");
                Console.Write("Enter your choice (1-6): ");

                string? input = Console.ReadLine()?.Trim();

                if (input == "6")
                {
                    Console.WriteLine("Exiting program. Goodbye!");
                    break;
                }

                if (input == "4")
                {
                    (currentSize, currentDistribution) = ConfigureTestData();
                    sampleArray = TestDataGenerator.Generate(currentSize, currentDistribution);
                    continue;
                }

                if (input == "5")
                {
                    sampleArray = TestDataGenerator.Generate(currentSize, currentDistribution);
                    Console.WriteLine($"\n[✓] Regenerated {currentSize:N0} integers with {TestDataGenerator.GetDistributionName(currentDistribution)} distribution.");
                    continue;
                }

                if (input != "1" && input != "2" && input != "3")
                {
                    Console.WriteLine("\n[!] Invalid selection. Please enter a number between 1 and 6.");
                    continue;
                }

                // Work on a copy so the same dataset can be reused across algorithms
                int[] workingArray = (int[])sampleArray.Clone();
                int originalStart = workingArray[0];
                int originalEnd = workingArray[^1];

                long comparisons = 0;
                long recursions = 0;
                string algorithmName = "";

                switch (input)
                {
                    case "1":
                        algorithmName = "MergeSort";
                        MergeSort(workingArray, 0, workingArray.Length - 1, ref comparisons, ref recursions);
                        break;
                    case "2":
                        algorithmName = "QuickSort (Pivot at Start)";
                        QuickSortPivotStart(workingArray, 0, workingArray.Length - 1, ref comparisons, ref recursions);
                        break;
                    case "3":
                        algorithmName = "QuickSort (Pivot at End)";
                        QuickSortPivotEnd(workingArray, 0, workingArray.Length - 1, ref comparisons, ref recursions);
                        break;
                }

                int sortedStart = workingArray[0];
                int sortedEnd = workingArray[^1];
                bool verified = IsSorted(workingArray);

                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine($"Algorithm:                  {algorithmName}");
                Console.WriteLine($"Distribution:               {TestDataGenerator.GetDistributionName(currentDistribution)}");
                Console.WriteLine($"Length of array:            {workingArray.Length:N0}");
                Console.WriteLine($"Start value (first item):   {sortedStart}  (Original: {originalStart})");
                Console.WriteLine($"End value (last item):      {sortedEnd}  (Original: {originalEnd})");
                Console.WriteLine($"Number of Comparisons made: {comparisons:N0}");
                Console.WriteLine($"Number of Recursions made:  {recursions:N0}");
                Console.WriteLine($"Verification:               {(verified ? "PASSED" : "FAILED")}");
                Console.WriteLine("--------------------------------------------------");
            }
        }
    }
}

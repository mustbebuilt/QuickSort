# Sorting Algorithm Benchmark: MergeSort vs QuickSort

An interactive C# .NET console application comparing the behavior, comparison counts, and recursion depth of **MergeSort**, **QuickSort (Pivot at Start)**, and **QuickSort (Pivot at End)** under various dataset sizes and distributions.

---

## Algorithms & Pivot Selection

### 1. MergeSort
* **Concept:** A classic divide-and-conquer algorithm. It recursively bisects the array into halves until single-element subarrays are reached, then merges the halves in sorted order.
* **Guarantees:** Always operates in $O(n \log n)$ time regardless of initial ordering (best, average, and worst cases).
* **Trade-off:** Requires $O(n)$ auxiliary memory for merge buffers.

### 2. QuickSort & Pivots
* **Concept:** Selects a *pivot* element and partitions the array into two sub-segments: elements $\le \text{pivot}$ and elements $> \text{pivot}$. The sub-segments are then sorted recursively in-place.
* **Complexity:** Average case is $O(n \log n)$, but pathological worst-case inputs cause unbalanced partitions and degrade performance to $O(n^2)$.
* **Pivot at Start (`A[left]`):**
  * Uses the first element of the subarray as the pivot.
  * Very efficient for random data.
  * **Vulnerability:** When the array is already sorted or reverse-sorted, the first element is the extreme minimum or maximum. Each partition only removes 1 element, causing maximum recursion depth ($O(n)$) and quadratic comparisons ($\approx \frac{n^2}{2}$).
* **Pivot at End (`A[right]`):**
  * Uses the last element of the subarray as the pivot (Lomuto partition scheme).
  * Very efficient on uniformly random inputs.
  * **Vulnerability:** Similar to pivot-at-start, already-sorted and reverse-sorted sequences pick the extreme element every time, degenerating to $O(n^2)$ comparisons.

---

## Predicted Test Outcomes

| Data Distribution | MergeSort | QuickSort (Pivot at Start) | QuickSort (Pivot at End) | Why? |
| :--- | :--- | :--- | :--- | :--- |
| **1. Random** | **Best / Steady**<br>~$n \log_2 n$ comparisons | **Fast**<br>~$1.38 n \ln n$ comparisons | **Fast**<br>~$1.38 n \ln n$ comparisons | Random pivots tend to split subarrays reasonably close to the middle. |
| **2. Already Sorted** | **Very Low**<br>~Minimal merge comparisons | **Worst Case ($O(n^2)$)**<br>$\approx \frac{n^2}{2}$ comparisons | **Worst Case ($O(n^2)$)**<br>$\approx \frac{n^2}{2}$ comparisons | The pivot is strictly the minimum or maximum; partitions split into $0$ and $n-1$ elements. |
| **3. Reverse-Sorted** | **Low / Steady**<br>~$n \log_2 n$ comparisons | **Worst Case ($O(n^2)$)**<br>$\approx \frac{n^2}{2}$ comparisons | **Worst Case ($O(n^2)$)**<br>$\approx \frac{n^2}{2}$ comparisons | Pivots are again extreme values, producing unbalanced partitions on every step. |
| **4. Duplicate-Heavy** | **Steady**<br>~$n \log_2 n$ comparisons | **Sub-optimal**<br>Higher comparisons | **Sub-optimal**<br>Higher comparisons | Standard 2-way partitioning places equal elements on one side, skewing balance unless 3-way partitioning is used. |

### Expected Comparison Counts by Size

| Size ($N$) | Algorithm | Random (approx.) | Already Sorted / Reverse | Duplicate-Heavy (values 1–5) |
| :--- | :--- | :--- | :--- | :--- |
| **100** | MergeSort | ~540 | ~320 | ~500 |
| | QuickSort (Start) | ~700 – 900 | **~5,050** ($O(n^2)$) | ~1,200 |
| | QuickSort (End) | ~600 – 800 | **~4,950** ($O(n^2)$) | ~1,100 |
| **1,000** | MergeSort | ~8,700 | ~5,000 | ~8,200 |
| | QuickSort (Start) | ~11,000 – 14,000 | **~500,500** ($O(n^2)$) | ~104,000 |
| | QuickSort (End) | ~10,000 – 13,000 | **~499,500** ($O(n^2)$) | ~101,000 |
| **5,000** | MergeSort | ~54,000 | ~32,000 | ~51,000 |
| | QuickSort (Start) | ~70,000 – 85,000 | **~12,502,500** ($O(n^2)$) | ~2,600,000 |
| | QuickSort (End) | ~65,000 – 80,000 | **~12,497,500** ($O(n^2)$) | ~2,500,000 |

---

## Code Structure

The project is structured into two main source files:

```
QuickSort/
├── Program.cs             # Sorting implementations, metrics instrumentation & interactive UI
├── TestDataGenerator.cs   # Data generation engine (sizes & distributions)
├── QuickSort.csproj       # Project file (.NET 10)
└── README.md              # Project documentation & predictions
```

### 1. `Program.cs`
* **Algorithm Implementations:**
  * `MergeSort` & `Merge`: Recursive merge sort using temporary sub-arrays and tracking element comparisons.
  * `QuickSortPivotStart` & `PartitionPivotStart`: Hoare-like two-pointer partitioning selecting `A[left]` as pivot.
  * `QuickSortPivotEnd` & `PartitionPivotEnd`: Lomuto partitioning selecting `A[right]` as pivot.
* **Instrumentation:**
  * `ref long comparisons`: Increments on each array element comparison.
  * `ref long recursions`: Increments each time a function makes a recursive call to itself.
  * `IsSorted(int[] array)`: Verifies correctness in ascending order.
* **Interactive Menu & State:**
  * Manages active dataset configuration (`currentSize`, `currentDistribution`).
  * Clones the array before each sort (`(int[])sampleArray.Clone()`) so all three algorithms run against the exact same data for fair comparison.

### 2. `TestDataGenerator.cs`
* **`enum DataDistribution`:**
  * `Random`: Generates positive integers across a wide uniform range.
  * `AlreadySorted`: Creates `1, 2, ..., N`.
  * `ReverseSorted`: Creates `N, N-1, ..., 1`.
  * `DuplicateHeavy`: Constrains all values to the range `[1..5]`.
* **`Generate(int size, DataDistribution distribution)`:** Factory method generating arrays of sizes `100`, `1,000`, and `5,000`.

---

## Running the Application

In the project directory, run:

```bash
dotnet run
```

1. Select dataset size (`100`, `1,000`, or `5,000`).
2. Select distribution (`Random`, `Already sorted`, `Reverse-sorted`, or `Duplicate heavy`).
3. Choose algorithms from the menu (`1` to `3`) to view and compare metrics.

using System;
using System.Collections.Generic;
using System.Threading;

namespace Text_Adventure_Game
{
    internal class Search_and_Sort<T>
    {
        //the list which will contain the items to be searched or sorted
        public List<T> itemList = new List<T>();
        // this variable will hold what needs to be found
        public dynamic find;


        #region Linear Search
        //this the method that  will have do the linear search, Returns true or false depending on if found
        public bool LinearSearch()
        {

            while (true)
            {

                int index = 0;
                //stored -1 or 0
                int returndValue = LinearSearching(itemList, index, find);
                if (returndValue == -1)
                {
                    return false;

                }
               
                return true;

             
            }
        }
        private int LinearSearching(List<T> list, int index, dynamic inputted)
        {
            //checking if the list at the index is the same as the one wanted
            if (list[index] == inputted)
            {
                return index;
            }
            else if (index == list.Count - 1)
            {
                return -1;
            }
            else
            {
                index = index + 1;

                return LinearSearching(list, index, inputted);
            }
        }
        #endregion

        #region Binary search
        //this returns true or false depending if found.
        public bool BinarySearchingStart()
        {

            int index = BinarySearchRecursive(itemList, find);
            if (index == 1)
            {

                return true;
            }

            return false;

        }
        //passes in the list from the class and the value to find from the class
        private static int BinarySearchRecursive(List<T> list, dynamic inputted)
        {
            return BinarySearchRecursive(list, 0, list.Count - 1, inputted);
        }

        private static int BinarySearchRecursive(List<T> array, int left, int right, dynamic item)
        {
            if (right >= left)
            {
                //finding the midpoint
                dynamic middle = left + (right - left) / 2;
                //if the item is found, it returns at what index
                if (array[middle] == item)
                {
                    return middle;

                }
                //if the item searching for is greater than half the list
                if (array[middle] > item)
                {
                    return BinarySearchRecursive(array, left, middle - 1, item);

                }

                return BinarySearchRecursive(array, middle + 1, right, item);
            }

            return -1;
        }
        #endregion

        #region BubbleSort
        //returns the ordered list
        public List<T> BubbleSort()
        {

            int count = 0;
            List<T> newSortedList = new List<T>();
            foreach (T item in itemList)
            {
                count++;
            }

            newSortedList = BubbleSorting(itemList, count);
            //returns the sorted list
            return newSortedList;

        }
        private List<T> BubbleSorting(List<T> sortList, int count)
        {
            int start = 0;
            dynamic biggerItem;
            //if the list has just one item
            if (count == 1)
            {
                return sortList;
            }
            //swapping out bigger items
            for (dynamic i = 0; i < count - 1; i++)
            {
                if (sortList[i] > sortList[i + 1])
                {
                    biggerItem = sortList[i];
                    sortList[i] = sortList[i + 1];
                    sortList[i + 1] = biggerItem;
                    start++;
                }
                //when count == 0 it means it is sorted
                if (count == 0)
                {
                    return sortList;
                }
                //count is minused each recursive event. 
                BubbleSorting(sortList, count - 1);

            }
            return sortList;
        }
        #endregion
        #region Merge Sort
        //returned the sorted array
        public int[] MergeSort(int[] array)
        {
            int[] firstHalf;
            int[] lastHalf;
            int[] result = new int[array.Length];

            //This makes sure there is not an infinete recurrsion
            if (array.Length <= 1)
            {
                return array;

            }
            // The middle of the array  
            int midPoint = array.Length / 2;
            //Will represent our 'firstHalf' array
            firstHalf = new int[midPoint];

            //this makes the the first half and the length of last half of the array are equal even if the array is odd

            if (array.Length % 2 == 0)
            {
                lastHalf = new int[midPoint];

            }
            //if array has an odd number of elements, the lastHalf array will have one more element than firstHalf
            else
            {
                lastHalf = new int[midPoint + 1];

            }
            //adding items in the first half array
            for (int i = 0; i < midPoint; i++)
            {
                firstHalf[i] = array[i];

            }
            //adding items in the second half array
            int x = 0;

            for (int i = midPoint; i < array.Length; i++)
            {
                lastHalf[x] = array[i];
                x++;
            }
            // recursive  process on the firt half
            firstHalf = MergeSort(firstHalf);
            //recursive  process  on second half
            lastHalf = MergeSort(lastHalf);
            //Mergeing the two arrays together
            result = Merge(firstHalf, lastHalf);
            return result;
        }

        //This method will be responsible for combining our two sorted arrays into one giant array
        private int[] Merge(int[] firstHalf, int[] lastHalf)
        {
            int resultLength = lastHalf.Length + firstHalf.Length;
            int[] result = new int[resultLength];
            //
            int indexFirstHalf = 0;
            int indexLastHalf = 0;
            int totalIndex = 0;
            //while either array still has an element
            while (indexFirstHalf < firstHalf.Length || indexLastHalf < lastHalf.Length)
            {
                //if both arrays have elements  
                if (indexFirstHalf < firstHalf.Length && indexLastHalf < lastHalf.Length)
                {
                    //adds the item ot the result array if the item in first half is greater than the item in second half
                    if (firstHalf[indexFirstHalf] <= lastHalf[indexLastHalf])
                    {
                        result[totalIndex] = firstHalf[indexFirstHalf];
                        indexFirstHalf++;
                        totalIndex++;
                    }
                    // else the item in the lastHalf array wll be added to the results array
                    else
                    {
                        result[totalIndex] = lastHalf[indexLastHalf];
                        indexLastHalf++;
                        totalIndex++;
                    }
                }
                //if only the firstHalf array still has elements, add all its items to the results array
                else if (indexFirstHalf < firstHalf.Length)
                {
                    result[totalIndex] = firstHalf[indexFirstHalf];
                    indexFirstHalf++;
                    totalIndex++;
                }
                //if only the lastHalf array still has elements, add all its items to the results array
                else if (indexLastHalf < lastHalf.Length)
                {
                    result[totalIndex] = lastHalf[indexLastHalf];
                    indexLastHalf++;
                    totalIndex++;
                }
            }
            return result;
        }
        #endregion
    }
}

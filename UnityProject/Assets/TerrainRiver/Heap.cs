using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace RiverSimulation {
    public class Heap<T> : IEnumerable {
        private ItemStorage<T> items;
        private int currentItemCount;

        private Dictionary<T, HeapItemInfo> itemToInfo;

        public Heap(int maxHeapSize) {
            itemToInfo = new Dictionary<T, HeapItemInfo>();
            items = new ItemStorageArray<T>(maxHeapSize);
        }

        public Heap() {
            itemToInfo = new Dictionary<T, HeapItemInfo>();
            items = new ItemStorageList<T>();
        }

        public void Set(Heap<T> heap) {
            foreach (T item in heap) {
                Set(item, heap.itemToInfo[item].fCost);
            }
        }

        public void Set(T item, float fCost) {
            HeapItemInfo itemHeapInfo;

            if (itemToInfo.TryGetValue(item, out itemHeapInfo)) {
                if (fCost == itemHeapInfo.fCost)
                    return;

                bool sortUp = fCost < itemHeapInfo.fCost;

                itemHeapInfo.fCost = fCost;

                if (sortUp) {
                    SortUp(item);
                } else {
                    SortDown(item);
                }

            } else {
                itemHeapInfo = new HeapItemInfo(currentItemCount, fCost);
                itemToInfo.Add(item, itemHeapInfo);
                items[currentItemCount] = item;
                currentItemCount++;
                SortUp(item);
            }

        }

        public T RemoveFirst() {
            T firstItem = items[0];
            currentItemCount--;
            T lastItem = items[currentItemCount];
            items[0] = lastItem;
            itemToInfo[lastItem].index = 0;
            SortDown(lastItem);

            itemToInfo.Remove(firstItem);

            return firstItem;
        }

        public int Count {
            get {
                return currentItemCount;
            }
        }

        public bool Contains(T item) {
            return itemToInfo.ContainsKey(item);
        }

        private void SortDown(T item) {
            HeapItemInfo itemHeapInfo = itemToInfo[item];

            while (true) {
                int childIndexLeft = itemHeapInfo.index * 2 + 1;
                int childIndexRight = itemHeapInfo.index * 2 + 2;

                if (childIndexLeft < currentItemCount) {

                    HeapItemInfo itemLeftHeapInfo = itemToInfo[items[childIndexLeft]];
                    HeapItemInfo itemRightHeapInfo = itemToInfo[items[childIndexRight]];

                    HeapItemInfo itemSwapHeapInfo = itemLeftHeapInfo;


                    if (childIndexRight < currentItemCount) {
                        if (itemLeftHeapInfo.CompareTo(itemRightHeapInfo) < 0) {
                            itemSwapHeapInfo = itemRightHeapInfo;
                        }
                    }

                    if (itemHeapInfo.CompareTo(itemSwapHeapInfo) < 0) {
                        Swap(item, itemHeapInfo, items[itemSwapHeapInfo.index], itemSwapHeapInfo);
                    } else {
                        return;
                    }

                } else {
                    return;
                }

            }
        }

        private void SortUp(T item) {
            HeapItemInfo itemHeapInfo = itemToInfo[item];
            int parentIndex = (itemHeapInfo.index - 1) / 2;

            while (true) {
                T parentItem = items[parentIndex];
                HeapItemInfo parentItemHeapInfo = itemToInfo[parentItem];
                if (itemHeapInfo.CompareTo(parentItemHeapInfo) > 0) {
                    Swap(item, itemHeapInfo, parentItem, parentItemHeapInfo);
                } else {
                    break;
                }

                parentIndex = (itemHeapInfo.index - 1) / 2;
            }
        }

        private void Swap(T itemA, HeapItemInfo itemAHeapInfo, T itemB, HeapItemInfo itemBHeapInfo) {
            items[itemAHeapInfo.index] = itemB;
            items[itemBHeapInfo.index] = itemA;

            int itemAIndex = itemAHeapInfo.index;
            itemAHeapInfo.index = itemBHeapInfo.index;
            itemBHeapInfo.index = itemAIndex;
        }

        private class HeapItemInfo : IComparable<HeapItemInfo> {
            public int index;
            public float fCost;

            public HeapItemInfo(int index, float fCost) {
                this.index = index;
                this.fCost = fCost;
            }

            public int CompareTo(HeapItemInfo nodeToCompare) {
                int compare = fCost.CompareTo(nodeToCompare.fCost);
                //if (compare == 0) {
                //    compare = hCost.CompareTo(nodeToCompare.hCost);
                //}
                return -compare;
            }
        }

        public void Clear() {
            items.Clear();
            currentItemCount = 0;

            itemToInfo.Clear();
        }

        // Implementation for the GetEnumerator method.
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator GetEnumerator() {
            return new HeapEnum(items, currentItemCount);
        }

        // When you implement IEnumerable, you must also implement IEnumerator.
        private class HeapEnum : IEnumerator {
            private ItemStorage<T> items;
            private int currentItemCount;

            // Enumerators are positioned before the first element
            // until the first MoveNext() call.
            int position = -1;

            public HeapEnum(ItemStorage<T> items, int currentItemCount) {
                this.items = items;
                this.currentItemCount = currentItemCount;
            }

            public bool MoveNext() {
                position++;
                return (position < currentItemCount);
            }

            public void Reset() {
                position = -1;
            }

            object IEnumerator.Current {
                get {
                    return Current;
                }
            }

            public T Current {
                get {
                    try {
                        return items[position];
                    } catch (IndexOutOfRangeException) {
                        throw new InvalidOperationException();
                    }
                }
            }
        }











        private interface ItemStorage<S> {
            void Clear();

            S this[int i] {
                get;
                set;
            }
        }

        private class ItemStorageList<S> : ItemStorage<S> {
            private List<S> list;

            public ItemStorageList() {
                list = new List<S>();
            }

            public S this[int i] {
                get {
                    return list[i];
                }
                set {
                    if (i == list.Count)
                        list.Add(value);
                    else
                        list[i] = value;
                }
            }

            public void Clear() {
                list.Clear();
            }
        }

        private class ItemStorageArray<S> : ItemStorage<S> {
            private S[] array;

            public ItemStorageArray(int size) {
                array = new S[size];
            }

            public S this[int i] {
                get {
                    return array[i];
                }
                set {
                    array[i] = value;
                }
            }

            public void Clear() {
                array = new S[array.Length];
            }
        }
    }
}

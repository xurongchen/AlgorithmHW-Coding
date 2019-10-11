//
// Created by xurongchen on 2019/10/8.
//

#ifndef SORT_SORT_H
#define SORT_SORT_H
#define UInt32 unsigned int
void insertionsort(UInt32 *array, int len);
void shellsort(UInt32 *array, int len);
void quicksort(UInt32 *array, int len);
void mergesort(UInt32 *array, int len);
void radixsort(UInt32 *array, int len, int radixBit);
void radixsort(UInt32 *array, int len);
//void radixsortDiscard2(UInt32 *array, int len, int radixBit);
//
//void radixsortDiscard(UInt32 *array, int len, int radixBit);
//
//void shellsortB2(UInt32 *array, int len);
#endif //SORT_SORT_H

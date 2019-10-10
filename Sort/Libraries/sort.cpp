//
// Created by xurongchen on 2019/10/8.
//
#include "include/sort.h"
#include "stdlib.h"
#include "memory.h"
#include "math.h"

inline void swap(UInt32 &x,UInt32 &y){
    x = x^y;
    y = x^y;
    x = x^y;
}

void insertionsort(UInt32 *array, int len){
    for(int i=1; i<len; ++i){
        UInt32 now = array[i];
        int j = i - 1;
        for(; j >= 0 && now < array[j]; j--) array[j+1] = array[j];
        array[j+1] = now;
    }
}

int GOODSHELLNUMBER[] = {1, 5, 19, 41, 109, 209, 505, 929, 2161, 3905, 8929, 16001, 36289, 64769, 146305, 260609, 587521,
                        1045505, 2354689, 4188161, 9427969, 16764929, 37730305, 67084289, 150958081, 268386305,
                        603906049, 1073643521}; //Refer from: https://oeis.org/A033622

void shellsort(UInt32 *array, int len){
    int firstGSN = 0;
    while(GOODSHELLNUMBER[firstGSN]<len) firstGSN++;
    firstGSN--;
    UInt32 *arrayEnd = array+len;
    for(; firstGSN>=0; firstGSN--){
        int d=GOODSHELLNUMBER[firstGSN];
        for(UInt32 *now=array+d; now<arrayEnd; ++now){
            UInt32 nv = *now;
            UInt32 *before=now-d, *beforePd = now;
            UInt32 bv;
            for(; before>=array && nv < (bv = *before); beforePd = before, before -= d) {
                *beforePd = bv;
            }
            *beforePd = nv;
        }
    }
}

void shellsortB2(UInt32 *array, int len){
    for(int d=len/2; d>0; d/=2){
        for(int i=d; i<len; ++i){
            UInt32 now = array[i];
            int j = i - d;
            for(; j>=0 && now < array[j]; j-=d) array[j+d] = array[j];
            array[j+d] = now;
        }
    }
}

void quicksort(UInt32 *array, int len){
    if(len <= 1) return;
    UInt32 *cp = array;
    UInt32 *np = array + 1, *ns = array + len;
    UInt32 cmp = *cp;
    for(; np < ns; ++np) {
        if(*np < cmp) {
            *cp = *np;
            ++cp;
            *np = *cp;
        }
    }
    *cp = cmp;
    quicksort(array, cp - array);
    UInt32 *rs = cp + 1;
    quicksort(rs, ns - rs);
}

void mergesort_core(UInt32 *array, UInt32 *help, int len){
    if(len <= 1) return;
    int mid = len / 2;
    UInt32 *lp = array, *rp = array + mid, *ls = array + mid, *rs = array + len;
    mergesort_core(array, help, mid);
    mergesort_core(rp, help, len - mid);
    for(UInt32 *bp = help, *bs = help + len; bp < bs; ++bp){
        if(lp == ls){
            *bp = *rp;
            ++rp;
        }
        else if(rp == rs){
            *bp = *lp;
            ++lp;
        }
        else{
            if(*lp <= *rp){
                *bp = *lp;
                ++lp;
            } else{
                *bp = *rp;
                ++rp;
            }
        }
    }
    memcpy(array, help, sizeof(UInt32)*len);
}

void mergesort(UInt32 *array, int len){
    UInt32 *help = (UInt32 *)malloc(sizeof(UInt32)*len);
    mergesort_core(array, help, len);
    free(help);
}


struct Bucket{
    UInt32 v;
    Bucket* next;
    Bucket(UInt32 _v):v(_v),next(nullptr){}
};
inline void AddNum(Bucket** bPos,UInt32 value) {
    Bucket* nb = new Bucket(value);
    nb->next = *bPos;
    *bPos = nb;
}
inline void SaveFromBucket(UInt32 *dst, Bucket** bList, int bLen){
    UInt32 * dp = dst;
    for(int i=0; i < bLen; ++i){
        Bucket* bn = bList[i];
        while(bn != nullptr){
            *dp = bn->v;
            dp++;
            Bucket* last = bn;
            bn = bn->next;
            delete last;
        }
    }
}

struct BucketA{
    UInt32 *vStart;
    UInt32 *vEnd, *vNow;
    BucketA *next;
    BucketA(int _space):vStart((UInt32*) malloc(_space* sizeof(UInt32))),vEnd(vStart + _space),vNow(vEnd - 1),next(nullptr){}
    BucketA(int _space,BucketA* _next):vStart((UInt32*) malloc(_space* sizeof(UInt32))),vEnd(vStart + _space),vNow(vEnd - 1),next(_next){}

};

int AddNumInitSpace = 1000;
int AddNumAddSapce = 1000;
inline void AddNum(BucketA** bPos,UInt32 value) {
    if(*bPos == nullptr) *bPos = new BucketA(AddNumInitSpace); // init
    BucketA* nb = *bPos;
    if(nb->vNow < nb->vStart) { // used all
        *bPos = new BucketA((int) AddNumAddSapce, nb);
        nb = *bPos;
    }
    *(nb->vNow) = value;
    nb->vNow--;
}

inline void SaveFromBucketA(UInt32 *dst, BucketA** bList, int bLen){
    UInt32 * dp = dst;
    for(int i=0; i < bLen; ++i){
        BucketA* bn = bList[i];
        while(bn != nullptr){
            int cpLen = bn->vEnd - bn->vNow - 1;
            if(cpLen > 0) memcpy(dp,bn->vNow + 1, sizeof(UInt32) * cpLen);
            dp += cpLen;
            BucketA* last = bn;
            bn = bn->next;
            delete[] last;
        }
    }
}

inline UInt32 Show(UInt32 num, int move, UInt32 mask) {
    return (num >> move) & mask;
}

#define NUMLENGTH 32
void radixsortDiscard(UInt32 *array, int len, int radixBit){
    int bsize = 1 << radixBit;
    UInt32 mask = bsize - 1;
    for(int mv = 0; mv<32; mv+=radixBit){
        Bucket** bList = new Bucket*[bsize]{nullptr};
        for(UInt32 *ap = array+len-1, *as = array; ap>=as; --ap){ // must reverse visit!

            AddNum(bList+Show(*ap,mv,mask),*ap);
        }
        SaveFromBucket(array,bList,bsize);
    }
}

void radixsort(UInt32 *array, int len, int radixBit){
    int bsize = 1 << radixBit;
    UInt32 mask = bsize - 1;
    AddNumInitSpace = len/bsize;
    AddNumInitSpace = AddNumInitSpace * (1.01);
    AddNumAddSapce = (UInt32) sqrt(AddNumInitSpace);
    if(AddNumAddSapce==0) AddNumAddSapce = 1;
    for(int mv = 0; mv<32; mv+=radixBit){
        BucketA** bList = new BucketA*[bsize]{nullptr};
        for(UInt32 *ap = array+len-1, *as = array; ap>=as; --ap){ // must reverse visit!
            AddNum(bList+Show(*ap,mv,mask),*ap);
        }
        SaveFromBucketA(array,bList,bsize);
    }
}

void radixsort(UInt32 *array, int len){
    int Bit = 8;
    radixsort(array,len,Bit);
}

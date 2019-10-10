#include <iostream>
#include "Libraries/include/sort.h"
#include "time.h"
#include "math.h"
#include "stdio.h"


#include <unistd.h>
#include <pthread.h>
#include <signal.h>

inline UInt32 RandUInt32(){
    return (rand()<<16)^rand(); //0~4294967295
}

// Your very slow function, it will finish running after 5 seconds, and print Exit message.
// But if we terminate the thread in 3 seconds, Exit message will not print.
//void * veryslow(void *arg)
//{
//    fprintf(stdout, "Enter veryslow...\n");
//    sleep(5);
//    fprintf(stdout, "Exit veryslow...\n");
//
//    return nullptr;
//}

pthread_t tid;
void alarm_handler(int a)
{
    fprintf(stdout, "Enter alarm_handler...\n");
    pthread_cancel(tid);    // terminate thread
}

int problemSize[] = {10, //0, 1e1
                     100, //1, 1e2
                     1000, //2, 1e3
                     10000, //3, 1e4
                     100000, //4, 1e5
                     1000000, //5, 1e6
                     10000000, //6, 1e7
                     100000000, //7, 1e8
                     200000000,  //8, 2e8
                     1000000000 //9, 1e9
                    };

struct PA{
    bool *IsTimeoutP;
    int *LENP;
    UInt32 *a;
    PA(bool *_t,int *_l,UInt32 *_a):IsTimeoutP(_t),LENP(_l),a(_a){}
};
void* RunStdSort(void *arg){
    PA* PAarg = (PA*) arg;
    bool *IsTimeoutP = PAarg->IsTimeoutP;
    int* LENP = PAarg->LENP;
    UInt32* a = PAarg->a;
    *IsTimeoutP = true;
    clock_t cs = clock();
    std::sort(a,a+ *LENP);
    clock_t ce = clock();
    printf("%.6lf",(ce-cs)*1.0/CLOCKS_PER_SEC);
    *IsTimeoutP = false;
    return nullptr;
}
void* RunInsertionSort(void *arg){
    PA* PAarg = (PA*) arg;
    bool *IsTimeoutP = PAarg->IsTimeoutP;
    int* LENP = PAarg->LENP;
    UInt32* a = PAarg->a;
    *IsTimeoutP = true;
    clock_t cs = clock();
    insertionsort(a, *LENP);
    clock_t ce = clock();
    printf("%.6lf",(ce-cs)*1.0/CLOCKS_PER_SEC);
    *IsTimeoutP = false;
    return nullptr;
}
void* RunShellSort(void *arg){
    PA* PAarg = (PA*) arg;
    bool *IsTimeoutP = PAarg->IsTimeoutP;
    int* LENP = PAarg->LENP;
    UInt32* a = PAarg->a;
    *IsTimeoutP = true;
    clock_t cs = clock();
    shellsort(a,*LENP);
    clock_t ce = clock();
    printf("%.6lf",(ce-cs)*1.0/CLOCKS_PER_SEC);
    *IsTimeoutP = false;
    return nullptr;
}
void* RunQuickSort(void *arg){
    PA* PAarg = (PA*) arg;
    bool *IsTimeoutP = PAarg->IsTimeoutP;
    int* LENP = PAarg->LENP;
    UInt32* a = PAarg->a;
    *IsTimeoutP = true;
    clock_t cs = clock();
    quicksort(a,*LENP);
    clock_t ce = clock();
    printf("%.6lf",(ce-cs)*1.0/CLOCKS_PER_SEC);
    *IsTimeoutP = false;
    return nullptr;
}
void* RunMergeSort(void *arg){
    PA* PAarg = (PA*) arg;
    bool *IsTimeoutP = PAarg->IsTimeoutP;
    int* LENP = PAarg->LENP;
    UInt32* a = PAarg->a;
    *IsTimeoutP = true;
    clock_t cs = clock();
    mergesort(a,*LENP);
    clock_t ce = clock();
    printf("%.6lf",(ce-cs)*1.0/CLOCKS_PER_SEC);
    *IsTimeoutP = false;
    return nullptr;
}
void* RunRadixSort(void *arg){
    PA* PAarg = (PA*) arg;
    bool *IsTimeoutP = PAarg->IsTimeoutP;
    int* LENP = PAarg->LENP;
    UInt32* a = PAarg->a;
    *IsTimeoutP = true;
    clock_t cs = clock();
    radixsort(a,*LENP);
    clock_t ce = clock();
    printf("%.6lf",(ce-cs)*1.0/CLOCKS_PER_SEC);
    *IsTimeoutP = false;
    return nullptr;
}

UInt32 *a;
int* LENP;
bool* IsTimeoutP;
int TIMEOUT = 600;

bool RunInTimeout(void *Func (void *)){
    PA* args = new PA(IsTimeoutP,LENP,a);
    pthread_create(&tid, nullptr, Func, args);

    signal(SIGALRM, alarm_handler);
    alarm(TIMEOUT);   // Run alarm_handler after TIMEOUT seconds, and terminate thread in it
    pthread_join(tid, nullptr); // Wait for thread finish
    sleep(1);
    return *IsTimeoutP;
}

int main()
{

    srand(time(NULL));
    printf("Timeout = %ds\n",TIMEOUT);
    LENP = new int;
    IsTimeoutP = new bool;
    for(int i=0;i<10;++i){
        int LEN = problemSize[i];
        *LENP = LEN;
        UInt32 *ab = (UInt32*) malloc(sizeof(UInt32) * LEN);
        for(int i = 0; i < LEN; ++i){
            ab[i] = RandUInt32();
        }

        a = (UInt32*) malloc(sizeof(UInt32) * LEN);
//        memcpy(a,ab,sizeof(UInt32) * LEN);

        printf("Problem Size:%d\n",LEN);

        printf("StdSort: \t\t");
        memcpy(a,ab,sizeof(UInt32) * LEN);
        RunInTimeout(RunStdSort);
        if(*IsTimeoutP) printf("Timeout");
        printf("\n");

//        printf("InsertionSort: \t");
//        memcpy(a,ab,sizeof(UInt32) * LEN);
//        RunInTimeout(RunInsertionSort);
//        if(*IsTimeoutP) printf("Timeout");
//        printf("\n");

        printf("ShellSort: \t\t");
        memcpy(a,ab,sizeof(UInt32) * LEN);
        RunInTimeout(RunShellSort);
        if(*IsTimeoutP) printf("Timeout");
        printf("\n");

        printf("QuickSort: \t\t");
        memcpy(a,ab,sizeof(UInt32) * LEN);
        RunInTimeout(RunQuickSort);
        if(*IsTimeoutP) printf("Timeout");
        printf("\n");

        printf("MergeSort: \t\t");
        memcpy(a,ab,sizeof(UInt32) * LEN);
        RunInTimeout(RunMergeSort);
        if(*IsTimeoutP) printf("Timeout");
        printf("\n");

        printf("RadixSort: \t\t");
        memcpy(a,ab,sizeof(UInt32) * LEN);
        RunInTimeout(RunRadixSort);
        if(*IsTimeoutP) printf("Timeout");
        printf("\n");
    }
}





//
//
//int LEN = 100000000;
////int LEN = 100;
//
//int main() {
//
//    srand(time(NULL));
//
//    UInt32 *a = (UInt32*) malloc(sizeof(UInt32) * LEN);
//    for(int i = 0; i < LEN; ++i){
//        a[i] = RandUInt32();
////        std::cout<<a[i]<<" ";
//    }
//
//    std::cout<<std::endl;
////
////    LEN = 10;
////    UInt32 a[] = {9,8,7,6,55,4,3,2,1,0};
//    time_t start,stop;
//    start = time(NULL);
//    clock_t ts = clock();
//
////    std::sort(a,a+LEN);
////    insertionsort(a, LEN);
//    shellsort(a,LEN);
////    shellsortB2(a,LEN);
////    quicksort(a,LEN);
////    mergesort(a,LEN);
////    radixsort(a,LEN,8);
////    radixsortDiscard(a,LEN,8);
//
//    clock_t te = clock();
//    stop = time(NULL);
//    printf("%.6lf\n",(te-ts)*1.0/CLOCKS_PER_SEC);
//    std::cout<<stop-start<<std::endl;
//    std::cout<<std::endl;
////    for(int i = 0; i < LEN; ++i){
////        std::cout<<a[i]<<"\n";
////    }
//    std::cout<<std::endl;
//
//    return 0;
//}
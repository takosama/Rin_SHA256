#include <iostream>
#include <immintrin.h>
using namespace std;

uint_fast32_t* K = new uint_fast32_t[64]
{
     0x428a2f98,
          0x71374491,
          0xb5c0fbcf,
           0xe9b5dba5,
           0x3956c25b,
           0x59f111f1,
           0x923f82a4,
           0xab1c5ed5,
           0xd807aa98,
           0x12835b01,
           0x243185be,
            0x550c7dc3,
            0x72be5d74,
            0x80deb1fe,
            0x9bdc06a7,
            0xc19bf174,
            0xe49b69c1,
            0xefbe4786,
            0x0fc19dc6,
            0x240ca1cc,
            0x2de92c6f,
            0x4a7484aa,
            0x5cb0a9dc,
            0x76f988da,
            0x983e5152,
            0xa831c66d,
            0xb00327c8,
            0xbf597fc7,
            0xc6e00bf3,
            0xd5a79147,
            0x06ca6351,
            0x14292967,
            0x27b70a85,
            0x2e1b2138,
            0x4d2c6dfc,
            0x53380d13,
            0x650a7354,
            0x766a0abb,
            0x81c2c92e,
            0x92722c85,
            0xa2bfe8a1,
            0xa81a664b,
            0xc24b8b70,
            0xc76c51a3,
            0xd192e819,
            0xd6990624,
            0xf40e3585,
            0x106aa070,
            0x19a4c116,
            0x1e376c08,
            0x2748774c,
            0x34b0bcb5,
            0x391c0cb3,
            0x4ed8aa4a,
            0x5b9cca4f,
            0x682e6ff3,
            0x748f82ee,
            0x78a5636f,
            0x84c87814,
            0x8cc70208,
            0x90befffa,
            0xa4506ceb,
            0xbef9a3f7,
            0xc67178f2
};

__m256i zero = _mm256_set_epi8(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
__m256i mask = _mm256_set_epi8(3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12, 3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12);

extern "C" int __stdcall compute(uint_fast8_t* input, int lng,uint_fast32_t* rtn)
{
 
     
    auto W =(uint_fast32_t*) malloc(sizeof( uint_fast32_t)*64);
    auto _padded = (uint_fast8_t*)malloc(sizeof(uint_fast8_t) * 64);
    uint_fast32_t* pa = (uint_fast32_t*)malloc(sizeof(uint_fast32_t) * 9);
    auto a = pa + 1;
         a[0] = 0x6a09e667;
         a[1] = 0xbb67ae85; 
         a[2] = 0x3c6ef372;
         a[3] = 0xa54ff53a;
         a[4] = 0x510e527f;
         a[5] = 0x9b05688c;
         a[6] = 0x1f83d9ab;
         a[7] = 0x5be0cd19;

       if (lng + 8 >= 64)
           return -1;

       _mm256_storeu_epi8(_padded, zero);
       _mm256_storeu_epi8(_padded+32, zero);

        for (int i = 0; i < lng; i++)
            _padded[i] = input[i];

        _padded[lng] = 0x80;


        auto size = lng* 8;

        _padded[64 - 4] = (uint_fast8_t)(size >> 24);
        _padded[64 - 3] = (uint_fast8_t)(0xff & (size >> 16));
        _padded[64 - 2] = (uint_fast8_t)(0xff & (size >> 8));
        _padded[64 - 1] = (uint_fast8_t)(0xff & size);

        _mm256_storeu_epi8(_padded, _mm256_shuffle_epi8(_mm256_loadu_epi8(_padded), mask));
        _mm256_storeu_epi8(_padded+32, _mm256_shuffle_epi8(_mm256_loadu_epi8(_padded+32), mask));

        for (int t = 0; t < 16; t++)
        {
            W[t] = ((uint_fast32_t*)_padded)[t];

            uint_fast32_t t1 = a[7] + (((a[4] >> 6) | (a[4] << (32 - 6))) ^ ((a[4] >> 11) | (a[4] << (32 - 11))) ^ ((a[4] >> 25) | (a[4] << (32 - 25)))) + ((a[4 ]& a[5]) ^ (~a[4] & a[6])) + K[t] + W[t];
            uint_fast32_t t2 = (((a[0] >> 2) | (a[0] << (32 - 2))) ^ ((a[0] >> 13) | (a[0] << (32 - 13))) ^ ((a[0] >> 22) | (a[0] << (32 - 22)))) + ((a[0] & a[1]) ^ (a[0] & a[2]) ^ (a[1] & a[2]));


            _mm256_storeu_epi32(pa + 1, _mm256_loadu_epi32 (pa));

          //  a[7] = a[6];
           // a[6] = a[5];
            //a[5] = a[4];
            a[4] = a[4] + t1;
           // a[3] = a[2];
           // a[2] = a[1];
           // a[1] = a[0];
            a[0] = t1 + t2;
        }
         
        for (int t = 16; t < 64; t++)
    {
        uint_fast32_t x0 = W[t - 2];
        uint_fast32_t x1 = W[t - 15];
        W[t] = (((x1 >> 7) | (x1 << (32 - 7))) ^ ((x1 >> 18) | (x1 << (32 - 18))) ^ (x1 >> 3)) + (((x0 >> 17) | (x0 << (32 - 17))) ^ ((x0 >> 19) | (x0 << (32 - 19))) ^ (x0 >> 10)) + W[t - 16] + W[t - 7];
        uint_fast32_t t1 = a[7] + (((a[4] >> 6) | (a[4] << (32 - 6))) ^ ((a[4] >> 11) | (a[4] << (32 - 11))) ^ ((a[4] >> 25) | (a[4] << (32 - 25)))) + ((a[4] & a[5]) ^ (~a[4] & a[6])) + K[t] + W[t];
        uint_fast32_t t2 = (((a[0] >> 2) | (a[0] << (32 - 2))) ^ ((a[0] >> 13) | (a[0] << (32 - 13))) ^ ((a[0] >> 22) | (a[0] << (32 - 22)))) + ((a[0] & a[1]) ^ (a[0] & a[2]) ^ (a[1] & a[2]));


        _mm256_storeu_epi32(pa + 1, _mm256_loadu_epi32(pa));

        //  a[7] = a[6];
         // a[6] = a[5];
          //a[5] = a[4];
        a[4] = a[4] + t1;
        // a[3] = a[2];
        // a[2] = a[1];
        // a[1] = a[0];
        a[0] = t1 + t2;
    }

    rtn[0] = a[7]+ 0x6a09e667;
    rtn[1] = a[6]+ 0xbb67ae85;
    rtn[2] = a[5]+ 0x3c6ef372;
    rtn[3] = a[4]+ 0xa54ff53a;
    rtn[4] = a[3]+ 0x510e527f;
    rtn[5] = a[2]+ 0x9b05688c;
    rtn[6] = a[1]+ 0x1f83d9ab;
    rtn[7] = a[0]+ 0x5be0cd19;

    free(pa);
    free(_padded);
    free(W);
    return 0;
} 
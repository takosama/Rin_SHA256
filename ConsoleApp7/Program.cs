using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Timers;
using Rin;

var result = new SHA256().RunTest();
Console.WriteLine(result);
if (result != 0)
    throw new Exception();

new Bentimark().Run();
class Bentimark
{
    int[] cnts = new int[16];

    public void Run()
    {

        Task.Run(() =>
        {
            Parallel.For(0, 16, i =>
        {
            //   SHA256 sha256 = new SHA256();
            System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create();

            byte[] input = Encoding.ASCII.GetBytes( "rintya!!");

            while (true)
            {
                var tmp = sha256.ComputeHash(input);
                cnts[i]++;
                input[0]++;
            }
        });
        });

        var timer = new System.Timers.Timer(1000);
        timer.Elapsed += OnTimedEvent;
        timer.Enabled = true;
        Console.ReadLine();
    }


    void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        Console.WriteLine(cnts.Sum());
        for (int i = 0; i < 16; i++)
            cnts[i] = 0;
    }

}


 

namespace Rin
{
  unsafe  public class SHA256
    {

        public int RunTest()
        {
            System.Security.Cryptography.SHA256 SSHA256 = System.Security.Cryptography.SHA256.Create();
            SHA256 RSHA256 = new SHA256();
            int rtn = 0;
            var input = RSHA256.ConvertToInput("rintya!!");

            for(int i=0;i<256;i++)
            {
                var ans=SSHA256.ComputeHash(input);
                Console.WriteLine("test "+i);
                var mans = String.Join("", ans.Select(x => x.ToString("x").PadLeft(2, '0')));
                var rans = RSHA256.ComputeHash(input).ToString();
                Console.WriteLine( "Microsoft " + mans);
                Console.WriteLine("Rintya    " + rans);
                Console.WriteLine();

                if (mans != rans)
                {
                    Console.WriteLine("error");
                    rtn++;
                }

                input[0]++;
            }

            return rtn;
        }

        public string ToString()
        {
            return String.Join("",new Span<uint>( this.Result,8) .ToArray() .Select(x => x.ToString("x").PadLeft(8, '0')));
        }

        IntPtr tmp;
        IntPtr tmp2;
        IntPtr tmp3;
        IntPtr tmp4;

        uint* K = null;
        public SHA256()
        {
            tmp = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(sizeof(byte) * 64);
            this._paded = (byte*)tmp.ToPointer();
            tmp2 = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(sizeof(uint) * 8);
            this.Result = (uint*)tmp2.ToPointer();
            tmp3 = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(sizeof(uint) * 64);
            this.K = (uint*)tmp3.ToPointer();
            tmp4 = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(sizeof(uint) * 64);
            this.rtn =(uint*)tmp4.ToPointer();
            K[0] = 0x428a2f98;
            K[1] = 0x71374491;
            K[2] = 0xb5c0fbcf;
            K[3] = 0xe9b5dba5;
            K[4] = 0x3956c25b;
            K[5] = 0x59f111f1;
            K[6] = 0x923f82a4;
            K[7] = 0xab1c5ed5;
            K[8] = 0xd807aa98;
            K[9] = 0x12835b01;
            K[10] = 0x243185be;
            K[11] = 0x550c7dc3;
            K[12] = 0x72be5d74;
            K[13] = 0x80deb1fe;
            K[14] = 0x9bdc06a7;
            K[15] = 0xc19bf174;
            K[16] = 0xe49b69c1;
            K[17] = 0xefbe4786;
            K[18] = 0x0fc19dc6;
            K[19] = 0x240ca1cc;
            K[20] = 0x2de92c6f;
            K[21] = 0x4a7484aa;
            K[22] = 0x5cb0a9dc;
            K[23] = 0x76f988da;
            K[24] = 0x983e5152;
            K[25] = 0xa831c66d;
            K[26] = 0xb00327c8;
            K[27] = 0xbf597fc7;
            K[28] = 0xc6e00bf3;
            K[29] = 0xd5a79147;
            K[30] = 0x06ca6351;
            K[31] = 0x14292967;
            K[32] = 0x27b70a85;
            K[33] = 0x2e1b2138;
            K[34] = 0x4d2c6dfc;
            K[35] = 0x53380d13;
            K[36] = 0x650a7354;
            K[37] = 0x766a0abb;
            K[38] = 0x81c2c92e;
            K[39] = 0x92722c85;
            K[40] = 0xa2bfe8a1;
            K[41] = 0xa81a664b;
            K[42] = 0xc24b8b70;
            K[43] = 0xc76c51a3;
            K[44] = 0xd192e819;
            K[45] = 0xd6990624;
            K[46] = 0xf40e3585;
            K[47] = 0x106aa070;
            K[48] = 0x19a4c116;
            K[49] = 0x1e376c08;
            K[50] = 0x2748774c;
            K[51] = 0x34b0bcb5;
            K[52] = 0x391c0cb3;
            K[53] = 0x4ed8aa4a;
            K[54] = 0x5b9cca4f;
            K[55] = 0x682e6ff3;
            K[56] = 0x748f82ee;
            K[57] = 0x78a5636f;
            K[58] = 0x84c87814;
            K[59] = 0x8cc70208;
            K[60] = 0x90befffa;
            K[61] = 0xa4506ceb;
            K[62] = 0xbef9a3f7;
            K[63] = 0xc67178f2;
        }

        ~SHA256()
        {
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(tmp);
        }



        public uint* Result
        {
            get; private set;
        } = null;


        public byte[] ConvertToInput(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public SHA256 ComputeHash(string str)
        {
             ComputeHash(Encoding.ASCII.GetBytes(str));
            return this;
        }

        Vector256<uint> H = Vector256.Create(0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19);
        Vector256<uint> shift0 = Vector256.Create(2, 13, 22, 0, 6, 11, 25, 0).AsUInt32();
        Vector256<uint> shift1 = Vector256.Create(32 - 2, 32 - 13, 32 - 22, 32 - 0, 32 - 6, 32 - 11, 32 - 25, 32 - 0).AsUInt32();
        Vector256<uint> maskSigma01 = Vector256.Create(1, 0, 0, 0, 1, 0, 0, 0).AsUInt32();

        public SHA256 ComputeHash(byte[] input)
        {
         
            uint* ah = stackalloc uint[9] {0, 0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19 };
            uint* ptr8 = stackalloc uint[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
           
            Padding(input);
            castToUint();

            var W = mapW((uint*)_paded);

            for (int t = 0; t < 64; t++)
            {
                uint ch(uint x, uint y, uint z) => (x & y) ^ (~x & z);
                uint maj(uint x, uint y, uint z) => ((x & y) ^ (x & z) ^ (y & z));

                var ch0 = ch(ah[5], ah[6], ah[7]);
                var maj0 = maj(ah[1], ah[2], ah[3]);
                var tp = Avx2.Shuffle(Avx2.LoadVector256(ah + 1), 0x00);
                var s0=Avx2.ShiftRightLogicalVariable(tp, shift0);
                var s1 = Avx2.ShiftLeftLogicalVariable(tp, shift1);
                var tp3 = Avx2.Or(s0,s1);
                Avx2.Store(ptr8, tp3);
                ptr8[0] = ptr8[0] ^ ptr8[1] ^ ptr8[2];
                ptr8[4] = ptr8[4] ^ ptr8[5] ^ ptr8[6];

                var t1 = ah[8] + ptr8[4] + ch0 + K[t] + W[t]  ;
                var t2 = ptr8[0] + maj0  ;
                Avx2.Store(ah+1, Avx2.LoadVector256(ah));
                ah[1] = t1 + t2;
                ah[5] = ah[5] + t1;
            }
         
            Avx2.Store(Result,Avx2.Add(Avx.LoadVector256(ah+1), H));
            return this;
        }

        Vector256<int> maskw0 = Vector256.Create(0, 0, 0, 0, 13, 13, 13, 0);
        Vector256<uint> maskw1 = Vector256.Create(7, 18, 3, 0, 17, 19, 10, 0u);
        Vector256<uint> maskw2 = Vector256.Create(32 - 7, 32 - 18, 32 - 3, 32 - 0, 32 - 17, 32 - 19, 32 - 10, 32 - 0u);
        Vector256<uint> maskw3 = Vector256.Create(-1, -1, 0, 0, -1, -1, 0, 0).AsUInt32();
        uint* rtn = null;
      
        uint* mapW(uint* arr)
        {
           
            uint* ptr = stackalloc uint[8];
            for (int i = 0; i < 64; i++)
            {
                if (i < 16)
                {
                    rtn[i] = arr[i];
                    continue;
                }


                var t = Avx2.GatherVector256(rtn + i-15, maskw0, 4);
                var v1 = Avx2.ShiftRightLogicalVariable(t, maskw1);
                var v2 = Avx2.ShiftLeftLogicalVariable(t, maskw2);
                v2 = Avx2.And(v2, maskw3);

                var r = Avx2.Or(v1, v2);

                Avx2.Store(ptr, r);
                var r0 = ptr[0] ^ ptr[1] ^ ptr[2];
                var r6 = ptr[4] ^ ptr[5] ^ ptr[6];
                var tmp = r6 + rtn[i - 7] + r0 + rtn[i - 16];
                rtn[i] = tmp;
            }
            return rtn;
        }

        public void lowerSigma01(uint* ptr,int index)
        {
     

        }
         

   //     uint* rtn = null;
        Vector256<byte> mask = Vector256.Create(3,2,1,0,7,6,5,4,11,10,9,8,15,14,13,12, 3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12).AsByte();
      void castToUint( )
        {
          var t=  Avx2.LoadVector256(_paded);
            Avx2.Store(_paded, Avx2.Shuffle(t, mask));

            t = Avx2.LoadVector256(_paded+32);
            Avx2.Store(_paded+32, Avx2.Shuffle(t, mask));
 
        }

        byte* _paded = null;
        void Padding(byte[] arr)
        {
            if (arr.Length + 8 >= 64)
                throw new Exception();


            for (int i = 0; i < arr.Length; i++)
                _paded[i] = arr[i];

            _paded[arr.Length] = 0x80;

            for (int i = arr.Length + 1; i < 64- 8; i++)
                _paded[i] = 0;

            _paded[64 - 8] = 0;
            _paded[64 - 7] = 0;
            _paded[64 - 6] = 0;
            _paded[64 - 5] = 0;
            var size = arr.Length * 8;

            _paded[64 - 4] = (byte)(size >> 24);
            _paded[64 - 3] = (byte)(0xff & (size >> 16));
            _paded[64 - 2] = (byte)(0xff & (size >> 8));
            _paded[64 - 1] = (byte)(0xff & size);
        }
    }
}
//Console.WriteLine(String.Join("",tmp.Select(x=>x.ToString("x"))));

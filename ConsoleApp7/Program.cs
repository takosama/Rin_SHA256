using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Timers;
using Rin;

var r=new SHA256_CPP().RunTest();

var result = new SHA256().RunTest();
Console.WriteLine(result);
//if (result != 0)
  //  throw new Exception();

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
         SHA256_CPP sha256 = new SHA256_CPP();
       //     System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create();

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
    unsafe public class SHA256_CPP
    {
        [DllImport(@"..\..\..\..\x64\Release\Rin_SHA256_CPP.dll", EntryPoint = "compute")]
        public static  extern int compute(byte* input, int lng, uint* rtn);

        uint[] rtna = new uint[8];
        [SkipLocalsInit]

        public SHA256_CPP ComputeHash(byte[] input)
        {
            byte* ptr=stackalloc byte[input.Length];
           for (int i = 0; i < input.Length; i++)
               ptr[i] = input[i];
            uint* rtn = stackalloc uint[8];

                compute(ptr, input.Length, rtn);
 for (int i = 0; i < rtna.Length; i++)
               rtna[i] = rtn[i];
            return this;
        }

            public int RunTest()
        {
            System.Security.Cryptography.SHA256 SSHA256 = System.Security.Cryptography.SHA256.Create();
            SHA256 RSHA256 = new SHA256();
            int rtn = 0;
            var input = RSHA256.ConvertToInput("rintya!!");

            for (int i = 0; i < 256; i++)
            {
                var ans = SSHA256.ComputeHash(input);
                Console.WriteLine("test " + i);
                var mans = String.Join("", ans.Select(x => x.ToString("x").PadLeft(2, '0')));

                uint[] rtna = new uint[8];
                fixed(byte* ptr=input)
                fixed(uint* rtnp= rtna)
                {
                     compute(ptr, input.Length, rtnp);

                }

            var rans=    String.Join("", rtna.ToArray().Select(x => x.ToString("x").PadLeft(8, '0')));

                Console.WriteLine("Microsoft " + mans);
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

    }
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

        uint* rtn = null;
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
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(tmp2);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(tmp3);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(tmp4);
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


    public    struct values
        {
            public uint a0 = 0x6a09e667;
            public uint a1 = 0xbb67ae85;
            public uint a2 = 0x3c6ef372;
            public uint a3 = 0xa54ff53a;
            public uint a4 = 0x510e527f;
            public uint a5 = 0x9b05688c;
            public uint a6 = 0x1f83d9ab;
            public uint a7 = 0x5be0cd19;

            public values()
            {

            }
        }

        byte* _paded = null;
        Vector256<byte> mask = Vector256.Create(3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12, 3, 2, 1, 0, 7, 6, 5, 4, 11, 10, 9, 8, 15, 14, 13, 12).AsByte();
        Vector256<byte> zero = Vector256<byte>.Zero;
        public SHA256 ComputeHash(byte[] input)
        {
            uint a0 = 0x6a09e667;
            uint a1 = 0xbb67ae85;
            uint a2 = 0x3c6ef372;
            uint a3 = 0xa54ff53a;
            uint a4 = 0x510e527f;
            uint a5 = 0x9b05688c;
            uint a6 = 0x1f83d9ab;
            uint a7 = 0x5be0cd19;

            if (input.Length + 8 >= 64)
                throw new Exception();

            Avx2.Store(_paded, zero);
            Avx2.Store(_paded+32, zero);

            for (int i = 0; i < input.Length; i++)
                _paded[i] = input[i];

            _paded[input.Length] = 0x80;

         
           var size = input.Length * 8;
           
           _paded[64 - 4] = (byte)(size >> 24);
           _paded[64 - 3] = (byte)(0xff & (size >> 16));
           _paded[64 - 2] = (byte)(0xff & (size >> 8));
           _paded[64 - 1] = (byte)(0xff & size);

            Avx2.Store(_paded, Avx2.Shuffle(Avx2.LoadVector256(_paded), mask));
            Avx2.Store(_paded + 32, Avx2.Shuffle(Avx2.LoadVector256(_paded + 32), mask));

       

            var W = rtn;
            for(int t=0;t<16;t++)
            {
                W[t] = ((uint*)_paded)[t];

                var t1 = a7 + (((a4 >> 6) | (a4 << (32 - 6))) ^ ((a4 >> 11) | (a4 << (32 - 11))) ^ ((a4 >> 25) | (a4 << (32 - 25)))) + ((a4 & a5) ^ (~a4 & a6)) + K[t] + W[t];
                var t2 = (((a0 >> 2) | (a0 << (32 - 2))) ^ ((a0 >> 13) | (a0 << (32 - 13))) ^ ((a0 >> 22) | (a0 << (32 - 22)))) + ((a0 & a1) ^ (a0 & a2) ^ (a1 & a2));

                a7 = a6;
                a6 = a5;
                a5 = a4;
                a4 = a3 + t1;
                a3 = a2;
                a2 = a1;
                a1 = a0;
                a0 = t1 + t2;

               
            }

            for (int t = 16; t < 64; t++)
            {
                var x0 = W[t - 2];
                var x1 = W[t - 15];
                W[t] = (((x1 >> 7) | (x1 << (32 - 7))) ^ ((x1 >> 18) | (x1 << (32 - 18))) ^ (x1 >> 3)) + (((x0 >> 17) | (x0 << (32 - 17))) ^ ((x0 >> 19) | (x0 << (32 - 19))) ^ (x0 >> 10)) + W[t - 16] + W[t - 7];
                var t1 = a7 + (((a4 >> 6) | (a4 << (32 - 6))) ^ ((a4 >> 11) | (a4 << (32 - 11))) ^ ((a4 >> 25) | (a4 << (32 - 25)))) + ((a4 & a5) ^ (~a4 & a6)) + K[t] + W[t];
                var t2 = (((a0 >> 2) | (a0 << (32 - 2))) ^ ((a0 >> 13) | (a0 << (32 - 13))) ^ ((a0 >> 22) | (a0 << (32 - 22)))) + ((a0 & a1) ^ (a0 & a2) ^ (a1 & a2));

                a7 = a6;
                a6 = a5;
                a5 = a4;
                a4 = a3 + t1;
                a3 = a2;
                a2 = a1;
                a1 = a0;
                a0 = t1 + t2;



            }

            Result[0] = a0 + 0x6a09e667;
            Result[1] = a1 + 0xbb67ae85;
            Result[2] = a2 + 0x3c6ef372;
            Result[3] = a3 + 0xa54ff53a;
            Result[4] = a4 + 0x510e527f;
            Result[5] = a5 + 0x9b05688c;
            Result[6] = a6 + 0x1f83d9ab;
            Result[7] = a7 + 0x5be0cd19;
            return this;
        }


    }
}

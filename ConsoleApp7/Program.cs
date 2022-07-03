using System.Text;
using System.Timers;
using Rin;

//new Bentimark().Run();
new SHA256().RunTest();
class Bentimark
{
    int[] cnts = new int[16];

    public void Run()
    {

        Task.Run(() =>
        {
            Parallel.For(0, 16, i =>
        {
            SHA256 sha256 = new SHA256();

            byte[] input = Encoding.ASCII.GetBytes("rintya!!");

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
    public class SHA256
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
            return String.Join("", this.Result.Select(x => x.ToString("x").PadLeft(8, '0')));
        }

     readonly   uint[] K = {
  0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5, 0xd807aa98,
  0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174, 0xe49b69c1, 0xefbe4786,
  0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da, 0x983e5152, 0xa831c66d, 0xb00327c8,
  0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967, 0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13,
  0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85, 0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819,
  0xd6990624, 0xf40e3585, 0x106aa070, 0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a,
  0x5b9cca4f, 0x682e6ff3, 0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7,
  0xc67178f2
        };

     public   uint[] Result
        {
            get;private set;
        }


        public byte[] ConvertToInput(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        public SHA256 ComputeHash(string str)
        {
             ComputeHash(Encoding.ASCII.GetBytes(str));
            return this;
        }
        public SHA256 ComputeHash(byte[] input)
        {
            uint[] H = { 0x6a09e667, 0xbb67ae85, 0x3c6ef372, 0xa54ff53a, 0x510e527f, 0x9b05688c, 0x1f83d9ab, 0x5be0cd19 };

            var paded = Padding(input);
            var W = mapW(castToUint(paded));

            uint a, b, c, d, e, f, g, h = 0;
            a = H[0];
            b = H[1];
            c = H[2];
            d = H[3];
            e = H[4];
            f = H[5];
            g = H[6];
            h = H[7];

            for (int t = 0; t < 64; t++)
            {
                var t1 = ((h + upperSigma1(e) + ch(e, f, g) + K[t] + W[t]) & 0xffffffff);
                var t2 = ((upperSigma0(a) + maj(a, b, c)) & 0xffffffff);
                h = g;
                g = f;
                f = e;
                e = (d + t1);
                d = c;
                c = b;
                b = a;
                a = (t1 + t2);

            }
            H[0] = H[0] + a;
            H[1] = H[1] + b;
            H[2] = H[2] + c;
            H[3] = H[3] + d;
            H[4] = H[4] + e;
            H[5] = H[5] + f;
            H[6] = H[6] + g;
            H[7] = H[7] + h;

            this.Result = H;
            return this;
        }

        uint[] mapW(uint[] arr)
        {
            var rtn = new uint[64];
            for (int i = 0; i < rtn.Length; i++)
            {
                if (i < 16)
                {
                    rtn[i] = arr[i];
                    continue;
                }

                var tmp = lowerSigma1(rtn[i - 2]) + rtn[i - 7] + lowerSigma0(rtn[i - 15]) + rtn[i - 16];
                rtn[i] = tmp;
            }
            return rtn;
        }

        uint rotr(uint x, int n) => ((x >> n) | (x << (32 - n)));
        uint shr(uint x, int n) => (x >> n);
        uint ch(uint x, uint y, uint z) => (x & y) ^ (~x & z);
        uint maj(uint x, uint y, uint z) => ((x & y) ^ (x & z) ^ (y & z));
        uint upperSigma0(uint x) => (rotr(x, 2) ^ rotr(x, 13) ^ rotr(x, 22));
        uint upperSigma1(uint x) => rotr(x, 6) ^ rotr(x, 11) ^ rotr(x, 25);
        uint lowerSigma0(uint x) => (rotr(x, 7) ^ rotr(x, 18) ^ shr(x, 3));
        uint lowerSigma1(uint x) => rotr(x, 17) ^ rotr(x, 19) ^ shr(x, 10);
        uint[] castToUint(byte[] arr)
        {
            uint[] rtn = new uint[64 / 4];

            for (int i = 0; i < rtn.Length; i++)
                rtn[i] = (uint)((uint)(arr[i * 4] << 24) + (uint)(arr[i * 4 + 1] << 16) + (uint)(arr[i * 4 + 2] << 8) + (uint)arr[i * 4 + 3]);

            return rtn;
        }
        byte[] Padding(byte[] arr)
        {
            if (arr.Length + 8 >= 64)
                throw new Exception();

            byte[] buf = new byte[64];

            for (int i = 0; i < arr.Length; i++)
                buf[i] = arr[i];

            buf[arr.Length] = 0x80;

            buf[64 - 8] = 0;
            buf[64 - 7] = 0;
            buf[64 - 6] = 0;
            buf[64 - 5] = 0;
            var size = arr.Length * 8;

            buf[64 - 4] = (byte)(size >> 24);
            buf[64 - 3] = (byte)(0xff & (size >> 16));
            buf[64 - 2] = (byte)(0xff & (size >> 8));
            buf[64 - 1] = (byte)(0xff & size);

            return buf;
        }
    }
}
//Console.WriteLine(String.Join("",tmp.Select(x=>x.ToString("x"))));

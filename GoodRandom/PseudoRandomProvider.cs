using System.Security.Cryptography;

namespace GoodRandom {
    public class PseudoRandomProvider : IRandomProvider
    {
        private readonly Random random;
        public readonly int Seed;
        
        private const int defaultBufferSize = 1000;
        private readonly int bufferSize;

        private readonly byte[] buffer;
        private int index = 0;

        public PseudoRandomProvider(int? seed = null, int bufferSize = defaultBufferSize) {
            this.Seed = seed ?? RandomNumberGenerator.GetInt32(int.MaxValue);
            this.random = new Random(this.Seed);
            this.bufferSize = bufferSize;
            buffer = new byte[this.bufferSize];
        }

        public byte GetNextByte() {
            refillBuffer();
            return buffer[index++];
        }

        private void refillBuffer() {
            if(index % bufferSize == 0) {
                random.NextBytes(buffer);
                index = 0;
            }
        }
    }
}
using System.Security.Cryptography;

namespace GoodRandom
{
    public class SecureRandomProvider : IRandomProvider
    {
        private const int defaultBufferSize = 1000;
        private readonly int bufferSize;

        private readonly byte[] buffer;
        private int index = 0;

        public SecureRandomProvider(int bufferSize = defaultBufferSize) {
            this.bufferSize = bufferSize;
            buffer = new byte[this.bufferSize];
        }

        public byte GetNextByte() {
            refillBuffer();
            return buffer[index++];
        }

        private void refillBuffer() {
            if(index % bufferSize == 0) {
                RandomNumberGenerator.Fill(buffer);
                index = 0;
            }
        }
    }
}
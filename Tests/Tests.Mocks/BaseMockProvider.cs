//using Moq;

//namespace atf.Tests.Mocks
//{
//    /// <summary>
//    /// Base class for providing Moq-based mocks.
//    /// </summary>
//    public abstract class BaseMockProvider: IDisposable
//    {
//        protected readonly MockRepository Mocks = new MockRepository(MockBehavior.Default);

//        public void Dispose()
//        {
//            VerifyAll();
//        }

//        public void VerifyAll() => Mocks.VerifyAll();
//    }
//}
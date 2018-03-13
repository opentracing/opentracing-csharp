using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using OpenTracing.Mock;
using OpenTracing.Noop;
using OpenTracing.Tag;

namespace OpenTracing.Benchmarks
{
    public class TagBenchmark
    {
        private const string _httpStatusKey = "http.status_code";

        private readonly ISpan _mockSpan;
        private readonly ISpan _noopSpan;

        public TagBenchmark()
        {
            var mockTracer = new MockTracer();
            _mockSpan = mockTracer.BuildSpan("foo").Start();

            var noopTracer = NoopTracerFactory.Create();
            _noopSpan = noopTracer.BuildSpan("foo").Start();
        }

        [Benchmark]
        public void MockSpan_IntTag()
        {
            _mockSpan.SetTag_IntTag(Tags.HttpStatus, 200);
        }

        [Benchmark]
        public void MockSpan_AbstractTag_Generic()
        {
            _mockSpan.SetTag_AbstractGeneric(Tags.HttpStatus, 200);
        }

        [Benchmark]
        public void MockSpan_AbstractTag_Typed()
        {
            _mockSpan.SetTag_AbstractTyped(Tags.HttpStatus, 200);
        }

        [Benchmark]
        public void MockSpan_IntTag_Key()
        {
            _mockSpan.SetTag_Int(Tags.HttpStatus.Key, 200);
        }

        [Benchmark]
        public void MockSpan_Const()
        {
            _mockSpan.SetTag_Int(_httpStatusKey, 200);
        }

        [Benchmark]
        public void MockSpan_Const_Overload()
        {
            _mockSpan.SetTag(_httpStatusKey, 200);
        }

        [Benchmark]
        public void NoopSpan_IntTag()
        {
            _noopSpan.SetTag_IntTag(Tags.HttpStatus, 200);
        }

        [Benchmark]
        public void NoopSpan_AbstractTag_Generic()
        {
            _noopSpan.SetTag_AbstractGeneric(Tags.HttpStatus, 200);
        }

        [Benchmark]
        public void NoopSpan_AbstractTag_Typed()
        {
            _noopSpan.SetTag_AbstractTyped(Tags.HttpStatus, 200);
        }

        [Benchmark]
        public void NoopSpan_IntTag_Key()
        {
            _noopSpan.SetTag_Int(Tags.HttpStatus.Key, 200);
        }

        [Benchmark]
        public void NoopSpan_Const()
        {
            _noopSpan.SetTag_Int(_httpStatusKey, 200);
        }

        [Benchmark]
        public void NoopSpan_Const_Overload()
        {
            _noopSpan.SetTag(_httpStatusKey, 200);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<TagBenchmark>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.Utils.Path;
using FluentAssertions;
using NUnit.Framework;

namespace Path.Tests
{
    [TestFixture]
    public class PathTests
    {
        [Test]
        public void AbsolutePath_WhenCheckedIfAbsolute_ShouldReturnTrue()
        {
            var absPath = @"c:\test\path.txt";

            absPath.IsAbsolute().Should().BeTrue();
        }

        [Test]
        public void RootedPath_WhenCheckedIfAbsolute_ShouldReturnFalse()
        {
            var absPath = @"\test\path.txt";

            absPath.IsAbsolute().Should().BeFalse();
        }

        [Test]
        public void RelativePath_WhenCheckedIfAbsolute_ShouldReturnFalse()
        {
            var absPath = @"test\path.txt";

            absPath.IsAbsolute().Should().BeFalse();
        }

        [Test]
        public void InvalidPath_WhenCheckedIfAbsolute_ShouldThrow()
        {
            var invalidPath = @"<>!#$%test\path.txt";

            Action act = () => { invalidPath.IsAbsolute(); };

            act.ShouldThrow<Exception>();
        }

        [Test]
        public void ValidAbsolutePath_WhenSolved_ShouldBeSame()
        {
            var absPath = @"c:\test\path.txt";

            absPath.SolvePath().Should().Be(absPath);
        }

        [Test]
        public void RelativePath_WhenSolved_ShouldBeRelativeToTheExecutingAssembly()
        {
            var relativePath = @"test\path.txt";
            //get the drive of executing assembly
            var pathToExecutingAssembly = AppDomain.CurrentDomain.BaseDirectory;
            var whatShouldBeAfterSolving = System.IO.Path.Combine(pathToExecutingAssembly, relativePath);

            relativePath.SolvePath().Should().Be(whatShouldBeAfterSolving);
        }

        [Test]
        public void RootedPath_WhenSolved_ShouldBeRelatedToRootOfTheExecutingAssembly()
        {
            var rootedPath = @"\test\path.txt";
            //get the drive of executing assembly
            var rootOfExecutingAssembly = System.IO.Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory);
            var whatShouldBeAfterSolving = System.IO.Path.Combine(rootOfExecutingAssembly, rootedPath);

            rootedPath.SolvePath().Should().Be(whatShouldBeAfterSolving);
        }

        [Test]
        public void InvalidPath_WhenSolved_ShouldBeNull()
        {
            var invalidPath = @"<>!#$%test\path.txt";
            invalidPath.SolvePath().Should().Be(null);
        }
    }
}
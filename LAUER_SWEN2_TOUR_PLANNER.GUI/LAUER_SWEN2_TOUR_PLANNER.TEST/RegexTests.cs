using System.Text.RegularExpressions;

namespace LAUER_SWEN2_TOUR_PLANNER.TEST
{
    public class Tests
    {
        Regex stringRegex = new(@"[a-zA-Z0-9\x20\-]");
        Regex numberRegex = new("^[0-9]+$");
        [SetUp]
        public void Setup()
        {
        }
        #region user input tests
        [Test]
        public void NumbersOnlyReg_NumbersOnlyTest()
        {
            if(numberRegex.IsMatch("02232324432"))
            {
                Assert.Pass();
            }
            Assert.Fail();
        }


        [Test]
        public void NumbersOnlyReg_NumbersAndSpaces()
        {
            if (numberRegex.IsMatch("02 232 32 443 2"))
            {
                Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        public void NumbersOnlyReg_NumbersAndCharactersTest()
        {
            if (numberRegex.IsMatch("0d2da22"))
            {
                Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        public void NumbersOnlyReg_SpecialCharactersTest()
        {
            if (numberRegex.IsMatch("%0d2d%a22%"))
            {
                Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        public void NumbersOnlyReg_Empty()
        {
            if (numberRegex.IsMatch(""))
            {
                Assert.Fail();
            }
            Assert.Pass();
        }


        [Test]
        public void NumbersAndCharactersReg_NumbersTest()
        {
            if (stringRegex.IsMatch("0234242032"))
            {
                Assert.Pass();
            }
            Assert.Fail();
        }


        [Test]
        public void NumbersAndCharactersReg_NumbersAndCharactersTest()
        {
            if (stringRegex.IsMatch("03294292323232dawdwa"))
            { 
                Assert.Pass();
            }
            Assert.Fail();
        }


        [Test]
        public void NumbersAndCharactersReg_NumbersAndCharactersTestAndSpaces()
        {
            if (stringRegex.IsMatch("032 9429 232323 2dawdw a"))
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void NumbersAndCharactersReg_NumbersAndCharactersTestAndSpacesMultiline()
        {
            if (stringRegex.IsMatch("032 9429 232323 2da\nwdw \na"))
            {
                Assert.Pass();
            }
            Assert.Fail();
        }


        [Test]
        public void NumbersAndCharactersReg_SpecialChars()
        {
            if (stringRegex.IsMatch("%$§$$$"))
            {
                Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        public void NumbersAndCharactersReg_SpecialCharsLinesAndSpaces()
        {
            if (stringRegex.IsMatch(" - - - - - - - "));
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        #endregion


    }
}
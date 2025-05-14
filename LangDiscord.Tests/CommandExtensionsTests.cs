using System.Collections.Generic;
using LangDiscord.Enums;
using LangDiscord.Extensions;
using LangDiscord.Interfaces;
using Moq;
using Xunit;

namespace LangDiscord.Tests
{
    public class CommandExtensionsTests
    {
        private readonly Mock<ILangCodeConverter> _mockLangCodeConverter;
        private readonly CommandExtensions _commandExtensions;

        public CommandExtensionsTests()
        {
            _mockLangCodeConverter = new Mock<ILangCodeConverter>();

            _mockLangCodeConverter.Setup(x => x.IsValidLanguageCode("eng")).Returns(true);
            _mockLangCodeConverter.Setup(x => x.IsValidLanguageCode("pol")).Returns(true);
            _mockLangCodeConverter.Setup(x => x.IsValidLanguageCode("spa")).Returns(true);
            _mockLangCodeConverter.Setup(x => x.IsValidLanguageCode(It.Is<string>(s => s != "eng" && s != "pol" && s != "spa")))
                                  .Returns(false);

            _commandExtensions = new CommandExtensions(_mockLangCodeConverter.Object);
        }

        [Theory]
        [InlineData("help", true)]
        [InlineData("main", true)]
        [InlineData("fav", true)]
        [InlineData("config", true)]
        [InlineData("langs", true)]
        [InlineData("!", true)]
        [InlineData("", false)]
        [InlineData("eng", true)]
        [InlineData("zzz", false)]
        public void DoesCommandExist_ReturnsExpectedResult(string input, bool expected)
        {
            var result = _commandExtensions.DoesCommandExist(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("help", Command.HELP)]
        [InlineData("main", Command.SET_MAIN_LANGUAGE)]
        [InlineData("fav", Command.SET_FAVORITE_LANGUAGE)]
        [InlineData("config", Command.CHECK_CONFIG)]
        [InlineData("langs", Command.AVAILABLE_LANGS)]
        [InlineData("!", Command.FAST_TRANSLATE)]
        public void GetCommandFromString_ValidCommand_ReturnsExpectedEnum(string input, Command expected)
        {
            var result = _commandExtensions.GetCommandFromString(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetCommandFromString_ValidLangCode_ReturnsLangAction()
        {
            var result = _commandExtensions.GetCommandFromString("pol");
            Assert.Equal(Command.LANG_ACTION, result);
        }

        [Fact]
        public void GetCommandFromString_InvalidInput_ReturnsNull()
        {
            var result = _commandExtensions.GetCommandFromString("unknown");
            Assert.Null(result);
        }
    }
}

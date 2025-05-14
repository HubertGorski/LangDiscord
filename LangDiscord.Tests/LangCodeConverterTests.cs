using System;
using System.Collections.Generic;
using LangDiscord.Enums;
using LangDiscord.Interfaces;
using Xunit;

namespace LangDiscord.Tests
{
    public class LangCodeConverterTests
    {
        private readonly LangCodeConverter _converter = new();

        [Theory]
        [InlineData("en", "eng")]
        [InlineData("pl", "pol")]
        [InlineData("es", "spa")]
        [InlineData("de", null)]
        public void GetLangCodeFromDetectedLang_ReturnsExpectedResult(string detectedLang, string expectedCode)
        {
            var result = _converter.GetLangCodeFromDetectedLang(detectedLang);
            Assert.Equal(expectedCode, result);
        }

        [Theory]
        [InlineData("eng", true)]
        [InlineData("pol", true)]
        [InlineData("spa", true)]
        [InlineData("deu", false)]
        [InlineData("", false)]
        public void IsValidLanguageCode_ReturnsCorrectResult(string code, bool expected)
        {
            var result = _converter.IsValidLanguageCode(code);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(Lang.ENGLISH, "eng")]
        [InlineData(Lang.POLISH, "pol")]
        [InlineData(Lang.SPANISH, "spa")]
        public void GetCode_ReturnsCorrectCode(Lang lang, string expectedCode)
        {
            var result = _converter.GetCode(lang);
            Assert.Equal(expectedCode, result);
        }

        [Fact]
        public void GetLangCodeMap_ContainsExpectedValues()
        {
            var map = _converter.GetLangCodeMap();
            Assert.Equal("eng", map[Lang.ENGLISH]);
            Assert.Equal("pol", map[Lang.POLISH]);
            Assert.Equal("spa", map[Lang.SPANISH]);
        }

        [Fact]
        public void GetAllLangCodes_ReturnsAllCodes()
        {
            var codes = _converter.GetAllLangCodes();
            Assert.Contains("eng", codes);
            Assert.Contains("pol", codes);
            Assert.Contains("spa", codes);
        }

        [Theory]
        [InlineData("eng", Lang.ENGLISH)]
        [InlineData("pol", Lang.POLISH)]
        [InlineData("spa", Lang.SPANISH)]
        public void GetLangFromCode_ValidCode_ReturnsExpectedLang(string code, Lang expectedLang)
        {
            var result = _converter.GetLangFromCode(code);
            Assert.Equal(expectedLang, result);
        }

        [Fact]
        public void GetLangFromCode_InvalidCode_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _converter.GetLangFromCode("deu"));
        }
    }
}

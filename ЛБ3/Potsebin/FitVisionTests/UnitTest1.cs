using System;
using Xunit;
using FitVisionAI_LB3;



namespace FitVisionTests
{

    public class FitVisionUnitTests
    {

        [Fact]
        public void ValidateQuality_ValidSize_ReturnsTrue()
        {
            var photo = new Photo("1", "test.jpg", 4.0f);

            bool result = photo.ValidateQuality();

            Assert.True(result);
        }

        [Fact]
        public void ValidateQuality_ExactUpperBoundary_ReturnsTrue()
        {
            var photo = new Photo("2", "test.jpg", 15.0f);

            bool result = photo.ValidateQuality();

            Assert.True(result);
        }

        [Fact]
        public void ValidateQuality_ExceedsUpperBoundary_ThrowsException()
        {
            var photo = new Photo("3", "test.jpg", 15.1f);

            var ex = Assert.Throws<Exception>(() => photo.ValidateQuality());

            Assert.Contains("Файл занадто великий", ex.Message);
        }

        [Fact]
        public void ValidateQuality_ZeroSize_ThrowsInvalidOperationException()
        {
            var photo = new Photo("4", "test.jpg", 0.0f);

            Assert.Throws<InvalidOperationException>(() => photo.ValidateQuality());
        }


        [Fact]
        public void Authenticate_CorrectCredentials_ReturnsTrue()
        {
            var auth = new AuthService();

            bool result = auth.Authenticate("user@fitvision.com", "qwerty");

            Assert.True(result);
        }

        [Fact]
        public void Authenticate_WrongPassword_ReturnsFalse()
        {
            var auth = new AuthService();

            bool result = auth.Authenticate("user@fitvision.com", "wrong_pass");

            Assert.False(result);
        }

        [Fact]
        public void Authenticate_EmptyEmail_ThrowsArgumentException()
        {
            var auth = new AuthService();

            Assert.Throws<ArgumentException>(() => auth.Authenticate("", "qwerty"));
        }

        [Fact]
        public void Authenticate_NullPassword_ThrowsArgumentException()
        {
            var auth = new AuthService();

            Assert.Throws<ArgumentException>(() => auth.Authenticate("user@fitvision.com", null));
        }

        [Fact]
        public void Register_ValidEmail_CreatesProfile()
        {
            var auth = new AuthService();
            var user = new User(1, "a@b.c", auth);

            user.Register();

            Assert.NotNull(user.UserProfile);
        }

        [Fact]
        public void Register_MissingAtSymbol_ThrowsFormatException()
        {
            var auth = new AuthService();
            var user = new User(2, "invalid.email.com", auth);

            Assert.Throws<FormatException>(() => user.Register());
        }

        [Fact]
        public void Register_MissingDot_ThrowsFormatException()
        {
            var auth = new AuthService();
            var user = new User(3, "invalid@emailcom", auth);
            
            Assert.Throws<FormatException>(() => user.Register());
        }
    }
}
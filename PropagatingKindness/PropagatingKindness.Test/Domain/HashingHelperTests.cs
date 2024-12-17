using PropagatingKindness.Domain.Services;

namespace PropagatingKindness.Test.Domain;

public class HashingHelperTests
{
    [Fact]
    public void CalculateHashWithSalt_ReturnsSaltedHash()
    {
        // Arrange
        string password = "TestPassword123";

        // Act
        string saltedHash = HashingHelper.CalculateHashWithSalt(password);

        // Assert
        Assert.NotNull(saltedHash);
        Assert.Contains(HashingHelper.SALT_SEPARATOR, saltedHash);

        var parts = saltedHash.Split(HashingHelper.SALT_SEPARATOR);
        Assert.Equal(2, parts.Length); // Ensure salt and hash are both present
        Assert.NotEmpty(parts[0]); // Salt
        Assert.NotEmpty(parts[1]); // Hash
    }

    [Fact]
    public void ValidateHashWithSalt_ValidatesCorrectPassword()
    {
        // Arrange
        string password = "TestPassword123";
        string saltedHash = HashingHelper.CalculateHashWithSalt(password);

        // Act
        bool isValid = HashingHelper.ValidateHashWithSalt(saltedHash, password);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void ValidateHashWithSalt_ReturnsFalseForIncorrectPassword()
    {
        // Arrange
        string password = "TestPassword123";
        string wrongPassword = "WrongPassword456";
        string saltedHash = HashingHelper.CalculateHashWithSalt(password);

        // Act
        bool isValid = HashingHelper.ValidateHashWithSalt(saltedHash, wrongPassword);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void ValidateHashWithSalt_ThrowsExceptionForInvalidSaltedHash()
    {
        // Arrange
        string invalidSaltedHash = "InvalidHashFormat";
        string password = "TestPassword123";

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => HashingHelper.ValidateHashWithSalt(invalidSaltedHash, password));
        Assert.Equal("ERROR: Salted hash is missing the salt separator.", exception.Message);
    }

    [Fact]
    public void CalculateHashWithSalt_ProducesUniqueSalts()
    {
        // Arrange
        string password = "TestPassword123";

        // Act
        string saltedHash1 = HashingHelper.CalculateHashWithSalt(password);
        string saltedHash2 = HashingHelper.CalculateHashWithSalt(password);

        // Assert
        Assert.NotEqual(saltedHash1, saltedHash2); // Each call should produce a unique salt
    }
}
